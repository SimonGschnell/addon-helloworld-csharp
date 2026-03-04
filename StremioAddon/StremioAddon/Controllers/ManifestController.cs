using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StremioAddon.Models;

namespace StremioAddon.Controllers
{
    [ApiController]
    public class ManifestController : ControllerBase
    {
        private static readonly Manifest Manifest = new Manifest
        {
            id = "org.stremio.gschnell.top10",
            version = "1.0.0",
            name = "Top10Addon",
            description = "Stremio Addon for top10 movies and series of different streaming providers",
            logo = "https://www.stremio.com/website/stremio-logo-small.png",
            resources =
            [
                "catalog"
            ],
            types =
            [
                "movie", "series"
            ],
            idPrefixes = ["tt"],
            catalogs =
            [
                new Catalog { id = CatalogId.netflixTop10, type = "movie", name = CatalogId.netflixTop10.ToString()},
                new Catalog { id = CatalogId.netflixTop10, type = "series", name = CatalogId.netflixTop10.ToString()},
                new Catalog { id = CatalogId.primeTop10, type = "movie", name = CatalogId.primeTop10.ToString()},
                new Catalog { id = CatalogId.primeTop10, type = "series", name = CatalogId.primeTop10.ToString()},
                new Catalog { id = CatalogId.disneyTop10, type = "movie", name = CatalogId.disneyTop10.ToString()},
                new Catalog { id = CatalogId.disneyTop10, type = "series", name = CatalogId.disneyTop10.ToString()},
                new Catalog { id = CatalogId.appleTop10, type = "movie", name = CatalogId.appleTop10.ToString()},
                new Catalog { id = CatalogId.appleTop10, type = "series", name = CatalogId.appleTop10.ToString()},
                new Catalog { id = CatalogId.hboTop10, type = "movie", name = CatalogId.hboTop10.ToString()},
                new Catalog { id = CatalogId.hboTop10, type = "series", name = CatalogId.hboTop10.ToString()},
            ]
        };

        private static readonly string manifestJSON = JsonConvert.SerializeObject(Manifest, new JsonSerializerSettings(){ 
            Converters = new List<JsonConverter>() {
                    new StringEnumConverter()
                },
            Formatting = Formatting.Indented
        });

        [Route("manifest.json")]
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(Manifest, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                Formatting = Formatting.Indented
            });
        }
    }
}
