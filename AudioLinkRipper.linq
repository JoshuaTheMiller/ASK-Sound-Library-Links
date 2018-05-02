async Task Main()
{
	var downloader = new HtmlTextDownloader();
	var retriever = new AudioTagRetriever();	
	var writer = new FileWriter();

	var texts = new List<TextualAudioClipCategory>();
	var textRetrievalTasks = SkillsKitLinks.Select(async (l) =>
		{
			texts.Add(await downloader.GetHtmlAsStringFromUrlAsync(l));
		}).ToArray();

	await Task.WhenAll(textRetrievalTasks);

	var audioClips = texts.Select(retriever.RipAudioTags).SelectMany(tagList => tagList.PossibleTags, What).OrderBy(tag => tag.Name).ToList();

	var readMeText = CreateReadMe(audioClips);

	var path = Path.GetTempPath();
	path.Dump();
	writer.WriteToFile($"{path}example.md", readMeText);
}

public static AudioClipFactory factory = new AudioClipFactory();

public static AudioClip What(SplitAudioClipCategoryGroup group, string tag)
{
	return factory.CreateFromText(tag, group);
}

#region SkillKitLinks
public static AudioClipCategory[] SkillsKitLinks = new[]
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
	}.Select(CreateClipCategoryFromLink).ToArray();

#endregion
#region HtmlTextRetrieval
public sealed class HtmlTextDownloader
{
	public async Task<TextualAudioClipCategory> GetHtmlAsStringFromUrlAsync(AudioClipCategory category)
	{
		var request = (HttpWebRequest)WebRequest.Create(category.Link);		

		using (var response = (HttpWebResponse)await request.GetResponseAsync())
		{
			using (var receiveStream = response.GetResponseStream())
			using (var readStream = new StreamReader(receiveStream))
			{
				return CreateFromCategory(category, readStream.ReadToEnd());
			}
		}
	}
}
#endregion

#region AudioClipStuff
public sealed class AudioClipCategory
{
	public string Name { get; }
	public Uri Link { get; }
	public AudioClipCategory(string name, Uri link)
	{
		Name = name;
		Link = link;
	}

	public static AudioClipCategory Create(string name, string link)
	{
		return new AudioClipCategory(name, new Uri(link));
	}
}

public static AudioClipCategory CreateClipCategoryFromLink(string link)
{
	return AudioClipCategory.Create(link, link);
}

public sealed class AudioClip
{
	public string Name { get; }
	public Uri Source { get; }
	public AudioClipCategory Category {get;}

	public AudioClip(string name, Uri source, AudioClipCategory category)
	{
		this.Name = name;
		this.Source = source;
		this.Category = category;
	}

	public static AudioClip Create(string name, string source, AudioClipCategory category)
	{
		return new AudioClip(name, new Uri(source), category);
	}
}

public sealed class TextualAudioClipCategory
{
	public string Name { get; }
	public Uri Link { get; }
	public string Text { get; }
	public TextualAudioClipCategory(string name, Uri link, string text)
	{
		Name = name;
		Link = link;
		Text = text;
	}	
}

public static TextualAudioClipCategory CreateFromCategory(AudioClipCategory category, string text)
{
	return new TextualAudioClipCategory(category.Name, category.Link, text);
}

public sealed class SplitAudioClipCategoryGroup
{
	public string Name { get; }
	public Uri Link { get; }
	public IEnumerable<string> PossibleTags {get;}
	public SplitAudioClipCategoryGroup(string name, Uri link, IEnumerable<string> possibleTags)
	{
		Name = name;
		Link = link;
		PossibleTags = possibleTags;
	}
}

public static SplitAudioClipCategoryGroup Create(TextualAudioClipCategory category, IEnumerable<string> tags)
{
	return new SplitAudioClipCategoryGroup(category.Name, category.Link, tags);
}

public sealed class AudioTagRetriever
{
	private readonly string regexString = "<audio(?= title=)(.*?)</audio>";
	private readonly Regex regex;

	public AudioTagRetriever()
	{
		regex = new Regex(regexString);
	}

	public SplitAudioClipCategoryGroup RipAudioTags(TextualAudioClipCategory text)
	{
		var matches = regex.Matches(text.Text);
		var groups = new List<string>();

		foreach (var group in matches)
		{
			groups.Add(group.ToString());
		}

		return Create(text, groups);
	}
}

public sealed class AudioClipFactory
{
	private readonly string titleRegexString = "(?<=title=\"Example:)(.*?)(?=\")";
	private readonly string sourceRegexString = "(?<=src=\")(.*?)(?=\" )";

	private readonly Regex titleRegex;
	private readonly Regex sourceRegex;

	public AudioClipFactory()
	{
		titleRegex = new Regex(titleRegexString);
		sourceRegex = new Regex(sourceRegexString);
	}

	public AudioClip CreateFromText(string text, SplitAudioClipCategoryGroup category)
	{
		var title = titleRegex.Match(text).Captures[0].Value;
		var source = sourceRegex.Match(text).Captures[0].Value;

		return AudioClip.Create(title, source, new AudioClipCategory(category.Name, category.Link));
	}
}
#endregion
#region ConvertToReadMe
public string ConverAudioClipToLink(AudioClip clip)
{
	return $"[{clip.Name}]({clip.Category.Link})";
}

public string CreateReadMe(IEnumerable<AudioClip> clips)
{
	var stringBuilder = new StringBuilder();

	stringBuilder.AppendLine("# Alexa Skill Kit Sound Library Links");
	stringBuilder.AppendLine("For the the actual reference, check out the [Alexa Skills Kit Sound Library page](https://developer.amazon.com/docs/custom-skills/ask-soundlibrary.html)");

	stringBuilder.AppendLine();

	foreach (var clip in clips)
	{
		stringBuilder.Append("* ");
		stringBuilder.AppendLine(ConverAudioClipToLink(clip));
	}

	return stringBuilder.ToString();
}
#endregion
#region ConvertToHtml
public string ConvertAudioClipToAudioTag(AudioClip clip)
{
	return $@"<audio title=""{clip.Name}"" controls=""controls"" oncontextmenu=""return false"" controlslist=""nodownload"">Your browser does not support the <code>audio</code> <source src=""{clip.Source}"" type=""audio/mpeg""></source></audio>";

	//return $@"<audio controls title=""{clip.Name}""><source src=""{clip.Source}"" type=""audio/mpeg"">Your browser does not support the audio tag.</audio>";
}

public string ConvertToHtmlDocument(IEnumerable<AudioClip> clips)
{
	var sb = new StringBuilder();

	foreach (var clip in clips)
	{
		sb.Append("<tr>");
		sb.Append("<td>");
		sb.Append(clip.Name);
		sb.Append("</td>");
		sb.Append("<td>");
		sb.Append(ConvertAudioClipToAudioTag(clip));
		sb.Append("</td>");
		sb.Append("</tr>");
	}

	var htmlAsString = $@"<!DOCTYPE html><html><body><table style=""width:100%"">{sb.ToString()}</table></body></html>";

	return htmlAsString;
}
#endregion

public sealed class FileWriter
{
	public void WriteToFile(string path, string text)
	{
		File.WriteAllText(path, text);
	}
}
