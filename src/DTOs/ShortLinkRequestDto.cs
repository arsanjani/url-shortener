using System.ComponentModel.DataAnnotations;

namespace ScissorLink.DTOs
{
    public class ShortLinkRequestDto
    {
        [StringLength(500)]
        public string? Title { get; set; }
        
        public string? Token { get; set; }
        
        [Required]
        [Url]
        public string OriginLink { get; set; } = string.Empty;
        
        public bool IsPublish { get; set; } = true;
    }
}
