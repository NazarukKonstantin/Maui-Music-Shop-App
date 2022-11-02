namespace DataLibrary
{
    public interface IDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sqlQuery, U parameters, string connectionString);
        Task SaveData<T>(string sqlQuery, T parameters, string connectionString);
    }
}