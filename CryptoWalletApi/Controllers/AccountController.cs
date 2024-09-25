using CryptoWalletApi.Models;
using CryptoWalletApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _service;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAccount([FromBody] RegisterUserDto registerUserDto)
    {
        await _service.RegisterAccountAsync(registerUserDto);

        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult<CountUsersDto>> LoginAccount([FromBody] LoginUserDto loginUserDto)
    {
        var token = await _service.LoginAccountAsync(loginUserDto);

        return Ok(token);
    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult> UpdateAddress([FromBody] AddAddressDto addAddressDto)
    {
        await _service.UpdateAddressAsync(addAddressDto);

        return NoContent();
    }

    [HttpGet("count")]
    public async Task<ActionResult<string>> GetNumberOfUsers()
    {
        var numberOfUsers = await _service.CountUsersAsync();

        return Ok(numberOfUsers);
    }
}
