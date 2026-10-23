
using System.Collections.Generic;

namespace AutomationFrameWork.Utilities
{
    public class ConfigReader
    {
        public CredentialsSettings Credentials { get; set; } = new();

        public UrlSettings Urls { get; set; } = new();

        public AutomationSettings AutomationSettings { get; set; } = new();
    }

    public class CredentialsSettings
    {
        public Dictionary<string, UserCredentials> Roles { get; set; } = new();
    }

    public class UserCredentials
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UrlSettings
    {
        public string QALMS { get; set; } = string.Empty;
    }

    public class AutomationSettings
    {
        public string Channel { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
        public bool Headless { get; set; }
        public float Timeout { get; set; }
    }

    public class JsonObjects
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string City { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = new();
    }
}
