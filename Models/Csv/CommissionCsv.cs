using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Csv
{
    public partial class CommissionCsv
    {
        public int CommissionCsvId { get; set; }
        public string? Type { get; set; }
        public double Commission { get; set; }
    }
}