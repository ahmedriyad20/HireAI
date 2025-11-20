using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitiatingTablesandAdd_Identity_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "UserSequence");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LinkedInPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GitHubPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Education = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Experience = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    YearsOfExperience = table.Column<float>(type: "real", nullable: true),
                    Certifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVs", x => x.Id);
                    table.CheckConstraint("CK_CV_YearsOfExperience", "[YearsOfExperience] >= 0 OR [YearsOfExperience] IS NULL");
                });

            migrationBuilder.CreateTable(
                name: "HRs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [UserSequence]"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValue: "Free"),
                    PremiumExpiry = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Siklls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siklls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobOpenings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    JobStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Active"),
                    ExamDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    ExperienceLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SalaryRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumberOfQuestions = table.Column<int>(type: "int", nullable: true),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ATSMinimumScore = table.Column<int>(type: "int", nullable: true),
                    AutoSend = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HRId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOpenings", x => x.Id);
                    table.CheckConstraint("CK_JobOpening_ATSScore", "([ATSMinimumScore] >= 0 AND [ATSMinimumScore] <= 100) OR [ATSMinimumScore] IS NULL");
                    table.CheckConstraint("CK_JobOpening_ExamDuration", "[ExamDurationMinutes] > 0 OR [ExamDurationMinutes] IS NULL");
                    table.CheckConstraint("CK_JobOpening_Questions", "[NumberOfQuestions] > 0 OR [NumberOfQuestions] IS NULL");
                    table.ForeignKey(
                        name: "FK_JobOpenings_HRs_HRId",
                        column: x => x.HRId,
                        principalTable: "HRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpgradeTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillingPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HrId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.CheckConstraint("CK_Payment_Amount", "[Amount] > 0");
                    table.CheckConstraint("CK_Payment_Currency", "LEN([Currency]) = 3");
                    table.ForeignKey(
                        name: "FK_Payments_HRs_HrId",
                        column: x => x.HrId,
                        principalTable: "HRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [UserSequence]"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResumeUrl = table.Column<string>(type: "varchar(200)", nullable: false),
                    CVId = table.Column<int>(type: "int", nullable: false),
                    JobOpeningId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applicants_CVs_CVId",
                        column: x => x.CVId,
                        principalTable: "CVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applicants_JobOpenings_JobOpeningId",
                        column: x => x.JobOpeningId,
                        principalTable: "JobOpenings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExamEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalScore = table.Column<float>(type: "real", nullable: false),
                    MaxTotal = table.Column<float>(type: "real", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    EvaluatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ExamSummaryId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamEvaluation", x => x.Id);
                    table.CheckConstraint("CK_ExamEvaluation_Scores", "[TotalScore] >= 0 AND [MaxTotal] > 0 AND [TotalScore] <= [MaxTotal]");
                    table.ForeignKey(
                        name: "FK_ExamEvaluation_JobOpenings_JobId",
                        column: x => x.JobId,
                        principalTable: "JobOpenings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSkills_JobOpenings_JobId",
                        column: x => x.JobId,
                        principalTable: "JobOpenings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobSkills_Siklls_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Siklls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillRate = table.Column<float>(type: "real", nullable: true),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantSkills", x => x.Id);
                    table.CheckConstraint("CK_ApplicantSkill_Rate", "([SkillRate] >= 0 AND [SkillRate] <= 100) OR [SkillRate] IS NULL");
                    table.ForeignKey(
                        name: "FK_ApplicantSkills_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicantSkills_Siklls_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Siklls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationStatus = table.Column<int>(type: "int", nullable: false),
                    DateApplied = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CVFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ScoreATS = table.Column<float>(type: "real", nullable: true),
                    HRId = table.Column<int>(type: "int", nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.CheckConstraint("CK_Application_Score", "([ScoreATS] >= 0 AND [ScoreATS] <= 100) OR [ScoreATS] IS NULL");
                    table.ForeignKey(
                        name: "FK_Applications_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_HRs_HRId",
                        column: x => x.HRId,
                        principalTable: "HRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_JobOpenings_JobId",
                        column: x => x.JobId,
                        principalTable: "JobOpenings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResetCode = table.Column<string>(type: "NVARCHAR(2000)", maxLength: 2000, nullable: true),
                    HRId = table.Column<int>(type: "int", nullable: true),
                    ApplicantId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_HRs_HRId",
                        column: x => x.HRId,
                        principalTable: "HRs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfQuestions = table.Column<int>(type: "int", nullable: false),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExamName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsAi = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.CheckConstraint("CK_Exam_Duration", "[DurationInMinutes] > 0");
                    table.CheckConstraint("CK_Exam_NumberOfQuestions", "[NumberOfQuestions] > 0");
                    table.ForeignKey(
                        name: "FK_Exams_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSummarys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    ExamEvaluationId = table.Column<int>(type: "int", nullable: true),
                    ApplicantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSummarys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSummarys_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExamSummarys_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamSummarys_ExamEvaluation_ExamEvaluationId",
                        column: x => x.ExamEvaluationId,
                        principalTable: "ExamEvaluation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ExamSummarys_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerNumber = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    ExamSummaryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantResponses", x => x.Id);
                    table.CheckConstraint("CK_ApplicantResponse_Answer", "[AnswerNumber] > 0");
                    table.ForeignKey(
                        name: "FK_ApplicantResponses_ExamSummarys_ExamSummaryId",
                        column: x => x.ExamSummaryId,
                        principalTable: "ExamSummarys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Feedback = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    EvaluatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ApplicantResponseId = table.Column<int>(type: "int", nullable: false),
                    ExamEvaluationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionEvaluations_ApplicantResponses_ApplicantResponseId",
                        column: x => x.ApplicantResponseId,
                        principalTable: "ApplicantResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionEvaluations_ExamEvaluation_ExamEvaluationId",
                        column: x => x.ExamEvaluationId,
                        principalTable: "ExamEvaluation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionNumber = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    ApplicantResponseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_ApplicantResponses_ApplicantResponseId",
                        column: x => x.ApplicantResponseId,
                        principalTable: "ApplicantResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Questions_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "varchar(100)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId1",
                        column: x => x.QuestionId1,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId_IsCorrect",
                table: "Answers",
                columns: new[] { "QuestionId", "IsCorrect" });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId1",
                table: "Answers",
                column: "QuestionId1");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantResponses_ExamSummaryId",
                table: "ApplicantResponses",
                column: "ExamSummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantResponses_ExamSummaryId_QuestionId",
                table: "ApplicantResponses",
                columns: new[] { "ExamSummaryId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CVId",
                table: "Applicants",
                column: "CVId",
                unique: true,
                filter: "[CVId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_Email",
                table: "Applicants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_JobOpeningId",
                table: "Applicants",
                column: "JobOpeningId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantSkills_ApplicantId",
                table: "ApplicantSkills",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantSkills_SkillId",
                table: "ApplicantSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicantId",
                table: "Applications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicantId_JobId",
                table: "Applications",
                columns: new[] { "ApplicantId", "JobId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationStatus",
                table: "Applications",
                column: "ApplicationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_HRId",
                table: "Applications",
                column: "HRId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobId",
                table: "Applications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicantId",
                table: "AspNetUsers",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HRId",
                table: "AspNetUsers",
                column: "HRId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CVs_ApplicantId",
                table: "CVs",
                column: "ApplicantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamEvaluation_ExamSummaryId",
                table: "ExamEvaluation",
                column: "ExamSummaryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamEvaluation_JobId",
                table: "ExamEvaluation",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamEvaluation_Status",
                table: "ExamEvaluation",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicantId",
                table: "Exams",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicantId_CreatedAt",
                table: "Exams",
                columns: new[] { "ApplicantId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicationId",
                table: "Exams",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamSummarys_ApplicantId",
                table: "ExamSummarys",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSummarys_ApplicationId",
                table: "ExamSummarys",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamSummarys_CreatedAt",
                table: "ExamSummarys",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSummarys_ExamEvaluationId",
                table: "ExamSummarys",
                column: "ExamEvaluationId",
                unique: true,
                filter: "[ExamEvaluationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSummarys_ExamId",
                table: "ExamSummarys",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_HRs_AccountType",
                table: "HRs",
                column: "AccountType");

            migrationBuilder.CreateIndex(
                name: "IX_HRs_CompanyName",
                table: "HRs",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_HRs_Email",
                table: "HRs",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOpenings_HRId",
                table: "JobOpenings",
                column: "HRId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_JobId",
                table: "JobSkills",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_JobId_SkillId",
                table: "JobSkills",
                columns: new[] { "JobId", "SkillId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_SkillId",
                table: "JobSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_HrId_CreatedAt",
                table: "Payments",
                columns: new[] { "HrId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentIntentId",
                table: "Payments",
                column: "PaymentIntentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionEvaluations_ApplicantResponseId",
                table: "QuestionEvaluations",
                column: "ApplicantResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionEvaluations_ExamEvaluationId_ApplicantResponseId",
                table: "QuestionEvaluations",
                columns: new[] { "ExamEvaluationId", "ApplicantResponseId" });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ApplicantResponseId",
                table: "Questions",
                column: "ApplicantResponseId",
                unique: true,
                filter: "[ApplicantResponseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExamId",
                table: "Questions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExamId_QuestionNumber",
                table: "Questions",
                columns: new[] { "ExamId", "QuestionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Siklls_Title",
                table: "Siklls",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_UserId",
                table: "UserRefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "ApplicantSkills");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "JobSkills");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "QuestionEvaluations");

            migrationBuilder.DropTable(
                name: "UserRefreshTokens");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Siklls");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ApplicantResponses");

            migrationBuilder.DropTable(
                name: "ExamSummarys");

            migrationBuilder.DropTable(
                name: "ExamEvaluation");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "CVs");

            migrationBuilder.DropTable(
                name: "JobOpenings");

            migrationBuilder.DropTable(
                name: "HRs");

            migrationBuilder.DropSequence(
                name: "UserSequence");
        }
    }
}
