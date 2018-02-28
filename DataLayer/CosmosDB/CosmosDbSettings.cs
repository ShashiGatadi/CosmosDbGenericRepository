using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.CosmosDB
{
    public class CosmosDbSettings
    {
        public CosmosDbSettings(IConfiguration configuration)
        {
            string endpoint = configuration["DocumentDb:Endpoint"];
            string authKey = configuration["DocumentDb:AuthKey"];
            string databaseId = configuration["DocumentDb:DatabaseId"];
            string collectionId = configuration["DocumentDb:CollectionId"];

            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentNullException("Endpoint");

            if (string.IsNullOrEmpty(authKey))
                throw new ArgumentNullException("AuthKey");

            if (string.IsNullOrEmpty(databaseId))
                throw new ArgumentNullException("DatabaseId");

            if (string.IsNullOrEmpty(collectionId))
                throw new ArgumentNullException("CollectionId");

            this.Endpoint = endpoint;
            this.AuthKey = authKey;
            this.DatabaseId = databaseId;
            this.CollectionId = collectionId;
        }

        public string Endpoint { get; }
        public string AuthKey { get; }
        public string DatabaseId { get; }
        public string CollectionId { get; }
    }
}
