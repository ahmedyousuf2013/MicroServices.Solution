using Stock.Service.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Service.Integration.Test.Utilities
{
    public abstract class BaseTest(CustomApiFactory factory)
    {
        protected HttpClient Client { get; } = factory.CreateClient();
        protected SharedFixture SharedContext { get; } = factory.SharedFixture;
        protected StockServiceContext DbContext { get; } = factory.SharedFixture.SuperHeroDbContext;
    }
}
