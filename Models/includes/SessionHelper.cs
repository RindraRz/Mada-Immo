using System.Globalization;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mada_immo.Models.Data;
using CsvHelper;


namespace Mada_immo.Models.includes;
public static class SessionHelper
{

    public static void ResetDataBase(ImmoContext context)
    {
        string sql = "DO $$ DECLARE table_record RECORD; ";
        sql += " BEGIN FOR table_record IN ";
        sql += " SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE' LOOP";
        sql += " EXECUTE format('TRUNCATE TABLE %I RESTART IDENTITY CASCADE', table_record.table_name); ";
        sql += " END LOOP;END $$; ";
        context.Database.ExecuteSqlRaw(sql);
        context.SaveChanges();
        string request = " insert into admin (email,mdp) values('admin@gmail.com','1234') ";
        context.Database.ExecuteSqlRaw(request);
        context.SaveChanges();
    }

    public static string GetFormatted(double d)
    {
        if (d == 0) return "0";
        return d.ToString("#,##", CultureInfo.CurrentCulture);
    }

    public static int GetTotalMonths(DateOnly firstDate, DateOnly lastDate)
    {
        int yearsApart = lastDate.Year - firstDate.Year;
        int monthsApart = lastDate.Month - firstDate.Month;
        return (yearsApart * 12) + monthsApart + 1;
    }
    public static List<string[]> GetDataFromCsvFile(string filePath, string delimiter)
    {
        try
        {
            List<string[]> allData = new List<string[]>();

            // Configuration de CsvHelper pour lire les fichiers CSV avec les virgules comme séparateurs
            var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = delimiter,
                HasHeaderRecord = true, // Si la première ligne contient les en-têtes, mettez cela à true
                Encoding = Encoding.UTF8
            };

            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                csv.Read();
                csv.ReadHeader();
                // Lecture des données ligne par ligne
                while (csv.Read())
                {
                    // Récupération des champs de la ligne actuelle
                    var record = csv.Parser.Record;
                    allData.Add(record);
                }
            }
            return allData;
        }
        catch (Exception ex)
        {
            // Gérer les exceptions ici
            throw new Exception(ex.Message);
        }
    }

    public static string GetNomMois(int Mois)
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
