using MeterReadingsDatabase.Models;
using MeterReadingsDatabase;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;

namespace MeterReadingApiIntergrationTests
{
    internal class IntergrationTestBase : IAsyncDisposable
    {
        private IEnumerable<(int id, string firstName, string secondName)> testAccounts = new List<(int, string, string)>()
            {
                (2344,"Tommy","Test"      ),
                (2233,"Barry","Test"      ),
                (8766,"Sally","Test"      ),
                (2345,"Jerry","Test"      ),
                (2346,"Ollie","Test"      ),
                (2347,"Tara","Test"       ),
                (2348,"Tammy","Test"      ),
                (2349,"Simon","Test"      ),
                (2350,"Colin","Test"      ),
                (2351,"Gladys","Test"     ),
                (2352,"Greg","Test"       ),
                (2353,"Tony","Test"       ),
                (2355,"Arthur","Test"     ),
                (2356,"Craig","Test"      ),
                (6776,"Laura","Test"      ),
                (4534,"JOSH","TEST"       ),
                (1234,"Freya","Test"      ),
                (1239,"Noddy","Test"      ),
                (1240,"Archie","Test"     ),
                (1241,"Lara","Test"       ),
                (1242,"Tim","Test"        ),
                (1243,"Graham","Test"     ),
                (1244,"Tony","Test"       ),
                (1245,"Neville","Test"    ),
                (1246,"Jo","Test"         ),
                (1247,"Jim","Test"        ),
                (1248,"Pam","Test"        )
            };
        protected MsSqlContainer _msSqlContainer;
        protected WebApplicationFactory<Program> appFactory;

       [SetUp]
        public async Task Setup()
        {
            _msSqlContainer
                    = new MsSqlBuilder()
                    .Build();

            appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices((IServiceCollection services) =>
                    {
                        services.RemoveAll<DbContextOptions<MeterReadingDbContext>>();
                        services.RemoveAll<DbConnection>();
                        services.AddDbContext<MeterReadingDbContext>(options =>
                        {
                            options.UseSqlServer(_msSqlContainer.GetConnectionString());
                        });
                    });
                });

            await _msSqlContainer.StartAsync();
            var scope = appFactory.Services.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<MeterReadingDbContext>();
            dbContext.Database.EnsureCreated();
            foreach (var account in testAccounts)
            {
                dbContext.Accounts.Add(new Account()
                {
                    AccountId = account.id,
                    FirstName = account.firstName,
                    LastName = account.secondName

                });

            }
            dbContext.SaveChanges();


        }



        public async ValueTask DisposeAsync()
        {
            await _msSqlContainer.StopAsync();
            await _msSqlContainer.DisposeAsync();
            await appFactory.DisposeAsync();
        }
    }
}
