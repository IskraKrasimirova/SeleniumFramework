using SeleniumTestFramework.UiTests.DatabaseOperations.Entities;
using SeleniumTestFramework.UiTests.DatabaseOperations.Queries;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace SeleniumTestFramework.UiTests.DatabaseOperations.Operations
{
    public class UserOperations : IDisposable
    {
        private readonly IDbConnection _connection;

        public UserOperations(IDbConnection connection)
        {
            this._connection = connection;
        }

        public void DeleteUserWithEmail(string email)
        {
            using var command = this._connection.CreateCommand();
            command.CommandText = UserQueries.DeleteUserByEmail;

            AddParameter(command, "@Email", email);

            command.ExecuteNonQuery();
        }

        public int InsertUser(UserEntity user)
        {
            using var command = this._connection.CreateCommand();
            command.CommandText = UserQueries.InsertUser;

            var hashedPassword = HashPassword(user.Password);
            var isAdminValue = user.IsAdmin ? 1 : 0;

            AddParameter(command, "@FirstName", user.FirstName);
            AddParameter(command, "@Surname", user.Surname);
            AddParameter(command, "@Title", user.Title);
            AddParameter(command, "@Country", user.Country);
            AddParameter(command, "@City", user.City);
            AddParameter(command, "@Email", user.Email);
            AddParameter(command, "@Password", hashedPassword);
            AddParameter(command, "@IsAdmin", isAdminValue);

            var result = command.ExecuteScalar();

            return Convert.ToInt32(result);
        }

        public bool CheckIfUserExistsByEmail(string email)
        {
            var command = this._connection.CreateCommand();
            command.CommandText = UserQueries.GetUserByEmail(email);
            var result = command.ExecuteScalar();

            return result != null && Convert.ToInt32(result) == 1;
        }

        public int GetAllUserSkillsCount()
        {
            var command = this._connection.CreateCommand();
            command.CommandText = UserQueries.GetAllUserSkillsCount();
            var result = command.ExecuteScalar();

            return Convert.ToInt32(result);
        }

        public List<UserSkillDto> GetAllUserSkillRecords()
        {
            var command = this._connection.CreateCommand();
            command.CommandText = UserQueries.GetAllUserSkills();

            var result = new List<UserSkillDto>();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new UserSkillDto
                {
                    FirstName = reader.GetString(0),
                    Surname = reader.GetString(1),
                    Email = reader.GetString(2),
                    Country = reader.GetString(3),
                    City = reader.GetString(4),
                    Skill = reader.GetString(5),
                    SkillCategory = reader.GetString(6)
                });
            }

            return result;
        }

        public void Dispose()
        {
            this._connection.Close();
            this._connection.Dispose();

            GC.SuppressFinalize(this);
        }

        private static void AddParameter(IDbCommand command, string parameterName, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }

        private string HashPassword(string password)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password))).ToLower();
        }
    }
}
