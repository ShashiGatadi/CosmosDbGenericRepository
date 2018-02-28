using InfrastructureLayer.Data;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.CosmosDB
{
    public class CosmosDbRepository<T> : IRepository<T> where T : class
    {
        private CosmosDbSettings _settings;
        private DocumentClient _client;

        public CosmosDbRepository(IConfiguration configuration)
        {
            this._settings = new CosmosDbSettings(configuration);
            Init();
        }

        private void Init()
        {
            try
            {
                _client = new DocumentClient(new Uri(this._settings.Endpoint), this._settings.AuthKey);

                CreateDatabaseIfNotExistsAsync().Wait();
                CreateCollectionIfNotExistsAsync().Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Init failed for DocumentDbRepository");
            }
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(this._settings.DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _client.CreateDatabaseAsync(new Database { Id = this._settings.DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await _client.ReadDocumentCollectionAsync(
                    UriFactory.CreateDocumentCollectionUri(this._settings.DatabaseId, this._settings.CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(this._settings.DatabaseId),
                        new DocumentCollection { Id = this._settings.CollectionId },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }

        private Uri GetCollectionUri()
        {
            return UriFactory.CreateDocumentCollectionUri(
                        this._settings.DatabaseId, this._settings.CollectionId);
        }

        private Uri GetDocumentUri(string documentId)
        {
            return UriFactory.CreateDocumentUri(
                        this._settings.DatabaseId, this._settings.CollectionId, documentId);
        }

        //CRUD methods
        public async Task<T> CreateItemAsync(T item)
        {
            Document document = await this._client.CreateDocumentAsync(GetCollectionUri(), item);
            return (T)(dynamic)document;
        }

        public async Task<T> CreateOrUpdateItemAsync(T item)
        {
            Document document = await this._client.UpsertDocumentAsync(GetCollectionUri(), item);
            return (T)(dynamic)document;
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._client.DeleteDocumentAsync(GetDocumentUri(id));
        }

        public async Task<T> GetItemAsync(string id)
        {
            Document document = await this._client.ReadDocumentAsync(GetDocumentUri(id));
            return (T)(dynamic)document;
        }

        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            var query = this._client
                            .CreateDocumentQuery<T>(GetCollectionUri())
                            .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var query = this._client
                            .CreateDocumentQuery<T>(GetCollectionUri())
                            .Where(predicate)
                            .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync(string sql)
        {
            var query = this._client
                            .CreateDocumentQuery<T>(GetCollectionUri(), sql)
                            .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<T> UpdateItemAsync(string id, T item)
        {
            Document document = await this._client.ReplaceDocumentAsync(GetDocumentUri(id), item);
            return (T)(dynamic)document;
        }
    }
}
