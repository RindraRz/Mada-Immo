using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class TypeBien
    {
        public int TypeBienId { get; set; }

        public string? Nom { get; set; }

        public double Commission { get; set; }

        public ICollection<Bien>? Biens { get; } = new List<Bien>();

    }
}