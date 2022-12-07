using System.Runtime.CompilerServices;

namespace COURSE_ASH.Services.Interfaces;

public interface ICRUD<T> where T: ICRUDable
{
    Task<bool> AddItemAsync(T item);
    Task AddCollection(ObservableCollection<T> items);
    Task<bool> UpdateItemAsync(T new_item, int targetID);
    Task<bool> DeleteItemAsync(int id);
    Task<T> GetItemAsync(int id);
    Task<ObservableCollection<T>> GetAsync(int startingID, int amount);
    Task<ObservableCollection<T>> GetAllItemsAsync();
}
