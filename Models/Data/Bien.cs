using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class Bien
    {
        public int BienId { get; set; }

        public string? Reference { get; set; }
        public string? Nom { get; set; }

        public string? Description { get; set; }

        public string? Region { get; set; }

        public double Loyer { get; set; }

        public int ProprietaireId { get; set; }

        public int TypeBienId { get; set; }

        public virtual Proprietaire? ProprietaireIdNavigation { get; set; }

        public virtual TypeBien? TypeBienIdNavigation { get; set; }

        public ICollection<Location>? Locations { get; } = new List<Location>();

        [NotMapped]
        public DateOnly DateDisponiblite { get; set; }
    }
}