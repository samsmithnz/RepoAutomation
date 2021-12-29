using System;
using System.Linq;


namespace RepoAutomation.Tests.Helpers
{
    public static class Utility
    {
        public static string TakeNLines(string text, int lines)
        {
            return string.Join(Environment.NewLine, text.Split(Environment.NewLine).Take(lines));
        }

        public static string TrimNewLines(string input)
        {
            //Trim off any leading or trailing new lines 
            input = input.TrimStart('\r', '\n');
            input = input.TrimEnd('\r', '\n');
            return input;
        }
    }
}
