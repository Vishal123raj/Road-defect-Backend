using Microsoft.AspNetCore.Http;
using RoadDefect.Api.DTOs.Defects;

namespace RoadDefect.Api.Services;

public interface IDefectImageService
{
    Task<DefectDetailsDto?> AddImageAsync(int defectId, IFormFile file, bool isBeforeRepair);
}
