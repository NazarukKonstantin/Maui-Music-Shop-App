namespace COURSE_ASH.Services;

public abstract class DBManager<T> : ICRUD<T>
{
    public string URL { get; set; }
    public string Secret { get; set; }

    public FirebaseClient BaseConstructor(string url, string secret)
    {
        if (!string.IsNullOrEmpty(url)) URL=url;
        else URL=string.Empty;
        if (!string.IsNullOrEmpty(secret)) Secret=secret;
        else Secret=string.Empty;

        return new(URL, new FirebaseOptions
        {
            AuthTokenAsyncFactory=() => Task.FromResult(Secret)
        });
    }

    public abstract Task<bool> Add(T value);

    public abstract Task<bool> Delete(string Key);

    public abstract Task<T> Get(string Key);

    public abstract Task<List<T>> GetAll();

    public abstract Task<bool> Update(T value);
}
