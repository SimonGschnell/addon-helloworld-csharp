using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StremioAddonExample.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StremioAddonExample.Controllers
{
    [ApiController]
    public class ManifestController : ControllerBase
    {
        private static readonly Manifest manifest = new Manifest
        {
            id = "org.stremio.gschnell.top10",
            version = "1.0.0",
            name = "CORE Example",
            description = "Sample addon made C# ASP.NET CORE 2.2 providing a few public domain movies",
            logo = "https://www.stremio.com/website/stremio-logo-small.png",
            resources = new object[] {
                "catalog",
            },
            types = new string[]
            {
                "movie", "series"
            },
            idPrefixes = new string[] { "tt" },
            catalogs = new Catalog[]
            {
                new Catalog(){ id = CatalogId.netflixTop10, type = "movie", name = CatalogId.netflixTop10.ToString()},
                new Catalog(){ id = CatalogId.netflixTop10, type = "series", name = CatalogId.netflixTop10.ToString()}
            }
        };

        private static readonly string manifestJSON = JsonConvert.SerializeObject(manifest, new JsonSerializerSettings(){ 
            Converters = new List<JsonConverter>() {
                    new StringEnumConverter()
                },
            Formatting = Formatting.Indented
        });

        [Route("manifest.json")]
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(manifest, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                Formatting = Formatting.Indented
            });
        }
    }
}
