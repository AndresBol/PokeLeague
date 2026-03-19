using System.Text.Json;

namespace PokeLeague.Web.Util
{
    public static class SweetAlertHelper
    {
        public static string CreateNotification(string title, string message, SweetAlertMessageType type)
        {
            var config = new
            {
                title,
                text = message,
                icon = type.ToString()
            };

            return JsonSerializer.Serialize(config);
        }
    }

    public enum SweetAlertMessageType
    {
        success,
        error,
        warning,
        info,
        question
    }
}
