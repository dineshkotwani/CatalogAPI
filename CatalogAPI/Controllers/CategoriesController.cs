
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
    public class CategoriesController : ApiController
    {
        private ICatalogRepository<Category> _categoryobj;
       
        public CategoriesController(ICatalogRepository<Category> categoryobj)
        {
            _categoryobj = categoryobj;
        }
        // GET: api/categories
        public async Task<HttpResponseMessage> Get()
        {
            var results =  await _categoryobj.GetCategoriesAsync();
            if(results!=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            else
            {
                string message = "Something went wrong , please try again";
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
            
        }

        // GET: api/categories/5
        public async Task<HttpResponseMessage> Get(string id)
        {

            var result= await _categoryobj.GetItemAsync(id);
            if(result!=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                string message = "Category doesn't exist or was not found ";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
        }

        // POST: api/categories
        public async Task<HttpResponseMessage> Post([FromBody]Category category)
        {
            if(ModelState.IsValid && !string.IsNullOrWhiteSpace(category.CategoryName))
            {
                var existingcategories = await _categoryobj.GetCategoriesAsync();
                if (existingcategories.Count()>0)
                {
                    var existing = existingcategories.Where(c => c.CategoryName == category.CategoryName);
                    if (existing.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Category already exists , please add a new unique category");
                    }

                }

              

                category.Type = typeof(Category).Name;
                Document createdcategory = await _categoryobj.CreateItemAsync(category);
                if(createdcategory!=null)
                    return Request.CreateResponse(HttpStatusCode.OK, createdcategory);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong , please try again");

            }
            else
            {
                string message = "Enter valid Category Name";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }
           
        }

        // PUT: api/categories/5
        public async Task<HttpResponseMessage> Put(string id,[FromBody]Category category)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(category.CategoryName) && !string.IsNullOrWhiteSpace(id))
            {

                var existingcategories = await _categoryobj.GetCategoriesAsync();
                if (existingcategories.Count() > 0)
                {
                    var existing = existingcategories.Where(c => c.CategoryName == category.CategoryName);
                    if (existing.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Category already exists , please add a new unique category");
                    }

                }

                category.Id = id;
                category.Type = typeof(Category).Name;
                Document categoryupdated = await _categoryobj.UpdateItemAsync(id, category);
                if (categoryupdated != null)
                    return Request.CreateResponse(HttpStatusCode.OK, categoryupdated);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong , please try again");

            }
            else
            {
                string message = "Enter valid input parameters ";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }


            
        }

        // DELETE: api/categories/5
        public async Task<HttpResponseMessage> Delete(string id)
        {
           if (!string.IsNullOrWhiteSpace(id))
            {
                bool hasChildren = await _categoryobj.ContainsSubCategories(id);

                if (!hasChildren)
                {
                    try
                    {
                        var deletedcategory = await _categoryobj.DeleteItemAsync(id);
                       
                       
                        return Request.CreateResponse(HttpStatusCode.NoContent);
                       
                    }
                    catch (Microsoft.Azure.Documents.DocumentClientException e)
                    {
                        string message = "Category to be deleted doesn't exist or was not found ";
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                    }
                   
                }
                else
                {
                    string message = "Category to be deleted has one or many subcategories , hence cannot be deleted ";
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
            }
            else
            {
                string message = "Enter valid Category Id to be deleted ";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }
            
            
        }

    }
}
