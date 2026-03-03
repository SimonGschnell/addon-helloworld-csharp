using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ResourceFetcher;
using ResourceFetcher.CronJobs;
using ResourceFetcher.Services;
using StremioAddonExample.Models;

namespace UnitTests;

public class Tests
{

    private readonly Mock<ILogger<FetchServiceOrchestrator>> _logger = new();
    private Mock<IResourceFetcherHttpClient> _client;
    private Mock<IFetchServiceCollection> _serviceCollection;
    private Mock<IPersistenceService> _persistence;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _client = new Mock<IResourceFetcherHttpClient>();
        _client.Setup(c => c.FetchAsync(It.IsAny<HttpRequestMessage>()))
            .ReturnsAsync("test");
        _serviceCollection = new Mock<IFetchServiceCollection>();
        _persistence = new Mock<IPersistenceService>();
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        var service1 = new Mock<IFetchService>();
        service1.Setup(x => x.CatalogId).Returns(CatalogId.netflixTop10);
        service1.Setup(service => service.ConvertToMetaData(It.IsAny<string>())).Returns([]);
        service1.Setup(service => service.GetRequest(It.IsAny<CatalogType>())).Returns(new HttpRequestMessage(HttpMethod.Get, "https://google.com"));
        _serviceCollection.Setup(x => x.Services).Returns([service1.Object]);
        var orchestrator = new FetchServiceOrchestrator(_logger.Object, _client.Object, _serviceCollection.Object, _persistence.Object);

        await orchestrator.FetchServicesAsync();
        
        _serviceCollection.Verify(x => x.Services, Times.AtLeastOnce);
        service1.Verify(x=>x.GetRequest(CatalogType.movie), Times.Once);
        service1.Verify(x=>x.GetRequest(CatalogType.series), Times.Once);
        service1.Verify(x=>x.ConvertToMetaData(It.IsAny<string>()), Times.Exactly(2));
        _persistence.Verify(x=>x.PersistCatalogMetaData(CatalogType.movie, CatalogId.netflixTop10, It.IsAny<string>()));
        _persistence.Verify(x=>x.PersistCatalogMetaData(CatalogType.series, CatalogId.netflixTop10, It.IsAny<string>()));
        VerifyLog(_logger, LogLevel.Information, "Fetching shows:", Times.Once());
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