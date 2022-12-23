using System.Globalization;
using Geocoding.Microsoft;

namespace COURSE_ASH.Services;

public class BillingAddressService
{
    private readonly BingMapsGeocoder geocoder =
            new ("AhxYjH0f7KdF01OnF0qQs_jIlPZdEMSkzxWSQHH - h9lNEOBHl44jNkvnviZP0OR0");

    public async Task<string> CheckAddressAsync(string country, string city, string street, int? buildingNumber, string postalCode)
    {
        if (AreFieldsEmpty(country, city, street, postalCode) || buildingNumber is null) return AddressAlerts.FIELDS_EMPTY;

        IEnumerable<BingAddress> addresses = await geocoder.GeocodeAsync($"{country}, {city}");

        BingAddress address = addresses.FirstOrDefault((BingAddress)null);

        if (!(address.CountryRegion.ToLower().Contains(country.ToLower()) &&
                address.Locality.ToLower().Contains(city.ToLower())))
            return AddressAlerts.WRONG_CITY;
        return AddressAlerts.SUCCESS;
    }
    public static IEnumerable<string> GetCountries()
    {
        List<string> countries = new();
        foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            RegionInfo ri = new(ci.Name);
            if (!countries.Contains(ri.EnglishName)) countries.Add(ri.EnglishName);
        }
        return from country in countries orderby country select country;
    }
    public static bool AreFieldsEmpty(params string[] strings)
    {
        foreach (string str in strings)
            if (string.IsNullOrEmpty(str))
                return true;
        return false;
    }
    public string GetFormattedAddress(string country, string city, string street, int? buildingNumber,
        int? apartmentNumber, string postalСode)
    {
        return apartmentNumber is null ?
         $"{country}, {city}, {street}, {buildingNumber}, {postalСode}" :
         $"{country}, {city}, {street}, {buildingNumber}, {apartmentNumber}, {postalСode}";
    }
}
