using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetCoreAuthSeed.Dominio;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IRepUsuario _repUsuario;

    public LoginController(
        IConfiguration configuration,
        IRepUsuario repUsuario)
    {
        _configuration = configuration;
        _repUsuario = repUsuario;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var user = _repUsuario.RecuperarPorUserName(dto.Username);

        if (user == null)
        {
            return Unauthorized(new Response
            {
                Message = $"Não foi encontrado o usuário {dto.Username}",
                ErrorCode = EnumErro.ValidacaoLoginUsuarioNaoExiste,
            });
        }

        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

        var token = CreateToken(authClaims);
        var refreshToken = GenerateRefreshToken();

        return Ok(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        });
    }

    [HttpPost]
    [Route("refreshToken")]
    public async Task<IActionResult> RefreshToken(Token tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Client request inválido");
        }

        string? accessToken = tokenModel.AccessToken;
        string? refreshToken = tokenModel.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return BadRequest("access token/refresh token inválidos");
        }

        string username = principal.Identity.Name;
        var user = _repUsuario.RecuperarPorUserName(username);

        if (user == null)
        {
            return BadRequest("Access token/refresh token inválidos");
        }

        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                             .GetBytes(_configuration["JWT:SecretKey"]));
        _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out
            int tokenValidityInMinutes);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                               .GetBytes(_configuration["JWT:SecretKey"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                        out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                  !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                 StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Token inválido");

        return principal;
    }
}
