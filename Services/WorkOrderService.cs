using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadDefect.Api.Data;
using RoadDefect.Api.DTOs.WorkOrders;
using RoadDefect.Api.Models;

namespace RoadDefect.Api.Services;

public class WorkOrderService : IWorkOrderService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkOrderService(
        ApplicationDbContext db,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _db = db;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    // 🔥 Get current logged-in user ID from JWT
    private int GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        var userIdClaim =
            user?.FindFirst("id")?.Value // optional
            ?? user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            throw new Exception("User not authenticated");

        return int.Parse(userIdClaim);
    }

    // -----------------------------------------------------
    // GET WORK ORDERS BY ENGINEER
    // -----------------------------------------------------
    public async Task<List<WorkOrderDetailsDto>> GetByEngineerAsync(int engineerId)
    {
        return await _db.WorkOrders
            .Include(w => w.Defect)
            .Include(w => w.AssignedToUser)
            .Where(w => w.AssignedToUserId == engineerId)
            .Select(w => new WorkOrderDetailsDto
            {
                Id = w.Id,
                DefectTitle = w.Defect.Title,
                AssignedEngineer = w.AssignedToUser.Name,
                Status = w.Status.ToString(),
                Priority = w.Priority.ToString(),
                DueDate = w.DueDate
            })
            .ToListAsync();
    }

    // -----------------------------------------------------
    // ADD COMMENT / UPDATE
    // -----------------------------------------------------
    public async Task<bool> AddUpdateAsync(int workOrderId, WorkOrderUpdateDto dto)
    {
        var workOrder = await _db.WorkOrders.FindAsync(workOrderId);
        if (workOrder == null) return false;

        var userId = GetCurrentUserId();

        var update = new WorkOrderUpdate
        {
            WorkOrderId = workOrderId,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow,
            Status = workOrder.Status,
            UpdatedByUserId = userId // ✅ FIXED
        };

        _db.WorkOrderUpdates.Add(update);
        await _db.SaveChangesAsync();

        return true;
    }

    // -----------------------------------------------------
    // GET UPDATES
    // -----------------------------------------------------
    public async Task<List<string>> GetUpdatesAsync(int workOrderId)
    {
        return await _db.WorkOrderUpdates
            .Where(u => u.WorkOrderId == workOrderId)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => $"{u.CreatedAt:g} - {u.Comment}")
            .ToListAsync();
    }

    // -----------------------------------------------------
    // GET ALL WORK ORDERS
    // -----------------------------------------------------
    public async Task<List<WorkOrderDetailsDto>> GetAllAsync()
    {
        var orders = await _db.WorkOrders
            .Include(o => o.Defect)
            .Include(o => o.AssignedToUser)
            .Include(o => o.Updates)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<WorkOrderDetailsDto>>(orders);
    }

    // -----------------------------------------------------
    // GET BY ID
    // -----------------------------------------------------
    public async Task<WorkOrderDetailsDto?> GetByIdAsync(int id)
    {
        var order = await _db.WorkOrders
            .Include(o => o.Defect)
            .Include(o => o.AssignedToUser)
            .Include(o => o.Updates)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order == null ? null : _mapper.Map<WorkOrderDetailsDto>(order);
    }

    // -----------------------------------------------------
    // CREATE WORK ORDER (🔥 MAIN FIX)
    // -----------------------------------------------------
    public async Task<WorkOrderDetailsDto> CreateAsync(WorkOrderCreateDto dto)
    {
        var userId = GetCurrentUserId();

        var order = new WorkOrder
        {
            DefectId = dto.DefectId,
            AssignedToUserId = dto.AssignedToUserId,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            CreatedAt = DateTime.UtcNow,
            Status = WorkOrderStatus.Open,
            CreatedByUserId = userId // ✅ CRITICAL FIX
        };

        _db.WorkOrders.Add(order);

        // Update defect
        var defect = await _db.Defects.FindAsync(dto.DefectId);
        if (defect != null)
        {
            defect.AssignedEngineerId = dto.AssignedToUserId;
            defect.Status = DefectStatus.InProgress;
        }

        await _db.SaveChangesAsync();

        await _db.Entry(order).Reference(o => o.Defect).LoadAsync();
        await _db.Entry(order).Reference(o => o.AssignedToUser).LoadAsync();

        return _mapper.Map<WorkOrderDetailsDto>(order);
    }

    // -----------------------------------------------------
    // UPDATE STATUS
    // -----------------------------------------------------
    public async Task<bool> UpdateStatusAsync(int id, WorkOrderUpdateStatusDto dto)
    {
        var order = await _db.WorkOrders
            .Include(o => o.Defect)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return false;

        var userId = GetCurrentUserId();

        order.Status = dto.Status;
        order.UpdatedAt = DateTime.UtcNow;

        _db.WorkOrderUpdates.Add(new WorkOrderUpdate
        {
            WorkOrderId = id,
            UpdatedByUserId = userId, // ✅ FIXED
            Comment = dto.Comment,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow
        });

        // Sync defect status
        if (dto.Status == WorkOrderStatus.Completed)
            order.Defect!.Status = DefectStatus.Resolved;

        await _db.SaveChangesAsync();
        return true;
    }

   
}