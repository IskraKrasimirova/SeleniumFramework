namespace SeleniumTestFramework.UiTests.DatabaseOperations.Queries
{
    public class LocationQueries
    {
        public static string AddCountryAndCity(string cityName, string countryName)
        {
            return $@"
                INSERT INTO cities (city_name, country_name)
                VALUES ('{cityName}', '{countryName}');
            ";
        }
        public static string DeleteCity(string cityName, string countryName)
        {
            return $@"
                DELETE FROM cities
                WHERE city_name ='{cityName}'
                AND country_name = '{countryName}';
            ";
        }

        public static string DeleteCountry(string countryName)
        {
            return $@"
                DELETE FROM cities
                WHERE country_name = '{countryName}';
            ";
        }
    }
}
