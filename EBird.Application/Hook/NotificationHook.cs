using EBird.Domain.Entities;

namespace EBird.Application.Hook
{
    public class NotificationHook
    {
        private readonly HttpClient httpClient;

        public NotificationHook()
        {
            // Create a new HttpClient to make HTTP requests
            httpClient = new HttpClient();
        }

        public async Task NotifyUser(NotificationEntity notification)
        {
            // Define the data to send to the webhook
            var data = new Dictionary<string, string>()
            {
                { "email", notification.Account.Email},
                { "name", notification.Account.FirstName + " " + notification.Account.LastName},
                { "type", notification.NotificationType.TypeName},
                { "content", notification.Content }
            };

            // Make a POST request to the webhook URL with the data
            var response = await httpClient.PostAsync("https://hooks.zapier.com/hooks/catch/14651704/3o05kwn/", new FormUrlEncodedContent(data));

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Notification sent successfully!");
            }
            else
            {
                Console.WriteLine($"Failed to send notification. Status code: {response.StatusCode}");
            }
        }
    }
}