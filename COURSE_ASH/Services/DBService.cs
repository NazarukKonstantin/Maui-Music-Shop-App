namespace COURSE_ASH.Services;

public class DBService<T> : ICRUD<T> where T : class,ICRUDable
{
    private readonly ChildQuery _firebaseQuery;

    public DBService()
    {
        _firebaseQuery = App.DBManager.Client.Child(GetTableName(typeof(T)));
    }

    public async Task<bool> AddItemAsync(T item)
    {
        try
        {
            await _firebaseQuery
                        .PostAsync(item);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task AddCollection(ObservableCollection<T> items)
    {
        foreach (var item in items)
        {
            if (!(await AddItemAsync(item)))
                Console.WriteLine("ERROR while adding in DB");
        }
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        if (id>=0)
        {
            try
            {
                await _firebaseQuery
                    .Child(id.ToString())
                    .DeleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteTable()
    {
        try
        {
            await _firebaseQuery
                .DeleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }

    public async Task<T> GetItemAsync(int id)
    {
        if (id<0)
        {
            Console.WriteLine("Cannot get table element with negative key");
            return null;
        }
        try
        {
            var item = await _firebaseQuery
                .Child(id.ToString())
                .OnceSingleAsync<T>();
            return item;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
            return null;
        }
    }

    public async Task<ObservableCollection<T>> GetAsync(int startingID=0, int amount=10)
    {
        if (startingID<0||amount<0)
        {
            Console.WriteLine("Cannot get table elements with negative keys");
            return null;
        }
        try
        {
            var collection = new ObservableCollection<T>();
            var fbOrder = _firebaseQuery
                .OrderByKey();

            var fbCollection= await fbOrder
                .StartAt(startingID)
                .LimitToFirst(amount)
                .OnceAsync<T>();
            foreach (var value in fbCollection)
            {
                collection.Add(value.Object);
            }
            return collection;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<ObservableCollection<T>> GetAllItemsAsync()
    {
        try
        {
            var collection = new ObservableCollection<T>();
            var fbCollection = await _firebaseQuery
                .OrderBy("ID").OnceAsync<T>();
            foreach (var value in fbCollection)
            {
                collection.Add(value.Object);
            }
            return collection;
        }
        catch (Exception e)
        {
            //unhandled
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<bool> UpdateItemAsync(T new_item, int targetID)
    {
        if (targetID>=0)
        {
            try
            {
                await _firebaseQuery
                    .Child(targetID.ToString())
                    .PutAsync(new_item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        else Console.WriteLine("id is less 0");
        return false;
    }

    public async Task<int> GetTotalAmount()
    {
        int num = (await _firebaseQuery
            .OnceAsync<T>())
            .Count;
        return num;
    }

    private string GetTableName(Type type)
    {
        return $"{type.Name.ToLower()}s";
    }
}
