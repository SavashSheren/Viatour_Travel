using Microsoft.AspNetCore.Http;

namespace Viatour_Travel.Dtos.TourImageDtos
{
    public class UploadTourImageDto
    {
        public string TourId { get; set; } = null!;
        public List<IFormFile> Images { get; set; } = new();
    }
}