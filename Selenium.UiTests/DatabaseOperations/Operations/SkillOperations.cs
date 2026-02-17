using Selenium.UiTests.DatabaseOperations.Queries;
using System.Data;

namespace Selenium.UiTests.DatabaseOperations.Operations
{
    public class SkillOperations : IDisposable
    {
        private readonly IDbConnection _connection;

        public SkillOperations(IDbConnection connection)
        {
            this._connection = connection;
        }

        public int GetSkillIdByName(string skillName)
        {
            using var command = this._connection.CreateCommand();
            command.CommandText = SkillQueries.GetSkillIdByName(skillName);

            var result = command.ExecuteScalar();

            if (result == null)
                throw new Exception($"Skill '{skillName}' does not exist in the database.");

            return Convert.ToInt32(result);
        }

        public void AddSkillToUser(int userId, int skillId, int competenceLevel)
        {
            using var command = this._connection.CreateCommand();
            command.CommandText = SkillQueries.AddSkillToUser(userId, skillId, competenceLevel);
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
