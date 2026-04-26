using RoadDefect.Api.DTOs.Defects;
using RoadDefect.Api.Models;

namespace RoadDefect.Api.Services;

public interface IDefectService
{
    Task<List<DefectListDto>> GetAllAsync(
        DefectStatus? status,
        int? areaId,
        int? assignedEngineerId);

    Task<DefectDetailsDto?> GetByIdAsync(int id);

    Task<DefectDetailsDto> CreateAsync(DefectCreateDto dto);

    Task<bool> UpdateAsync(int id, DefectUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}
