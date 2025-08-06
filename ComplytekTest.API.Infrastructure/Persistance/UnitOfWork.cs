using ComplytekTest.API.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplytekTest.API.Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ComplytekTestDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ComplytekTestDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }
    }
}
