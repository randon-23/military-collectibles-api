using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface  IInsigniaSeriesDataAccess
    {
        Task<InsigniaSeries> GetInsigniaSeries(int id);
        Task<List<InsigniaSeries>> GetAllInsigniaSeries();
        Task CreateInsigniaSeries(InsigniaSeries insigniaSeries);
        Task UpdateInsigniaSeries(int id, InsigniaSeries insigniaSeries);
        Task DeleteInsigniaSeries(int id);
    }
    public class InsigniaSeriesDataAccess
    {
    }
}
