using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Client.Desktop.Ui.Services
{
    internal interface IDisciplineSessionService
    {
        IEnumerable<StartListEntry> Roster { get; }
    }
}
