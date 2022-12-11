namespace COURSE_ASH.Services.DataStorageStrategies;

public class FirebaseDataStorage : IDataStorage<FirebaseStorage>
{
    public FirebaseStorage GetClient(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new EmptyURLException();
        return new FirebaseStorage(url);
    }
}
