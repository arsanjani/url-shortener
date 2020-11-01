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
        public Int64 ID { get; set; }
        public int ShortLinkID { get; set; }
        public String Country { get; set; }
        public String OS { get; set; }
        public String Browser { get; set; }


    }
}
