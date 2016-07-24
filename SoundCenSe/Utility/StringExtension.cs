// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: StringExtension.cs
// 
// Last modified: 2016-07-24 13:52

namespace SoundCenSe.Utility
{
    public static class StringExtension
    {
        public static string Capitalize(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}