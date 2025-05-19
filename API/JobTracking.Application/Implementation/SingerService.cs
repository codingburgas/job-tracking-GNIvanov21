namespace JobTracking.Aplication.Implementation;

public class SingerService : ISingerService
{
    protected DependencyProvider Provider { get; set; }

    public SingerService(DependencyProvider Provider)
    {
        Provider = provider;
    }

    public Task<List<Singer>> GetAllSingers(int page, int pageCount)
    {
        return Provider.Db.Singers
            .Skip((page - 1) * pageCount)
            .Take(pageCount)
            .ToListAsync();
    }

    public Task<Singer> GetSinger(int singerId)
    {
        return Provider.Db.Singers;
    }
    
    
    
    
}








































