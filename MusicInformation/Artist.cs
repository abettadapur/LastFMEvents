using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicInformation
{
    public class ArtistWrapper
    {
        public ArtistDetails artist { get; set; }
    }
    public class ArtistDetails:Artist
    {
    
        
        public Similar similar { get; set; }
        //public TagWrapper tags { get; set; }
        public ArtistBio bio { get; set; }
    }
    public class Artist
    {
        [JsonProperty("name")]
        public string Title { get; set; }
        public string url { get; set; }
        public LastFMImage[] image { get; set; }
        public string mbid { get; set; }
        private bool byname = false;
        public bool ByName { get { return byname; } set { byname = value; } }
      
    }
    public class LastFMImage
    {
        [JsonProperty("#text")]
        public string url { get; set; }
        
        public string size { get; set; }

    }
    public class ArtistStats
    {
        public string listeners { get; set; }
        public string playcount { get; set; }
    }
    public class ArtistBio
    {
        public string published { get; set; }
        public string summary { get; set; }
        public string content { get; set; }
    }
    public class Similar
    {
        public Artist[] artist { get; set; }
    }
    public class TagWrapper
    {
        public Tag[] tag { get; set; }
    }
    public class Tag
    {
        public string name { get; set; }
        public string url { get; set; }
    }
   
}
