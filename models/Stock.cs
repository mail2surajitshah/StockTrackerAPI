namespace StockTrackerAPI.Models;

public class Stock_Master
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal ChangePercent { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

public class StockUpdateDto
{
    public int Id { get; set; }
    public decimal? Price { get; set; }
    public decimal? ChangePercent { get; set; }
    public string? CompanyName { get; set; }
}