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
        public DbSet<Applicant> Applicants { get; set; } = default!;
        public DbSet<HR> HRs { get; set; } = default!;
        public DbSet<JobOpening> JobOpenings { get; set; } = default!;
        public DbSet<Application> Applications { get; set; } = default!;
        public DbSet<Payment> Payments { get; set; } = default!;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // TPC (Table Per Concrete class) 
     
            modelBuilder.Entity<User>().UseTpcMappingStrategy();
     
            modelBuilder.Entity<HR>().ToTable("HRs");
            modelBuilder.Entity<Applicant>().ToTable("Applicants");

            // Apply configuration classes from this assembly (IEntityTypeConfiguration implementations)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HireAIDbContext).Assembly);

            
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source =(localdb)\\MSSQLLocalDB; Initial Catalog = HireAIDb; Integrated Security = True; Encrypt = False");
        }
    }
}
