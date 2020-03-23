using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Dbconn
{
    class Fbdbconn
    {
        private string _dbpath;

        public Fbdbconn(string dbpath)
        {
            _dbpath = dbpath;
        }

        private FbConnection _fbConnection
        {
            get
            {
                string connStr = $"User ID={ConfigurationManager.AppSettings["dbuser"]};" +
                                 $"Password={ConfigurationManager.AppSettings["dbpassword"]};" + 
                                 $"Database=localhost:{_dbpath}; " + 
                                 $"DataSource=localhost;Charset=NONE;";
                return new FbConnection(connStr);
            }
        }

        private FbDataReader _executeCommand(string sql)
        {
            FbConnection fbConnection = _fbConnection;
            if (fbConnection.State.ToString() == "Open")
            {
                fbConnection.Close();
            }
            fbConnection.Open();
            FbCommand fbCommand = new FbCommand(sql, fbConnection);
            FbDataReader fbDataReader = fbCommand.ExecuteReader();
            return fbDataReader;
        }

        /**
         * SQL SELECT
         * 
         */
        protected FbDataReader select(string tablename)
        {
            string sql = $"SELECT * FROM {tablename};";
            return _executeCommand(sql);
        }

        /**
         * Override
         * 
         */
        protected FbDataReader select(string tablename, string condition)
        {
            string sql = $"SELECT * FROM {tablename} WHERE {condition};";
            return _executeCommand(sql);
        }

        /**
         * Override
         * 
         */
        protected FbDataReader select(string tablename, string condition, string limit)
        {
            string sql = $"SELECT * FROM {tablename} WHERE {condition} LIMIT {limit};";
            return _executeCommand(sql);
        }
    }
}
