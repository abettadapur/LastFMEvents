using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicInformation
{
   public class TopArtistWrapper
    {
       public TopArtists artists { get; set; }
    }
   public class TopArtists
   {
       public Artist[] artist { get; set; }
   }

}
