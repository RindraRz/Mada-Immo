using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mada_immo.Models.includes;
using Microsoft.EntityFrameworkCore;

namespace Mada_immo.Models.Data
{
    public partial class Admin
    {

        public static Admin? LogIn(ImmoContext context, string email, string mdp)
        {
            Admin? equipe = context.Admins
                                    .Where(c => c.Email == email && c.Mdp == mdp)
                                    .FirstOrDefault();
            return equipe;
        }



        public static StatistiqueGain GetStatistiqueGainByDate(ImmoContext context, DateOnly d)
        {
            //sup_inf mijery oe daty miakatra sa midina
            DateOnly date = new DateOnly(d.Year, d.Month, 1);
            StatistiqueGain statistiqueGain = new StatistiqueGain();
            string query = "";
            query = @" SELECT detail_location_id,location_id,loyer,commission,mois,num_mois_location 
                FROM v_location WHERE mois ='" + date + "'";

            List<DetailLocation> locations = context.DetailLocations.FromSqlRaw(query).ToList();
            for (int i = 0; i < locations.Count; i++)
            {
                string query2 = "";
                query = @" SELECT * FROM v_location where location_id =" + locations.ElementAt(i).LocationId;
                Location location = context.Locations.FromSqlRaw(query).FirstOrDefault();
                locations.ElementAt(i).LocationIdNavigation = location;
            }
            double chiffreAffaire = 0;

            double gain = 0;

            foreach (DetailLocation location in locations)
            {
                DateOnly deb = location.LocationIdNavigation.DateDebut;
                DateOnly de = new DateOnly(deb.Year, deb.Month, 1);
                if (de == d)
                {
                    statistiqueGain.ContainDebut = true;
                }
                double loca = location.Loyer;
                if (location.NumMoisLocation == 1)
                {
                    loca = loca / 2;
                }
                gain = gain + loca * (location.Commission / 100);
                chiffreAffaire = chiffreAffaire + location.Loyer;

            }

            statistiqueGain.Mois = date.Month;
            statistiqueGain.Annee = date.Year;
            statistiqueGain.ChiffreAffaire = chiffreAffaire;
          
            statistiqueGain.Gain = gain;
            statistiqueGain.NbLocation = locations.Count;

            
            statistiqueGain.ContainIntervall= Intervall.IsIntervall(context,chiffreAffaire);
        


            return statistiqueGain;
        }


        public static List<StatistiqueGain> GetStatistiqueGainsEntreDeuxDates(ImmoContext context, DateOnly d1, DateOnly d2)
        {
            List<StatistiqueGain> statistiqueGains = new List<StatistiqueGain>();
            DateOnly date1 = new DateOnly(d1.Year, d1.Month, 1);
            DateOnly date2 = new DateOnly(d2.Year, d2.Month, 1);
            while (date1 <= date2)
            {
                StatistiqueGain statistiqueGain = new StatistiqueGain();

                statistiqueGain = GetStatistiqueGainByDate(context, date1);
                statistiqueGains.Add(statistiqueGain);

                date1 = date1.AddMonths(1);
            }
            return statistiqueGains;
        }

        public static void AddDetailLocation(ImmoContext context, Location l)
        {
            string query2 = "";
            query2 = @" SELECT * FROM v_location_date_fin where location_id =" + l.LocationId;
            Location location = context.Locations.FromSqlRaw(query2)
                                                    .Include(c => c.BienIdNavigation)
                                                    .ThenInclude(c => c.TypeBienIdNavigation)
                                                    .FirstOrDefault();

            DateOnly dateDebut = new DateOnly(location.DateDebut.Year, location.DateDebut.Month, 1);
            DateOnly date2 = new DateOnly(dateDebut.Year, dateDebut.Month, 1);
            DateOnly dateFin = date2.AddMonths(location.Duree).AddDays(-1);
            int num_mois = 1;

            while (dateDebut <= dateFin)
            {
                DetailLocation detail = new DetailLocation();
                detail.LocationId = location.LocationId;

                if (num_mois == 1)
                {
                    detail.Loyer = location.BienIdNavigation.Loyer * 2;
                    detail.Commission = 100;
                }
                else
                {
                    detail.Loyer = location.BienIdNavigation.Loyer;
                    detail.Commission = location.BienIdNavigation.TypeBienIdNavigation.Commission;
                }
                detail.Mois = dateDebut;
                detail.NumMoisLocation = num_mois;

                context.DetailLocations.Add(detail);
                context.SaveChanges();
                dateDebut = dateDebut.AddMonths(1);
                num_mois++;
            }

        }

        public static void AddLocation(ImmoContext context, Location location)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {


                    context.Add(location);
                    context.SaveChanges();
                    AddDetailLocation(context, location);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                }
            }
        }
        public static List<DetailLocation> GetDetailLocationByDate(ImmoContext context, DateOnly d)
        {
            DateOnly date = new DateOnly(d.Year, d.Month, 1);
            StatistiqueGain statistiqueGain = new StatistiqueGain();
            string query = "";
            query = @" SELECT detail_location_id,location_id,loyer,commission,mois,num_mois_location 
                FROM v_location WHERE mois ='" + date + "'";

            List<DetailLocation> locations = context.DetailLocations.FromSqlRaw(query).ToList();

            return locations;
        }
        public static List<Location> GetAllLocations(ImmoContext context)
        {
            List<Location> locations = context.Locations.FromSqlRaw("select * from v_location_date_fin")
                        .Include(c => c.BienIdNavigation)
                        .ToList();
            return locations;
        }

        public static List<DetailLocation> GetDetailLocation(ImmoContext context, int idLocation)
        {
            string query = "";
            query = @" SELECT detail_location_id,location_id,loyer,commission,mois,num_mois_location 
                FROM v_location WHERE location_id =" + idLocation;
            List<DetailLocation> details = context.DetailLocations.FromSqlRaw(query).ToList();
            for (int i = 0; i < details.Count; i++)
            {
                DateTime d = DateTime.Now;
                DateOnly now = new DateOnly(d.Year, d.Month, d.Day);
                double loca = details.ElementAt(i).Loyer;


                if (details.ElementAt(i).Mois <= now)
                {
                    details.ElementAt(i).EstPayee = true;
                }
                else
                {
                    details.ElementAt(i).EstPayee = false;
                }
            }

            return details;
        }
    }
}