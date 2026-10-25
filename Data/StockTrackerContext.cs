using Microsoft.EntityFrameworkCore;
using StockTrackerAPI.Models;

namespace StockTrackerAPI.Data;

public class StockTrackerContext : DbContext
{
    public StockTrackerContext(DbContextOptions<StockTrackerContext> options)
        : base(options) { }

    public DbSet<Stock_Master> Stocks => Set<Stock_Master>();
}