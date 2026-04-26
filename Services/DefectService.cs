using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadDefect.Api.Data;
using RoadDefect.Api.DTOs.Defects;
using RoadDefect.Api.Models;

namespace RoadDefect.Api.Services;

public class DefectService : IDefectService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public DefectService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<DefectListDto>> GetAllAsync(
        DefectStatus? status,
        int? areaId,
        int? assignedEngineerId)
    {
        var query = _db.Defects
            .Include(d => d.Area)
            .Include(d => d.RoadSegment)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(d => d.Status == status.Value);

        if (areaId.HasValue)
            query = query.Where(d => d.AreaId == areaId.Value);

        if (assignedEngineerId.HasValue)
            query = query.Where(d => d.AssignedEngineerId == assignedEngineerId.Value);

        var defects = await query
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<DefectListDto>>(defects);
    }

    public async Task<DefectDetailsDto?> GetByIdAsync(int id)
    {
        var defect = await _db.Defects
            .Include(d => d.Area)
            .Include(d => d.RoadSegment)
            .Include(d => d.Images)
            .FirstOrDefaultAsync(d => d.Id == id);

        return defect == null
            ? null
            : _mapper.Map<DefectDetailsDto>(defect);
    }

    public async Task<DefectDetailsDto> CreateAsync(DefectCreateDto dto)
    {
        var defect = _mapper.Map<Defect>(dto);

        defect.Status = DefectStatus.New;
        defect.Severity = DefectSeverity.Medium;
        defect.CreatedAt = DateTime.UtcNow;

        _db.Defects.Add(defect);
        await _db.SaveChangesAsync();

        await _db.Entry(defect).Reference(d => d.Area).LoadAsync();

        return _mapper.Map<DefectDetailsDto>(defect);
    }

    public async Task<bool> UpdateAsync(int id, DefectUpdateDto dto)
    {
        var defect = await _db.Defects.FindAsync(id);
        if (defect == null)
            return false;

        _mapper.Map(dto, defect);
        defect.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var defect = await _db.Defects.FindAsync(id);
        if (defect == null)
            return false;

        _db.Defects.Remove(defect);
        await _db.SaveChangesAsync();

        return true;
    }
}
