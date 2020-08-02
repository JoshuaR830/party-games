using System.Threading.Tasks;

namespace Chat.DatabasePopulator
{
    public interface IAddItemsToDatabase
    {
        Task SetDatabaseToUpdate();
    }
}