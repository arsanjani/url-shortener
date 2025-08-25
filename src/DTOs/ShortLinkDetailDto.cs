namespace ScissorLink.DTOs
{
    public class ShortLinkDetailDto
    {
        public long ID { get; set; }
        public int ShortLinkID { get; set; }
        public DateTime VisitDate { get; set; }
        public string? Country { get; set; }
        public string? OS { get; set; }
        public string? Browser { get; set; }
    }
}
