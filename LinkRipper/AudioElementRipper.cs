using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LinkRipper
{  
    public sealed class AudioElementRipper
    {        
        private readonly string regexString = "<audio(?= title=)(.*?)</audio>";
        private readonly Regex regex;

        public AudioElementRipper()
        {
            regex = new Regex(regexString);
        }

        public AudioTagCategory RipAudioTags(Uri sourceUri, string text)
        {
            var matches = regex.Matches(text);
            var groups = new List<string>();

            foreach (var group in matches)
            {
                groups.Add(group.ToString());
            }

            var categoryName = Path.GetDirectoryName(sourceUri.AbsolutePath);

            return new AudioTagCategory(categoryName, sourceUri, groups);
        }
    }    
}