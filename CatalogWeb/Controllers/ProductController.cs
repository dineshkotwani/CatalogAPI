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
    public class ProductController : Controller
    {
        // GET: Category
        private readonly string BaseUrl = ConfigurationManager.AppSettings["serviceURL"];
        HttpClient client = new HttpClient();

        // GET: Category
        public async Task<ActionResult> Index()
        {

            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/products").Result;
            List<ProductViewModel> products = await response.Content.ReadAsAsync<List<ProductViewModel>>();


            return View(products);
        }

        // GET: Category/Details/5
        public async Task<ActionResult> Details(string id)
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/products/" + id).Result;
            ProductViewModel product = await response.Content.ReadAsAsync<ProductViewModel>();


            return View(product);


        }

        // GET: Category/Create
        public async Task<ActionResult> Create()
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/subcategories").Result;
            List<SubCategoryViewModel> subcategories = await response.Content.ReadAsAsync<List<SubCategoryViewModel>>();

            HttpResponseMessage resp = client.GetAsync(BaseUrl + "api/categories").Result;
            List<CategoryViewModel> categories = await resp.Content.ReadAsAsync<List<CategoryViewModel>>();

            ViewBag.Categories = categories;
            ViewBag.SubCategories = subcategories;
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductName,SubCategoryId")]ProductViewModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = client.PostAsJsonAsync(BaseUrl + "api/products", product).Result;
                    if (response.StatusCode==HttpStatusCode.OK)
                    {
                        ProductViewModel productcreated = await response.Content.ReadAsAsync<ProductViewModel>();
                        if (productcreated != null)
                        {
                            TempData["Message"] = "Product Created Successfully";
                            TempData["Status"] = "Success";
                            return RedirectToAction("Index");
                        }
                      
                    }
                    else
                    {
                        TempData["Message"] = "Product Creation Failed , Please try again";
                        TempData["Status"] = "Error";
                        return RedirectToAction("Index");
                    }


                }
                else
                {
                    TempData["Message"] = "Select all fields , category , subcategory and enter valid product name";
                    TempData["Status"] = "Error";
                    return RedirectToAction("Create");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(product);
        }

        // GET: Category/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/products/" + id).Result;
            ProductViewModel product = await response.Content.ReadAsAsync<ProductViewModel>();
           
            HttpResponseMessage resp = client.GetAsync(BaseUrl + "api/subcategories").Result;
            List<SubCategoryViewModel> subcategories = await resp.Content.ReadAsAsync<List<SubCategoryViewModel>>();

            ViewBag.SubCategories = subcategories;

            return View(product);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind(Include = "ProductName,SubCategoryId")]ProductViewModel product)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = client.PutAsJsonAsync(BaseUrl + "api/products/" + id, product).Result;
                    ProductViewModel productupdated = await response.Content.ReadAsAsync<ProductViewModel>();

                    if (productupdated != null)
                    {
                        TempData["Message"] = "Product Updated Successfully";
                        TempData["Status"] = "Success";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "Product Updation Failed , Please try again";
                        TempData["Status"] = "Error";
                        return RedirectToAction("Index");
                    }

                   
                }
                else
                {
                    TempData["Message"] = "Select all fields , category , subcategory and enter valid product name";
                    TempData["Status"] = "Error";
                    return RedirectToAction("Edit");
                }

            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(product);

        }

        // GET: Category/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            HttpResponseMessage response = client.GetAsync(BaseUrl + "api/products/" + id).Result;
            ProductViewModel product = await response.Content.ReadAsAsync<ProductViewModel>();
            return View(product);
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
                HttpResponseMessage response = client.DeleteAsync(BaseUrl + "api/products/" + id).Result;
                if(response.StatusCode==HttpStatusCode.NoContent)
                {
                    TempData["Message"] = "Product Deleted Successfully";
                    TempData["Status"] = "Success";
                    
                }
                else
                {
                    TempData["Message"] = "Product Deletion Failed";
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
