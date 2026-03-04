using Newtonsoft.Json;
using StremioAddon.Models;

namespace ResourceFetcher.Models.Adapters;

public class AdapterForMovieOfTheNight(ShowObject obj) : IMeta, IMetaAdapter
{
    public AdapterForMovieOfTheNight() : this(new ShowObject())
    {
    }
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

    public List<IMeta> ConvertToStandardizedMetaData(string response)
    {
        var showObjects = JsonConvert.DeserializeObject<ShowObject[]>(response) ?? [];
        var metasList = new List<IMeta>();

        foreach (var showObject in showObjects)
        {
            var meta = new AdapterForMovieOfTheNight(showObject);
            metasList.Add(meta);
        }

        return metasList;
    }
}

public interface IMetaAdapter
{
    public List<IMeta> ConvertToStandardizedMetaData(string response);
}