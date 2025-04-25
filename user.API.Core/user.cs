using System.Text.Json.Serialization;

namespace User.Api.Core
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Birthday { get; set; }

        [JsonIgnore]
        public string RawPassword { get; set; }

        public string Password { get; set; }
    }
}