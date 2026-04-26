namespace RoadDefect.Api.Models;

public enum RoadFunctionalClass
{
    Local = 0,
    Collector = 1,
    Arterial = 2,
    Highway = 3
}

public enum TrafficImportance
{
    Low = 0,
    Medium = 1,
    High = 2
}

public class RoadSegment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AreaId { get; set; }
    public Area? Area { get; set; }

    public double StartLat { get; set; }
    public double StartLng { get; set; }
    public double EndLat { get; set; }
    public double EndLng { get; set; }

    public RoadFunctionalClass FunctionalClass { get; set; } = RoadFunctionalClass.Local;
    public TrafficImportance TrafficImportance { get; set; } = TrafficImportance.Medium;

    public ICollection<Defect> Defects { get; set; } = new List<Defect>();
}
