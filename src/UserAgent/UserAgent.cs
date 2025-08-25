using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScissorLink.UserAgent
{
    public class UserAgent
    {
        private readonly string _userAgent;

        private ClientBrowser? _browser;
        public ClientBrowser Browser
        {
            get
            {
                _browser ??= new ClientBrowser(_userAgent);
                return _browser;
            }
        }

        private ClientOS? _os;
        public ClientOS OS
        {
            get
            {
                _os ??= new ClientOS(_userAgent);
                return _os;
            }
        }

        public UserAgent(string userAgent)
        {
            _userAgent = userAgent ?? string.Empty;
        }
    }
}
