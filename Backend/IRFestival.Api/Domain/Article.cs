using Newtonsoft.Json;

namespace IRFestival.Api.Domain
{
    public class Article
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Tag)]
        public string Tag { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "ImagePath")]
        public string ImagePath { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "Date")]
        public DateTime Date { get; set; }
    }

    public enum Status
    {
        Published,
        Unpublished
    }
}