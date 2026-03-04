using Newtonsoft.Json;
using StremioAddon.Models;

namespace ResourceFetcher.Models.Adapters;

public class AdapterForMovieOfTheNight : IMetaAdapter
{
    public List<Meta> ConvertToStandardizedMetaData(string response)
    {
        var showObjects = JsonConvert.DeserializeObject<ShowObject[]>(response) ?? [];
        var metasList = new List<Meta>();

        foreach (var showObject in showObjects)
        {
            var meta = new Meta
            {
                Id = showObject.imdbId,
                Type = showObject.showType,
                Name = showObject.title,
                Genres = showObject.genres?.Select(gen => gen.name).ToArray() ?? [],
                Poster = $"https://images.metahub.space/poster/medium/{showObject.imdbId}/img"
            };
            if (meta.Id == null) continue;
            metasList.Add(meta);
        }

        return metasList;
    }
}

public interface IMetaAdapter
{
    public List<Meta> ConvertToStandardizedMetaData(string response);
}