namespace CarRentalSystem.Common.Services.Data
{
    using System.Threading.Tasks;
    using Common.Data.Models;

    public interface IDataService<in TEntity>
        where TEntity : class
    {
        Task MarkMessageAsPublished(int id);

        Task Save(TEntity entity, params Message[] messages);
    }
}
