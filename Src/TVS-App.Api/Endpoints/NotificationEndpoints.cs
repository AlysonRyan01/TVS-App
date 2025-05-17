using TVS_App.Application.Handlers;

namespace TVS_App.Api.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this WebApplication app)
    {
        app.MapPost("/notifications", async (NotificationHandler handler, string title, string message) =>
        {
            var result = await handler.CreateNotification(title, message);
            return Results.Ok(result);
        }).WithTags("Notifications");

        app.MapGet("/notifications/unread", async (NotificationHandler handler) =>
        {
            var result = await handler.GetUnreadNotifications();
            return Results.Ok(result);
        }).WithTags("Notifications");

        app.MapPut("/notifications/{id}/read", async (NotificationHandler handler, long id) =>
        {
            var result = await handler.MarkNotificationAsRead(id);
            return Results.Ok(result);
        }).WithTags("Notifications");
    }
}