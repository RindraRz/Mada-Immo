using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class DetailLocation
    {
        public int DetailLocationId { get; set; }

        public int LocationId { get; set; }

        public double Loyer { get; set; }

        public double Commission { get; set; }

        public DateOnly Mois { get; set; }

        public int NumMoisLocation { get; set; }

        [NotMapped]
        public bool EstPayee { get; set; }

        [NotMapped]
        public double GainAdmin { get; set; }

        [NotMapped]
        public double GainProprio { get; set; }

        [NotMapped]
        public int Moiss { get; set; }

        public virtual Location? LocationIdNavigation { get; set; }

        public bool GetPayer()
        {
            if (this.EstPayee)
            {
                return true;
            }
            return false;
        }
        public string GetStatut()
        {
            if (this.EstPayee)
            {
                return " Payee";
            }
            return "A payer";
        }
    }
}