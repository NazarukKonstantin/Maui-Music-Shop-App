using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataLibrary
{
    public class DataAccess : IDataAccess
    {
        public async Task<List<T>> LoadData<T, U>(string sqlQuery, U parameters, string connectionString)
        {
            //Opens connection to a DB
            using IDbConnection connection = new MySqlConnection(connectionString);
            var rows = await connection.QueryAsync<T>(sqlQuery, parameters);

            return rows.ToList();
        }
        public Task SaveData<T>(string sqlQuery, T parameters, string connectionString)
        {
            //Opens connection to a DB
            using IDbConnection connection = new MySqlConnection(connectionString);
            return connection.ExecuteAsync(sqlQuery, parameters);
        }
    }
}
