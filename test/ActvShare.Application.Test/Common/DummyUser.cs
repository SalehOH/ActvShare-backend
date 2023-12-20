using ActvShare.Application.Authentication.Common;
using ActvShare.Domain.Users;
using ActvShare.Domain.Users.ValueObjects;

namespace ActvShare.Application.Test.Common
{
    public static class DummyUser
    {
        public static User GetDummyUser()
        {
            var hashedPassword = new PasswordHashing().HashPassword("johndoe" ,"password123");
            var user = User.Create
            (
                name: "John Doe",
                username: "johndoe",
                email: "jondoe@example.com",
                password: hashedPassword
            );
            user.AddProfileImage("profile.jpg", "xxx.jpg", "jpg", 15);   

            return user;
        }

        public static User CreateUser(string username){
            var hashedPassword = new PasswordHashing().HashPassword(username ,"password123");
            var user = User.Create
            (
                name: "John Doe",
                username: username,
                email: "jondoe2@example.com",
                password: hashedPassword
            );
            
            user.AddProfileImage("profile.jpg", "xxx.jpg", "jpg", 15);  
            return user;
        }

    }
}