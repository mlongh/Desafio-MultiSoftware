using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Models.Map
{
    public class OutflowMap : ClassMapping<Outflow>
    {
        public OutflowMap()
        {
            Id(x => x.OutflowCode, x =>
            {
                x.Generator(Generators.UUIDString);
                x.Type(NHibernateUtil.String);
                x.Column("Id");
            });

            Property(b => b.OutflowDate, x =>
            {
                x.Type(NHibernateUtil.DateTime);
                x.NotNullable(true);
            });

            Property(b => b.OutflowDescription, x =>
            {
                x.Length(700);
                x.Type(NHibernateUtil.String);
                x.NotNullable(true);
            });

            Property(b => b.OutflowAmount, x =>
            {
                x.Type(NHibernateUtil.Double);
                x.Scale(2);
                x.Precision(15);
                x.NotNullable(true);
            });

            Table("Outflow");
        }
    }
}
