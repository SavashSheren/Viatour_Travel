using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Viatour_Travel.Services.UploadService;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _environment;

    public FileUploadService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string?> SaveImageAsync(IFormFile? file, string folderName)
    {
        if (file == null || file.Length == 0)
            return null;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new Exception("Only .jpg, .jpeg, .png and .webp files are allowed.");

        var uploadsRoot = Path.Combine(_environment.WebRootPath, "uploads", folderName);

        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsRoot, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/{folderName}/{fileName}";
    }
}