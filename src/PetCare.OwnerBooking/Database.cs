using System;
using System.Data;
using System.Data.SqlClient;

namespace BookingRequests_System
{
    internal static class Database
    {
        internal static SqlConnection CreateConnection()
        {
            return new SqlConnection(DatabaseConfig.ConnectionString);
        }

        internal static DataTable FillDataTable(string query)
        {
            return FillDataTable(query, null);
        }

        internal static DataTable FillDataTable(string query, Action<SqlParameterCollection> addParameters)
        {
            using (SqlConnection connection = CreateConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                if (addParameters != null)
                {
                    addParameters(adapter.SelectCommand.Parameters);
                }

                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }
}
