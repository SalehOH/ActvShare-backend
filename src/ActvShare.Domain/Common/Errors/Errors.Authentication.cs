using ErrorOr;

namespace ActvShare.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Auth.InvalidCred",
            description: "Invalid credentials.");

        public static Error RefreshTokenNotFound => Error.Custom(
            code: "Auth.RefreshTokenNotFound",
            description: "Refresh token not found.",
            type: 403);

    }
}