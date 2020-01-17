using GymBookingNC19.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBookingNC19.Data.Repositories
{
    public class ApplicationUserGymClassRepository : IApplicationUserGymClassRepository
    {



        private readonly ApplicationDbContext context;



        public ApplicationUserGymClassRepository(ApplicationDbContext context)

        {

            this.context = context;

        }



        public void Add(ApplicationUserGymClass book)

        {

            context.ApplicationUserGymClasses.Add(book);

        }



        public void Remove(ApplicationUserGymClass attending)

        {

            context.ApplicationUserGymClasses.Remove(attending);

        }



        public async Task<List<GymClass>> GetAllBookingsAsync(string userId)

        {

            return await context.ApplicationUserGymClasses

                .Where(ag => ag.ApplicationUserId == userId)

                .IgnoreQueryFilters()

                .Select(ag => ag.GymClass)

                .ToListAsync();

        }

    }

}
