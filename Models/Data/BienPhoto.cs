using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mada_immo.Models.Data
{
    public partial class BienPhoto
    {
        public int BienPhotoId { get; set; }

        public int BienId { get; set; }

        public string? Path { get; set; }
    }
}