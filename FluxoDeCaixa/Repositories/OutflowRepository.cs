using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluxoDeCaixa.Models;
using NHibernate;

namespace FluxoDeCaixa.Repositories
{
    public class OutflowRepository : IRepository<Outflow>
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

        public IEnumerable<Outflow> FindAll() => _session.Query<Outflow>().ToList();

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