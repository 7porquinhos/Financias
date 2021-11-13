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

namespace FinanciasWeb.Controllers
{
    [Authorize]
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
                return Request.CreateResponse<List<Usuario>>(HttpStatusCode.OK, listaUsuarios);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            
        }

        [HttpPost]
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
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Usuario não foi incluído com sucesso");
                }
                
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Valores vazios");
            }
        }
    }
}
