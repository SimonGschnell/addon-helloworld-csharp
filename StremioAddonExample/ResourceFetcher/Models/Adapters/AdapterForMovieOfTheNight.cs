using StremioAddonExample.Models;

namespace ResourceFetcher.Models.Adapters;

public class AdapterForMovieOfTheNight(ShowObject obj) : IMeta
{
    public string Id
    {
        get => obj.imdbId;
        set => obj.imdbId = value;
    }
    public string Type 
    {
        get => obj.showType;
        set => obj.showType = value;
    }
    public string Name
    {
        get => obj.title;
        set => obj.title = value;
    }
    public string[] Genres => obj.genres.Select(gen => gen.name).ToArray();

    public string Poster
    {
        get => obj.imageSet.verticalPoster.w240;
        set => obj.imageSet.verticalPoster.w240 = value;
    }
}