using System;
using System.Net;
using System.Web.Http;
using refactor_me.Context;
using System.Linq;
using System.Net.Http;
using refactor_me.Helpers;
using System.Collections.Generic;
using System.Data.Entity.Core;

namespace refactor_me.Controllers
{
     [RoutePrefix("products")]
    public class ProductsController : ApiController
    {       
        private readonly IProductService _productService;
        public ProductsController() { }
     

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            IEnumerable<Product> products = null;
            HttpResponseMessage errorResponse = null;

            try
            {
                products = _productService.GetAll();
                if (!products.Any() || products == null)
                     errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, "The product could not be found.");
            }
            catch
            {
                // In case our back-end throws an exception, don't allow this to be returned to the user.
                // Instead, ignore the exception and return InternalServerError.
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            // If there is an error, throw an HttpResponseException, which will automatically create an error response with a message in the body.
            if (errorResponse != null)
                throw new HttpResponseException(errorResponse);

            return products;           
        }


        [HttpGet]
        public IQueryable<Product> SearchByName(string name)
        {
            IQueryable<Product> productNames = null;
            HttpResponseMessage errorResponse = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    productNames = _productService.SearchByName(name.Trim());
                    if (productNames == null)
                        errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, "The product name could not be found.");
                }
                else
                {
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Characters to search product name is invalid.");
                }
            }
            catch
            {   // In case our back-end throws an exception, don't allow this to be returned to the user.
                // Instead, ignore the exception and return InternalServerError.
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            // If there is an error, throw an HttpResponseException, which will automatically create an error response with a message in the body.
            if (errorResponse != null)
                throw new HttpResponseException(errorResponse);

            return productNames;           

        }
    
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            Product product = null;
            HttpResponseMessage errorResponse = null;

            try
            {
                if (id != Guid.Empty || id != null)
                {
                    product = _productService.GetProduct(id);                    
                    if (product == null)
                    {
                        var message = string.Format("Product with id = {0} not found", id);
                        errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                    }
                }
                else
                {
                    var message = string.Format("Product id is invalid");
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
            }
            catch
            {   
                // In case our back-end throws an exception, don't allow this to be returned to the user.
                // Instead, ignore the exception and return InternalServerError.             
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            // If there is an error, throw an HttpResponseException, which will automatically create an error response with a message in the body.
            if (errorResponse != null)
            {
                throw new HttpResponseException(errorResponse);
            }

            return product;
        }


        [HttpPost]
        public HttpResponseMessage Create(Product product)
        {          

            HttpResponseMessage errorResponse = null;

            try
            {
                if (product == null)                
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product data must be supplied.");                
                else                
                    _productService.Create(product);
            }            
            catch
            {
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            if (errorResponse != null)
                throw new HttpResponseException(errorResponse);

            return Request.CreateResponse(HttpStatusCode.OK, product);

        }
        
        
        [HttpPut]
        public HttpResponseMessage Update(Guid id, Product product)
        {          

            HttpResponseMessage errorResponse = null;

            try
            {
                if (product == null)
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product data must be supplied.");
                else if (id == Guid.Empty || id == null)
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product id is invalid.");
                else
                {
                    var productExist = _productService.GetProduct(id);
                    if (productExist == null)
                    {
                        var message = string.Format("Product with id = {0} not found", id);
                        errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                    }
                    else
                    {
                        _productService.Update(id, product);
                    }
                }
            }
            catch
            {
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            if (errorResponse != null)
                throw new HttpResponseException(errorResponse);

            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

       
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {            

            HttpResponseMessage errorResponse = null;
            Product product = null;

            try
            {
                if (id != Guid.Empty || id != null)
                {
                    product = _productService.GetProduct(id);
                    if (product != null)
                        _productService.Delete(product);  
                    else
                        errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with Id = " + id.ToString() + " not found to delete");

                }
                else
                {
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The product id invalid.");
                }
            }           
            catch
            {
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            if (errorResponse != null)
            {
                throw new HttpResponseException(errorResponse);
            }

            return Request.CreateResponse(HttpStatusCode.OK, product);
        }


  
        [ActionName("options")]
        [HttpGet]
        public IQueryable<ProductOption> GetOptions(Guid productId)
        {
            IQueryable<ProductOption> productOptions = null;
            HttpResponseMessage errorResponse = null;

            try
            {
                if (productId != Guid.Empty || productId != null)
                {
                    productOptions = _productService.GetOptions(productId);
                    if (productOptions == null)
                    {
                        var message = string.Format("Product options with id = {0} not found", productId);
                        errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                    }
                }
                else
                {
                    var message = string.Format("Product option id is invalid");
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
            }
            catch
            {
                // In case our back-end throws an exception, don't allow this to be returned to the user.
                // Instead, ignore the exception and return InternalServerError.             
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            // If there is an error, throw an HttpResponseException, which will automatically create an error response with a message in the body.
            if (errorResponse != null)
            {
                throw new HttpResponseException(errorResponse);
            }

            return productOptions;
        }


        [ActionName("options")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {          

            ProductOption productOption = null;
            HttpResponseMessage errorResponse = null;

            try
            {
                if (id != null || productId != null)
                {
                    productOption = _productService.GetOption(productId,id);
                    if (productOption == null)
                    {
                        var message = string.Format("Product Option not found");
                        errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                    }
                }
                else
                {
                    var message = string.Format("Product option ids are invalid");
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
            }
            catch
            {
                // In case our back-end throws an exception, don't allow this to be returned to the user.
                // Instead, ignore the exception and return InternalServerError.             
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            // If there is an error, throw an HttpResponseException, which will automatically create an error response with a message in the body.
            if (errorResponse != null)
            {
                throw new HttpResponseException(errorResponse);
            }

            return productOption;
        }

        [ActionName("options")]
        [HttpPost]
        public HttpResponseMessage CreateOption(Guid productId, ProductOption option)
        {        

            HttpResponseMessage errorResponse = null;

            try
            {
                if (option == null)
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product option data must be supplied.");
                else
                    _productService.CreateOption(productId,option);
            }
            catch
            {
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            if (errorResponse != null)
                throw new HttpResponseException(errorResponse);

            return Request.CreateResponse(HttpStatusCode.Created, option);
        }


       
        [ActionName("options")]
        [HttpPut]
        public HttpResponseMessage UpdateOption(Guid id, ProductOption option)
        {

            HttpResponseMessage errorResponse = null;

            try
            {
                if (option == null)
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product option data must be supplied.");
                else if (id == null)
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product option id is invalid.");
                else
                {
                    var productOptionExist = _productService.GetOption(id, Guid.Empty);
                    if (productOptionExist == null)
                    {
                        var message = string.Format("Product option with id = {0} not found", id);
                        errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                    }
                    else
                    {
                        _productService.UpdateOption(id, option);
                    }
                }
            }
            catch
            {
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            if (errorResponse != null)
                throw new HttpResponseException(errorResponse);

            return Request.CreateResponse(HttpStatusCode.OK, option);
        }

        [ActionName("options")]
        [HttpDelete]
        public HttpResponseMessage DeleteOption(Guid id)
        {

            HttpResponseMessage errorResponse = null;
            ProductOption option = null;

            try
            {
                if (id != null)
                {
                    option = _productService.GetOption(Guid.Empty, id);
                    if (option != null)
                        _productService.DeleteOption(option);
                    else
                        errorResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product option with Id = " + id.ToString() + " not found to delete");

                }
                else
                    errorResponse = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The product option id invalid.");
            }
            catch
            {
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }

            if (errorResponse != null)
                throw new HttpResponseException(errorResponse);

            return Request.CreateResponse(HttpStatusCode.OK, option);
        }
    }
}
