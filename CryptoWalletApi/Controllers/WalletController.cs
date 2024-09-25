using AutoMapper;
using CryptoWalletApi.Entities;
using CryptoWalletApi.Models;
using CryptoWalletApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly WalletService _service;

    public WalletController(WalletService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult> CreateWallet([FromBody] CreateWalletDto createWalletDto)
    {
        await _service.CreateWalletAsync(createWalletDto);

        return Created();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WalletDto>> OpenWallet([FromRoute] int id, [FromBody] string password)
    {
        var walletDto = await _service.OpenWalletAsync(id, password);

        return Ok(walletDto);
    }

    [HttpPost("deposit/{id}")]
    public async Task<ActionResult> ChargeWallet([FromRoute] int id, [FromBody] MoneyOperationsWalletDto moneyOperationsWalletDto)
    {
        await _service.ChargeWalletAsync(id, moneyOperationsWalletDto);

        return Ok();
    }

    [HttpPost("withdraw/{id}")]
    public async Task<ActionResult> WithdrawWallet([FromRoute] int id, [FromBody] MoneyOperationsWalletDto moneyOperationsWalletDto)
    {
        await _service.WithdrawWalletAsync(id, moneyOperationsWalletDto);

        return Ok();
    }

    [HttpPost("send/{id}")]
    public async Task<ActionResult> SendCurrenciesToOtherWallet([FromRoute] int id, [FromBody] SendMoneyOperationsWalletDto sendMoneyOperationsWalletDto)
    {
        await _service.SendCurrenciesToOtherWalletAsync(id, sendMoneyOperationsWalletDto);

        return Ok();
    }
}