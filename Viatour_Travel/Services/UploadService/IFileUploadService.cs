namespace Viatour_Travel.Services.UploadService
{
    public interface IFileUploadService
    {
        Task<string?> SaveImageAsync(IFormFile? file, string folderName);
    }
}
