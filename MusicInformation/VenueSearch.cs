using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MusicInformation
{
    public class VenueSearch
    {
        public VenueSearchResults results { get; set; }
    }
    public class VenueSearchResults
    {
        [JsonProperty("opensearch:totalResults")]
        public int totalResults { get; set; }
        public VenueMatches venuematches { get; set; }
    }
    public class VenueMatches
    {
        [JsonConverter(typeof(VenueSerializer))]
        public GigVenue[] venue { get; set; }
    }
    public class VenueSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize<GigVenue[]>(reader);
            }
            else
            {
                GigVenue venue= serializer.Deserialize<GigVenue>(reader);
                return new[] { venue };
            }
        }
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

    }
}

  

