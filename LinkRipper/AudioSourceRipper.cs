using System.Text.RegularExpressions;

namespace LinkRipper
{
    public sealed class AudioSourceRipper
    {
        private readonly string sourceRegexString = "(?<=src=\")(.*?)(?=\" )";

        private readonly Regex sourceRegex;        

        public AudioSourceRipper()
        {
            sourceRegex = new Regex(sourceRegexString);            
        }

        public string Rip(string text)
        {            
            return sourceRegex.Match(text).Captures[0].Value;            
        }
    }
}