using RoadDefect.Api.Models;

namespace RoadDefect.Api.DTOs.Defects;

public class DefectCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int AreaId { get; set; }
    public int? RoadSegmentId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class DefectUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DefectType DefectType { get; set; }
    public DefectSeverity Severity { get; set; }
    public DefectStatus Status { get; set; }
    public int? AssignedEngineerId { get; set; }
    public int? RoadSegmentId { get; set; }
}

public class DefectListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class DefectDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Area { get; set; } = string.Empty;
    public string? RoadSegment { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<string> Images { get; set; } = new();
}
