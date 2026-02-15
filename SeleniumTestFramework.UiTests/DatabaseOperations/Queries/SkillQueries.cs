namespace SeleniumTestFramework.UiTests.DatabaseOperations.Queries
{
    public class SkillQueries
    {
        public static string GetSkillIdByName(string name)
        {
            return $@"
                SELECT id FROM skills
                WHERE skill_name = '{name}'
                LIMIT 1;
            ";
        }

        public static string AddSkillToUser(int userId, int skillId, int competenceLevel)
        {
            return $@"
                INSERT INTO user_skills (user_id, skill_id, competence_level)
                VALUES ({userId}, {skillId}, {competenceLevel})
                ON DUPLICATE KEY UPDATE competence_level = '{competenceLevel}';
            ";
        }
    }
}
