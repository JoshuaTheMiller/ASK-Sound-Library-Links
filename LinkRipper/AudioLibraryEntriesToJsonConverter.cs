using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LinkRipper
{
    public sealed class AudioLibraryEntriesToJsonConverter
    {
        public string ConvertToJsonString(IEnumerable<AudioLibraryEntry> entries)
        {
            var convertedList = entries.Select(AudioEntryDao.Convert).ToList();

            var jsonString = JsonConvert.SerializeObject(convertedList);

            return jsonString;
        }

        private sealed class AudioEntryDao
        {
            public string Category { get; set; }
            public string CategoryUri { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }

            public static AudioEntryDao Convert(AudioLibraryEntry entry)
            {
                return new AudioEntryDao()
                {
                    Category = entry.Category,
                    CategoryUri = entry.CategoryAddress.ToString(),
                    Name = entry.Name,
                    Address = entry.Address.ToString()
                };
            }
        }
    }
}
