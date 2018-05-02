using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkRipper
{
    public sealed class AudioLibraryEntriesRetriever
    {
        private readonly HtmlTextDownloader downloader;
        private readonly AudioElementRipper audioElementRipper;
        private readonly AudioTitleRipper audioTitleRipper;
        private readonly AudioSourceRipper audioSourceRipper;

        public AudioLibraryEntriesRetriever(HtmlTextDownloader downloader, AudioElementRipper audioElementRipper, AudioTitleRipper audioTitleRipper, AudioSourceRipper audioSourceRipper)
        {
            this.downloader = downloader;
            this.audioElementRipper = audioElementRipper;
            this.audioTitleRipper = audioTitleRipper;
            this.audioSourceRipper = audioSourceRipper;
        }

        public async Task<IEnumerable<AudioLibraryEntry>> RetrieveAsync(IEnumerable<Uri> links)
        {
            var downloadTasks = links.Select(u => downloader.GetAsync(u)).ToList();
            var htmlTextResponses = await Task.WhenAll(downloadTasks);

            return htmlTextResponses.Select(r => audioElementRipper.RipAudioTags(r.Address, r.Text))
                .SelectMany(cat => cat.AudioTags, (cat, tag) => Create(cat, tag, audioTitleRipper, audioSourceRipper))
                .ToList();
        }

        private static AudioLibraryEntry Create(AudioTagCategory category, string tag, AudioTitleRipper titleRipper, AudioSourceRipper sourceRipper)
        {
            var title = titleRipper.Rip(tag);
            var source = sourceRipper.Rip(tag);

            return new AudioLibraryEntry(category.Name, category.Address, title, new Uri(source));
        }
    }
}
