using ErrorOr;

namespace ActvShare.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "Email is already used.");

        public static Error DuplicateUsername => Error.Conflict(
            code: "User.DuplicateUsername",
            description: "Username is already used.");

        public static Error UserNotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found.");

        public static Error UserIsAlreadyFollowed => Error.Conflict(
            code: "User.IsAlreadyFollowed",
            description: "User is already followed.");

        public static Error UserIsNotFollowed => Error.Conflict(
            code: "User.IsNotFollowed",
            description: "User is not followed.");

        public static Error NotificationIsAlreadyRead => Error.Conflict(
            code: "User.NotificationIsAlreadyRead",
            description: "Notification is already read.");

        public static Error NoNotificationsFound => Error.NotFound(
            code: "User.NoNotificationsFound",
            description: "No notifications found.");

        public static Error NoFollowersFound => Error.NotFound(
            code: "User.NoFollowersFound",
            description: "No followers found.");
    }
}