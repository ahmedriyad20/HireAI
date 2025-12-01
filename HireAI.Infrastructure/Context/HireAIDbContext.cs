using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Data.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace HireAI.Infrastructure.Context
{
    public class HireAIDbContext : IdentityDbContext<ApplicationUser>
    {
        public HireAIDbContext(DbContextOptions options): base(options)
        {
        }

        // DbSets for concrete entities
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        // New DbSets for payment system
        public DbSet<BillingInfo> BillingInfos { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

        public DbSet<Applicant> Applicants { get; set; } = default!;
        public DbSet<HR> HRs { get; set; } = default!;
        public DbSet<JobOpening> JobOpenings { get; set; } = default!;
        public DbSet<Application> Applications { get; set; } = default!;
        public DbSet<Payment> Payment { get; set; } = default!;
        public DbSet<CV> CVs { get; set; } = default!;
        public DbSet<Answer> Answers { get; set; } = default!;
        public DbSet<ApplicantSkill> ApplicantSkills { get; set; } = default!;
        public DbSet<ApplicantResponse> ApplicantResponses { get; set; } = default!;
        public DbSet<Exam> Exams { get; set; } = default!;
        public DbSet<ExamEvaluation> ExamEvaluations { get; set; } = default!;
        public DbSet<ExamSummary> ExamSummarys { get; set; } = default!;
        public DbSet<JobSkill> JobSkills { get; set; } = default!;
        public DbSet<QuestionEvaluation> QuestionEvaluations { get; set; } = default!;
        public DbSet<Question> Questions { get; set; } = default!;
        public DbSet<Skill> Siklls { get; set; } = default!;



        #region connections db
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source =GOHARY\\SQLEXPRESS; Initial Catalog = HireAIDb; Integrated Security = True; Encrypt = False",
                    b => b.MigrationsAssembly("HireAI.API")); 
            }
        }
        #endregion



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            


        // TPC (Table Per Concrete class) 

        modelBuilder.Entity<User>().UseTpcMappingStrategy();
     
            modelBuilder.Entity<HR>().ToTable("HRs");
            modelBuilder.Entity<Applicant>().ToTable("Applicants");

            // Apply configuration classes from this assembly (IEntityTypeConfiguration implementations)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HireAIDbContext).Assembly);

            
            base.OnModelCreating(modelBuilder);



            // Configure relationships
            modelBuilder.Entity<HR>()
                .HasMany(h => h.Payments)
                .WithOne(p => p.HR)
                .HasForeignKey(p => p.HrId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HR>()
                .HasOne(h => h.BillingInfo)
                .WithOne(b => b.HR)
                .HasForeignKey<BillingInfo>(b => b.HRId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Plan)
                .WithMany()
                .HasForeignKey(p => p.PlanId)
                .OnDelete(DeleteBehavior.Restrict);


            #region payment and subscription plan configuration
            // Configure decimal precision for Payment
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.PerJobPrice)
                .HasPrecision(18, 2);

            // Configure decimal precision for SubscriptionPlan
            modelBuilder.Entity<SubscriptionPlan>()
                .Property(p => p.MonthlyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SubscriptionPlan>()
                .Property(p => p.YearlyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SubscriptionPlan>()
                .Property(p => p.PerJobPrice)
                .HasPrecision(18, 2);

            // Configure enum defaults to avoid sentinel warnings
            modelBuilder.Entity<HR>()
            .Property(h => h.AccountType)
            .HasDefaultValue(Data.Helpers.Enums.enAccountType.Free)
            .HasConversion<int>()
            .HasSentinel(Data.Helpers.Enums.enAccountType.Free);

            modelBuilder.Entity<JobOpening>()
            .Property(j => j.JobStatus)
            .HasDefaultValue(Data.Helpers.Enums.enJobStatus.Draft)
            .HasConversion<int>()
            .HasSentinel(Data.Helpers.Enums.enJobStatus.Draft);

            // Configure relationships
            modelBuilder.Entity<HR>()
                .HasMany(h => h.Payments)
                .WithOne(p => p.HR)
                .HasForeignKey(p => p.HrId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HR>()
                .HasOne(h => h.BillingInfo)
                .WithOne(b => b.HR)
                .HasForeignKey<BillingInfo>(b => b.HRId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Plan)
                .WithMany()
                .HasForeignKey(p => p.PlanId)
                .OnDelete(DeleteBehavior.Restrict);





            //// Seed subscription plans
            //modelBuilder.Entity<SubscriptionPlan>().HasData(
            //    new SubscriptionPlan
            //    {
            //        Id = 1,
            //        Name = "Starter",
            //        Description = "Perfect for small businesses",
            //        MonthlyPrice = 99.00m,
            //        YearlyPrice = 950.00m, // ~20% savings
            //        PerJobPrice = 49.00m,
            //        AccountType = enAccountType.Premium,
            //        JobPostingsLimit = 10,
            //        ApplicantsLimit = 100,
            //        HasAIScreening = true,
            //        HasAdvancedAIFeatures = false,
            //        HasPrioritySupport = false,
            //        HasCustomBranding = false,
            //        StripeMonthlyPriceId = "price_starter_monthly",
            //        StripeYearlyPriceId = "price_starter_yearly",
            //        StripePerJobPriceId = "price_per_job",
            //        IsActive = true,
            //        DisplayOrder = 1
            //    },
            //    new SubscriptionPlan
            //    {
            //        Id = 2,
            //        Name = "Professional",
            //        Description = "For growing companies",
            //        MonthlyPrice = 299.00m,
            //        YearlyPrice = 2870.00m, // ~20% savings
            //        PerJobPrice = 49.00m,
            //        AccountType = enAccountType.Pro,
            //        JobPostingsLimit = -1, // Unlimited
            //        ApplicantsLimit = 500,
            //        HasAIScreening = true,
            //        HasAdvancedAIFeatures = true,
            //        HasPrioritySupport = true,
            //        HasCustomBranding = true,
            //        StripeMonthlyPriceId = "price_professional_monthly",
            //        StripeYearlyPriceId = "price_professional_yearly",
            //        StripePerJobPriceId = "price_per_job",
            //        IsActive = true,
            //        DisplayOrder = 2
            //    },
            //    new SubscriptionPlan
            //    {
            //        Id = 3,
            //        Name = "Enterprise",
            //        Description = "For large organizations",
            //        MonthlyPrice = 0, // Custom pricing
            //        YearlyPrice = 0, // Custom pricing
            //        PerJobPrice = 49.00m,
            //        AccountType = enAccountType.Pro,
            //        JobPostingsLimit = -1, // Unlimited
            //        ApplicantsLimit = -1, // Unlimited
            //        HasAIScreening = true,
            //        HasAdvancedAIFeatures = true,
            //        HasPrioritySupport = true,
            //        HasCustomBranding = true,
            //        HasDedicatedSupport = true,
            //        HasCustomIntegrations = true,
            //        StripeMonthlyPriceId = null, // Contact sales
            //        StripeYearlyPriceId = null, // Contact sales
            //        StripePerJobPriceId = "price_per_job",
            //        IsActive = true,
            //        DisplayOrder = 3
            //    }
            //);
            // Seed subscription plans
            modelBuilder.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan
                {
                    Id = 1,
                    Name = "Starter",
                    Description = "Perfect for small businesses",
                    MonthlyPrice = 99.00m,
                    YearlyPrice = 950.00m,
                    PerJobPrice = 49.00m,
                    AccountType = Data.Helpers.Enums.enAccountType.Premium,
                    JobPostingsLimit = 10,
                    ApplicantsLimit = 100,
                    HasAIScreening = true,
                    HasAdvancedAIFeatures = false,
                    HasPrioritySupport = false,
                    HasCustomBranding = false,
                    IsActive = true,
                    DisplayOrder = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new SubscriptionPlan
                {
                    Id = 2,
                    Name = "Professional",
                    Description = "For growing companies",
                    MonthlyPrice = 299.00m,
                    YearlyPrice = 2870.00m,
                    PerJobPrice = 49.00m,
                    AccountType = Data.Helpers.Enums.enAccountType.Pro,
                    JobPostingsLimit = -1,
                    ApplicantsLimit = 500,
                    HasAIScreening = true,
                    HasAdvancedAIFeatures = true,
                    HasPrioritySupport = true,
                    HasCustomBranding = true,
                    IsActive = true,
                    DisplayOrder = 2,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new SubscriptionPlan
                {
                    Id = 3,
                    Name = "Enterprise",
                    Description = "For large organizations",
                    MonthlyPrice = 0m,
                    YearlyPrice = 0m,
                    PerJobPrice = 49.00m,
                    AccountType = Data.Helpers.Enums.enAccountType.Pro,
                    JobPostingsLimit = -1,
                    ApplicantsLimit = -1,
                    HasAIScreening = true,
                    HasAdvancedAIFeatures = true,
                    HasPrioritySupport = true,
                    HasCustomBranding = true,
                    HasDedicatedSupport = true,
                    HasCustomIntegrations = true,
                    IsActive = true,
                    DisplayOrder = 3,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }


        #endregion
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer("Data Source =GOHARY\\SQLEXPRESS; Initial Catalog = HireAIDb; Integrated Security = True; Encrypt = False");
        //}
    }
}
