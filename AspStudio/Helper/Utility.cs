using AspStudio.Models.DTOs;
using System.Text.Json;

namespace AspStudio.Helper
{
    public class Utility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Utility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddSession(string key, LoginMenuModels value)
        {
            var jsonValue = JsonSerializer.Serialize(value);
            _httpContextAccessor.HttpContext.Session.SetString(key, jsonValue);
        }

        public void NullSession(string key, LoginMenuModels value)
        {
            var jsonValue = JsonSerializer.Serialize(value);
            _httpContextAccessor.HttpContext.Session.SetString(key, jsonValue);
        }
        public LoginMenuModels GetSession(string key)
        {
            var jsonValue = _httpContextAccessor.HttpContext.Session.GetString(key);
            if (jsonValue != null)
            {
                return JsonSerializer.Deserialize<LoginMenuModels>(jsonValue);
            }
            return null;
        }
        public class UserSession
        {

            public static string BEMISLogin = "BEMISLogin";


        }
    }
}
