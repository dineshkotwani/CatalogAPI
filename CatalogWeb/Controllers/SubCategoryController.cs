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
    public class SubCategoryController : Controller
    {
        // GET: Category
        private readonly string BaseUrl = ConfigurationManager.AppSettings["serviceURL"];
        HttpClient client = new HttpClient();

        // GET: Category
        public async Task<ActionResult> Index()
        {

            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/subcategories").Result;
            List<SubCategoryViewModel> subcategories = await response.Content.ReadAsAsync<List<SubCategoryViewModel>>();


            return View(subcategories);
        }

        // GET: Category/Details/5
        public async Task<ActionResult> Details(string id)
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/subcategories/" + id).Result;
            SubCategoryViewModel subcategory = await response.Content.ReadAsAsync<SubCategoryViewModel>();


            return View(subcategory);


        }

        // GET: Category/Create
        public async Task<ActionResult> Create()
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/categories").Result;
            List<CategoryViewModel> categories = await response.Content.ReadAsAsync<List<CategoryViewModel>>();
            ViewBag.Categories = categories;
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SubCategoryName,CategoryId")]SubCategoryViewModel subcategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = client.PostAsJsonAsync(BaseUrl + "api/subcategories", subcategory).Result;
                    if (response.StatusCode==HttpStatusCode.OK)
                    {
                        SubCategoryViewModel subcategorycreated = await response.Content.ReadAsAsync<SubCategoryViewModel>();
                        if (subcategorycreated != null)
                        {
                            TempData["Message"] = "SubCategory Created Successfully";
                            TempData["Status"] = "Success";
                            return RedirectToAction("Index");
                        }
                       
                    }
                    else
                    {
                        TempData["Message"] = "SubCategory Creation Failed , Please try again";
                        TempData["Status"] = "Error";
                        return RedirectToAction("Index");
                    }


                }
                else
                {
                    TempData["Message"] = "Enter valid SubCategory Name";
                    TempData["Status"] = "Error";
                    return RedirectToAction("Create");
                }

            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(subcategory);
        }

        // GET: Category/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/subcategories/" + id).Result;
            SubCategoryViewModel subcategory = await response.Content.ReadAsAsync<SubCategoryViewModel>();
            HttpResponseMessage resp = client.GetAsync(BaseUrl + "api/categories").Result;
            List<CategoryViewModel> categories = await resp.Content.ReadAsAsync<List<CategoryViewModel>>();
            ViewBag.Categories = categories;

            return View(subcategory);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind(Include = "SubCategoryName,CategoryId")]SubCategoryViewModel subcategory)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = client.PutAsJsonAsync(BaseUrl + "api/subcategories/" + id, subcategory).Result;
                    if (response.StatusCode==HttpStatusCode.OK)
                    {
                        SubCategoryViewModel subcategoryupdated = await response.Content.ReadAsAsync<SubCategoryViewModel>();
                        if (subcategoryupdated != null)
                        {
                            TempData["Message"] = "SubCategory Updated Successfully";
                            TempData["Status"] = "Success";
                            return RedirectToAction("Index");
                        }
                       
                    }
                    else
                    {
                        TempData["Message"] = "SubCategory Updation Failed , Please try again";
                        TempData["Status"] = "Error";
                        return RedirectToAction("Index");
                    }


                }
                else
                {
                    TempData["Message"] = "Enter valid SubCategory Name";
                    TempData["Status"] = "Error";
                    return RedirectToAction("Edit");
                }


            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(subcategory);

        }

        // GET: Category/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/subcategories/" + id).Result;
            SubCategoryViewModel subcategory = await response.Content.ReadAsAsync<SubCategoryViewModel>();
            return View(subcategory);
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
                HttpResponseMessage response = client.DeleteAsync(BaseUrl + "api/subcategories/" + id).Result;
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    TempData["Message"] = "SubCategory Deleted Successfully";
                    TempData["Status"] = "Success";

                }
                else
                {
                    TempData["Message"] = "SubCategory Deletion Failed";
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
