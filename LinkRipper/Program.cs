using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LinkRipper
{  
    class Program
    {
        private static readonly Uri[] skillsKitLinks = new[]
        {
            "https://developer.amazon.com/docs/custom-skills/ambience-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/animal-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/battle-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/cartoon-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/foley-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/home-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/human-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/impact-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/magic-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/musical-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/nature-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/office-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/scifi-sounds.html",
            "https://developer.amazon.com/docs/custom-skills/transportation-sounds.html"
        }.Select(s => new Uri(s)).ToArray();

        static async Task Main(string[] args)
        {            
            var downloader = new HtmlTextDownloader();
            var audioElementRipper = new AudioElementRipper();
            var titleRipper = new AudioTitleRipper();
            var sourceRipper = new AudioSourceRipper();
            var audioLibraryEntriesToJsonConverter = new AudioLibraryEntriesToJsonConverter();

            var audioLibraryEntriesRetriever = new AudioLibraryEntriesRetriever(downloader, audioElementRipper, titleRipper, sourceRipper);            

            try
            {
                var request = ProcessArgs(args);

                var audioLibraryEntries = await audioLibraryEntriesRetriever.RetrieveAsync(skillsKitLinks);

                var jsonString = audioLibraryEntriesToJsonConverter.ConvertToJsonString(audioLibraryEntries);

                var path = Path.GetDirectoryName(request.WritePath.AbsolutePath + @"\");
        
                Console.WriteLine($"{request.WritePath.AbsolutePath}" );

                await File.WriteAllTextAsync(path, jsonString); 
            }
            catch(System.UriFormatException)
            {
                Console.Write("There was a problem processing your request.");
            }                       
        }

        private static LinkRequest ProcessArgs(string[] args)
        {
            if(args.Length < 1)
            {
                return new LinkRequest();
            }            

            return new LinkRequest();
        }

        private sealed class LinkRequest
        {
            public Uri WritePath { get; } = new Uri(Path.GetTempPath() + @"audioLibrary.json");
            public LinkRequest(string writePath)
            {
                WritePath = new Uri(writePath);
            }

            public LinkRequest() { }
        }
    }
}
