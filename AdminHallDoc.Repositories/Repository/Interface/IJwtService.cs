using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using System.IdentityModel.Tokens.Jwt;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IJwtService
    {
        string GenerateJWTAuthetication(UserInfo userinfo);
        bool ValidateToken(string token, out JwtSecurityToken jwtSecurityTokenHandler);
    }
}
