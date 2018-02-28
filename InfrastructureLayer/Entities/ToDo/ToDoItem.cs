using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureLayer.Entities.ToDo
{
    public class ToDoItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime TargetDate { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
