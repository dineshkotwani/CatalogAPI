using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CatalogAPI.Data
{
    public static class CatalogDB
    {
        private static DocumentClient _client;
        private static string _databaseId;
        private static string collectionId;

       

        public static DocumentClient Client { get { return _client; } }
        public static string DatabaseId { get { return _databaseId; }  }
        public static string CollectionId { get { return collectionId; } }

        public async static void Initialize()
        {
            _client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"], new ConnectionPolicy { EnableEndpointDiscovery = false });
            _databaseId = ConfigurationManager.AppSettings["databaseID"];
            collectionId = ConfigurationManager.AppSettings["collectionID"];
            Database database = await CreateDatabaseIfNotExistsAsync();
            DocumentCollection collection = await CreateCollectionIfNotExistsAsync(database, collectionId);

        }

        private static async Task<Database> CreateDatabaseIfNotExistsAsync()
        {
            // Create a connection to our database account
          
               

                // Check if database already exists
                Database database = Client.CreateDatabaseQuery().Where(db => db.Id == DatabaseId).AsEnumerable().FirstOrDefault();

                if (database == null)
                {
                    // Create the database
                    database = await Client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }

                return database;
        }
        private static async Task<DocumentCollection> CreateCollectionIfNotExistsAsync(Database db,string collectionId)
        {
            
             
                // Check if collection already exists
                DocumentCollection collection =
                  Client.CreateDocumentCollectionQuery(db.CollectionsLink)
                    .Where(c => c.Id == collectionId)
                    .AsEnumerable()
                    .FirstOrDefault();

                if (collection == null)
                {
                    // Create collection
                    collection = await Client.CreateDocumentCollectionAsync(
                      db.CollectionsLink,
                      new DocumentCollection { Id = collectionId });
                }

                return collection;
            
        }



    }
}