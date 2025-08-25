using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScissorLink.UserAgent
{
    public class MatchExpression
    {
        public List<Regex> Regexes { get; set; } = new();

        public Action<Match, object> Action { get; set; } = null!;
    }
}
