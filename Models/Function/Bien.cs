using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mada_immo.Models.Data
{
    public partial class Bien
    {
        public static List<Bien> GetAll(ImmoContext context)
        {
            return context.Biens.ToList();
        }

        public static DateOnly GetDateDisponibilite(ImmoContext context, int idBien)
        {
            string query = "";
            query = @" SELECT *
                FROM v_location where bien_id = " + idBien + " order by date_fin desc";
            Location location = context.Locations.FromSqlRaw(query).FirstOrDefault();
            if (location == null)
            {
                DateTime d = DateTime.Now;
                return new DateOnly(d.Year, d.Month, d.Day);
            }
            return location.DateFin.AddDays(1);
        }

    }
}