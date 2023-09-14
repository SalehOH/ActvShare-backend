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
		public static class Chat
        {
			public static Error ChatNotFound => Error.NotFound(
				code: "Chat.NotFound",
				description: "Chat not found.");

			public static Error ChatAlreadyExists => Error.NotFound(
				code: "Chat.AlreadyExists",
				description: "Chat already exists.");

			public static Error ChatAlreadyJoined => Error.Conflict(
				code: "Chat.AlreadyJoined",
				description: "Chat already joined by user.");

			public static Error ChatAlreadyDeleted => Error.Conflict(
				code: "Chat.AlreadyDeleted",
				description: "Chat already deleted by");
        }
    }
}
