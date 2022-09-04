using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service.IService;
using Service.Model;

namespace Service.Service
{
    public class JwtService : IJwtService

    {
        private readonly SignInManager<Data.User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Data.User> _userManager;

        public JwtService(SignInManager<Data.User> signInManager, RoleManager<IdentityRole> roleManager, UserManager<Data.User> userManager)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Token> GenerateAsync(Data.User user)
        {
            var secretKey = Encoding.UTF8.GetBytes("wJ4PT85k3Z3KYL9Drk");
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionKey = Encoding.UTF8.GetBytes("xxV93VD3XF1FTzOp");
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await _getClaimsAsync(user);

            var enumerable = claims as Claim[] ?? claims.ToArray();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "shoeeshop",
                Audience = "shoeeshop",
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow.AddMinutes(0),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(enumerable),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            var token = new Token(securityToken);
            token.Role = "user";

            if (enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value.ToLower() == "admin") != null)
            {
                token.Role = "admin";
            }

            return token;
        }

        public string GetTest()
        {
            return "Test";
        }


        private async Task<IEnumerable<Claim>> _getClaimsAsync(Data.User user)
        {
            var result = await _signInManager.ClaimsFactory.CreateAsync(user);
            var list = new List<Claim>(result.Claims);
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;
            list.Add(new Claim(securityStampClaimType, user.SecurityStamp.ToString()));
            list.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            var roles = await _userManager.GetRolesAsync(user);
            list.AddRange(roles.Select(item => new Claim(ClaimTypes.Role, item)));

            return list;
        }
    }
}
