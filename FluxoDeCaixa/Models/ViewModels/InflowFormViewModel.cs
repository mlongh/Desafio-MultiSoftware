using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models.ViewModels
{
    public class InflowFormViewModel
    {
        public Inflow Inflow { get; set; }

        public ICollection<Person> People { get; set; }
    }
}
