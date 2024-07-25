using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class Intervall
    {
        public int Id { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }


        public static  bool IsIntervall(ImmoContext context,double m){
            Intervall intervall = context.Intervalls.FirstOrDefault();
            if(intervall.Min<=m && m<=intervall.Max){

                return true;
            }
            
            return false;
        }
    }
}