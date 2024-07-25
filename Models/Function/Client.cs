using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mada_immo.Models.Data
{
    public partial class Client
    {
        public static Client LogIn(ImmoContext context, string mail)
        {
            Client? client = context.Clients
                                    .Where(c => c.Email == mail)
                                    .FirstOrDefault();
            if (client == null)
            {
                Client d = new Client();
                d.Email = mail;
                context.Clients.Add(d);
                context.SaveChanges();
                client = d;
            }
            return client;
        }

        public static List<Client> GetAll(ImmoContext context)
        {
            return context.Clients.ToList();
        }

        public static List<DetailLocation> GetLoyerByDate(ImmoContext context, int idClient, DateOnly date)
        {
            //sup_inf mijery oe daty miakatra sa midina
            string query = "";
            query = @" SELECT detail_location_id,location_id,loyer,commission,mois,num_mois_location 
                FROM v_location WHERE mois ='" + date + "' AND client_id = " + idClient;

            List<DetailLocation> lo = context.DetailLocations.FromSqlRaw(query).AsNoTracking()
                                                        .ToList();
            for (int i = 0; i < lo.Count; i++)
            {
                string query2 = "";
                query = @" SELECT *FROM v_location where location_id = " + lo.ElementAt(i).LocationId;
                Location location = context.Locations.FromSqlRaw(query).FirstOrDefault();
                location.BienIdNavigation = context.Biens.Where(c => c.BienId == location.BienId).FirstOrDefault();
                lo.ElementAt(i).LocationIdNavigation = location;
            }
            for (int i = 0; i < lo.Count; i++)
            {
                DateTime d = DateTime.Now;
                DateOnly now = new DateOnly(d.Year, d.Month, 1);
                DateOnly date1 = new DateOnly(date.Year, date.Month, 1);

                if (date1 <= now)
                {
                    lo.ElementAt(i).EstPayee = true;
                }
                else
                {
                    lo.ElementAt(i).EstPayee = false;
                }

            }
            return lo;
        }
        public static List<DetailLocation> GetLoyerEntreDeuxDates(ImmoContext context, int idClient, DateOnly d1, DateOnly d2)
        {
            List<DetailLocation> locations = new List<DetailLocation>();

            DateOnly date1 = new DateOnly(d1.Year, d1.Month, 1);
            DateOnly date2 = new DateOnly(d2.Year, d2.Month, 1);
            int i = 0;

            while (date1 <= date2)
            {
                List<DetailLocation> location = new List<DetailLocation>();
                StatistiqueGain statistiqueGain = new StatistiqueGain();


                location = GetLoyerByDate(context, idClient, date1);

                locations.AddRange(location);
                date1 = date1.AddMonths(1);
            }
            return locations;
        }
    }
}