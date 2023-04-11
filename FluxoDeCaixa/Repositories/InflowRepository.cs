using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluxoDeCaixa.Models;
using NHibernate;

namespace FluxoDeCaixa.Repositories
{
    public class InflowRepository : IRepository<Inflow>
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

        public IEnumerable<Inflow> FindAll() => _session.Query<Inflow>().ToList();

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