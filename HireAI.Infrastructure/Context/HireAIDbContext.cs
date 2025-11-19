using HireAI.Data.Models;
using HireAI.Data.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HireAI.Infrastructure.Context
{
    public class HireAIDbContext : DbContext
    {
        public HireAIDbContext(DbContextOptions<HireAIDbContext> options) 
        {
        }

        // DbSets for concrete entities
        public DbSet<User> Users { get; set; } = default!;
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
        public DbSet<ExamEvaluation> ExamEvaluation { get; set; } = default!;
        public DbSet<ExamSummary> ExamSummarys { get; set; } = default!;
        public DbSet<JobSkill> JobSkills { get; set; } = default!;
        public DbSet<QuestionEvaluation> QuestionEvaluations { get; set; } = default!;
        public DbSet<Question> Questions { get; set; } = default!;
        public DbSet<Skill> Siklls { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TPC
            modelBuilder.Entity<HR>().ToTable("HRs");
            modelBuilder.Entity<Applicant>().ToTable("Applicant");

            // Apply configuration classes from this assembly (your IEntityTypeConfiguration implementations)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HireAIDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HireAIDb;Trusted_Connection=True;TrustServerCertificate=True;\"\r\n");

        }
    }
}
