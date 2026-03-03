using Newtonsoft.Json;

namespace StremioAddonExample.Models
{
    [System.Serializable]
    public class CatalogModel
    {
            [JsonProperty("metas", Required = Required.Always)]
            public IMeta[] Metas { get; set; }
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