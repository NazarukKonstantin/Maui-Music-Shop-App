namespace COURSE_ASH.Services.Interfaces;

public interface IDataStorage<T>
{
    T GetClient(string url);
}
