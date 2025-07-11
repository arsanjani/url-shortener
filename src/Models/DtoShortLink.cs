using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace akhr.ir.Models
{
    [Table("ShortLink")]
    public class DtoShortLink
    {
        public int ID { get; set; }
        
        [Key]
        public string Token { get; set; } = string.Empty;
        
        public string OriginLink { get; set; } = string.Empty;
        
        public bool IsPublish { get; set; }
    }
}
