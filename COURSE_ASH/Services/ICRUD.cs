namespace COURSE_ASH.Services;

public interface ICRUD<T>
{
    Task<bool> Add(T value);
    Task<bool> Update(T value);
    Task<bool> Delete(string Key);
    Task<List<T>> GetAll();
    Task<T> Get(string Key);
}
