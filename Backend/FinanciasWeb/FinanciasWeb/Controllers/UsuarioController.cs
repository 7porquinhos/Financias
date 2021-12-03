using FinanciasWeb.Domain.Entities;
using FinanciasWeb.Domain.Interfaces;
using FinanciasWeb.Repository.Data.Context;
using FinanciasWeb.Repository.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FinanciasWeb.Controllers
{
    [Authorize]
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : ApiController
    {
        private readonly IBaseRepository<Usuario> _baseRepositoryUsuario;

        public UsuarioController()
        {
            _baseRepositoryUsuario = new BaseRepository<Usuario>(new MySqlContext());
        }
        public UsuarioController(IBaseRepository<Usuario> baseRepository)
        {
            _baseRepositoryUsuario = baseRepository;
        }

        [HttpGet]
        public HttpResponseMessage GetUsuarios()
        {
            try
            {
                List<Usuario> listaUsuarios = _baseRepositoryUsuario.Select().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, listaUsuarios);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [HttpPost]
        //[Route("Insert-Usuario")]
        public HttpResponseMessage PostUsuario(Usuario usuario)
        {
            if (usuario != null)
            {
                try
                {
                    _baseRepositoryUsuario.Insert(usuario);

                    var response = Request.CreateResponse<Usuario>(HttpStatusCode.Created, usuario);
                    string uri = Url.Link("DefaultApi", new { id = usuario.Id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
                catch (Exception)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Usuario não foi incluído com sucesso");
                }

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Valores vazios");
            }
        }

        [HttpPut]
        public HttpResponseMessage PutUsuario(int id, Usuario usuario)
        {
            if (usuario != null)
            {
                if (id != usuario.Id)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ID´s diferentes!");

                var baseUsuario = new BaseRepository<Usuario>(new MySqlContext());
                var user = baseUsuario.Select(id);


                try
                {
                    var userPut = new Usuario()
                    {
                        Id = id,
                        Nome = usuario.Nome != user.Nome ? usuario.Nome : user.Nome,
                        Telefone = usuario.Telefone != user.Telefone ? usuario.Telefone : user.Telefone,
                        Email = usuario.Email != user.Email ? usuario.Email : user.Email,
                        Senha = usuario.Senha != user.Senha ? usuario.Senha : user.Senha
                    };

                    _baseRepositoryUsuario.Update(userPut);


                    var response = Request.CreateResponse<Usuario>(HttpStatusCode.Created, usuario);
                    string uri = Url.Link("DefaultApi", new { id = usuario.Id });
                    response.Headers.Location = new Uri(uri);
                    return response;

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

        [HttpDelete]
        //[ActionName("Excluir-Usuario")]
        public HttpResponseMessage DeleteUsuario(int id)
        {
            try
            {
                Usuario usuario = _baseRepositoryUsuario.Select(id);
                if (usuario == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Id Invalido, Usuario não encontrado!");
                }

                _baseRepositoryUsuario.Delete(usuario.Id);

                return Request.CreateResponse(HttpStatusCode.OK, usuario);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
