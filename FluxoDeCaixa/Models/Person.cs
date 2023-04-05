using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models
{
    public class Person
    {
        public virtual long? Id { get; set; }

        [Display(Name = "Nome")]
        public virtual string Name { get; set; }

        public virtual string Email { get; set; }

        [Display(Name = "Salário")]
        public virtual double Salary { get; set; }

        [Display(Name = "Limite da Conta")]
        public virtual double AccountLimit { get; set; }

        [Display(Name = "Valor mínimo em conta")]
        public virtual double MinimumValue { get; set; }
        public virtual double Balance { get; set; }

        public Person()
        {
        }

        public Person(int id, string name, string email, double salary, double accountLimit, double minimumValue)
        {
            Id = id;
            Name = name;
            Email = email;
            Salary = salary;
            AccountLimit = accountLimit;
            MinimumValue = minimumValue;
        }
    }

}
