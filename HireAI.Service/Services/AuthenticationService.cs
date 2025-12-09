using HireAI.Data.Helpers.DTOs.Authentication;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Data.Models.Identity;
using HireAI.Infrastructure.Context;
using HireAI.Service.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HireAI.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly HireAIDbContext _dbContext;
        private readonly IS3Service _s3Service;

        public AuthenticationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration config, HireAIDbContext dbContext, IS3Service s3Service)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _dbContext = dbContext;
            _s3Service = s3Service;
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
                var ApplicationUser = new ApplicationUser
                {
                    UserName = registerDto.Email, // Set email as username
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.Phone
                };

                var result = await _userManager.CreateAsync(ApplicationUser, registerDto.Password);

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
                await _userManager.AddToRoleAsync(ApplicationUser, "Applicant");

                // Handle CV upload
                if (registerDto == null || registerDto.CvFile == null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Registration failed due to failed file upload",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Upload to S3
                string resumeKey;
                try
                {
                    // Cast dto.CvFile to Microsoft.AspNetCore.Http.IFormFile f possible
                    resumeKey = await _s3Service.UploadFileAsync(registerDto.CvFile);
                }
                catch (Exception ex)
                {
                    // log ex in real app
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Registration failed due to failed file upload",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }


                // Create Applicant profile (Domain Model)
                var applicant = new Applicant
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    Address = registerDto.Address ?? string.Empty,
                    DateOfBirth = registerDto.DateOfBirth,
                    Phone = registerDto.Phone,
                    Title = registerDto.Title,
                    ResumeUrl = resumeKey ?? string.Empty,
                    SkillLevel = registerDto.SkillLevel,
                    Role = enRole.Applicant,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                // Save applicant to database
                _dbContext.Applicants.Add(applicant);
                await _dbContext.SaveChangesAsync();

                // Link ApplicationUser to Applicant
                ApplicationUser.ApplicantId = applicant.Id;
                await _userManager.UpdateAsync(ApplicationUser);

                return new AuthResponseDto
                {
                    IsAuthenticated = true,
                    IdentityUserId = ApplicationUser.Id,
                    UserId = applicant.Id,
                    UserRole = "Applicant",
                    Message = "Applicant registered successfully"
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

        public async Task<AuthResponseDto> RegisterHRAsync(RegisterHrDto registerDto)
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
                var ApplicationUser = new ApplicationUser
                {
                    UserName = registerDto.Email, // Set email as username
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.Phone
                };

                var result = await _userManager.CreateAsync(ApplicationUser, registerDto.Password);

                if (!result.Succeeded)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Registration failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Assign HR role
                await _userManager.AddToRoleAsync(ApplicationUser, "HR");

                // Create HR profile (Domain Model)
                var hr = new HR
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    Address = registerDto.HrAddress ?? string.Empty,
                    DateOfBirth = registerDto.DateOfBirth,
                    Phone = registerDto.Phone,
                    Title = registerDto.JobTitle,
                    CompanyName = registerDto.CompanyName,
                    CompanyAddress = registerDto.CompanyAddress,
                    CompanyDescription = registerDto.CompanyDescription,
                    AccountType = registerDto.AccountType ?? enAccountType.Free,
                    Role = enRole.HR,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                // Save HR to database
                _dbContext.HRs.Add(hr);
                await _dbContext.SaveChangesAsync();

                // Link ApplicationUser to HR
                ApplicationUser.HRId = hr.Id;
                await _userManager.UpdateAsync(ApplicationUser);

                return new AuthResponseDto
                {
                    IsAuthenticated = true,
                    IdentityUserId = ApplicationUser.Id,
                    UserId = hr.Id,
                    UserRole = "HR",
                    Message = "HR user registered successfully"
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
                var ApplicationUser = await _userManager.FindByEmailAsync(loginDto.Email);

                if (ApplicationUser == null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Invalid email or password"
                    };
                }

                var isPasswordCorrect = await _userManager.CheckPasswordAsync(ApplicationUser, loginDto.Password);

                if (!isPasswordCorrect)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Invalid email or password"
                    };
                }

                var roles = await _userManager.GetRolesAsync(ApplicationUser);
                var token = GenerateJwtToken(ApplicationUser, roles);
                var refreshToken = GenerateRefreshToken();

                // ✅ Detach the user to prevent EF from tracking it in this context
                //_dbContext.Entry(ApplicationUser).State = EntityState.Detached;


                // Save refresh token to database - FIXED: Set User navigation property
                var userRefreshToken = new UserRefreshToken
                {
                    UserId = ApplicationUser.Id,
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    JwtId = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false // ✅ Explicitly set this
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

                if (ApplicationUser.ApplicantId.HasValue)
                {
                    userSpecificId = ApplicationUser.ApplicantId; ;
                    userRole = "Applicant";

                    var applicant = await _dbContext.Applicants.FindAsync(ApplicationUser.ApplicantId.Value);
                    applicant.LastLogin = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
                }
                else if (ApplicationUser.HRId.HasValue)
                {
                    userSpecificId = ApplicationUser.HRId;
                    userRole = "HR";

                    var Hr = await _dbContext.HRs.FindAsync(ApplicationUser.HRId.Value);
                    Hr.LastLogin = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
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
                    IdentityUserId = ApplicationUser.Id, // Identity framework ID
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

        public async Task<AuthResponseDto> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "User not found"
                    };
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, currentPassword);

                if (!passwordCheck)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Current password is incorrect"
                    };
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (!result.Succeeded)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Password change failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Revoke all existing tokens for security
                await RevokeTokenAsync(userId);

                return new AuthResponseDto
                {
                    IsAuthenticated = true,
                    Message = "Password changed successfully. Please login again."
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "An error occurred during password change",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<AuthResponseDto> ChangeEmailAsync(string userId, string newEmail)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "User not found"
                    };
                }

                // Check if new email already exists
                var existingUser = await _userManager.FindByEmailAsync(newEmail);
                if (existingUser != null && existingUser.Id != userId)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Email is already in use"
                    };
                }

                // Generate email change token
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
                var result = await _userManager.ChangeEmailAsync(user, newEmail, token);

                if (!result.Succeeded)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Email change failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Update username to match email
                user.UserName = newEmail;
                await _userManager.UpdateAsync(user);

                // Update email in Applicant or HR table
                if (user.ApplicantId.HasValue)
                {
                    var applicant = await _dbContext.Applicants.FindAsync(user.ApplicantId.Value);
                    if (applicant != null)
                    {
                        applicant.Email = newEmail;
                    }
                }
                else if (user.HRId.HasValue)
                {
                    var hr = await _dbContext.HRs.FindAsync(user.HRId.Value);
                    if (hr != null)
                    {
                        hr.Email = newEmail;
                    }
                }

                await _dbContext.SaveChangesAsync();

                return new AuthResponseDto
                {
                    IsAuthenticated = true,
                    Message = "Email changed successfully"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "An error occurred during email change",
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

                // Validate refresh token from database - Query directly instead of using navigation property
                var storedRefreshToken = await _dbContext.Set<UserRefreshToken>()
                    .FirstOrDefaultAsync(t => 
                        t.UserId == userId && 
                        t.RefreshToken == refreshToken && 
                        !t.IsRevoked && 
                        t.ExpiryDate > DateTime.UtcNow);

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

                // Determine user type and ID 
                int? userSpecificId = null;
                string? userRole = null;

                if (user.ApplicantId.HasValue)
                {
                    userSpecificId = user.ApplicantId;
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
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresOn = DateTime.UtcNow.AddMinutes(30),
                    IdentityUserId = user.Id,
                    UserId = userSpecificId, 
                    UserRole = userRole, 
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

                // Query tokens directly from database
                var userTokens = await _dbContext.Set<UserRefreshToken>()
                    .Where(t => t.UserId == userId)
                    .ToListAsync();

                // Mark all user tokens as revoked
                foreach (var token in userTokens)
                {
                    token.IsRevoked = true;
                }

                _dbContext.Set<UserRefreshToken>().UpdateRange(userTokens);
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

            //Add all User Roles to the UserClaims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //Add the Token Generated id change (JWT Predefind Claims ) to generate new token every login
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            //Now it's time for the Signature part in the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecurityKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Design Token
            var token = new JwtSecurityToken(
                issuer: _config["JWT:IssuerIP"],
                audience: _config["JWT:AudienceIP"], //Angular Localhost
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            //Generate Token response
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
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

        public async Task<AuthResponseDto> DeleteAccountAsync(string userId, string password)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "User not found"
                    };
                }

                // Verify password for security
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
                if (!isPasswordCorrect)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Invalid password. Account deletion failed."
                    };
                }

                // Delete associated profile data (Applicant or HR)
                if (user.ApplicantId.HasValue)
                {
                    var applicant = await _dbContext.Applicants.FindAsync(user.ApplicantId.Value);
                    if (applicant != null)
                    {
                        _dbContext.Applicants.Remove(applicant);
                    }
                }
                else if (user.HRId.HasValue)
                {
                    var hr = await _dbContext.HRs.FindAsync(user.HRId.Value);
                    if (hr != null)
                    {
                        _dbContext.HRs.Remove(hr);
                    }
                }

                // Delete all refresh tokens
                var userTokens = await _dbContext.Set<UserRefreshToken>()
                    .Where(t => t.UserId == userId)
                    .ToListAsync();

                _dbContext.Set<UserRefreshToken>().RemoveRange(userTokens);

                // Save changes before deleting Identity user
                await _dbContext.SaveChangesAsync();

                // Delete the Identity user (this will cascade delete related Identity data)
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return new AuthResponseDto
                    {
                        IsAuthenticated = false,
                        Message = "Account deletion failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                return new AuthResponseDto
                {
                    IsAuthenticated = false, // Account is deleted, no longer authenticated
                    Message = "Account deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = "An error occurred during account deletion",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
