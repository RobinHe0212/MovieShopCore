using MovieShop.Core.ApiModels.Response;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Infrastructure.Repositories
{
    public class PurchasedRepository : EfRepository<Purchase>, IPurchaseRepository
    {
        public PurchasedRepository(MovieShopDbContext dbContext) : base(dbContext)
        {

        }

    }
}
