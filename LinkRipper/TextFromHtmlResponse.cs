using System;

namespace LinkRipper
{  
    public sealed class TextFromHtmlResponse
    {
        public Uri Address { get; }
        public string Text { get; }

        public TextFromHtmlResponse(Uri address, string text)
        {
            Address = address;
            Text = text;
        }

        public static TextFromHtmlResponse Create(Uri uri, string text)
        {
            return new TextFromHtmlResponse(uri, text);
        }
    }
}