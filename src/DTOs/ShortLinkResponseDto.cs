namespace ScissorLink.DTOs
{
    public class ShortLinkResponseDto
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string Token { get; set; } = string.Empty;
        public string OriginLink { get; set; } = string.Empty;
        public bool IsPublish { get; set; }
        public DateTime CreateAdminDate { get; set; }
        public DateTime? EditAdminDate { get; set; }
        public int ClickCount { get; set; }
        public List<ShortLinkDetailDto> RecentClicks { get; set; } = new List<ShortLinkDetailDto>();
    }
}
