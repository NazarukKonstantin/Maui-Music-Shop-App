using Firebase.Database;
using Firebase.Storage;

namespace COURSE_ASH.Services;

public static class DataStorageService<T> where T : IDBItem
{
    private static readonly ChildQuery _firebaseQuery =
        App.FbClientManager.GetDataStorageClient().Child(GetTableName(typeof(T)));
    private static readonly FirebaseStorage _storage =
        App.FbStorageManager.GetDataStorageClient();

    private static string GetTableName(Type type)
    {
        return type.Name;
        //return $"{type.Name.ToLower()}s";
    }

    public static async Task<IEnumerable<T>> GetItemListAsync()
    {
        return (from item in (await _firebaseQuery
                .OrderByKey()
                .OnceAsync<T>())
                select item.Object)?.AsEnumerable();
    }

    public static async Task<T> GetItemAsync(string fieldName, int value)
    {
        var item = await _firebaseQuery
            .OrderBy(fieldName)
            .EqualTo(value)
            .OnceAsync<T>();
        return item.Any() ? item.First().Object : default;
    }

    public static async Task<T> GetItemAsync(string fieldName, string value)
    {
        var item = await _firebaseQuery
            .OrderBy(fieldName)
            .EqualTo(value)
            .OnceAsync<T>();
        return item.Any() ? item.First().Object : default;
    }
    public static async Task<IEnumerable<T>> GetItemsAsync(string fieldName,int value)
    {
        return (from item in (await _firebaseQuery
            .OrderBy(fieldName)
            .EqualTo(value)
            .OnceAsync<T>())
                select item.Object)?.AsEnumerable<T>();
    }
    public static async Task<IEnumerable<T>> GetItemsAsync(string fieldName,string value)
    {
        return (from item in (await _firebaseQuery
            .OrderBy(fieldName)
            .EqualTo(value)
            .OnceAsync<T>())
                select item.Object)?.AsEnumerable<T>();
    }
    public static async Task AddItemAsync(T item)
    {
        await _firebaseQuery
            .PostAsync(item);
    }
    public static async Task DeleteItemAsync(string fieldName, int value)
    {
        var item = (await _firebaseQuery
            .OrderBy(fieldName)
            .EqualTo(value)
            .OnceAsync<T>()).First();

        await _firebaseQuery
            .Child(item.Key)
            .DeleteAsync();
    }
    public static async Task DeleteItemAsync(string fieldName, string value)
    {
        var item = (await _firebaseQuery
            .OrderBy(fieldName)
            .EqualTo(value)
            .OnceAsync<T>()).First();

        await _firebaseQuery
            .Child(item.Key)
            .DeleteAsync();
    }
    public static async Task UpdateItemAsync(T item, string fieldName, int value)
    {
        var target = (await _firebaseQuery
            .OrderBy(fieldName)
            .EqualTo(value)
            .OnceAsync<T>()).First();

        await _firebaseQuery
            .Child(target.Key)
            .PatchAsync(item);
    }
    public static async Task UpdateItemAsync(T item, string fieldName, string value)
    {
        var target = (await _firebaseQuery
            .OrderBy(fieldName)
            .EqualTo(value)
            .OnceAsync<T>()).First();

        await _firebaseQuery
            .Child(target.Key)
            .PatchAsync(item);
    }

    public static async Task<int> GetIDAsync()
    {
        return (await _firebaseQuery
           .OnceAsync<T>())?.Count + 1 ?? 1;
    }

    public static async Task<string> LinkToStorageAsync(FileResult file)
    {
        Stream imageToUpload = await file.OpenReadAsync();
        string fileName = file.FileName;

        await _storage
            .Child($"{fileName}")
            .PutAsync(imageToUpload);

        return await _storage
            .Child($"{fileName}")
            .GetDownloadUrlAsync();
    }

    public static async Task DeleteFileAsync(string fileURI)
    {
        await _storage.Child(Path.GetFileName(new Uri(fileURI).LocalPath))
            .DeleteAsync();
    }
}
