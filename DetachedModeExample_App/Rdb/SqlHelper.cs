using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetachedModeExample_App.Rdb
{
    // SqlHelper - статический класс с вспомогательными методами работы с БД
    internal static class SqlHelper
    {
        // CreateDbConnection - метод создания объекта подключения БЕЗ его открытия
        public static SqlConnection CreateDbConnection()
        {
            string useConnection = ConfigurationManager.AppSettings["UseConnection"] ?? "DefaultDbConnection";
            string connectionString = ConfigurationManager.ConnectionStrings[useConnection].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
