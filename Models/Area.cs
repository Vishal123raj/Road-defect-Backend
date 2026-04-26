namespace RoadDefect.Api.Models;

public class Area
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentAreaId { get; set; }
    public Area? ParentArea { get; set; }
    public string? GeoJson { get; set; }

    public ICollection<RoadSegment> RoadSegments { get; set; } = new List<RoadSegment>();
    public ICollection<Defect> Defects { get; set; } = new List<Defect>();
}
