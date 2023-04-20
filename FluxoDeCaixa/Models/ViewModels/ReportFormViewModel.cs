using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models.ViewModels
{
    public class ReportFormViewModel
    {
        public List<Inflow> Inflow { get; set; }

        public List<Outflow> Outflow { get; set; }

        public Filter Filter { get; set; }
    }
}
