using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace akhr.ir.Repos
{
    public class BaseRepo: IDisposable
    {
        protected IDbConnection con;
        private IConfiguration _config;
        protected IConfiguration Config
        {
            get { return _config; }
            set
            {
                _config = value;
                if (con == null)
                    con = new SqlConnection(_config.GetConnectionString("dbScissorLink"));
            }
            
        }

        public void Dispose()
        {
            con.Close();
            con.Dispose();
        }
    }
}
