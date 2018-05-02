using System.Text.RegularExpressions;

namespace LinkRipper
{
    public sealed class AudioTitleRipper
    {
        private readonly string titleRegexString = "(?<=title=\"Example:)(.*?)(?=\")";        

        private readonly Regex titleRegex;        

        public AudioTitleRipper()
        {
            titleRegex = new Regex(titleRegexString);            
        }

        public string Rip(string text)
        {            
            return titleRegex.Match(text).Captures[0].Value;            
        }
    }
}