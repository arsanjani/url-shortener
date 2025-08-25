using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScissorLink.Models
{
    [Table("ShortLinkDetail")]
    public class DtoShortLinkDetail
    {
        [Key]
        public long ID { get; set; }
        
        public int ShortLinkID { get; set; }
        
        public DateTime VisitDate { get; set; } = DateTime.Now;
        
        [StringLength(50)]
        public string? Country { get; set; }
        
        [StringLength(50)]
        public string? OS { get; set; }
        
        [StringLength(50)]
        public string? Browser { get; set; }

        // Navigation property
        [ForeignKey("ShortLinkID")]
        public virtual DtoShortLink? ShortLink { get; set; }
    }
}
