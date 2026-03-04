using Newtonsoft.Json;
using StremioAddon.Models;

namespace ResourceFetcher.Models.Adapters;

public class AdapterForMovieOfTheNight : Meta, IMetaAdapter
{
    public AdapterForMovieOfTheNight(ShowObject obj)
    {
        Id = obj.imdbId;
        Type = obj.showType;
        Name = obj.title;
        Genres = obj.genres?.Select(gen => gen.name).ToArray() ?? [];
        Poster = obj.imageSet?.verticalPoster?.w240;
    }

    public List<Meta> ConvertToStandardizedMetaData(string response)
    {
        var showObjects = JsonConvert.DeserializeObject<ShowObject[]>(response) ?? [];
        var metasList = new List<Meta>();

        foreach (var showObject in showObjects)
        {
            var adapter = new AdapterForMovieOfTheNight(showObject);
            if (adapter.Id == null) continue;
            metasList.Add(adapter);
        }

        return metasList;
    }
}

public interface IMetaAdapter
{
    public List<Meta> ConvertToStandardizedMetaData(string response);
}