using CatalogAPI.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Documents;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using CatalogAPI.Data;
using CatalogAPI.Models;

namespace CatalogAPI.Repository
{
    public class CatalogRepository<T> : ICatalogRepository<T> where T : class
    {

        private DocumentClient _client;
        private string _databaseId;
        private string _collectionId;

        public CatalogRepository()
        {
            _client = CatalogDB.Client;
            _databaseId = CatalogDB.DatabaseId;
            _collectionId = CatalogDB.CollectionId;
         
        }
       

        public async Task<Document> CreateItemAsync(T item)
        {
            return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);
        }

        public async Task<Document> DeleteItemAsync(string id)
        {
           
            return await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            IDocumentQuery<Product> query = _client.CreateDocumentQuery<Product>(
              UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
              new FeedOptions { MaxItemCount = -1 })
              .Where(d => d.Type == typeof(Product).Name)
              .AsDocumentQuery();

            List<Product> results = new List<Product>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Product>());
            }

            return results;

        }
        public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync()
        {
            IDocumentQuery<SubCategory> query = _client.CreateDocumentQuery<SubCategory>(
              UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
              new FeedOptions { MaxItemCount = -1 })
              .Where(d => d.Type == typeof(SubCategory).Name)
              .AsDocumentQuery();

            List<SubCategory> results = new List<SubCategory>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<SubCategory>());
            }

            return results;

        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            IDocumentQuery<Category> query = _client.CreateDocumentQuery<Category>(
              UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
              new FeedOptions { MaxItemCount = -1 })
              .Where(d=>d.Type==typeof(Category).Name)
              .AsDocumentQuery();

            List<Category> results = new List<Category>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Category>());
            }

            return results;

        }

        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            Uri updatelink = UriFactory.CreateDocumentUri(_databaseId, _collectionId, id);
            return await _client.ReplaceDocumentAsync(updatelink, item);
        }

        public async Task<bool> ContainsSubCategories(string id)
        {
            Task<IEnumerable<SubCategory>> tasksubcategory = GetSubCategoriesByCategoryIdAsync(id);
            var subcategories = await tasksubcategory;
            if (subcategories.Count() > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> ContainsProducts(string id)
        {
            Task<IEnumerable<Product>> taskproduct = GetProductsBySubCategoryIdAsync(id);
            var products = await taskproduct;
            if (products.Count() > 0)
                return true;
            else
                return false;
        }


        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(string categoryId)
        {
            IDocumentQuery<SubCategory> query = _client.CreateDocumentQuery<SubCategory>(
              UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
              new FeedOptions { MaxItemCount = -1 })
              .Where(d => d.Type == typeof(SubCategory).Name && d.CategoryId == categoryId)
              .AsDocumentQuery();

            List<SubCategory> results = new List<SubCategory>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<SubCategory>());
            }

            return results;

        }

       

        public async Task<IEnumerable<Product>> GetProductsBySubCategoryIdAsync(string subcategoryId)
        {
            IDocumentQuery<Product> query = _client.CreateDocumentQuery<Product>(
             UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
             new FeedOptions { MaxItemCount = -1 })
             .Where(d => d.Type == typeof(Product).Name && d.SubCategoryId == subcategoryId)
             .AsDocumentQuery();

            List<Product> results = new List<Product>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Product>());
            }

            return results;

        }

    

   

     


    }
}