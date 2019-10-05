using System;
using ProjectsBaseShared.Models;

namespace ProjectsBaseSharedTests.Helpers
{
    public static class UserGenerator
    {
        public static User GenerateUser()
        {
            var random = RandomString();

            return new User()
            {
                UserName = random,
                Email = random
            };
        }

        public static string RandomString()
        {
            return new Random().Next().ToString();
        }
    }
}
