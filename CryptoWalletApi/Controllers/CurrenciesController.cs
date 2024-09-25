using CryptoWalletApi.Models;
using CryptoWalletApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class CurrenciesController : ControllerBase
{
    private readonly CurrencyService _service;

    public CurrenciesController(CurrencyService service)
    {
        _service = service;
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddCurrency([FromBody] CurrencyDto newCurrencyDto)
    {
        await _service.AddCurrencyAsync(newCurrencyDto);

        return Created();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCurrency([FromBody] CurrencyDto updateCurrencyDto, [FromRoute] int id)
    {
        await _service.UpdateCurrencyAsync(updateCurrencyDto, id);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCurrency([FromRoute] int id)
    {
        await _service.DeleteCurrencyAsync(id);

        return NoContent();
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<ActionResult<List<CurrencyDto>>> GetAllCurrencies([FromQuery] PaginationSettings? paginationSettings)
    {
        var currencies = await _service.GetAllCurrenciesAsync(paginationSettings);

        return Ok(currencies);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<CurrencyDto>> GetCurrencyById([FromRoute] int id)
    {
        var currency = await _service.GetCurrencyByIdAsync(id);

        return Ok(currency);
    }
}
