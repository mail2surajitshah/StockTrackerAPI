using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTrackerAPI.Data;
using StockTrackerAPI.Models;

namespace StockTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly StockTrackerContext _context;

    public StocksController(StockTrackerContext context)
    {
        _context = context;
    }

    // GET: api/stocks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stock_Master>>> GetStocks()
    {
        return await _context.Stocks.ToListAsync();
    }

    // GET: api/stocks/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Stock_Master>> GetStock(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock is null)
            return NotFound();

        return stock;
    }

    // POST: api/stocks
    [HttpPost]
    public async Task<ActionResult<Stock_Master>> CreateStock(Stock_Master stock)
    {
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stock);
    }

    // PUT: api/stocks/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStock(int id, StockUpdateDto dto)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock is null)
            return NotFound();

        if (dto.Price.HasValue)
            stock.Price = dto.Price.Value;

        if (dto.ChangePercent.HasValue)
            stock.ChangePercent = dto.ChangePercent.Value;

        if (!string.IsNullOrWhiteSpace(dto.CompanyName))
            stock.CompanyName = dto.CompanyName;

        stock.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // PUT: api/stocks/bulk
    [HttpPut("bulk")]
    public async Task<IActionResult> BulkUpdateStocks(
        [FromBody] List<StockUpdateDto> updates)
    {
        if (updates is null || updates.Count == 0)
            return BadRequest("Provide at least one stock update.");

        var ids = updates.Select(u => u.Id).ToList();
        var stocks = await _context.Stocks
            .Where(s => ids.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id);

        var updated = 0;

        foreach (var update in updates)
        {
            if (!stocks.TryGetValue(update.Id, out var stock))
                continue;

            if (update.Price.HasValue)
                stock.Price = update.Price.Value;

            if (update.ChangePercent.HasValue)
                stock.ChangePercent = update.ChangePercent.Value;

            if (!string.IsNullOrWhiteSpace(update.CompanyName))
                stock.CompanyName = update.CompanyName;

            stock.LastUpdated = DateTime.UtcNow;
            updated++;
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            requested = updates.Count,
            updated,
            notFound = updates.Count - updated
        });
    }

    // DELETE: api/stocks/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStock(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock is null)
            return NotFound();

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}