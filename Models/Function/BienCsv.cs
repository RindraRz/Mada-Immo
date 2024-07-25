using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mada_immo.Models.Data
{
    public partial class BienCsv
    {
        public BienCsv() { }

        public BienCsv(string[] valeurs)
        {
            try
            {
                this.Reference = valeurs[0].Trim();
                this.Nom = valeurs[1].Trim();
                this.Description = valeurs[2].Trim();
                this.Type = valeurs[3].Trim();
                this.Region = valeurs[4].Trim();
                this.Loyer = double.Parse(valeurs[5].Trim());
                this.Proprietaire = valeurs[6].Trim();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static void InsertProprietaire(ImmoContext context)
        {
            try
            {
                string request = "insert into proprietaire(contact) ";
                request += " select distinct  proprietaire  from bien_csv";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void InsertBien(ImmoContext context)
        {
            try
            {
                string request = "insert into bien(reference,nom,description,region,loyer,type_bien_id,proprietaire_id) ";
                request += @"select 
                        reference,
                        b.nom,
                        b.description,
                        b.region,
                        b.loyer,
                        tb.type_bien_id,
                        p.proprietaire_id
                    from bien_csv b
                    join type_bien tb on tb.nom = b.type
                    join proprietaire p on p.contact = b.proprietaire ";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
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
                        BienCsv bien = new BienCsv(une_ligne);
                        context.BienCsvs.Add(bien);
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
                        BienCsv.InsertProprietaire(context);

                        BienCsv.InsertBien(context);

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