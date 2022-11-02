using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;
using System.Globalization;

namespace COURSE_ASH.SearchHandlers
{
    public class ObsCollectionToListConverter<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ObservableCollection<T>)value).ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((List<T>)value).ToObservableCollection();
        }
    }
}
