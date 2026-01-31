using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Service.Domain.Entities;
using Stock.Service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Stock.Service.Persistence.Configuration
{
    public class AccountContextConfiguration : IEntityTypeConfiguration<Account>
    {
        private Guid[] _ids;

        public AccountContextConfiguration(Guid[] ids)
        {
            _ids = ids;
        }

        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder
                .HasData(
                new Account
                {
                    Id =new Guid("7d8e0925-525f-469e-bae7-942059e6e121"),
                    Type = TypeOfAccount.Cash,
                    Description = "Cash account for our users",
                    OwnerId = _ids[0]
                },
                new Account
                {
                    Id = new Guid("d0301f10-8b0b-4ed6-bc17-cf78a9e31d9c"),
                    Type = TypeOfAccount.Savings,
                    Description = "Savings account for our users",
                    OwnerId = _ids[1]
                },
                new Account
                {
                    Id = new Guid("57232482-1ae2-4114-bc26-54ba9b4d32df"),
                    Type = TypeOfAccount.Income,
                    Description = "Income account for our users",
                    OwnerId = _ids[1]
                }
           );
        }
    }
}
