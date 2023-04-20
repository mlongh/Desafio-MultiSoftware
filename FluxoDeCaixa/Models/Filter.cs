using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models
{
    public class Filter
    {
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime MinDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime MaxDate { get; set; }

        public int Periodo { get; set; }

        public Filter()
        {

        }
    }
}
