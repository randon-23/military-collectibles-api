using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.DataAccessLayer
{
    public interface ILiteratureSeriesDataAccess
    {
        Task<LiteratureSeries> GetLiteratureSeries(int id);
        Task<List<LiteratureSeries>> GetAllLiteratureSeries();
        Task CreateLiteratureSeries(LiteratureSeries literatureSeries);
        Task UpdateLiteratureSeries(int id, LiteratureSeries literatureSeries);
        Task DeleteLiteratureSeries(int id);
    }
    public class LiteratureSeriesDataAccess
    {
    }
}
