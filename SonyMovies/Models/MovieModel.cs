﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SonyMovies.Models
{
    public class MovieModel
    {
        public int Id { get; set; }

        public int MovieId { get; set;}

        public string Title { get; set; }

        public string Language { get; set; }

        public string Duration { get; set; }

        public int ReleaseYear { get; set; }

    }
}
