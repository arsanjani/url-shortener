using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace akhr.ir.UserAgent
{
    public class ClientBrowser
    {
        private static readonly Dictionary<string, string> VersionMap = new()
        {
            { "/8", "1.0" },
            { "/1", "1.2" },
            { "/3", "1.3" },
            { "/412", "2.0" },
            { "/416", "2.0.2" },
            { "/417", "2.0.3" },
            { "/419", "2.0.4" },
            { "?", "/" }
        };

        public ClientBrowser(string userAgent)
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
                        Major = new Regex(@"\d*").Match(Version).Value;
                        return;
                    }
                }
            }
        }

        public string Major { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;

        private static void NameVersionAction(Match match, object obj)
        {
            if (obj is not ClientBrowser current) return;

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
                    new(@"(opera\smini)\/([\w\.-]+)", RegexOptions.IgnoreCase),
                    new(@"(opera\s[mobiletab]+).+version\/([\w\.-]+)", RegexOptions.IgnoreCase),
                    new(@"(opera).+version\/([\w\.]+)", RegexOptions.IgnoreCase),
                    new(@"(opera)[\/\s]+([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(opios)[\/\s]+([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "Opera Mini";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"\s(opr)\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "Opera";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"compatible;\s*MSIE\s*([^)]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    current.Name = "MSIE";
                    // Extract everything after "MSIE " - the whole capture group
                    current.Version = match.Groups[1].Value;
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(kindle)\/([\w\.]+)", RegexOptions.IgnoreCase),
                    new(@"(lunascape|maxthon|netfront|jasmine|blazer)[\/\s]?([\w\.]+)*", RegexOptions.IgnoreCase),
                    new(@"(avant\s|iemobile|slim|baidu)(?:browser)?[\/\s]?([\w\.]*)", RegexOptions.IgnoreCase),
                    new(@"(rekonq)\/([\w\.]+)*", RegexOptions.IgnoreCase),
                    new(@"(chromium|flock|rockmelt|midori|epiphany|silk|skyfire|ovibrowser|bolt|iron|vivaldi|iridium|phantomjs)\/([\w\.-]+)", RegexOptions.IgnoreCase),
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(?:ms|\()(ie)\s([\w\.]+)", RegexOptions.IgnoreCase),
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(trident).+rv[:\s]([\w\.]+).+like\sgecko", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    current.Name = "IE";
                    current.Version = "11";
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(edge)\/((\d+)?[\w\.]+)", RegexOptions.IgnoreCase),
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(yabrowser)\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "Yandex";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(comodo_dragon)\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = nameAndVersion[0].Replace('_', ' ');
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(micromessenger)\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "WeChat";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"xiaomi\/miuibrowser\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "MIUI Browser";
                    current.Version = nameAndVersion[0];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"\swv\).+(chrome)\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = new Regex("(.+)").Replace(nameAndVersion[0], "$1 WebView");
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"android.+samsungbrowser\/([\w\.]+)", RegexOptions.IgnoreCase),
                    new(@"android.+version\/([\w\.]+)\s+(?:mobile\s?safari|safari)*", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "Android Browser";
                    current.Version = nameAndVersion[0];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(chrome|omniweb|arora|[tizenoka]{5}\s?browser)\/v?([\w\.]+)", RegexOptions.IgnoreCase),
                    new(@"(qqbrowser)[\/\s]?([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(uc\s?browser)[\/\s]?([\w\.]+)", RegexOptions.IgnoreCase),
                    new(@"ucweb.+(ucbrowser)[\/\s]?([\w\.]+)", RegexOptions.IgnoreCase),
                    new(@"juc.+(ucweb)[\/\s]?([\w\.]+)", RegexOptions.IgnoreCase),
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "UC Browser";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(dolfin)\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "Dolphin";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"((?:android.+)crmo|crios)\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "Chrome";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"version\/([\w\.]+).+?mobile\/\w+\s(safari)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "Mobile Safari";
                    current.Version = nameAndVersion.Length > 1 ? nameAndVersion[1].Split(' ')[0] : "";
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"version\/([\w\.]+).+?(mobile\s?safari|safari)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "Safari";
                    current.Version = nameAndVersion[0];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"webkit.+?(gsa)\/([\w\.]+).+?(mobile\s?safari|safari)(\/[\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "GSA";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"webkit.+?(mobile\s?safari|safari)(\/[\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = (Match match, object obj) =>
                {
                    if (obj is not ClientBrowser current) return;
                    var nameAndVersion = match.Value.Split('/');
                    current.Name = "Safari";
                    current.Version = nameAndVersion[1];
                }
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(konqueror)\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(webkit|khtml)\/([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(navigator|netscape)\/([\w\.-]+)", RegexOptions.IgnoreCase)
                },
                Action = NameVersionAction
            },
            new MatchExpression
            {
                Regexes = new List<Regex>
                {
                    new(@"(swiftfox)", RegexOptions.IgnoreCase),
                    new(@"(icedragon|iceweasel|camino|chimera|fennec|maemo\sbrowser|minimo|conkeror)[\/\s]?([\w\.\+]+)", RegexOptions.IgnoreCase),
                    new(@"(firefox|seamonkey|k-meleon|icecat|iceape|firebird|phoenix|palemoon|basilisk|waterfox)\/([\w\.-]+)$", RegexOptions.IgnoreCase),
                    new(@"(mozilla)\/([\w\.]+).+rv\:.+gecko\/\d+", RegexOptions.IgnoreCase),
                    new(@"(polaris|lynx|dillo|icab|doris|amaya|w3m|netsurf|sleipnir)[\/\s]?([\w\.]+)", RegexOptions.IgnoreCase),
                    new(@"(links)\s\(([\w\.]+)", RegexOptions.IgnoreCase),
                    new(@"(gobrowser)\/?([\w\.]+)*", RegexOptions.IgnoreCase),
                    new(@"(ice\s?browser)\/v?([\w\._]+)", RegexOptions.IgnoreCase),
                    new(@"(mosaic)[\/\s]([\w\.]+)", RegexOptions.IgnoreCase)
                },
                Action = NameVersionAction
            }
        };
    }
}
