using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluxoDeCaixa.Models;
using NHibernate;

namespace FluxoDeCaixa.Repositories
{
    public class InflowRepository
    {
        private ISession _session;
        public InflowRepository(ISession session) => _session = session;

        public async Task Add(Inflow item)
        {
            ITransaction transaction = null;
            try
            {
                transaction = _session.BeginTransaction();
                await _session.SaveAsync(item);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await transaction?.RollbackAsync();
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public List<Inflow> FindAll() => _session.Query<Inflow>().ToList();

        //Filtros

        //Filtro de pesquisa
        public List<Inflow> SearchFilter(Filter filter)
        {
            var result = _session.Query<Inflow>();

            result = _session.Query<Inflow>().Where(i =>
            i.Person.Name.Contains(filter.Name != null ? filter.Name : "[a-zA-Z]") &&
            i.InflowDate >= (filter.Periodo > 0 ?
                filter.Periodo == 1 ? DateTime.Now.AddDays(-7) :
                filter.Periodo == 2 ? DateTime.Now.AddDays(-15) :
                filter.Periodo == 3 ? DateTime.Now.AddDays(-30) :
                filter.MinDate : filter.MinDate) &&
            i.InflowDate <= (filter.Periodo > 0 ? DateTime.Now : filter.MaxDate != DateTime.MinValue ? filter.MaxDate : DateTime.MaxValue)
            );

            return result.ToList();
        }

       public double SumAmount()
        {
            return _session.Query<Inflow>().Sum(i => i.InflowAmount);
        }

        public async Task<Inflow> FindByID(long id) => await _session.GetAsync<Inflow>(id);



        public async Task Remove(long id)
        {
            ITransaction transaction = null;
            try
            {
                transaction = _session.BeginTransaction();
                var item = await _session.GetAsync<Inflow>(id);
                await _session.DeleteAsync(item);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await transaction?.RollbackAsync();
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public async Task Update(Inflow item)
        {
            ITransaction transaction = null;
            try
            {
                transaction = _session.BeginTransaction();
                await _session.UpdateAsync(item);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await transaction?.RollbackAsync();
            }
            finally
            {
                transaction?.Dispose();
            }
        }
    }
}