using System.Text.Json;
using System.Text.Json.Serialization;

namespace User.Api.Core
{
    public class FileDatabase : Database
    {
        private readonly string _directoryPath = "UserFiles";

        public FileDatabase()
        {
            if (!Directory.Exists(_directoryPath))
                Directory.CreateDirectory(_directoryPath);
        }

        public override void ExecuteNeonQuery(User user)
        {
            string filePath = Path.Combine(_directoryPath, $"{user.UserName}.json");

            if (File.Exists(filePath))
                throw new Exception("A user with this username already exists.");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            string json = JsonSerializer.Serialize(user, options);
            File.WriteAllText(filePath, json);
        }

        public override string GetJson(string username = null)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            if (!string.IsNullOrEmpty(username))
            {
                string filePath = Path.Combine(_directoryPath, $"{username}.json");
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"User '{username}' not found.");

                string json = File.ReadAllText(filePath);
                return json;
            }
            else
            {
                var allUsers = new List<User>();
                foreach (var file in Directory.GetFiles(_directoryPath, "*.json"))
                {
                    string content = File.ReadAllText(file);
                    var user = JsonSerializer.Deserialize<User>(content);
                    if (user != null)
                        allUsers.Add(user);
                }

                return JsonSerializer.Serialize(allUsers, options);
            }
        }
    }
}