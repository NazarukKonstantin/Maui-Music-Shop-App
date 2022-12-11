namespace COURSE_ASH.Services.DataStorageStrategies;

public class FirebaseRealtimeDB: IDataStorage<FirebaseClient>
{
    public FirebaseClient GetClient(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new EmptyURLException();
        return new FirebaseClient(url);
    }
}
