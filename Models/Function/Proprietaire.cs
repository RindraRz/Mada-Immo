using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mada_immo.Models.Data
{
    public partial class Proprietaire
    {
        public static Proprietaire LogIn(ImmoContext context, string numero)
        {
            Proprietaire? client = context.Proprietaires
                                    .Where(c => c.Contact == numero)
                                    .FirstOrDefault();
            if (client == null)
            {
                Proprietaire d = new Proprietaire();
                d.Contact = numero;
                context.Proprietaires.Add(d);
                context.SaveChanges();
                client = d;
            }
            return client;
        }

        public static List<Bien> GetBiens(ImmoContext context, int idProp)
        {
            List<Bien> biens = context.Biens.Where(c => c.ProprietaireId == idProp)
                                            .Include(c => c.TypeBienIdNavigation)
                                            .ToList();

            for (int i = 0; i < biens.Count; i++)
            {
                biens.ElementAt(i).DateDisponiblite = Bien.GetDateDisponibilite(context, biens.ElementAt(i).BienId);
            }
            return biens;
        }
        public static StatistiqueGain GetStatistiquePropByMonth(ImmoContext context, int idProp, DateOnly d)
        {
            //sup_inf mijery oe daty miakatra sa midina
            DateOnly date = new DateOnly(d.Year, d.Month, 1);
            StatistiqueGain statistiqueGain = new StatistiqueGain();
            string query = "";
            query = @" SELECT detail_location_id,location_id,loyer,commission,mois,num_mois_location 
                FROM v_location WHERE mois ='" + date + "' AND proprietaire_id = " + idProp;


            List<DetailLocation> locations = context.DetailLocations.FromSqlRaw(query)
                                                        .ToList();

            for (int i = 0; i < locations.Count; i++)
            {
                string query2 = "";
                query = @" SELECT *FROM v_location where location_id =" + locations.ElementAt(i).LocationId;
                Location location = context.Locations.FromSqlRaw(query).FirstOrDefault();
                locations.ElementAt(i).LocationIdNavigation = location;
            }
            double chiffreAffaire = 0;

            double gain = 0;
            foreach (DetailLocation location in locations)
            {

                double loca = location.Loyer;
                if (location.NumMoisLocation == 1)
                {
                    loca = location.Loyer / 2;
                    gain = gain + loca;
                    //gain = gain + (loca - (loca * location.Commission / 100));
                }
                else
                {
                    gain = gain + (loca - (loca * location.Commission / 100));

                }


            }

            statistiqueGain.Mois = date.Month;
            statistiqueGain.Annee = date.Year;
            statistiqueGain.ChiffreAffaire = chiffreAffaire;
            statistiqueGain.Gain = gain;

            return statistiqueGain;
        }

        public static List<StatistiqueGain> GetStatistiquePropEntreDeuxDates(ImmoContext context, int idProp, DateOnly d1, DateOnly d2)
        {
            List<StatistiqueGain> statistiqueGains = new List<StatistiqueGain>();
            DateOnly date1 = new DateOnly(d1.Year, d1.Month, 1);
            DateOnly date2 = new DateOnly(d2.Year, d2.Month, 1);
            while (date1 <= date2)
            {
                StatistiqueGain statistiqueGain = new StatistiqueGain();
                statistiqueGain = GetStatistiquePropByMonth(context, idProp, date1);
                statistiqueGains.Add(statistiqueGain);
                date1 = date1.AddMonths(1);
            }


            return statistiqueGains;
        }


    }
}