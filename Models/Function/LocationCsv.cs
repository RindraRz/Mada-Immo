using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mada_immo.Models.Data
{
    public partial class LocationCsv
    {
        public LocationCsv() { }

        public LocationCsv(string[] valeurs)
        {
            try
            {
                this.Reference = valeurs[0].Trim();
                this.DateDebut = DateOnly.Parse(valeurs[1].Trim());
                this.Duree = int.Parse(valeurs[2].Trim());
                this.Client = valeurs[3].Trim();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static void InsertClient(ImmoContext context)
        {
            try
            {
                string request = "insert into client(email) ";
                request += " select distinct  client  from location_csv";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void InsertLocation(ImmoContext context)
        {
            try
            {
                string request = "insert into location(client_id,duree,date_debut,bien_id) ";
                request += @" select 
                c.client_id,
                l.duree,
                l.date_debut,
                b.bien_id
                from location_csv l 
                join client c on c.email = l.client 
                join bien b on b.reference = l.reference ";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void InsertDetailLocation(ImmoContext context)
        {
            try
            {
                string request = @"select 
            location_id ,
            loyer ,
            client_id ,
            l.duree ,
            l.date_debut,
            l.date_fin,
            l.bien_id,
            commission
            from location_csv l_csv 
            join v_location_bien l on l.date_debut=l_csv.date_debut and l.reference=l_csv.reference";
                List<Location> locations = context.Locations.FromSqlRaw(request).ToList();

                foreach (Location location in locations)
                {
                    Admin.AddDetailLocation(context, location);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static bool Insert(ImmoContext context, List<string[]> listes)
        {
            string errors = "";
            int i = 1;
            bool firstException = false;
            bool secondexception = false;
            using (var transaction = context.Database.BeginTransaction())
            {
                foreach (string[] une_ligne in listes)
                {
                    try
                    {
                        LocationCsv location = new LocationCsv(une_ligne);
                        context.LocationCsvs.Add(location);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        firstException = true;
                        errors += "ligne " + i + " : " + e.Message + " \n";
                    }
                    i++;
                }

                if (firstException == false)
                {
                    try
                    {
                        LocationCsv.InsertClient(context);
                        LocationCsv.InsertLocation(context);
                        LocationCsv.InsertDetailLocation(context);
                    }
                    catch (Exception e)
                    {
                        secondexception = true;
                        errors += "insertion base : " + e.Message + "\n";
                    }
                    finally
                    {
                        if (secondexception == true)
                        {
                            transaction.Rollback();
                            throw new Exception(errors);
                        }
                        else
                        {
                            transaction.Commit();

                        }

                    }

                }
                else
                {
                    transaction.Rollback();
                    throw new Exception(errors);
                }
                return true;
            }
        }

    }
}