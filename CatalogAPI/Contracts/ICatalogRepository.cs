using CatalogAPI.Models;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CatalogAPI.Contracts
{
    public interface ICatalogRepository<T> where T : class
    {
          Task<T> GetItemAsync(string id);

          Task<IEnumerable<Category>> GetCategoriesAsync();

          Task<IEnumerable<SubCategory>> GetSubCategoriesAsync();

          Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(string categoryId);

          Task<IEnumerable<Product>> GetProductsAsync();

          Task<IEnumerable<Product>> GetProductsBySubCategoryIdAsync(string subcategoryId);

          Task<Document> CreateItemAsync(T item);

          Task<Document> UpdateItemAsync(string id, T item);

          Task<Document> DeleteItemAsync(string id);

          Task<bool> ContainsSubCategories(string id);

          Task<bool> ContainsProducts(string id);




    }
}