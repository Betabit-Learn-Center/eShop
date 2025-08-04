using Microsoft.EntityFrameworkCore;
using eShop.Basket.API.Model;

namespace eShop.Basket.API.Repositories;

public class EfCoreBasketRepository(DbContext dbContext, ILogger<EfCoreBasketRepository> logger) : IBasketRepository
{
    private readonly DbSet<CustomerBasket> _baskets = dbContext.Set<CustomerBasket>();

    public async Task<bool> DeleteBasketAsync(string id)
    {
        var basket = await _baskets.FindAsync(id);
        if (basket == null)
        {
            return false;
        }

        _baskets.Remove(basket);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<CustomerBasket> GetBasketAsync(string customerId)
    {
        return await _baskets.FindAsync(customerId);
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var existingBasket = await _baskets.FindAsync(basket.BuyerId);
        if (existingBasket == null)
        {
            await _baskets.AddAsync(basket);
        }
        else
        {
            dbContext.Entry(existingBasket).CurrentValues.SetValues(basket);
        }

        await dbContext.SaveChangesAsync();
        logger.LogInformation("Basket item persisted successfully.");
        return basket;
    }
}