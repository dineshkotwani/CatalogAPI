using CatalogWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace CatalogWeb.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        private readonly string BaseUrl = ConfigurationManager.AppSettings["serviceURL"];
        HttpClient client = new HttpClient();

        // GET: Category
        public async Task<ActionResult> Index()
        {
           
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/categories").Result;
            List<CategoryViewModel> categories = await response.Content.ReadAsAsync<List<CategoryViewModel>>();
           

            return View(categories);
        }

        // GET: Category/Details/5
        public async Task<ActionResult> Details(string id)
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/categories/"+id).Result;
            CategoryViewModel category = await response.Content.ReadAsAsync<CategoryViewModel>();


            return View(category);


        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CategoryName")]CategoryViewModel category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = client.PostAsJsonAsync(BaseUrl + "api/categories", category).Result;
                    if (response.StatusCode==HttpStatusCode.OK)
                    {
                        CategoryViewModel categorycreated = await response.Content.ReadAsAsync<CategoryViewModel>();
                        if (categorycreated != null)
                        {
                            TempData["Message"] = "Category Created Successfully";
                            TempData["Status"] = "Success";
                            return RedirectToAction("Index");
                        }
                       
                    }
                    else
                    {
                        TempData["Message"] = "Category Creation Failed , Please try again";
                        TempData["Status"] = "Error";
                        return RedirectToAction("Index");
                    }


                }
                else
                {
                    TempData["Message"] = "Enter valid Category Name";
                    TempData["Status"] = "Error";
                    return RedirectToAction("Create");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/categories/" + id).Result;
            CategoryViewModel category = await response.Content.ReadAsAsync<CategoryViewModel>();
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind(Include = "CategoryName")]CategoryViewModel category)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = client.PutAsJsonAsync(BaseUrl + "api/categories/" + id, category).Result;
                    if (response.StatusCode==HttpStatusCode.OK)
                    {
                        CategoryViewModel categoryupdated = await response.Content.ReadAsAsync<CategoryViewModel>();
                        if (categoryupdated != null)
                        {
                            TempData["Message"] = "Category Updated Successfully";
                            TempData["Status"] = "Success";
                            return RedirectToAction("Index");
                        }
                       
                    }
                    else
                    {
                        TempData["Message"] = "Category Updation Failed , Please try again";
                        TempData["Status"] = "Error";
                        return RedirectToAction("Index");
                    }


                }
                else
                {
                    TempData["Message"] = "Enter valid Category Name";
                    TempData["Status"] = "Error";
                    return RedirectToAction("Edit");
                }

            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(category);

        }

        // GET: Category/Delete/5
        public async Task<ActionResult> Delete(string id)
        {

            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/categories/" + id).Result;
            CategoryViewModel category = await response.Content.ReadAsAsync<CategoryViewModel>();
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                    HttpResponseMessage response = client.DeleteAsync(BaseUrl + "api/categories/" + id).Result;
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    TempData["Message"] = "Category Deleted Successfully";
                    TempData["Status"] = "Success";

                }
                else
                {
                    TempData["Message"] = "Category Deletion Failed";
                    TempData["Status"] = "Error";

                }

                return RedirectToAction("Index");
                
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to delete category. Try again, and if the problem persists see your system administrator.");
            }

            return View();
        }
    }
}
