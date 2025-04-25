using System.Net.Http;
using System.Text.Json;

namespace User.Api.Core
{
    public class SaveUserBusiness
    {
        public static async Task<ServiceState> Process()
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync("https://randomuser.me/api/");
                using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var userJson = doc.RootElement.GetProperty("results")[0];

                var user = new User
                {
                    FirstName = userJson.GetProperty("name").GetProperty("first").GetString(),
                    LastName = userJson.GetProperty("name").GetProperty("last").GetString(),
                    UserName = userJson.GetProperty("login").GetProperty("username").GetString(),
                    Birthday = userJson.GetProperty("dob").GetProperty("date").GetString(),
                    RawPassword = userJson.GetProperty("login").GetProperty("password").GetString()
                };
                 Console.WriteLine($"Raw Password (before hashing): {user.RawPassword}");

                user.Password = PasswordHash.Generate(user.RawPassword, user.UserName);
                Database.Instance.ExecuteNeonQuery(user);
                return ServiceState.Accepted;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return ServiceState.Rejected;
            }
        }
    }
}