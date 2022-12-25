using Firebase.Database;
using Firebase.Storage;

namespace COURSE_ASH.Services;

public class ImageManager<T> where T: IImageDisposable 
{
    private const string FB_REALTIME_DB_URL = "https://musicshop-725ec-default-rtdb.europe-west1.firebasedatabase.app/";
    private const string FB_STORAGE_URL = "musicshop-725ec.appspot.com";

    private static readonly ChildQuery _firebaseQuery =
    new FirebaseClient(FB_REALTIME_DB_URL).Child(GetTableName());
    private static readonly FirebaseStorageReference _storage =
        new FirebaseStorage(FB_STORAGE_URL).Child(GetTableName());
    private static readonly FirebaseStorage _firebaseStorage =
        new FirebaseStorage(FB_STORAGE_URL);
    private static string GetTableName()
    {
        return typeof(T).Name;
        //return $"{type.Name.ToLower()}s";
    }

    public static async Task<string> LinkImageToStorageAsync(FileResult file)
    {
        //Stream imageToUpload = await file.OpenReadAsync();
        //string fileName = file.FileName;

        //await _storage
        //    .Child($"{fileName}")
        //    .PutAsync(imageToUpload);

        //string ret = await _storage
        //    .Child($"{fileName}")
        //    .GetDownloadUrlAsync();

        //return ret;
        Stream imageToUpload = await file.OpenReadAsync();

        await _firebaseStorage
            .Child(typeof(T).Name)
            .Child($"{file.FileName}")
            .PutAsync(imageToUpload);

        return await _firebaseStorage
            .Child(typeof(T).Name)
            .Child($"{file.FileName}")
            .GetDownloadUrlAsync();
    }

    public static async Task RemoveImageAsync(string fileURI)
    {
        await _storage
            .Child(Path.GetFileName(new Uri(fileURI).LocalPath))
            .DeleteAsync();
    }

    public static async Task<int> CountLinksAsync(string imageLink)
    {
        int count = 0;
        IEnumerable<IImageDisposable> objects = 
        (IEnumerable<IImageDisposable>) (from item 
                                         in await _firebaseQuery
                                        .OnceAsync<T>()
                                        select item.Object);

        foreach (IImageDisposable obj in objects)
            if ((Path.GetFileName(
                new Uri(obj.ImageLink).LocalPath)) == 
                (Path.GetFileName(new Uri(imageLink).LocalPath)))
                count++;

        return count;
    }
}
