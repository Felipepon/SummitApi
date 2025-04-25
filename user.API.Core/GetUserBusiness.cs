namespace User.Api.Core
{
    public class GetUserBusiness
    {
        public static string Process(string username = null)
        {
            return Database.Instance.GetJson(username);
        }
    }
}