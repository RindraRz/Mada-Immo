using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class LocationCsv
    {
        public int LocationCsvId { get; set; }
        public string? Reference { get; set; }
        public DateOnly DateDebut { get; set; }

        public int Duree { get; set; }

        public string? Client { get; set; }
    }
}