using ResourceFetcher.Helpers;
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
        var directoryPath = Path.Combine(EnvironmentHelper.GetOutputPath(), catalogType.ToString());

        var filePath = Path.Combine(directoryPath, $"{catalogId.ToString()}.json");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        await File.WriteAllTextAsync(filePath, data);
    }
}