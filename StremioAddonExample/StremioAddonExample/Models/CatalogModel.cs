using Newtonsoft.Json;

namespace StremioAddonExample.Models
{
    [System.Serializable]
    public class CatalogModel
    {
            [JsonProperty("metas", Required = Required.Always)]
            public Meta[] Metas { get; set; }
    }

    public enum CatalogId
    {
        netflixTop10
    }
}