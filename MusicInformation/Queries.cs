using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicInformation
{
    
    public class Queries
    {
        public const string API_KEY = "08dd2c001a8176908e4feb6bd51391a1";
        public const string ARTIST_SEARCH_URL = "http://ws.audioscrobbler.com/2.0/?method=artist.search&api_key="+API_KEY+"&format=json";
        public const string ARTIST_URL = "http://ws.audioscrobbler.com/2.0/?format=json&method=artist.getinfo&api_key="+API_KEY;
        public const string ARTIST_EVENTS_URL = "http://ws.audioscrobbler.com/2.0/?method=artist.getevents&api_key="+API_KEY+"&format=json";
        public const string LOCAL_EVENTS_URL = "http://ws.audioscrobbler.com/2.0/?method=geo.getevents&limit=20&api_key="+API_KEY+"&format=json";
       public const string TOP_URL="http://ws.audioscrobbler.com/2.0/?limit=12&method=chart.gettopartists&api_key="+API_KEY+"&format=json";
        public const string VENUE_SEARCH_URL="http://ws.audioscrobbler.com/2.0/?method=venue.search&format=json&api_key="+API_KEY;
        public const string VENUE_DETAIL_URL="http://ws.audioscrobbler.com/2.0/?method=venue.getevents&format=json&api_key="+API_KEY;
        //TODO: RETURN CLASS, NOT STRING
        public async static Task<ArtistSearch> ArtistSearchAsync(string name)
        {
            string requesturl = ARTIST_SEARCH_URL + "&artist=" + name;
            Stream stream = await MakeRequest(requesturl);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            ArtistSearch searchResults = JsonConvert.DeserializeObject<ArtistSearch>(json);
            return searchResults;
        }

        public async static Task<Gigs> TryArtistGigsAsync(string mbid)
        {
           // name = name.Replace(' ', '+');
            string requesturl = ARTIST_EVENTS_URL + "&mbid=" + mbid;
            Stream stream = await MakeRequest(requesturl);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            Gigs gigs = JsonConvert.DeserializeObject<Gigs>(json);
            if (gigs.events.gigs == null)
            {
                return null;
            }
            fixDates(gigs);
            return gigs;
        }

        public async static Task<Gigs> LocalGigsCityAsync(string city)
        {

            string requesturl = LOCAL_EVENTS_URL + "&location=" + city;
            Stream stream = await MakeRequest(requesturl);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            Gigs gigs = JsonConvert.DeserializeObject<Gigs>(json);
            fixDates(gigs);
            return gigs;
        }

        public async static Task<Gigs> LocalGigsCoorAsync(double lat, double longt)
        {

            string requesturl = LOCAL_EVENTS_URL +"&long=" + longt + "&lat=" + lat;
            Stream stream = await MakeRequest(requesturl);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            Gigs gigs = JsonConvert.DeserializeObject<Gigs>(json);
            fixDates(gigs);
            return gigs;
        }


        public async static Task<ArtistDetails> ArtistDetailsAsync(string mbid)
        {
            //name = name.Replace(' ', '+');
            string requesturl = ARTIST_URL + "&mbid=" + mbid;
           
            Stream stream =await MakeRequest(requesturl);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            ArtistWrapper artist = JsonConvert.DeserializeObject<ArtistWrapper>(json);
            artist.artist.bio.content = parseHTML(artist.artist.bio.content);
            return artist.artist;
        }
   
        public async static Task<TopArtistWrapper> TopArtistsAsync()
        {
            Stream stream = await MakeRequest(TOP_URL);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            TopArtistWrapper topArtists = JsonConvert.DeserializeObject<TopArtistWrapper>(json);
            return topArtists;
        }
        public async static Task<VenueSearchResults> VenueSearchAsync(string query)
        {
            string requesturl = VENUE_SEARCH_URL + "&venue=" + query;
            Stream stream = await MakeRequest(requesturl);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            VenueSearch results = JsonConvert.DeserializeObject<VenueSearch>(json);
            return results.results;
        }
        public async static Task<Gigs> VenueDetailsAsync(string id)
        {
            string requesturl = VENUE_DETAIL_URL + "&venue=" + id;
            Stream stream = await MakeRequest(requesturl);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            Gigs gigs = JsonConvert.DeserializeObject<Gigs>(json);
            fixDates(gigs);
            return gigs;
         
        }

        public async static Task<Stream> MakeRequest(string url)
        {
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0,0,15);
            return await client.GetStreamAsync(url);
        }
        public static string parseHTML(string data)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(data);
            HtmlNode node = doc.DocumentNode;
            return WebUtility.HtmlDecode(node.InnerText);
        }
        public static void fixDates(Gigs gig)
        {
            foreach (Gig g in gig.events.gigs)
            {
                string date = g.startDate;
                g.day = date.Substring(5, 2);
                g.month = date.Substring(8, 3);
                g.year = date.Substring(12, 4);

            }
        }


        public async static Task<ArtistDetails> AritstDetailsByNameAsync(string name)
        {
            name = name.Replace(' ', '+');
            string requesturl = ARTIST_URL + "&artist=" + name;

            Stream stream = await MakeRequest(requesturl);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            ArtistWrapper artist = JsonConvert.DeserializeObject<ArtistWrapper>(json);
            artist.artist.bio.content = parseHTML(artist.artist.bio.content);
            return artist.artist;
        }

        public async static Task<Gigs> TryArtistGigsByNameAsync(string name)
        {
             name = name.Replace(' ', '+');
            string requesturl = ARTIST_EVENTS_URL + "&artist=" + name;
            Stream stream = await MakeRequest(requesturl);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            Gigs gigs = JsonConvert.DeserializeObject<Gigs>(json);
            if (gigs.events.gigs == null)
            {
                return null;
            }
            fixDates(gigs);
            return gigs;
        }
    }
}
