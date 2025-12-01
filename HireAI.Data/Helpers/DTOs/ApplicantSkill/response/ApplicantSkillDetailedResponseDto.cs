namespace HireAI.Data.Helpers.DTOs.ApplicantSkill.Response
{
    /// <summary>
    /// DTO used to return detailed ApplicantSkill data including related skill information.
    /// Contains all applicant skill information plus skill details.
    /// </summary>
    public class ApplicantSkillDetailedResponseDto
    {
        public int Id { get; set; }

        public int ApplicantId { get; set; }

        public int SkillId { get; set; }

        public string SkillTitle { get; set; } = default!;

        public string? SkillDescription { get; set; }

        public float? SkillRate { get; set; }

        public float? ImprovementPercentage { get; set; }

        public string? Notes { get; set; }
    }
}