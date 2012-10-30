using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicInformation
{
    public class ArtistSearch
    {
        public SearchResults results { get; set; }
    }
    public class SearchResults
    {
        [JsonProperty("opensearch:totalResults")]
        public int totalResults { get; set; }
        public ArtistMatches artistmatches { get; set; }
    }
    public class ArtistMatches
    {
       
        public ArtistMatch[] artist { get; set; }
    }
    public class ArtistMatch:Artist
    {
        
        public int listeners { get; set; }
        
        
    }
   

  

}
