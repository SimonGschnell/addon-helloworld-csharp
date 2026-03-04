namespace ResourceFetcher.Models;

public class ShowObject
{
    public string itemType { get; set; }
    public string showType { get; set; }
    public string id { get; set; }
    public string imdbId { get; set; }
    public string tmdbId { get; set; }
    public string title { get; set; }
    public string overview { get; set; }
    public int releaseYear { get; set; }
    public string originalTitle { get; set; }
    public Genres[] genres { get; set; }
    public string[] directors { get; set; }
    public string[] cast { get; set; }
    public int rating { get; set; }
    public int runtime { get; set; }
    public ImageSet imageSet { get; set; }
    public StreamingOptions streamingOptions { get; set; }
}

public class Genres
{
    public string id { get; set; }
    public string name { get; set; }
}

public class ImageSet
{
    public VerticalPoster verticalPoster { get; set; }
    public HorizontalPoster horizontalPoster { get; set; }
    public VerticalBackdrop verticalBackdrop { get; set; }
    public HorizontalBackdrop horizontalBackdrop { get; set; }
}

public class VerticalPoster
{
    public string w240 { get; set; }
    public string w360 { get; set; }
    public string w480 { get; set; }
    public string w600 { get; set; }
    public string w720 { get; set; }
}

public class HorizontalPoster
{
    public string w360 { get; set; }
    public string w480 { get; set; }
    public string w720 { get; set; }
    public string w1080 { get; set; }
    public string w1440 { get; set; }
}

public class VerticalBackdrop
{
    public string w240 { get; set; }
    public string w360 { get; set; }
    public string w480 { get; set; }
    public string w600 { get; set; }
    public string w720 { get; set; }
}

public class HorizontalBackdrop
{
    public string w360 { get; set; }
    public string w480 { get; set; }
    public string w720 { get; set; }
    public string w1080 { get; set; }
    public string w1440 { get; set; }
}

public class StreamingOptions
{
    public At[] at { get; set; }
}

public class At
{
    public Service service { get; set; }
    public string type { get; set; }
    public string link { get; set; }
    public string quality { get; set; }
    public Audios[] audios { get; set; }
    public Subtitles[] subtitles { get; set; }
    public Price price { get; set; }
    public bool expiresSoon { get; set; }
    public int availableSince { get; set; }
    public string videoLink { get; set; }
    public Addon addon { get; set; }
}

public class Service
{
    public string id { get; set; }
    public string name { get; set; }
    public string homePage { get; set; }
    public string themeColorCode { get; set; }
    public ImageSet1 imageSet { get; set; }
}

public class ImageSet1
{
    public string lightThemeImage { get; set; }
    public string darkThemeImage { get; set; }
    public string whiteImage { get; set; }
}

public class Audios
{
    public string language { get; set; }
    public string region { get; set; }
}

public class Subtitles
{
    public bool closedCaptions { get; set; }
    public Locale locale { get; set; }
}

public class Locale
{
    public string language { get; set; }
    public string region { get; set; }
}

public class Price
{
    public string amount { get; set; }
    public string currency { get; set; }
    public string formatted { get; set; }
}

public class Addon
{
    public string id { get; set; }
    public string name { get; set; }
    public string homePage { get; set; }
    public string themeColorCode { get; set; }
    public ImageSet2 imageSet { get; set; }
}

public class ImageSet2
{
    public string lightThemeImage { get; set; }
    public string darkThemeImage { get; set; }
    public string whiteImage { get; set; }
}