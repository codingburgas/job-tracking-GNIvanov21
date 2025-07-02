using JobTracking.Domain.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace JobTracking.DataAccess
{
    public interface IDataService
    {
        DatabaseModel GetDatabase();
        void SaveChanges(DatabaseModel database);
    }
}