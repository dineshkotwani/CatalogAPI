using CatalogAPI.Contracts;
using CatalogAPI.Models;
using CatalogAPI.Repository;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CatalogAPI.Controllers
{
    public class ProductsController : ApiController
    {
        private ICatalogRepository<Product> _productobj;


        public ProductsController(ICatalogRepository<Product> productobj)
        {
            _productobj = productobj;
        }

        // GET: api/products
        public async Task<HttpResponseMessage> Get()
        {
            var results = await _productobj.GetProductsAsync();
           
            if (results != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            else
            {
                string message = "Something went wrong , please try again";
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }

        }

        // GET: api/products/5
        public async Task<HttpResponseMessage> Get(string id)
        {
            var result = await _productobj.GetItemAsync(id);

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                string message = "Product doesn't exist or was not found ";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
        }

        // POST: api/products
        public async Task<HttpResponseMessage> Post([FromBody]Product product)
        {
          
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(product.ProductName))
            {
                var existingproducts = await _productobj.GetProductsAsync();
                if (existingproducts.Count() > 0)
                {
                    var existing = existingproducts.Where(c => c.ProductName == product.ProductName);
                    if (existing.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Product already exists , please add a new unique product");
                    }

                }
                product.Type = typeof(Product).Name;
                Document createdproduct = await _productobj.CreateItemAsync(product);
                if (createdproduct != null)
                    return Request.CreateResponse(HttpStatusCode.OK, createdproduct);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong , please try again");

            }
            else
            {
                string message = "Enter valid Product Name";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

        // PUT: api/products/5
        public async Task<HttpResponseMessage> Put(string id, [FromBody]Product product)
        {
           
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(product.ProductName) && !string.IsNullOrWhiteSpace(id))
            {
                var existingproducts = await _productobj.GetProductsAsync();
                if (existingproducts.Count() > 0)
                {
                    var existing = existingproducts.Where(c => c.ProductName == product.ProductName);
                    if (existing.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Product already exists , please add a new unique product");
                    }

                }

                product.Id = id;
                product.Type = typeof(Product).Name;
                Document productupdated = await _productobj.UpdateItemAsync(id, product);
                if (productupdated != null)
                    return Request.CreateResponse(HttpStatusCode.OK, productupdated);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong , please try again");

            }
            else
            {
                string message = "Enter valid input parameters ";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

        // DELETE: api/products/5
        public async Task<HttpResponseMessage> Delete(string id)
        {
           
            if (!string.IsNullOrWhiteSpace(id))
            {
                    try
                    {
                        var deletedsubcategory = await _productobj.DeleteItemAsync(id);

                        return Request.CreateResponse(HttpStatusCode.NoContent);

                    }
                    catch (Microsoft.Azure.Documents.DocumentClientException e)
                    {
                        string message = "Product to be deleted doesn't exist or was not found ";
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                    }

            }
            else
            {
                string message = "Enter valid Product Id to be deleted ";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

    }
}
