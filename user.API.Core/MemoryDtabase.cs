using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace User.Api.Core
{
    public class MemoryDatabase : Database
    {
        private List<User> UserList = new();

        public override void ExecuteNeonQuery(User user)
        {
            if (UserList.Any(u => u.UserName == user.UserName))
            {
                throw new Exception("User already exists.");
            }
            UserList.Add(user);
        }

        public override string GetJson(string username = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                return JsonSerializer.Serialize(UserList);
            }
            var user = UserList.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null) return "{}";
            return JsonSerializer.Serialize(user);
        }
    }
}