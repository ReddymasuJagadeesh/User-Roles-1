using System;
using System.Linq;

namespace UserRoles.Helpers
{
    public static class PasswordHelper
    {
        public static string GeneratePassword(int length = 10)
        {
            const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string lower = "abcdefghijkmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "@#$%&*";

            var random = new Random();

            // 🔒 GUARANTEE ALL REQUIRED RULES
            var passwordChars = new[]
            {
                upper[random.Next(upper.Length)],     // Uppercase
                lower[random.Next(lower.Length)],     // Lowercase
                digits[random.Next(digits.Length)],   // 🔥 DIGIT (FIX)
                special[random.Next(special.Length)]  // Special
            }.ToList();

            string allChars = upper + lower + digits + special;

            // Fill remaining characters
            for (int i = passwordChars.Count; i < length; i++)
            {
                passwordChars.Add(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle for randomness
            return new string(passwordChars
                .OrderBy(_ => random.Next())
                .ToArray());
        }
    }
}
