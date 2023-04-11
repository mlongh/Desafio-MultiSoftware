using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models.Map
{
    public class InflowMap : ClassMapping<Inflow>
    {
      public InflowMap()
        {
            Id(x => x.Id, x =>
            {
                x.Generator(Generators.Increment);
                x.Type(NHibernateUtil.Int64);
                x.Column("Id");
            });

            Property(b => b.InflowDate, x =>
            {
                x.Type(NHibernateUtil.DateTime);
                x.NotNullable(true);
            });

            Property(b => b.InflowDescription, x =>
            {
                x.Length(700);
                x.Type(NHibernateUtil.String);
                x.NotNullable(true);
            });

            Property(b => b.InflowAmount, x =>
            {
                x.Type(NHibernateUtil.Double);
                x.Scale(2);
                x.Precision(15);
                x.NotNullable(true);
            });

            ManyToOne(x => x.Person, x =>
            {
                x.Column("PES_Id");
                x.NotNullable(true);
            });

            Table("Inflow");
        }
    }
}
