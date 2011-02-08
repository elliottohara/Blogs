using System;

namespace BankVue.Question2
{
    public static class StringExtensions
    {
        public static string Reverse(this string s)
        {
            var characters = s.ToCharArray();
            Array.Reverse(characters);
            return new string(characters);
        }
    }
}