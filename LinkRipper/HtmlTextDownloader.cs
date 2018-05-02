using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LinkRipper
{  
	public sealed class HtmlTextDownloader
	{    
		public async Task<TextFromHtmlResponse> GetAsync(Uri uri)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);		

			using (var response = (HttpWebResponse)await request.GetResponseAsync())
			{
				using (var receiveStream = response.GetResponseStream())
				using (var readStream = new StreamReader(receiveStream))
				{
					return TextFromHtmlResponse.Create(uri, readStream.ReadToEnd());
				}
			}
		}
	}
}