using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluxoDeCaixa.Models;
using NHibernate;

namespace FluxoDeCaixa.Repositories
{
    public class OutflowRepository
    {
        private ISession _session;
        public OutflowRepository(ISession session) => _session = session;
        public async Task Add(Outflow item)
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

        public List<Outflow> FindAll() => _session.Query<Outflow>().ToList();
        public List<Outflow> FindAllById(long id) => _session.Query<Outflow>().Where(x => x.Person.Id == id).ToList();

        //Filtros

        //Filtro de pesquisa
        public string CountAllOutflows()
        {
           var count =  _session.Query<Outflow>().Count();
            return count.ToString();
        }

        public string CountUserOutflows(long id)
        {
            var count = _session.Query<Outflow>().Where(x => x.Person.Id == id).Count();
            return count.ToString();
        }
        public List<Outflow> SearchFilter(Filter filter)
        {
            var result = _session.Query<Outflow>();

            result = _session.Query<Outflow>().Where(i =>
            i.Person.Name.Contains(filter.Name != null ? filter.Name : "[a-zA-Z]") &&
            i.OutflowDate >= (filter.Periodo > 0 ?
                filter.Periodo == 1 ? DateTime.Now.AddDays(-7) :
                filter.Periodo == 2 ? DateTime.Now.AddDays(-15) :
                filter.Periodo == 3 ? DateTime.Now.AddDays(-30) :
                filter.MinDate : filter.MinDate) &&
            i.OutflowDate <= (filter.Periodo > 0 ? DateTime.Now : filter.MaxDate != DateTime.MinValue ? filter.MaxDate : DateTime.MaxValue)
            );

            return result.ToList();
        }

        public double SumAmount()
        {
            return _session.Query<Outflow>().Sum(i => i.OutflowAmount);
        }


        public async Task<Outflow> FindByID(long id) => await _session.GetAsync<Outflow>(id);

        public async Task Remove(long id)
        {
            ITransaction transaction = null;
            try
            {
                transaction = _session.BeginTransaction();
                var item = await _session.GetAsync<Outflow>(id);
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

        public async Task Update(Outflow item)
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