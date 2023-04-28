using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models
{
    public class Person
    {
        [Display(Name = "Código")]
        public virtual long Id { get; set; }

        [Display(Name = "Nome")]
        public virtual string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }

        [Display(Name = "Salário")]
        [DataType(DataType.Currency)]
        public virtual double? Salary { get; set; }

        [Display(Name = "Limite da Conta")]
        [DataType(DataType.Currency)]
        public virtual double? AccountLimit { get; set; }

        [Display(Name = "Valor mínimo em conta")]
        [DataType(DataType.Currency)]
        public virtual double MinimumValue { get; set; }

        [Display(Name = "Saldo")]
        [DataType(DataType.Currency)]
        public virtual double Balance { get; set; }

        [Display(Name = "Nome de Usuário")]
        public virtual string Username { get; set; }
       
        [Display(Name = "Senha")]
        public virtual string Password { get; set; }

        public Person()
        {
        }

        public Person(int id, string name, string email, double salary, double accountLimit, double minimumValue, string username, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Salary = salary;
            AccountLimit = accountLimit;
            MinimumValue = minimumValue;
            Username = username;
            Password = password;
        }

        
    }

}
