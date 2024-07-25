using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mada_immo.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace Mada_immo.Models.Csv
{
    public partial class CommissionCsv
    {
        public CommissionCsv() { }
        public CommissionCsv(string[] valeurs)
        {
            try
            {
                this.Type = valeurs[0].Trim();
                this.Commission = double.Parse(valeurs[1].Trim().Replace("%", ""));


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static void InsertTypeBien(ImmoContext context)
        {
            try
            {
                string request = "insert into type_bien(nom,commission) ";
                request += " select distinct  type , commission   from commission_csv";
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
                        CommissionCsv commission = new CommissionCsv(une_ligne);
                        context.CommissionCsvs.Add(commission);
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
                        CommissionCsv.InsertTypeBien(context);

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