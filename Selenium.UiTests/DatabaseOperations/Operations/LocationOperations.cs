using Selenium.UiTests.DatabaseOperations.Queries;
using System.Data;

namespace Selenium.UiTests.DatabaseOperations.Operations
{
    public class LocationOperations : IDisposable
    {
        private readonly IDbConnection _connection;

        public LocationOperations(IDbConnection connection)
        {
            this._connection = connection;
        }

        public void InsertCityAndCountry(string cityName, string countryName)
        {
            var command = this._connection.CreateCommand();
            command.CommandText = LocationQueries.AddCountryAndCity(cityName,countryName);
            command.ExecuteNonQuery();
        }

        public void DeleteCity(string cityName, string countryName)
        {
            var command = this._connection.CreateCommand();
            command.CommandText = LocationQueries.DeleteCity(cityName, countryName);
            command.ExecuteNonQuery();
        }

        public void DeleteCountry(string countryName)
        {
            var command = this._connection.CreateCommand();
            command.CommandText = LocationQueries.DeleteCountry(countryName);
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            this._connection.Close();
            this._connection.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
