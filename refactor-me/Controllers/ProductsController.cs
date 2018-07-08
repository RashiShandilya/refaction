using System;
using System.Net;
using System.Web.Http;
using refactor_me.Context;
using System.Linq;
using System.Net.Http;

namespace refactor_me.Controllers
{
     [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private ProductContext db = new ProductContext();
        
        [HttpGet]
        public IQueryable<Product> GetAll()
        {
            return db.Products;
        }


        [HttpGet]
        public IQueryable<Product> SearchByName(string name)
        {
            return db.Products.Where(b => b.Name.Contains(name.ToLower()));

        }
    
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            Product item = db.Products.FirstOrDefault(p => p.Id == id);
            if (item == null)
            {
                var message = string.Format("Product with id = {0} not found", id);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            else
            {
                return item;
            }
        }


        [HttpPost]
        public HttpResponseMessage Create(Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, product);
                message.Headers.Location = new Uri(Request.RequestUri + product.Id.ToString());

                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        
        
        [HttpPut]
        public HttpResponseMessage Update(Guid id, Product product)
        {
            try
            {
                var entity = db.Products.FirstOrDefault(e => e.Id == id);
                if (entity == null)
                {
                    var message = string.Format("Product with Id " + id.ToString() + " not found to update");
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                }
                else
                {
                    entity.Name = product.Name;
                    entity.Description = product.Description;
                    entity.DeliveryPrice = product.DeliveryPrice;
                    entity.Price = product.Price;

                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

       
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                var entity = db.Products.FirstOrDefault(p => p.Id == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Product with Id = " + id.ToString() + " not found to delete");
                }
                else
                {
                    db.Products.Remove(entity);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


  
        [ActionName("options")]
        [HttpGet]
        public IQueryable<ProductOption> GetOptions(Guid productId)
        {
            return db.ProductOptions.Where(p => p.ProductId == productId);
        }


        [ActionName("options")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            ProductOption option = db.ProductOptions.FirstOrDefault(p => p.Id == id && p.ProductId == productId);
            if (option == null)
            {
                var message = string.Format("Product Option with id = {0} and productId = {1} not found", id, productId);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            else
            {
                return option;
            }
        }

        [ActionName("options")]
        [HttpPost]
        public HttpResponseMessage CreateOption(Guid productId, ProductOption option)
        {
            try
            {
                option.ProductId = productId;
                db.ProductOptions.Add(option);
                db.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, option);
                message.Headers.Location = new Uri(Request.RequestUri + option.Id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [ActionName("options")]
        [HttpPut]
        public HttpResponseMessage UpdateOption(Guid id, ProductOption option)
        {
            try
            {
                var entity = db.ProductOptions.FirstOrDefault(e => e.Id == id);
                if (entity == null)
                {
                    var message = string.Format("Product Option with Id " + id.ToString() + " not found to update");
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                }
                else
                {
                    entity.ProductId = option.ProductId;
                    entity.Name = option.Name;
                    entity.Description = option.Description;

                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
           
        [ActionName("options")]
        [HttpDelete]
        public HttpResponseMessage DeleteOption(Guid id)
        {
            try
            {
                var entity = db.ProductOptions.FirstOrDefault(p => p.Id == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Product Option with Id = " + id.ToString() + " not found to delete");
                }
                else
                {
                    db.ProductOptions.Remove(entity);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
