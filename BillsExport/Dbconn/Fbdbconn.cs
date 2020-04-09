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
                                 $"DataSource=localhost;Charset=NONE; " + 
                                 $"Pooling=false";
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

        private void _executeInsert(string sql)
        {
            FbCommand cmd = new FbCommand(sql);
            cmd.CommandType = System.Data.CommandType.Text;
            using(cmd.Connection = _fbConnection)
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        
        protected FbDataReader rawQuery(string query)
        {
            return _executeCommand(query);
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

        protected void insert(string tablename, string values)
        {
            string sql = $"INSERT INTO {tablename} values ({values});";
            _executeInsert(sql);
        }

        /**
         * SQL INSERT
         * 
         */
        protected void insert(string tablename, string columns, string values)
        {
            string sql = $"INSERT INTO {tablename} ({columns}) values ({values});";
            _executeInsert(sql);
        } 

        protected void update(string tablename, string values, string condition)
        {
            string sql = $"UPDATE {tablename} SET {values} WHERE {condition};"; 
            _executeInsert(sql);
        }

        /**
         * Get the next value for auto generated columns in each table. 
         * 
         */
        protected int NextValueFor(string genName)
        {
            int genId = 0;
            string sql = String.Format("SELECT NEXT VALUE FOR {0} AS Id FROM RDB$DATABASE", genName);
            FbDataReader fbDataReader = _executeCommand(sql);
            if (fbDataReader.HasRows)
            {
                while (fbDataReader.Read())
                {
                    object[] values = new object[fbDataReader.FieldCount];
                    fbDataReader.GetValues(values);
                    try
                    {
                        genId = int.Parse(values[0].ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            return genId;
        }

        protected int NextFromKey(string tableName)
        {
            int genId = 0;
            string sql = String.Format("SELECT FIRST 1 FROMKEY FROM {0} ORDER BY DOCKEY DESC;", tableName);
            FbDataReader fbDataReader = _executeCommand(sql);
            if (fbDataReader.HasRows)
            {
                while (fbDataReader.Read())
                {
                    object[] values = new object[fbDataReader.FieldCount];
                    fbDataReader.GetValues(values);
                    try
                    {
                        genId = int.Parse(values[0].ToString()) + 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            return genId;
        }
    }
}
