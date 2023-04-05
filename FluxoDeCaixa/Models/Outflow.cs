using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models
{
    public class Outflow
    {
        public virtual string OutflowCode { get; set; }
        public virtual DateTime OutflowDate { get; set; }
        public virtual string OutflowDescription { get; set; }
        public virtual double OutflowAmount { get; set; }
    }
}
