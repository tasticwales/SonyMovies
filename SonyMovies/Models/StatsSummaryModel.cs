using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SonyMovies.Models
{
    public class StatsSummaryModel
    {
        public int MovieId { get; set;}

        public string Title { get; set; }

        public long AverageWatchDurationS { get; set; }

        public long Watches { get; set; }

        public int ReleaseYear { get; set; }
    }
}
