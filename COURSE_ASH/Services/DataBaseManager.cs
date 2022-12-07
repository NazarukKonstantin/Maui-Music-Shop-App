namespace COURSE_ASH.Services;

public class DataBaseManager
{
    private string _url;
    public FirebaseClient Client;

    public DataBaseManager (string url)
    {
        if (!string.IsNullOrEmpty(url)) _url=url;
        else _url=string.Empty;

        Client=new(_url);
    }
}
