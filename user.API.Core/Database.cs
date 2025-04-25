namespace User.Api.Core
{
    public abstract class Database
    {
        public static Database Instance { get; set; }

        public abstract void ExecuteNeonQuery(User user);
        public abstract string GetJson(string? username = null);
    }
}