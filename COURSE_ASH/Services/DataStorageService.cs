using Firebase.Database;

namespace COURSE_ASH.Services;

public static class DataStorageService<T> where T : class
{
    private const string FB_REALTIME_DB_URL = "https://musicshop-725ec-default-rtdb.europe-west1.firebasedatabase.app/";

    private static readonly ChildQuery _firebaseQuery =
        new FirebaseClient(FB_REALTIME_DB_URL).Child(GetTableName(typeof(T)));

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

    public static async Task<T> GetItemByAsync(string searchFieldName, int value)
    {
        var item = await _firebaseQuery
            .OrderBy(searchFieldName)
            .EqualTo(value)
            .OnceAsync<T>();
        return item.Any() ? item.First().Object : default;
    }

    public static async Task<T> GetItemByAsync(string searchFieldName, string value)
    {
        var item = await _firebaseQuery
            .OrderBy(searchFieldName)
            .EqualTo(value)
            .OnceAsync<T>();
        return item.Any() ? item.First().Object : default;
    }
    public static async Task<IEnumerable<T>> GetItemsByAsync(string searchFieldName, int value)
    {
        return (from item in (await _firebaseQuery
            .OrderBy(searchFieldName)
            .EqualTo(value)
            .OnceAsync<T>())
                select item.Object)?.AsEnumerable<T>();
    }
    public static async Task<IEnumerable<T>> GetItemsByAsync(string searchFieldName, string value)
    {
        return (from item in (await _firebaseQuery
            .OrderBy(searchFieldName)
            .EqualTo(value)
            .OnceAsync<T>())
                select item.Object)?.AsEnumerable<T>();
    }
    public static async Task AddItemAsync(T item)
    {
        await _firebaseQuery
            .PostAsync(item);
    }
    public static async Task DeleteItemsAsync(string searchFieldName, int value)
    {
        var items = (await _firebaseQuery
            .OrderBy(searchFieldName)
            .EqualTo(value)
            .OnceAsync<T>());

        foreach (var item in items)
        {
            await _firebaseQuery
                .Child(item.Key)
                .DeleteAsync();
        }
    }
    public static async Task DeleteItemsAsync(string searchFieldName, string value)
    {
        var items = (await _firebaseQuery
                    .OrderBy(searchFieldName)
                    .EqualTo(value)
                    .OnceAsync<T>());

        foreach (var item in items)
        {
            await _firebaseQuery
                .Child(item.Key)
                .DeleteAsync();
        }
    }
    public static async Task UpdateItemAsync(T item, string searchFieldName, int value)
    {
        var target = (await _firebaseQuery
            .OrderBy(searchFieldName)
            .EqualTo(value)
            .OnceAsync<T>());
        if (target.Count is 0)
        {
            await AddItemAsync(item);
            return;
        }
        await _firebaseQuery
            .Child(target.First().Key)
            .PatchAsync(item);
    }
    public static async Task UpdateItemAsync(T item, string searchFieldName, string value)
    {
        var target = (await _firebaseQuery
            .OrderBy(searchFieldName)
            .EqualTo(value)
            .OnceAsync<T>());
        if (target.Count is 0)
        {
            await AddItemAsync(item);
            return;
        }
        await _firebaseQuery
            .Child(target.First().Key)
            .PatchAsync(item);
    }
}
