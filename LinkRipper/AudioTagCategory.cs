using System;
using System.Collections.Generic;

namespace LinkRipper
{
    public sealed class AudioTagCategory 
    {
        public string Name { get; }
        public Uri Address { get; }
        public IEnumerable<string> AudioTags { get; }        
        public AudioTagCategory(string name, Uri uri, IEnumerable<string> audioTags)
        {
            Name = name;
            Address = uri;
            AudioTags = audioTags;
        }
    }
}