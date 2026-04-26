namespace RoadDefect.Api.Models;

public enum DefectType
{
    Unknown = 0,
    Pothole = 1,
    Crack = 2,
    FadedMarking = 3,
    Waterlogging = 4,
    BrokenDivider = 5,
    Other = 6
}

public enum DefectSeverity
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

public enum DefectStatus
{
    New = 0,
    Verified = 1,
    InProgress = 2,
    Resolved = 3,
    Rejected = 4
}

public class Defect
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DefectType DefectType { get; set; } = DefectType.Unknown;
    public DefectSeverity Severity { get; set; } = DefectSeverity.Medium;
    public DefectStatus Status { get; set; } = DefectStatus.New;

    public int? ReportedByUserId { get; set; }
    public User? ReportedByUser { get; set; }

    public int? AssignedEngineerId { get; set; }
    public User? AssignedEngineer { get; set; }

    public int AreaId { get; set; }
    public Area? Area { get; set; }

    public int? RoadSegmentId { get; set; }
    public RoadSegment? RoadSegment { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public string Source { get; set; } = "CitizenApp";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public ICollection<DefectImage> Images { get; set; } = new List<DefectImage>();
    public WorkOrder? WorkOrder { get; set; }
}
