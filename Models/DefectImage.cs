namespace RoadDefect.Api.Models;

public class DefectImage
{
    public int Id { get; set; }
    public int DefectId { get; set; }
    public Defect? Defect { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    public bool IsBeforeRepair { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
