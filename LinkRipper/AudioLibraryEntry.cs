using System;

namespace LinkRipper
{  
    public sealed class AudioLibraryEntry
    {
        public string Category { get; }
        public Uri CategoryAddress { get; }
        public string Name { get; }
        public Uri Address { get; }
        
        public AudioLibraryEntry(string category, Uri categoryUri, string name, Uri uri)
        {
            Category = category;
            CategoryAddress = categoryUri;
            Name = name;
            Address = uri;
        }
    }
}