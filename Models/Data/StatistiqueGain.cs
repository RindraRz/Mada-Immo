using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class StatistiqueGain
    {
        public int Mois { get; set; }



        public int Annee { get; set; }

        public double ChiffreAffaire { get; set; }

        public double Gain { get; set; }

        public int NbLocation { get; set; }

        public bool ContainIntervall { get; set; }

        public bool ContainDebut { get; set; }














        public string GetMois(int Mois)
        {
            switch (Mois)
            {
                case 1: return "Janvier";
                case 2: return "Fevrier";
                case 3: return "Mars";
                case 4: return "Avril";
                case 5: return "Mai";
                case 6: return "Juin";
                case 7: return "Juillet";
                case 8: return "Aout";
                case 9: return "Septembre";
                case 10: return "Octobre";
                case 11: return "Novembre";
                case 12: return "Decembre";
                default: return "";
            }
        }
    }
}