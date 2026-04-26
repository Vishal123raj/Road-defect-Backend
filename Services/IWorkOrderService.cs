using RoadDefect.Api.DTOs.WorkOrders;

namespace RoadDefect.Api.Services;

public interface IWorkOrderService
{
    Task<WorkOrderDetailsDto?> GetByIdAsync(int id);
    Task<List<WorkOrderDetailsDto>> GetAllAsync();
    Task<WorkOrderDetailsDto> CreateAsync(WorkOrderCreateDto dto);
    Task<bool> UpdateStatusAsync(int id, WorkOrderUpdateStatusDto dto);
    Task<List<WorkOrderDetailsDto>> GetByEngineerAsync(int engineerId);
    Task<bool> AddUpdateAsync(int workOrderId, WorkOrderUpdateDto dto);
    Task<List<string>> GetUpdatesAsync(int workOrderId);

}
