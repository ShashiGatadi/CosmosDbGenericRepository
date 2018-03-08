using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.CosmosDB
{
    public partial class CosmosDbSettings
    {
        public string Endpoint { get; set; }
        public string AuthKey { get; set; }
        public string DatabaseId { get; set; }
        public string CollectionId { get; set; }
    }
}
