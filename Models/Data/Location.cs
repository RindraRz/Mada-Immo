

using System.ComponentModel.DataAnnotations.Schema;

namespace Mada_immo.Models.Data
{
    public partial class Location
    {
        public int LocationId { get; set; }

        public int ClientId { get; set; }

        public int Duree { get; set; }

        public DateOnly DateDebut { get; set; }


        public DateOnly DateFin { get; set; }

        public int BienId { get; set; }

        public ICollection<DetailLocation> DetailLocations { get; } = new List<DetailLocation>();

        public virtual Bien? BienIdNavigation { get; set; }

        [NotMapped]
        public int DureeLoyee { get; set; }
    }
}