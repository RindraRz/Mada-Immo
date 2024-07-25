using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class Proprietaire
    {
        public int ProprietaireId { get; set; }

        [Required(ErrorMessage = "Champ obligatoire")]
        [RegularExpression("^034\\d{7}$|^032\\d{7}$|^033\\d{7}$|^037\\d{7}$", ErrorMessage = "Le numéro de contact doit etre un 034 ou 032 ou 037 ou 033 ou 033.")]
        public string? Contact { get; set; }

        public ICollection<Bien>? Biens { get; } = new List<Bien>();
    }
}