using FinanciasWeb.Domain.Entities;
using FinanciasWeb.Domain.Interfaces;
using FinanciasWeb.Repository.Data.Context;
using FinanciasWeb.Repository.Data.Repository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FinanciasWeb.Controllers
{
    public class UserLoginController : ApiController
    {
        private readonly IBaseRepository<Usuario> _baseRepositoryUsuario;

        public UserLoginController()
        {
            _baseRepositoryUsuario = new BaseRepository<Usuario>(new MySqlContext());
        }
        public UserLoginController(IBaseRepository<Usuario> baseRepository)
        {
            _baseRepositoryUsuario = baseRepository;
        }

        [HttpPost]
        public HttpResponseMessage PostLogin(UserLogin usuario)
        {
            if (usuario != null)
            {
                try
                {
                    List<Usuario> listaUsuarios = _baseRepositoryUsuario.Select().ToList();

                    var user = listaUsuarios.Find(
                        x => x.Email.ToUpper() == usuario.UserName.ToUpper() &&
                        x.Senha.ToUpper() == usuario.Password.ToUpper());
                    if (user != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            token = GenerateJWToken(user)
                        });
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Usuario ou Senha incorretos!");
                    }
                    
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Valores vazios");
            }
        }

        private string GenerateJWToken(Usuario user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Nome)
            };


            var key = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(ConfigurationManager.AppSettings["Token"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
