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
    [RoutePrefix("api/Produto")]
    public class ProdutoController : ApiController
    {
        private readonly IBaseRepository<Produto> _baseRepositoryProduto;

        public ProdutoController()
        {
            _baseRepositoryProduto = new BaseRepository<Produto>(new MySqlContext());
        }
        public ProdutoController(IBaseRepository<Produto> baseRepository)
        {
            _baseRepositoryProduto = baseRepository;
        }

        [HttpGet]
        public HttpResponseMessage GetProdutos()
        {
            try
            {
                List<Produto> listaProdutos = _baseRepositoryProduto.Select().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, listaProdutos);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [HttpPost]
        //[Route("Insert-Produto")]
        public HttpResponseMessage PostProduto(Produto produto)
        {
            if (produto != null)
            {
                try
                {
                    _baseRepositoryProduto.Insert(produto);

                    var response = Request.CreateResponse<Produto>(HttpStatusCode.Created, produto);
                    string uri = Url.Link("DefaultApi", new { id = produto.Id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
                catch (Exception)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Produto não foi incluído com sucesso");
                }

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Valores vazios");
            }
        }

        [HttpPut]
        public HttpResponseMessage PutProduto(int id, Produto produto)
        {
            if (produto != null)
            {
                if (id != produto.Id)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "ID´s diferentes!");

                var baseProduto = new BaseRepository<Produto>(new MySqlContext());
                var product = baseProduto.Select(id);

                try
                {
                    var productPut = new Produto()
                    {
                        Id = id,
                        Nome = produto.Nome != product.Nome ? produto.Nome : product.Nome,
                        Valor = produto.Valor != product.Valor ? produto.Valor : product.Valor
                    };

                    _baseRepositoryProduto.Update(productPut);


                    var response = Request.CreateResponse<Produto>(HttpStatusCode.Created, produto);
                    string uri = Url.Link("DefaultApi", new { id = produto.Id });
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
        public HttpResponseMessage DeleteUsuario(int id)
        {
            try
            {
                Produto produto = _baseRepositoryProduto.Select(id);
                if (produto == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Id Invalido, Produto não encontrado!");
                }

                _baseRepositoryProduto.Delete(produto.Id);

                return Request.CreateResponse(HttpStatusCode.OK, produto);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
