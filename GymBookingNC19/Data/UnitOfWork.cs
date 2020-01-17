using GymBookingNC19.Core;

using GymBookingNC19.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBookingNC19.Data
{
     //Complite Work
        public class UnitOfWork : IUnitOfWork
        {
            private readonly ApplicationDbContext context;
            public IGymClassesRepository gymClassesRepository { get; private set; }

            public IApplicationUserGymClassRepository applicationUserGymClassRepository { get; private set; }

            public UnitOfWork(ApplicationDbContext context)
            {

                this.context = context;

                gymClassesRepository = new GymClassesRepository(context);

                applicationUserGymClassRepository = new ApplicationUserGymClassRepository(context);

            }

            public async Task CompleteAsync()
            {
                await context.SaveChangesAsync();

            }

        }

    
}
