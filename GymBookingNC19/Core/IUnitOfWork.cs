using System.Threading.Tasks;
using GymBookingNC19.Data.Repositories;

namespace GymBookingNC19.Core
{
    public interface IUnitOfWork
    {
        IApplicationUserGymClassRepository applicationUserGymClassRepository { get; }
        IGymClassesRepository gymClassesRepository { get; }

        Task CompleteAsync();
    }
}