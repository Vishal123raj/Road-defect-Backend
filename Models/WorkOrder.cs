namespace RoadDefect.Api.Models;

public enum WorkOrderStatus
{
    Open = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}

public enum WorkOrderPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}

public class WorkOrder
{
    public int Id { get; set; }
    public int DefectId { get; set; }
    public Defect? Defect { get; set; }

    public int CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public int AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }

    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Open;
    public WorkOrderPriority Priority { get; set; } = WorkOrderPriority.Medium;

    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<WorkOrderUpdate> Updates { get; set; } = new List<WorkOrderUpdate>();
}
