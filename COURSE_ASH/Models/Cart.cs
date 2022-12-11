
using System.Text.Json;

namespace COURSE_ASH.Models;

public class Cart : IAccountBased
{
    public string CurrentLogin { get; set; }
    public List<CartProduct> Products { get; set; }
}
