using HireAI.Data.Helpers.DTOs.Authentication;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Data.Models.Identity;
using HireAI.Infrastructure.Context;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HireAI.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly HireAIDbContext _dbContext;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config,
            HireAIDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _dbContext = dbContext;
        }

        public async Task<AuthResponseDto> RegisterApplicantAsync(RegisterApplicantDto registerDto)
        {
            try
            {
                // Validate if email already exists
                var existingEmail = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingEmail != null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Email already registered"
                    };
                }

                // Create ApplicationUser (Identity) - Use email as username
                var user = new ApplicationUser
                {
                    UserName = registerDto.Email, // Set email as username
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.Phone
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Registration failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Assign Applicant role
                await _userManager.AddToRoleAsync(user, "Applicant");

                // Create Applicant profile (Domain Model)
                var applicant = new Applicant
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    Address = registerDto.Address ?? string.Empty,
                    DateOfBirth = registerDto.DateOfBirth,
                    Phone = registerDto.Phone,
                    Title = registerDto.Title,
                    ResumeUrl = registerDto.ResumeUrl ?? string.Empty,
                    SkillLevel = registerDto.SkillLevel,
                    Role = enRole.Applicant,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                // Save applicant to database
                _dbContext.Applicants.Add(applicant);
                await _dbContext.SaveChangesAsync();

                // Link ApplicationUser to Applicant
                user.ApplicantId = applicant.Id;
                await _userManager.UpdateAsync(user);

                return new AuthResponseDto
                {
                    IsAuthenticated = true,
                    IdentityUserId = user.Id,
                    Message = "User registered successfully"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "An error occurred during registration",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Find user by email instead of username
                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Invalid email or password"
                    };
                }

                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (!isPasswordCorrect)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Invalid email or password"
                    };
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user, roles);
                var refreshToken = GenerateRefreshToken();

                // Save refresh token to database
                var userRefreshToken = new UserRefreshToken
                {
                    UserId = user.Id,
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    JwtId = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                };

                _dbContext.Set<UserRefreshToken>().Add(userRefreshToken);
                await _dbContext.SaveChangesAsync();

               /* // Determine the user-specific ID based on role
                string? userSpecificId = null;

                if (roles.Contains("Applicant"))
                {
                    if (!user.ApplicantId.HasValue)
                    {
                        return new AuthResponseDto
                        {
                            IsAuthenticated = false,
                            Message = "Applicant profile not found"
                        };
                    }
                    userSpecificId = user.ApplicantId.Value.ToString();
                }
                else if (roles.Contains("HR"))
                {
                    if (!user.HRId.HasValue)
                    {
                        return new AuthResponseDto
                        {
                            IsAuthenticated = false,
                            Message = "HR profile not found"
                        };
                    }
                    userSpecificId = user.HRId.Value.ToString();
                }
                else
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Invalid user role"
                    };
                }*/

                // Determine user type and ID first
                int? userSpecificId = null;
                string? userRole = null;

                if (user.ApplicantId.HasValue)
                {
                    userSpecificId = user.ApplicantId; ;
                    userRole = "Applicant";
                }
                else if (user.HRId.HasValue)
                {
                    userSpecificId = user.HRId;
                    userRole = "HR";
                }
                else
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "User profile not found"
                    };
                }

                return new AuthResponseDto
                {
                    IsAuthenticated = true,
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(30),
                    IdentityUserId = user.Id, // Identity framework ID
                    UserId = userSpecificId, // Applicant/HR ID
                    UserRole = userRole, // roles.Contains("Applicant") ? "Applicant" : "HR",
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "An error occurred during login",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(accessToken);

                if (principal == null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Invalid token"
                    };
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Invalid token claims"
                    };
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "User not found"
                    };
                }

                // Validate refresh token from database
                var storedRefreshToken = user.UserRefreshTokens.FirstOrDefault(t =>
                    t.RefreshToken == refreshToken && !t.IsRevoked && t.ExpiryDate > DateTime.UtcNow);

                if (storedRefreshToken == null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Invalid or expired refresh token"
                    };
                }

                var roles = await _userManager.GetRolesAsync(user);
                var newAccessToken = GenerateJwtToken(user, roles);
                var newRefreshToken = GenerateRefreshToken();

                // Update refresh token
                storedRefreshToken.RefreshToken = newRefreshToken;
                storedRefreshToken.AccessToken = newAccessToken;
                storedRefreshToken.CreatedDate = DateTime.UtcNow;

                _dbContext.Set<UserRefreshToken>().Update(storedRefreshToken);
                await _dbContext.SaveChangesAsync();

                return new AuthResponseDto
                {
                    IsAuthenticated = true,
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(30),
                    IdentityUserId = user.Id,
                    Message = "Token refreshed successfully"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "Token refresh failed",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<bool> RevokeTokenAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                    return false;

                // Mark all user tokens as revoked
                foreach (var token in user.UserRefreshTokens)
                {
                    token.IsRevoked = true;
                }

                _dbContext.Set<UserRefreshToken>().UpdateRange(user.UserRefreshTokens);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email ?? string.Empty), // Use email in Name claim
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecurityKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:IssuerIP"],
                audience: _config["JWT:AudienceIP"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecurityKey"])),
                    ValidateLifetime = false // Allow expired tokens
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
