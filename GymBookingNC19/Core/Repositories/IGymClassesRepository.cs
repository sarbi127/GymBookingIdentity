using System.Collections.Generic;
using System.Threading.Tasks;
using GymBookingNC19.Core.Models;

namespace GymBookingNC19.Data.Repositories
{
    public interface IGymClassesRepository
    {
        void Add(GymClass gymClass);
        Task<List<GymClass>> GetAllWithUsersAsync();
        bool GetAny(int id);
        Task<GymClass> GetAsync(int? id);
        Task<List<GymClass>> GetHistoryAsync();
        Task<GymClass> GetWithAttendingMembersAsync(int? id);
        void Remove(GymClass gymClass);
        void Update(GymClass gymClass);
        Task<IEnumerable<GymClass>> GetAllAsync();
    }
}