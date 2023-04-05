using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models
{
    public class Inflow
    {
        public virtual string InflowCode { get; set; }
        public virtual DateTime InflowDate { get; set; }
        public virtual string InflowDescription { get; set; }
        public virtual double InflowAmount { get; set; }
    }
}
