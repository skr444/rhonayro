using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    public sealed class Discipline : Entity
    {
        public string? Name { get; set; }

        public Discipline()
        {
            Name = null;
        }
    }
}
