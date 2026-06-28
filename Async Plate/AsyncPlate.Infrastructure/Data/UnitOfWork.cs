using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Application.Services.Implementation;
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
    public class UnitOfWork : IUnitOfWork
    {


        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;


        public ICustomerRepo customers { get; }
        public IKitchenChefRepo kitchenChefs { get; }
        public IAdminRepo admins { get; }
        public IRefreshTokenRepo refreshtokens { get; }
        public IOneTimeTokenRepo onetimetokens { get; }
        public ISupplierRepo suppliers { get; }
        public IInventoryRepo inventories { get; }
        public IProductRepo products { get; }
        public IProductExtraRepo ProductExtras { get; }

        public IRecipeRepo recipes { get; }
        public IOfferRepo offers { get; }
        public ICategoryRepo categories { get; }
        public IOrderRepo orders { get; }
        public INotificationRepo notifications { get; }

        public UnitOfWork(AppDbContext context, ICustomerRepo customerRepo, IKitchenChefRepo kitchenChefRepo, IAdminRepo adminRepo,
                IRefreshTokenRepo RefreshTokenRepo, IOneTimeTokenRepo onetimetokenRepo,
                ISupplierRepo supplierRepo, IInventoryRepo inventoryRepo, IProductRepo productRepo, IRecipeRepo recipeRepo,
                IOfferRepo offerRepo, ICategoryRepo categoryRepo, IOrderRepo orderRepo
            , IProductExtraRepo ProductExtraRepo , INotificationRepo notificationRepo)
        {
            _context = context;
            customers = customerRepo;
            kitchenChefs = kitchenChefRepo;
            admins = adminRepo;
            refreshtokens = RefreshTokenRepo;
            onetimetokens = onetimetokenRepo;
            suppliers = supplierRepo;
            inventories = inventoryRepo;
            recipes = recipeRepo;
            products = productRepo;
            ProductExtras =ProductExtraRepo;
            offers = offerRepo;
            categories = categoryRepo;
            orders = orderRepo;
            notifications = notificationRepo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Transaction already started.");

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null) return;

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollBackTransactionAsync()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

    }

}