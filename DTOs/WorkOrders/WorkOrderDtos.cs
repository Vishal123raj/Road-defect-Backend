using RoadDefect.Api.Models;

namespace RoadDefect.Api.DTOs.WorkOrders;

public class WorkOrderCreateDto
{
    public int DefectId { get; set; }
    public int AssignedToUserId { get; set; }
    public WorkOrderPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
}

public class WorkOrderUpdateStatusDto
{
    public WorkOrderStatus Status { get; set; }
    public string? Comment { get; set; }
}

public class WorkOrderDetailsDto
{
    public int Id { get; set; }
    public string DefectTitle { get; set; } = string.Empty;
    public string AssignedEngineer { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<string> Updates { get; set; } = new();
    public DateTime? DueDate { get; set; }
}
