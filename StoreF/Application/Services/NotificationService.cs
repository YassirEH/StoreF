namespace webApi.Application.Services
{
    public class NotificationService : INotificationService
    {
        
        private static readonly List<Notification> Notifications = new();

        public void Notify(string message, string type, ErrorType errorType)
        {

            int statusCode;
            switch (errorType)
            {
                case ErrorType.Success:
                    statusCode = 200;
                    break;

                case ErrorType.Error:
                    statusCode = 500;
                    break;

                case ErrorType.Info:
                    statusCode = 200;
                    break;

                case ErrorType.NotFound:
                    statusCode = 404;
                    break;

                case ErrorType.Conflict:
                    statusCode = 409;
                    break;

                default:
                    statusCode = 500;
                    break;
            }

            Notifications.Add(new Notification(message, type,statusCode));
        }

        public void Destroy()
        {
            Notifications.Clear();
        }

        public bool HasAnyNotification() => Notifications.Any();
    }

    public enum ErrorType
    {
        Success,
        Error,
        Info,
        Conflict,
        NotFound
    }
}


