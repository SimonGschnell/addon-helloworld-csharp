using Microsoft.Extensions.Logging;
using Moq;
using ResourceFetcher;
using ResourceFetcher.Services;
using StremioAddonExample.Models;

namespace UnitTests;

public class FetchServiceOrchestratorTests
{

    private readonly Mock<ILogger<FetchServiceOrchestrator>> _logger = new();
    private Mock<IResourceFetcherHttpClient> _client;
    private Mock<IFetchServiceCollection> _serviceCollection;
    private Mock<IPersistenceService> _persistence;
    private Mock<IFetchService> _service1;
    
    [SetUp]
    public void Setup()
    {
        _client = new Mock<IResourceFetcherHttpClient>();
        _serviceCollection = new Mock<IFetchServiceCollection>();
        _persistence = new Mock<IPersistenceService>();
        _service1 = new Mock<IFetchService>();
        _client.Setup(c => c.FetchAsync(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync("test");
        _service1.Setup(service => service.ConvertToMetaData(It.IsAny<string>())).Returns([]);
        _service1.Setup(service => service.GetRequest(It.IsAny<CatalogType>())).Returns(new HttpRequestMessage(HttpMethod.Get, "https://google.com"));
        _serviceCollection.Setup(x => x.Services).Returns([_service1.Object]);
        _logger.Reset();
    }

    [Test]
    public async Task Orchestrator_Logs_Fetch_Date()
    {
        var orchestrator = new FetchServiceOrchestrator(_logger.Object, _client.Object, _serviceCollection.Object, _persistence.Object);

        await orchestrator.FetchServicesAsync();
        
        VerifyLog(_logger, LogLevel.Information, "Fetching shows:", Times.Once());
    }
    
    [Test]
    public async Task Orchestrator_Logs_Fetched_Service()
    {
        _service1.Setup(x => x.Name).Returns("netflixService");
        var orchestrator = new FetchServiceOrchestrator(_logger.Object, _client.Object, _serviceCollection.Object, _persistence.Object);

        await orchestrator.FetchServicesAsync();
        
        VerifyLog(_logger, LogLevel.Information, $"Fetching {CatalogType.movie.ToString()} for netflixService:", Times.Once());
        VerifyLog(_logger, LogLevel.Information, $"Fetching {CatalogType.series.ToString()} for netflixService:", Times.Once());
    }
    
    [Test]
    public async Task OrchestratorUsesServices()
    {
        var orchestrator = new FetchServiceOrchestrator(_logger.Object, _client.Object, _serviceCollection.Object, _persistence.Object);

        await orchestrator.FetchServicesAsync();
        
        _serviceCollection.Verify(x => x.Services, Times.AtLeastOnce);
    }
    
    [Test]
    public async Task Orchestrator_Fetches_Movie_And_Series_For_Services()
    {
        var orchestrator = new FetchServiceOrchestrator(_logger.Object, _client.Object, _serviceCollection.Object, _persistence.Object);

        await orchestrator.FetchServicesAsync();
        
        _serviceCollection.Verify(x => x.Services, Times.AtLeastOnce);
        _service1.Verify(x=>x.GetRequest(CatalogType.movie), Times.Once);
        _service1.Verify(x=>x.GetRequest(CatalogType.series), Times.Once);
    }
    
    [Test]
    public async Task Orchestrator_Converts_Service_Response_To_Metadata()
    {
        var orchestrator = new FetchServiceOrchestrator(_logger.Object, _client.Object, _serviceCollection.Object, _persistence.Object);

        await orchestrator.FetchServicesAsync();
        
        _service1.Verify(x=>x.ConvertToMetaData(It.IsAny<string>()), Times.Exactly(2));
    }
    
    [Test]
    public async Task Orchestrator_Persists_Catalog_Metadata()
    {
        _service1.Setup(x => x.CatalogId).Returns(CatalogId.netflixTop10);
        var orchestrator = new FetchServiceOrchestrator(_logger.Object, _client.Object, _serviceCollection.Object, _persistence.Object);

        await orchestrator.FetchServicesAsync();
        
        _persistence.Verify(x=>x.PersistCatalogMetaData(CatalogType.movie, CatalogId.netflixTop10, It.IsAny<string>()));
        _persistence.Verify(x=>x.PersistCatalogMetaData(CatalogType.series, CatalogId.netflixTop10, It.IsAny<string>()));
    }

    private static void VerifyLog(Mock<ILogger<FetchServiceOrchestrator>> logger, LogLevel level, string containsText, Times times)
    {
        logger.Verify(x =>
                x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(containsText)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }
}