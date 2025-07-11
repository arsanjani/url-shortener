using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace akhr.ir.UserAgent
{
    public class ClientOS
    {
        private static readonly Dictionary<string, string> VersionMap = new()
        {
            { "4.90", "ME" },
            { "NT3.51", "NT 3.11" },
            { "NT4.0", "NT 4.0" },
            { "NT 5.0", "2000" },
            { "NT 5.1", "XP" },
            { "NT 5.2", "XP" },
            { "NT 6.0", "Vista" },
            { "NT 6.1", "7" },
            { "NT 6.2", "8" },
            { "NT 6.3", "8.1" },
            { "NT 6.4", "10" },
            { "NT 10.0", "10" },
            { "ARM", "RT" }
        };

        public ClientOS(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return;
            }

            foreach (var matchItem in Matches)
            {
                foreach (var regexItem in matchItem.Regexes)
                {
                    if (regexItem.IsMatch(userAgent))
                    {
                        var match = regexItem.Match(userAgent);
                        matchItem.Action(match, this);
                        return;
                    }
                }
            }
        }

        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;

        private static void NameVersionAction(Match match, object obj)
        {
            if (obj is not ClientOS current) return;

            current.Name = new Regex(@"^[a-zA-Z]+", RegexOptions.IgnoreCase).Match(match.Value).Value;
            if (match.Value.Length > current.Name.Length)
            {
                current.Version = match.Value.Substring(current.Name.Length + 1);
            }
        }

        private static readonly List<MatchExpression> Matches = new()
        {
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"microsoft\s(windows)\s(vista|xp)", RegexOptions.IgnoreCase),
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(windows)\snt\s6\.2;\s(arm)", RegexOptions.IgnoreCase),
                    new(@"(windows\sphone(?:\sos)*)[\s\/]?([\d\.\s]+\w)*", RegexOptions.IgnoreCase),
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;

                    current.Name = new Regex(@"(^[a-zA-Z]+\s[a-zA-Z]+)", RegexOptions.IgnoreCase).Match(match.Value).Value;

                    if (current.Name.Length < match.Value.Length)
                    {
                        var version = match.Value.Substring(current.Name.Length + 1);
                        current.Version = VersionMap.ContainsKey(version) ? VersionMap[version] : version;
                    }
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(windows\smobile|windows)[\s\/]?([ntce\d\.\s]+\w)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;

                    current.Name = new Regex(@"(^[a-zA-Z]+)", RegexOptions.IgnoreCase).Match(match.Value).Value;

                    if (current.Name.Length < match.Value.Length)
                    {
                        var version = match.Value.Substring(current.Name.Length + 1);
                        current.Version = VersionMap.ContainsKey(version) ? VersionMap[version] : version;
                    }
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(win(?=3|9|n)|win\s9x\s)([nt\d\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;

                    var spaceIndex = match.Value.IndexOf(" ");
                    if (spaceIndex == -1) return;

                    var nameAndVersion = new string[] { match.Value.Substring(0, spaceIndex), match.Value.Substring(spaceIndex + 1) };
                    current.Name = "Windows";
                    var version = nameAndVersion[1];
                    current.Version = VersionMap.ContainsKey(version) ? VersionMap[version] : version;
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"\((bb)(10);", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;
                    current.Name = "BlackBerry";
                    current.Version = "BB10";
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(blackberry)\w*\/?([\w\.]+)*", RegexOptions.IgnoreCase),
                    new(@"(tizen)[\/\s]([\w\.]+)", RegexOptions.IgnoreCase),
                    new(@"(android|webos|palm\sos|qnx|bada|rim\stablet\sos|meego|contiki)[\/\s-]?([\w\.]+)*", RegexOptions.IgnoreCase),
                    new(@"linux;.+(sailfish);", RegexOptions.IgnoreCase)
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(symbian\s?os|symbos|s60(?=;))[\/\s-]?([\w\.]+)*", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;

                    var spaceIndex = match.Value.IndexOf(" ");
                    if (spaceIndex == -1) return;

                    var nameAndVersion = new string[] { match.Value.Substring(0, spaceIndex), match.Value.Substring(spaceIndex + 1) };
                    current.Name = "Symbian";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"\((series40);", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;
                    current.Name = match.Value;
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"mozilla.+\(mobile;.+gecko.+firefox", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;

                    var spaceIndex = match.Value.IndexOf(" ");
                    if (spaceIndex == -1) return;

                    var nameAndVersion = new string[] { match.Value.Substring(0, spaceIndex), match.Value.Substring(spaceIndex + 1) };
                    current.Name = "Firefox OS";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(nintendo|playstation)\s([wids34portablevu]+)", RegexOptions.IgnoreCase),
                    new(@"(mint)[\/\s\(]?(\w+)*", RegexOptions.IgnoreCase),
                    new(@"(mageia|vectorlinux)[;\s]", RegexOptions.IgnoreCase),
                    new(@"(joli|[kxln]?ubuntu|debian|[open]*suse|gentoo|(?=\s)arch|slackware|fedora|mandriva|centos|pclinuxos|redhat|zenwalk|linpus)[\/\s-]?(?!chrom)([\w\.-]+)*", RegexOptions.IgnoreCase),
                    new(@"(gnu)\s?([\w\.]+)*", RegexOptions.IgnoreCase)
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(hurd|linux)\s?([\w\.]+)*", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;
                    
                    current.Name = new Regex(@"^[a-zA-Z]+", RegexOptions.IgnoreCase).Match(match.Value).Value;
                    // Extract architecture (e.g., "x86_64") from the match
                    var archMatch = Regex.Match(match.Value, @"linux\s+([\w\.]+)", RegexOptions.IgnoreCase);
                    if (archMatch.Success)
                    {
                        current.Version = archMatch.Groups[1].Value;
                    }
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(cros)\s[\w]+\s([\w\.]+\w)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;

                    current.Name = "CrOS";
                    
                    // Extract the architecture part (e.g., "x86_64") from the match
                    var architectureMatch = Regex.Match(match.Value, @"cros\s+(\w+)", RegexOptions.IgnoreCase);
                    if (architectureMatch.Success)
                    {
                        current.Version = architectureMatch.Groups[1].Value;
                    }
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(sunos)\s?([\w\.]+\d)*", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;

                    var spaceIndex = match.Value.IndexOf(" ");
                    if (spaceIndex == -1) return;

                    var nameAndVersion = new string[] { match.Value.Substring(0, spaceIndex), match.Value.Substring(spaceIndex + 1) };
                    current.Name = "Solaris";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"\s([frentopc-]{0,4}bsd|dragonfly)\s?([\w\.]+)*", RegexOptions.IgnoreCase),
                    new(@"(haiku)\s(\w+)", RegexOptions.IgnoreCase)
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(ip[honead]+)(?:.*os\s([\w]+)*\slike\smac|;\sopera)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;

                    var spaceIndex = match.Value.IndexOf(" ");
                    if (spaceIndex == -1) return;

                    var nameAndVersion = new string[] { match.Value.Substring(0, spaceIndex), match.Value.Substring(spaceIndex + 1) };
                    current.Name = "iOS";
                    current.Version = new Regex(@"\d+(?:\.\d+)*").Match(nameAndVersion[1].Replace("_", ".")).Value;
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(mac\sos\sx)\s?([\w\s\.]+\w)*", RegexOptions.IgnoreCase),
                    new(@"(macintosh|mac(?=_powerpc)\s)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientOS current) return;

                    current.Name = "Mac OS";
                    
                    // Extract version from the match using regex
                    var versionMatch = Regex.Match(match.Value, @"(\d+[_\.]\d+[_\.]\d+|\d+[_\.]\d+)", RegexOptions.IgnoreCase);
                    if (versionMatch.Success)
                    {
                        current.Version = versionMatch.Value;
                    }
                }
            }
        };
    }
}
