using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models
{
    public class Outflow
    {
        public virtual long Id { get; set; }

        [Display(Name = "Data")]
        public virtual DateTime OutflowDate { get; set; }
        
        [Display(Name = "Descrição")]
        public virtual string OutflowDescription { get; set; }
        
        [Display(Name = "Valor")]
        public virtual double OutflowAmount { get; set; }

        [Display(Name = "Pessoa")]
        public virtual Person Person { get; set; }

        public Outflow()
        {
        }

        public Outflow(long id, DateTime outflowDate, string outflowDescription, double outflowAmount)
        {
            Id = id;
            OutflowDate = outflowDate;
            OutflowDescription = outflowDescription;
            OutflowAmount = outflowAmount;
        }
    }
}
