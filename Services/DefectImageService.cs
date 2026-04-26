using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RoadDefect.Api.Data;
using RoadDefect.Api.DTOs.Defects;
using RoadDefect.Api.Models;

namespace RoadDefect.Api.Services;

public class DefectImageService : IDefectImageService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public DefectImageService(ApplicationDbContext db, IMapper mapper, IWebHostEnvironment env)
    {
        _db = db;
        _mapper = mapper;
        _env = env;
    }

    public async Task<DefectDetailsDto?> AddImageAsync(int defectId, IFormFile file, bool isBeforeRepair)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty.", nameof(file));

        var defect = await _db.Defects
            .Include(d => d.Area)
            .Include(d => d.RoadSegment)
            .Include(d => d.Images)
            .FirstOrDefaultAsync(d => d.Id == defectId);

        if (defect == null)
            return null;

        // Ensure upload folder exists
        var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsFolder = Path.Combine(webRoot, "uploads", "defects");
        Directory.CreateDirectory(uploadsFolder);

        var ext = Path.GetExtension(file.FileName);
        var fileName = $"defect_{defectId}_{Guid.NewGuid():N}{ext}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // URL that frontend can use (served from wwwroot)
        var relativeUrl = $"/uploads/defects/{fileName}".Replace("\\", "/");

        var img = new DefectImage
        {
            DefectId = defectId,
            ImageUrl = relativeUrl,
            IsBeforeRepair = isBeforeRepair,
            CreatedAt = DateTime.UtcNow
        };

        _db.DefectImages.Add(img);
        await _db.SaveChangesAsync();

        // Reload images so DTO has latest data
        await _db.Entry(defect).Collection(d => d.Images).LoadAsync();

        return _mapper.Map<DefectDetailsDto>(defect);
    }
}
