using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicInformation
{
    public class Gigs
    {
        public GigWrapper events { get; set; }
    }
    public class GigWrapper
    {
        [JsonProperty("event"), JsonConverter(typeof(GigsSerializer))]
        public Gig[] gigs { get; set; }
    }
    public class Gig
    {
        public string id { get; set; }
        public string title { get; set; }
        public GigArtists artists { get; set; }
        public GigVenue venue { get; set; }
        public string startDate { get; set; }
        public string description { get; set; }
        public LastFMImage[] image { get; set; }
        public string attendance { get; set; }
        public string reviews { get; set; }
        public string url { get; set; }
        public string cancelled { get; set; }
        public string tickets { get; set; }
        public string day { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        

    }
    public class GigArtists
    {
        public string headliner{get;set;}
       [JsonConverter(typeof(ArtistsNameSerializer))]
        public string[] artist{get;set;}
    }
    public class GigVenue
    {
        public string id { get; set; }
        [JsonProperty("name")]
        public string Title { get; set; }
        public GeoLocation location { get; set; }
        public string url{get;set;}
        public string website{get;set;}
        public string phonenumber{get;set;}
        public LastFMImage[] image { get; set; }
    }
    public class GeoLocation
    {
        [JsonProperty("geo:point")]
        public GeoPoint point { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string street { get; set; }
        public string postalcode { get; set; }

    }
    public class GeoPoint
    {
        [JsonProperty("geo:lat")]
        public string lat { get; set; }
        [JsonProperty("geo:long")]
        public string longt { get; set; }
    }

    public class ArtistsNameSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize<string[]>(reader);
            }
            else
            {
                string artist = serializer.Deserialize<string>(reader);
                return new[] { artist };
            }
        }
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

    }
       public class GigsSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize<Gig[]>(reader);
            }
            else
            {
                Gig gig = serializer.Deserialize<Gig>(reader);
                return new[] { gig };
            }
        }
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

    }
}
