using Bookstore.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.IntegrationTests;

public abstract class IntegrationTestBase : IClassFixture<IntegrationTestFactory>
{
    protected readonly BookstoreContext DbContext;

    public IntegrationTestBase(IntegrationTestFactory factory)
    {
        DbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<BookstoreContext>();
    }

    public async Task AddAsync<T>(T entity) where T : class
    {
        await DbContext.AddAsync(entity);
        DbContext.SaveChanges();
    }
}
