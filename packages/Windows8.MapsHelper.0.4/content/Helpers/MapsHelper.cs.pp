using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$.Helpers
{
    public static class MapsHelper
    {
        private async static void DefaultLaunch(Uri uri)
        {
            // Launch the URI
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        public static void OpenMaps(string query = null, string where = null, double? lat = null, double? lon = null, double? zoomLevel = null, bool? roadView = null, bool? isTrafficOn = null)
        {
            var helper = new MapsProtocolBuilder()
            {
                Query = query,
                Where = where,
                RoadView = roadView,
                TrafficOn = isTrafficOn,
                Lat = lat,
                Lon = lon,
                Zoom = zoomLevel,
            };
            var uri = new Uri(helper.ToString());
            DefaultLaunch(uri);
        }

        private class MapsProtocolBuilder
        {
            public double? Lat;
            public double? Lon;
            public double? Zoom;

            public string Query;
            public string Where;
            public bool? TrafficOn;
            public bool? RoadView;

            public override string ToString()
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                if (Lat.HasValue && Lon.HasValue)
                {
                    parameters.Add("cp", string.Format("{0}~{1}", Lat.Value, Lon.Value));
                }
                if (Zoom.HasValue)
                {
                    parameters.Add("lvl", Zoom.Value.ToString());
                }
                if (!String.IsNullOrWhiteSpace(Query))
                {
                    parameters.Add("q", Query);
                }
                if (!String.IsNullOrWhiteSpace(Where))
                {
                    parameters.Add("where", Where);
                }
                if (TrafficOn.HasValue)
                {
                    parameters.Add("trfc", TrafficOn.Value ? "1" : "0");
                }
                if (RoadView.HasValue)
                {
                    parameters.Add("sty", RoadView.Value ? "r" : "a");
                }
                return parameters.ToQueryString("bingmaps://dreamteam-mobile.com/");
            }
        }
        
        
        private static string ToQueryString(this Dictionary<string, string> source, string baseUrl)
        {
            if (source.Count == 0)
                return baseUrl;

            var queryString = String.Join("&", source.Select(kvp => String.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value))).ToArray());
            var queryStringFormat = baseUrl.Contains("?") ? "{0}&{1}" : "{0}?{1}";
            return string.Format(queryStringFormat, baseUrl, queryString);
        }
    }
}
