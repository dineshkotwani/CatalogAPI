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
    public class SubCategoriesController : ApiController
    {
        private ICatalogRepository<SubCategory> _subcategoryobj;


        public SubCategoriesController(ICatalogRepository<SubCategory> subcategoryobj)
        {
            _subcategoryobj = subcategoryobj;
        }

        // GET: api/subcategories
        public async Task<HttpResponseMessage> Get()
        {
            var results = await _subcategoryobj.GetSubCategoriesAsync();
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

        // GET: api/subcategories/5
        public async Task<HttpResponseMessage> Get(string id)
        {
            var result = await _subcategoryobj.GetItemAsync(id);

            if (result != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                string message = "Sub-Category doesn't exist or was not found ";
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
        }

        // POST: api/subcategories
        public async Task<HttpResponseMessage> Post([FromBody]SubCategory subcategory)
        {
           
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(subcategory.SubCategoryName))
            {

                var existingsubcategories = await _subcategoryobj.GetSubCategoriesAsync();
                if (existingsubcategories.Count() > 0)
                {
                    var existing = existingsubcategories.Where(c => c.SubCategoryName == subcategory.SubCategoryName);
                    if (existing.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Category already exists , please add a new unique category");
                    }

                }


                subcategory.Type = typeof(SubCategory).Name;
                Document createdsubcategory = await _subcategoryobj.CreateItemAsync(subcategory);
                if (createdsubcategory != null)
                    return Request.CreateResponse(HttpStatusCode.OK, createdsubcategory);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong , please try again");

            }
            else
            {
                string message = "Enter valid SubCategory Name";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

        // PUT: api/subcategories/5
        public async Task<HttpResponseMessage> Put(string id, [FromBody]SubCategory subcategory)
        {
           
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(subcategory.SubCategoryName) && !string.IsNullOrWhiteSpace(id))
            {
                var existingsubcategories = await _subcategoryobj.GetSubCategoriesAsync();
                if (existingsubcategories.Count() > 0)
                {
                    var existing = existingsubcategories.Where(c => c.SubCategoryName == subcategory.SubCategoryName);
                    if (existing.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "SubCategory already exists , please add a new unique subcategory");
                    }

                }

                subcategory.Id = id;
                subcategory.Type = typeof(SubCategory).Name;
                Document subcategoryupdated = await _subcategoryobj.UpdateItemAsync(id, subcategory);
                if (subcategoryupdated != null)
                    return Request.CreateResponse(HttpStatusCode.OK, subcategoryupdated);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong , please try again");

            }
            else
            {
                string message = "Enter valid input parameters ";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

        // DELETE: api/subcategories/5
        public async Task<HttpResponseMessage> Delete(string id)
        {
           
            if (!string.IsNullOrWhiteSpace(id))
            {
                bool hasChildren = await _subcategoryobj.ContainsProducts(id);

                if (!hasChildren)
                {
                    try
                    {
                        var deletedsubcategory = await _subcategoryobj.DeleteItemAsync(id);

                        return Request.CreateResponse(HttpStatusCode.NoContent);

                    }
                    catch (Microsoft.Azure.Documents.DocumentClientException e)
                    {
                        string message = "Sub-Category to be deleted doesn't exist or was not found ";
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                    }

                }
                else
                {
                    string message = "Sub-Category to be deleted has one or many subcategories , hence cannot be deleted ";
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                }
            }
            else
            {
                string message = "Enter valid SubCategoryId to be deleted ";
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
            }

        }

    }
}
