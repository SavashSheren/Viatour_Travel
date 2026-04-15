namespace Viatour_Travel.Dtos.AdminSearchDtos
{
    public class ResultAdminSearchDto
    {
        public string Query { get; set; } = string.Empty;
        public List<AdminSearchItemDto> Tours { get; set; } = new();
        public List<AdminSearchItemDto> Categories { get; set; } = new();
        public List<AdminSearchItemDto> Reservations { get; set; } = new();
        public List<AdminSearchItemDto> Reviews { get; set; } = new();
    }

    public class AdminSearchItemDto
    {
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusText { get; set; } = string.Empty;
        public string ResultUrl { get; set; } = string.Empty;
    }
}