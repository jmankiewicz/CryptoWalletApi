using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using CryptoWalletApi.Entities;
using CryptoWalletApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CryptoWalletApi.Services;

public class AccountService
{
    private readonly CryptoWalletDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserContextService _userContextService;

    public AccountService(CryptoWalletDbContext dbContext, IMapper mapper, AuthenticationSettings authenticationSettings, IPasswordHasher<User> passwordHasher, IUserContextService userContextService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authenticationSettings = authenticationSettings;
        _passwordHasher = passwordHasher;
        _userContextService = userContextService;
    }

    public async Task RegisterAccountAsync(RegisterUserDto registerUserDto)
    {
        var user = _mapper.Map<User>(registerUserDto);

        var address = _mapper.Map<Address>(registerUserDto.Address);
        user.Address = address;

        var hashedPassword = _passwordHasher.HashPassword(user, registerUserDto.Password);

        user.HashedPassword = hashedPassword;

        await _dbContext
            .Users
            .AddAsync(user);
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task<string> LoginAccountAsync(LoginUserDto loginUserDto)
    {
        var user = await _dbContext
            .Users
            .Include(u => u.Address)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);

        if(user is null)
        {
            throw new BadHttpRequestException("Incorrect email or password. Try again.");
        }

        var isCorrectPassword = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, loginUserDto.Password);

        if(isCorrectPassword == PasswordVerificationResult.Failed)
        {
            throw new BadHttpRequestException("Incorrect email or password. Try again.");
        }

        return GenerateJwtToken(user);
    }

    public async Task UpdateAddressAsync(AddAddressDto addAddressDto)
    {
        var address = _mapper.Map<Address>(addAddressDto);
        address.Id = (int)_userContextService.GetUserAddressId;
        address.UserId = (int)_userContextService.GetUserId;

        _dbContext
            .Addresses
            .Update(address);
        await _dbContext
            .SaveChangesAsync();
    }

    public async Task<CountUsersDto> CountUsersAsync()
    {
        var numberOfUsers = await _dbContext
            .Users
            .CountAsync();

        var countUsersDto = new CountUsersDto
        {
            NumberOfUsers = numberOfUsers.ToString(),
        };

        return countUsersDto;
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Nickname),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("AddressId", user.Address.Id.ToString()),
            new Claim("Fee", user.Role.Fee.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: _authenticationSettings.Issuer,
            audience: _authenticationSettings.Issuer,
            claims: claims,
            expires: DateTime.Today.AddDays(_authenticationSettings.ExpireDays),
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}
