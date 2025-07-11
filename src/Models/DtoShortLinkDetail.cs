using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace akhr.ir.Models
{
    [Table("ShortLinkDetail")]
    public class DtoShortLinkDetail
    {
        [Key]
        public long ID { get; set; }
        public int ShortLinkID { get; set; }
        public string? Country { get; set; }
        public string? OS { get; set; }
        public string? Browser { get; set; }
    }
}
