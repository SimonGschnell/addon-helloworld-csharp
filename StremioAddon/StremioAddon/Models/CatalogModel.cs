using Newtonsoft.Json;

namespace StremioAddon.Models
{
    [System.Serializable]
    public class CatalogModel
    {
            [JsonProperty("metas", Required = Required.Always)]
            public Meta[] Metas { get; set; }
    }

    public enum CatalogId
    {
        netflixTop10,
        primeTop10,
        disneyTop10,
        appleTop10,
        hboTop10
    }
    
    public enum CatalogType
    {
        movie,
        series
    }
}