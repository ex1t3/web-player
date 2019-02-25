using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Server.REST
{
  public class SongSearchAPI
  {
    private static string URL = "https://musixmatchcom-musixmatch.p.rapidapi.com/wsr/1.1/track.search?f_has_lyrics=0";
    private static string API_KEY = "1f965be495msh382ca5056f7e75cp135645jsn3c27ae3addc6";
    private static string HEADER = "X-RapidAPI-Key";

    public async Task<SearchingModel> Search(string songName, string songArtist)
    {
      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
      var httpRequestMessage = new HttpRequestMessage
      {
        Method = HttpMethod.Get,
        RequestUri = new Uri(URL + "&q_track=" + songName.Trim() + "&q_artist=" + songArtist.Trim() + "&s_track_rating = asc & page_size = 1 & page = 1"),
        Headers = {
          { HEADER, API_KEY }
        }
    };
 
      var response = client.SendAsync(httpRequestMessage).Result;
      if (response.IsSuccessStatusCode)
      {
        var responseString = await response.Content.ReadAsStringAsync();
        var responseData = JsonConvert.DeserializeObject<List<SearchingModel>>(responseString);
        if (responseData.Count>0)
        {
          return responseData[0];
        }
      }
      // Need to call dispose on the HttpClient object
      // when done using it, so the app doesn't leak resources
      client.Dispose();
      
      return null;
    }

  }

  public class SearchingModel
  {
    public string track_name { get; set; }
    public string artist_name { get; set; }
    public string album_name { get; set; }
    public MusicGenres primary_genres { get; set; }
  }

  public class MusicGenres
  {
    public List<Genre> music_genre { get; set; }
  }

  public class Genre
  {
    public string music_genre_name { get; set; }
  }
 
}
