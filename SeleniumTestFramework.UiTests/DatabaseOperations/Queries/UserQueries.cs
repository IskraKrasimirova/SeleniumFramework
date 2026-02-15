namespace SeleniumTestFramework.UiTests.DatabaseOperations.Queries
{
    public static class UserQueries
    {
        public const string DeleteUserByEmail = @"
            DELETE FROM users 
            WHERE Email = @Email; 
        ";

        public const string InsertUser = @" 
            INSERT INTO users (first_name, sir_name, title, country, city, email, password, is_admin) 
            VALUES (@FirstName, @Surname, @Title, @Country, @City, @Email, @Password, @IsAdmin); 
            SELECT LAST_INSERT_ID(); 
        ";

        public static string GetUserByEmail(string email)
        {
            return $@"
                SELECT 1 FROM users
                WHERE email = '{email}';
            ";
        }

        public static string GetAllUserSkillsCount()
        {
            return $@"
                SELECT COUNT(*)
                FROM user_skills;
            ";
        }

        public static string GetAllUserSkills()
        {
            return $@"
                SELECT
                    u.first_name AS firstName,
                    u.sir_name AS surname,
                    u.email AS email,
                    u.country AS country,
                    u.city AS city,
                    s.skill_name AS skill,
                    s.skill_category AS skillCategory
                FROM users u
                INNER JOIN user_skills us ON us.user_id = u.id
                INNER JOIN skills s ON s.id = us.skill_id;
            ";
        }
    }
}