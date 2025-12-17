namespace HireAI.Data.Helpers.DTOs.ApplicantSkill.Response
{
    /// <summary>
    /// DTO used to return minimal ApplicantSkill data in list views.
    /// Contains essential information only.
    /// </summary>
    public class ApplicantSkillMinimalResponseDto
    {
        public int Id { get; set; }

        public string SkillTitle { get; set; } = default!;

        public float? SkillRate { get; set; }

        public float? ImprovementPercentage { get; set; }
    }
}