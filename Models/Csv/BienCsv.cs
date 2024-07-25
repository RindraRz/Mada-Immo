using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class BienCsv
    {
        public int BienCsvId { get; set; }

        public string? Reference { get; set; }
        public string? Nom { get; set; }

        public string? Description { get; set; }

        public string? Type { get; set; }

        public string? Region { get; set; }
        public double Loyer { get; set; }

        public string? Proprietaire { get; set; }


    }
}