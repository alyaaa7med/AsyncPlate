using AsyncPlate.Core.Interfaces;
using AsyncPlate.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {


        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public ICustomerRepo customers { get; }
        public IKitchenChefRepo kitchenChefs { get; }

        public IAdminRepo admins { get; }
        public IRefreshTokenRepo refreshtokens { get; }
        public IOneTimeTokenRepo onetimetokens { get; }

        public ISupplierRepo suppliers { get;  }
        public IInventoryRepo inventories { get; }
        public UnitOfWork(AppDbContext context, ICustomerRepo customerRepo, IKitchenChefRepo kitchenChefRepo,IAdminRepo adminRepo,
                IRefreshTokenRepo RefreshTokenRepo, IOneTimeTokenRepo onetimetokenRepo,
                ISupplierRepo supplierRepo, IInventoryRepo inventoryRepo)
        {
            _context = context;
            customers = customerRepo;
            kitchenChefs = kitchenChefRepo;
            admins = adminRepo;
            refreshtokens = RefreshTokenRepo;
            onetimetokens = onetimetokenRepo;
            suppliers = supplierRepo;
            inventories = inventoryRepo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null) await _transaction.CommitAsync();

        }

        public async Task RollBackTransactionAsync()
        {
            if (_transaction != null) await _transaction.RollbackAsync();
        }


        public void Dispose()
        {
            // 1. Dispose the transaction first
            _transaction?.Dispose();
            _transaction = null; // Important: Clear the reference

            // 2. Dispose the context
            _context.Dispose();
        }
    }
}