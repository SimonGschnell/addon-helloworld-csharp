using StremioAddon.Models;

namespace ResourceFetcher.Services;

public interface IPersistenceService
{
    public Task PersistCatalogMetaData(CatalogType catalogType, CatalogId catalogId, string data);
}

public class FilePersistence : IPersistenceService
{
    public async Task PersistCatalogMetaData(CatalogType catalogType, CatalogId catalogId, string data)
    {
        var outputPath = Environment.GetEnvironmentVariable("STREMIO_DATA");
        if (string.IsNullOrEmpty(outputPath))
        {
            throw new Exception("STREMIO_DATA env not set");
        }

        var directoryPath = Path.Combine(outputPath, catalogType.ToString());

        var filePath = Path.Combine(directoryPath, $"{catalogId.ToString()}.json");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        await File.WriteAllTextAsync(filePath, data);
    }
}