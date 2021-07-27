using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using whatsapp2api.Contracts.Repositories;
using whatsapp2api.Contracts.Services;
using whatsapp2api.Entities;
using whatsapp2api.Helpers;
using whatsapp2api.Models.Auth;
using whatsapp2api.Models.Context;
using whatsapp2api.Models.User;

namespace whatsapp2api.Services
{
    public class UserService : IUserService
    {
        private readonly RepositoryContext _context;
        private readonly IUserRepository _repo;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public UserService(RepositoryContext context, IUserRepository repo)
        {
            _context = context;
            _repo = repo;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = await _repo.GetAllUsers();

            return users.Select(x => x.ToDto());
        }

        public async Task<IEnumerable<UserModel>> GetUsersMinusCaller(Guid senderId)
        {
            var users = await _repo.GetFilteredUsers(x => x.Id != senderId);

            return users.Select(x => x.ToDto());
        }

        public async Task<UserModel?> GetUserById(Guid id)
        {
            var entity = await _repo.GetOneByCondition(x => x.Id == id);

            return entity?.ToDto();
        }

        public async Task<UserModel?> GetUserByPhone(string phone)
        {
            var entity = await _repo.GetOneByCondition(x => x.Phone == phone);

            return entity?.ToDto();
        }

        public async Task<Guid?> GetUserIdByConnectionId(string connectionId)
        {
            var entity = await _repo.GetOneByCondition(x => x.SocketConnectionId == connectionId);

            return entity?.Id;
        }

        public async Task<UserModel?> CreateUser(UserCreate owner)
        {
            var isUserExist = await DoesUserExist(owner.Phone);

            if (isUserExist) return null;

            var entity = await _repo.CreateUser(owner);

            return entity.ToDto();
        }

        public async Task<UserModel?> UpdateUser(Guid id, UserUpdate owner)
        {
            var isUserExist = await DoesUserExist(id);

            if (isUserExist) return null;

            var entity = await _repo.UpdateUser(id, owner);

            return entity.ToDto();
        }

        public async Task<UserModel?> DeleteUser(Guid id)
        {
            var isUserExist = await DoesUserExist(id);

            if (isUserExist) return null;

            var entity = await _repo.DeleteUser(id);

            return entity.ToDto();
        }

        public async Task<Tuple<Tokens, string>?> Authenticate(UserAuthenticate owner)
        {
            var isUserExist = await DoesUserExist(owner.Phone);

            if (!isUserExist) return null;

            var entity = await _repo.GetOneByCondition(x => x.Phone == owner.Phone);

            if (entity is null) return null;

            var isValid = entity.ValidatePassword(owner.Password);

            if (!isValid) return null;

            var tokens = GenerateClientTokens(entity);
            var refreshToken = GenerateRefreshToken(entity);

            return tokens != null && refreshToken != null ? new Tuple<Tokens, string>(tokens, refreshToken) : null;
        }

        public async Task<Tuple<Tokens, string>?> Register(UserCreate owner)
        {
            var user = await CreateUser(owner);

            if (user is null) return null;

            var entity = await _repo.GetOneByCondition(x => x.Id == user.Id);

            if (entity is null) return null;

            var tokens = GenerateClientTokens(entity);
            var refreshToken = GenerateRefreshToken(entity);

            return tokens != null && refreshToken != null ? new Tuple<Tokens, string>(tokens, refreshToken) : null;
        }

        public Task<bool> DoesUserExist(Guid id)
        {
            return _context.Users.AnyAsync(x => x.Id == id);
        }

        public Task<bool> DoesUserExist(string phone)
        {
            return _context.Users.AnyAsync(x => x.Phone == phone);
        }

        private Tokens? GenerateClientTokens(UserEntity entity)
        {
            var key = Encoding.ASCII.GetBytes(new Guid().ToString()); // TODO: COGER KEY DEL ENVIRONMENT!

            var idTokenPayload = new IdTokenPayload {Id = entity.Id.ToString()};
            var accessTokenPayload = new AccessTokenPayload {Phone = entity.Phone, Username = entity.Username};

            var idTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {new Claim(nameof(idTokenPayload.Id), idTokenPayload.Id)}),
                Expires = DateTime.Now.AddHours(8),
                IssuedAt = DateTime.Now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var idToken = _tokenHandler.WriteToken(_tokenHandler.CreateToken(idTokenDescriptor));

            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(nameof(accessTokenPayload.Phone), accessTokenPayload.Phone),
                    new Claim(nameof(accessTokenPayload.Username), accessTokenPayload.Username)
                }),
                Expires = DateTime.Now.AddHours(8),
                IssuedAt = DateTime.Now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var accessToken = _tokenHandler.WriteToken(_tokenHandler.CreateToken(accessTokenDescriptor));

            return idToken != null && accessToken != null
                ? new Tokens {IdToken = idToken, AccessToken = accessToken}
                : null;
        }

        private string? GenerateRefreshToken(UserEntity entity)
        {
            var salt = Crypto.Salt(128);
            var hashPayload = string.Join(',', entity.Id, entity.Phone,
                DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            var hash = Encoding.ASCII.GetString(Crypto.Hash(hashPayload, salt));

            var refreshTokenPayload = new RefreshTokenPayload {Id = entity.Id.ToString(), Hash = hash};

            var key = Encoding.ASCII.GetBytes(new Guid().ToString()); // TODO: COGER KEY DEL ENVIRONMENT!

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(nameof(refreshTokenPayload.Id), refreshTokenPayload.Id),
                    new Claim(nameof(refreshTokenPayload.Hash), refreshTokenPayload.Hash)
                }),
                Expires = DateTime.Now.AddDays(7),
                IssuedAt = DateTime.Now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            return _tokenHandler.WriteToken(_tokenHandler.CreateToken(refreshTokenDescriptor));
        }

        public async Task<UserModel?> UserFromToken(string idToken)
        {
            var token = _tokenHandler.ReadJwtToken(idToken);

            var idClaim = token.Claims.FirstOrDefault(x => x.Type == nameof(IdTokenPayload.Id));

            if (idClaim == null) return null;

            var id = Guid.Parse(idClaim.Value);

            return await GetUserById(id);
        }

        public async Task SetUserConnectionId(Guid userId, string connectionId)
        {
            var updateDto = new UserUpdate {SocketConnectionId = connectionId};

            await _repo.UpdateUser(userId, updateDto);
        }

        public async Task RemoveUserConnectionId(string connectionId)
        {
            var user = await _repo.GetOneByCondition(x => x.SocketConnectionId == connectionId);

            if (user == null) return;

            var updateDto = new UserUpdate {SocketConnectionId = string.Empty};

            await _repo.UpdateUser(user.Id, updateDto);
        }
    }
}