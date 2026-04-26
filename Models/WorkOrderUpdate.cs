namespace RoadDefect.Api.Models;

public class WorkOrderUpdate
{
    public int Id { get; set; }
    public int WorkOrderId { get; set; }
    public WorkOrder? WorkOrder { get; set; }

    public int UpdatedByUserId { get; set; }
    public User? UpdatedByUser { get; set; }

    public WorkOrderStatus Status { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
