using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;

namespace ActvShare.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Post
        {
            public static Error PostNotFound => Error.NotFound(
                code: "Post.NotFound",
                description: "Post not found.");

            public static Error PostNotLiked => Error.Failure(
                code: "Post.NotLiked",
                description: "Post not liked by user.");

            public static Error PostAlreadyLiked => Error.Conflict(
                code: "Post.AlreadyLiked",
                description: "Post already liked by user.");

            public static Error PostAlreadyUnshared => Error.Conflict(
                code: "Post.AlreadyUnshared",
                description: "Post already unshared by user.");

            public static Error PostAlreadyShared => Error.Conflict(
                code: "Post.AlreadyShared",
                description: "Post already shared by user.");

            public static Error PostDoesNotHaveReplies => Error.Conflict(
                code: "Post.DoesNotHaveReplies",
                description: "Post does not have replies.");
        }
    }
}
