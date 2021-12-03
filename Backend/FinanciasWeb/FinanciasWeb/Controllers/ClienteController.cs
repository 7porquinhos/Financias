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
    [RoutePrefix("api/Cliente")]
    public class ClienteController : ApiController
    {

        private readonly IBaseRepository<Cliente> _baseRepositoryCliente;

        public ClienteController()
        {
            _baseRepositoryCliente = new BaseRepository<Cliente>(new MySqlContext());
        }
        public ClienteController(IBaseRepository<Cliente> baseRepository)
        {
            _baseRepositoryCliente = baseRepository;
        }

        [HttpGet]
        public HttpResponseMessage GetClientes()
        {
            try
            {
                List<Cliente> listaClientes = _baseRepositoryCliente.Select().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, listaClientes);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [HttpPost]
        [Route("Consultar-CEP")]
        public HttpResponseMessage GetWSConsultaCEP(EnderecoCorreios enderecoCorreios)
        {
            var CEP = enderecoCorreios.CEP.Replace("-", "").Trim();
            EnderecoCorreios WSEndereco = new EnderecoCorreios();
            try
            {
                if (!string.IsNullOrWhiteSpace(CEP))
                {
                    using (var ws = new WSCorreios.AtendeClienteClient())
                    {
                        try
                        {
                            var endereco = ws.consultaCEP(CEP);
                            WSEndereco.CEP = CEP;
                            WSEndereco.Endereco = endereco.end;
                            WSEndereco.Bairro = endereco.bairro;
                            WSEndereco.Cidade = endereco.cidade;

                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                }
                if (WSEndereco != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, WSEndereco);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Web Service, inativo!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [HttpPost]
        //[Route("Insert-Cliente")]
        public HttpResponseMessage PostCliente(Cliente cliente)
        {
            if (cliente != null)
            {
                try
                {
                    _baseRepositoryCliente.Insert(cliente);

                    var response = Request.CreateResponse<Cliente>(HttpStatusCode.Created, cliente);
                    string uri = Url.Link("DefaultApi", new { id = cliente.Id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
                catch (Exception)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cliente não foi incluído com sucesso");
                }

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Valores vazios");
            }
        }

        [HttpPut]
        public HttpResponseMessage PutCliente(int id, Cliente cliente)
        {
            if (cliente != null)
            {
                if (id != cliente.Id)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ID´s diferentes!");

                var baseCliente = new BaseRepository<Cliente>(new MySqlContext());
                var client = baseCliente.Select(id);


                try
                {
                    var userPut = new Cliente()
                    {
                        Id = id,
                        Nome = cliente.Nome != client.Nome ? cliente.Nome : client.Nome,
                        Telefone = cliente.Telefone != client.Telefone ? cliente.Telefone : client.Telefone,
                        Email = cliente.Email != client.Email ? cliente.Email : client.Email,
                        CEP = cliente.CEP != client.CEP ? cliente.CEP : client.CEP,
                        EnderecoCliente = cliente.EnderecoCliente != client.EnderecoCliente ? cliente.EnderecoCliente : client.EnderecoCliente,
                        Cidade = cliente.Cidade != client.Cidade ? cliente.Cidade : client.Cidade
                    };

                    _baseRepositoryCliente.Update(userPut);


                    var response = Request.CreateResponse<Cliente>(HttpStatusCode.Created, cliente);
                    string uri = Url.Link("DefaultApi", new { id = cliente.Id });
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
        //[ActionName("Excluir-Cliente")]
        public HttpResponseMessage DeleteUsuario(int id)
        {
            try
            {
                Cliente cliente = _baseRepositoryCliente.Select(id);
                if (cliente == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Id Invalido, Cliente não encontrado!");
                }

                _baseRepositoryCliente.Delete(cliente.Id);

                return Request.CreateResponse(HttpStatusCode.OK, cliente);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }

}
