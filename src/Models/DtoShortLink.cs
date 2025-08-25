using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScissorLink.Models
{
    [Table("ShortLink")]
    public class DtoShortLink
    {
        [Key]
        public int ID { get; set; }
        
        [StringLength(500)]
        public string? Title { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Token { get; set; } = string.Empty;
        
        [Required]
        public string OriginLink { get; set; } = string.Empty;
        
        public bool IsPublish { get; set; }
        
        public int? EditAdminID { get; set; }
        
        public DateTime? EditAdminDate { get; set; }
        
        public int CreateAdminID { get; set; }
        
        public DateTime CreateAdminDate { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<DtoShortLinkDetail> Details { get; set; } = new List<DtoShortLinkDetail>();
    }
}
