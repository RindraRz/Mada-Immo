using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class Admin
    {
        public int AdminId { get; set; }

        public string? Email { get; set; }

        public string? Mdp { get; set; }

    }
}