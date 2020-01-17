using GymBookingNC19.Core.Models;
using System.Collections.Generic;

namespace GymBookingNC19.Core.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<GymClass> GymClasses { get; set; }
        public bool History { get; set; }
    }
}