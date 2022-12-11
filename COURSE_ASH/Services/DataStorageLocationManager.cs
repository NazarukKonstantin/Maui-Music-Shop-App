namespace COURSE_ASH.Services;

public class DataStorageLocationManager<T> where T : class
{
    private IDataStorage<T> _dataStorage;
    private readonly string _url;

    public DataStorageLocationManager(string url)
    {
        _url=url;
    }
    
    public void SetDataStorage(IDataStorage<T> dataStorage)
    {
        _dataStorage=dataStorage;
    }

    public T GetDataStorageClient()
    {
        return _dataStorage.GetClient(_url);
    }
}
