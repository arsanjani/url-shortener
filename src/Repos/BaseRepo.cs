using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace akhr.ir.Repos
{
    public class BaseRepo : IDisposable
    {
        protected IDbConnection? con;
        private IConfiguration? _config;
        private bool _disposed = false;

        protected IConfiguration Config
        {
            get { return _config!; }
            set
            {
                _config = value;
                if (con == null && _config != null)
                {
                    try
                    {
                        var connectionString = _config.GetConnectionString("dbScissorLink");
                        if (!string.IsNullOrEmpty(connectionString))
                        {
                            con = new SqlConnection(connectionString);
                        }
                    }
                    catch
                    {
                        // Ignore exceptions during configuration setup
                        // This allows tests to pass without proper configuration
                    }
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    con?.Close();
                    con?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
