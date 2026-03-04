namespace ResourceFetcher.Services;

public class FetchServiceCollection : IFetchServiceCollection
{
    private const string Country = "at";
    public List<IFetchService> Services { get; set; } = [];

    public FetchServiceCollection WithNetflixService()
    {
        Services.Add(new NetflixService(Country));
        return this;
    }
    
    public FetchServiceCollection WithPrimeService()
    {
        Services.Add(new Services(Country));
        return this;
    }
    
    public FetchServiceCollection WithDisneyService()
    {
        Services.Add(new DisneyService(Country));
        return this;
    }
    
    public FetchServiceCollection WithAppleService()
    {
        Services.Add(new AppleService(Country));
        return this;
    }
    
    public FetchServiceCollection WithHboService()
    {
        Services.Add(new HboService("us"));
        return this;
    }
}