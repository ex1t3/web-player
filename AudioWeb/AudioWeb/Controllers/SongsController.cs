using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Model.Models;
using Service.Services;
using Service.SessionHandlers;

namespace AudioWeb.Controllers
{
  [SessionAuthorize]
  [RoutePrefix("api/Songs")]
  public class SongsController : ApiController
  {
    private readonly SongService _songService; // SongService object made to connect to song's contexts 
    private readonly UserService _userService; // UserService object made to connect to account management contexts

    public SongsController() // Main SongsController constructor
    {
      this._songService = new SongService();
      this._userService = new UserService();
    }

    [HttpPost]
    [Route("UploadSong")]
    public async Task<IHttpActionResult> UploadSong() // Method used for uploading user's songs and store them on the server
    {
      var userId = _userService.GetUserByName(User.Identity.Name).Id;
      var file = HttpContext.Current.Request.Files["NewSong"];
      if (file == null) return Json(false);
      var rand = new Random();
      var newFileName = DateTime.Now.ToString("yy-MM-dd") + "_" + rand.Next(1, 1000000) + "_audio_web" + Path.GetExtension(file.FileName);
      file.SaveAs(HttpContext.Current.Server.MapPath("~/Songs/" + newFileName));
      var newPath = Path.GetFullPath(HttpContext.Current.Server.MapPath("~/Songs/" + newFileName));
      var tags = TagLib.File.Create(newPath).Tag;
      if (tags == null) return Json(false);
      var image = tags.Pictures.Length >= 1 ?
        "data:" + tags.Pictures[0].MimeType + ";base64," + Convert.ToBase64String(tags.Pictures[0].Data.Data) :
        "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNTEyIDUxMiIgd2lkdGg9IjUxMnB4Ij48cGF0aCBkPSJtNTUuMTc1NzgxIDI1NmgtMTUuMTAxNTYyYzAtNTcuNjc1NzgxIDIyLjQ1NzAzMS0xMTEuODk4NDM4IDYzLjI0MjE4Ny0xNTIuNjgzNTk0czk1LjAwNzgxMy02My4yNDIxODcgMTUyLjY4MzU5NC02My4yNDIxODd2MTUuMTAxNTYyYy0xMTAuNzM0Mzc1IDAtMjAwLjgyNDIxOSA5MC4wODk4NDQtMjAwLjgyNDIxOSAyMDAuODI0MjE5em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtMjU2IDQ3MS45MjU3ODF2LTE1LjEwMTU2MmMxMTAuNzM0Mzc1IDAgMjAwLjgyNDIxOS05MC4wODk4NDQgMjAwLjgyNDIxOS0yMDAuODI0MjE5aDE1LjEwNTQ2OWMwIDU3LjY3NTc4MS0yMi40NjA5MzggMTExLjg5ODQzOC02My4yNDYwOTQgMTUyLjY4MzU5NHMtOTUuMDA3ODEzIDYzLjI0MjE4Ny0xNTIuNjgzNTk0IDYzLjI0MjE4N3ptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTQxLjkzMzU5NCAzNjYuNTg5ODQ0Yy0xNy44MDA3ODItMzQuMzg2NzE5LTI2LjgyODEyNS03MS41OTM3NS0yNi44MjgxMjUtMTEwLjU4OTg0NCAwLTY0LjM0Mzc1IDI1LjA1ODU5My0xMjQuODM1OTM4IDcwLjU1ODU5My0xNzAuMzM1OTM4czEwNS45OTIxODgtNzAuNTU4NTkzIDE3MC4zMzU5MzgtNzAuNTU4NTkzYzQ3LjE3MTg3NSAwIDkyLjg1MTU2MiAxMy42Mjg5MDYgMTMyLjA5NzY1NiAzOS40MTc5NjlsOC4yOTY4NzUtMTIuNjI1Yy00MS43MjI2NTYtMjcuNDEwMTU3LTkwLjI2OTUzMS00MS44OTg0MzgtMTQwLjM5NDUzMS00MS44OTg0MzgtNjguMzc1IDAtMTMyLjY2NDA2MiAyNi42Mjg5MDYtMTgxLjAxNTYyNSA3NC45ODQzNzUtNDguMzU1NDY5IDQ4LjM1MTU2My03NC45ODQzNzUgMTEyLjY0MDYyNS03NC45ODQzNzUgMTgxLjAxNTYyNSAwIDQwLjg1MTU2MiA5Ljg2MzI4MSA4MS40OTYwOTQgMjguNTE5NTMxIDExNy41MzUxNTYgMTguMDU0Njg4IDM0Ljg2NzE4OCA0NC4zNjMyODEgNjUuNjEzMjgyIDc2LjA4MjAzMSA4OC45MTQwNjNsOC45NDUzMTMtMTIuMTcxODc1Yy0yOS44NTkzNzUtMjEuOTMzNTk0LTU0LjYyMTA5NC01MC44NzEwOTQtNzEuNjEzMjgxLTgzLjY4NzV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00MTguMDQyOTY5IDU3LjgwODU5NC05LjU2NjQwNyAxMS42ODc1YzU2LjE5MTQwNyA0NS45OTYwOTQgODguNDE3OTY5IDExMy45NzY1NjIgODguNDE3OTY5IDE4Ni41MDM5MDYgMCA2NC4zMzk4NDQtMjUuMDU4NTkzIDEyNC44MzU5MzgtNzAuNTU4NTkzIDE3MC4zMzU5MzhzLTEwNS45OTIxODggNzAuNTU4NTkzLTE3MC4zMzU5MzggNzAuNTU4NTkzYy00MS41NDI5NjkgMC04Mi40OTYwOTQtMTAuNzQ2MDkzLTExOC40MzM1OTQtMzEuMDc4MTI1bC03LjQzNzUgMTMuMTQ4NDM4YzM4LjIwMzEyNSAyMS42MDkzNzUgODEuNzI2NTYzIDMzLjAzNTE1NiAxMjUuODcxMDk0IDMzLjAzNTE1NiA2OC4zNzUgMCAxMzIuNjY0MDYyLTI2LjYyODkwNiAxODEuMDE1NjI1LTc0Ljk4NDM3NSA0OC4zNTU0NjktNDguMzUxNTYzIDc0Ljk4NDM3NS0xMTIuNjQwNjI1IDc0Ljk4NDM3NS0xODEuMDE1NjI1IDAtNzcuMDc0MjE5LTM0LjI0NjA5NC0xNDkuMzEyNS05My45NTcwMzEtMTk4LjE5MTQwNnptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTMwNC4wODU5MzggMjMyLjQxNzk2OWMtMjYuMjU3ODEzIDAtNDcuNjI1IDIxLjM2MzI4MS00Ny42MjUgNDcuNjI1czIxLjM2NzE4NyA0Ny42MjUgNDcuNjI1IDQ3LjYyNWMyNi4yNjE3MTggMCA0Ny42MjUtMjEuMzYzMjgxIDQ3LjYyNS00Ny42MjV2LTE3OS40NTcwMzFsLTEzNS4zMjAzMTMgNTQuMTI4OTA2djE3MC43MjY1NjJjLTguNTE5NTMxLTcuOTc2NTYyLTE5Ljk1NzAzMS0xMi44Nzg5MDYtMzIuNTE5NTMxLTEyLjg3ODkwNi0yNi4yNjE3MTkgMC00Ny42MjUgMjEuMzYzMjgxLTQ3LjYyNSA0Ny42MjVzMjEuMzYzMjgxIDQ3LjYyNSA0Ny42MjUgNDcuNjI1YzI2LjI2MTcxOCAwIDQ3LjYyNS0yMS4zNjMyODEgNDcuNjI1LTQ3LjYyNXYtMTk1LjI0NjA5NGwxMDUuMTA5Mzc1LTQyLjA0Njg3NXYxMjIuNDAyMzQ0Yy04LjUxOTUzMS03Ljk4MDQ2OS0xOS45NTMxMjUtMTIuODc4OTA2LTMyLjUxOTUzMS0xMi44Nzg5MDZ6bS0xMjAuMjE0ODQ0IDE2MC4yODkwNjJjLTE3LjkzMzU5NCAwLTMyLjUxOTUzMi0xNC41ODk4NDMtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkzMzU5NCAxNC41ODU5MzgtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTI5Njg3IDAgMzIuNTE5NTMxIDE0LjU4NTkzNyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTI5Njg4LTE0LjU4OTg0NCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMTIwLjIxNDg0NC04MC4xNDQ1MzFjLTE3LjkyOTY4OCAwLTMyLjUxOTUzMi0xNC41ODU5MzgtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkyOTY4OCAxNC41ODk4NDQtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTMzNTkzIDAgMzIuNTE5NTMxIDE0LjU4OTg0MyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTMzNTkzLTE0LjU4NTkzOCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTM5LjYwOTM3NSAyNzIuNDkyMTg4aDE2LjAzMTI1djE1LjEwMTU2MmgtMTYuMDMxMjV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00NTYuMzU5Mzc1IDIyNC40MDYyNWgxNi4wMzEyNXYxNS4xMDE1NjJoLTE2LjAzMTI1em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNDI0LjMwNDY4OCAyMjQuNDA2MjVoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNzEuNjY3OTY5IDI3Mi40OTIxODhoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48L3N2Zz4K";
      var genres = string.Join(",", tags.Genres);
      var artists = tags.Performers.Length >= 1 ? string.Join(" & ", tags.Performers) : null;
      var song = new Song()
      {
        Name = tags.Title,
        Artist = artists,
        Album = tags.Album ?? "",
        Activity = 0,
        Duration = null,
        Genre = genres ?? "",
        AlbumCover = image,
        Source = newFileName,
        Year = tags.Year.ToString()
      };
      var result = await _songService.AddSong(song, userId, newPath);
      if (result != null) return Json(result);
      return Json(false);

    }

    [HttpPost]
    [Route("IncreaseActivity")]
    public async Task<IHttpActionResult> IncreaseActivity([FromBody]int songId) // Method used for increasing song's rate and updating last user's played song
    {
      if (songId <= 0) return Json("false");
      var user = _userService.GetUserByName(User.Identity.Name);
      await _songService.IncreaseActivity(songId);
      await _songService.UpdateLastPlayedSongs(songId, user.Id);
      return Json("true");

    }

    [HttpPost]
    [Route("SearchForSong")]
    public async Task<IHttpActionResult> SearchForSong([FromBody]string typedText) // Method used for searching similiar songs or artists to user's typed text
    {
      var keyWords = typedText.TrimEnd().Split(' ');
      var songs = await _songService.GetSearchedSongs(keyWords);
      var artists = await _songService.GetSearchedArtists(keyWords);
      var result = new SearchingViewModel()
      {
        Artists = artists,
        Songs = songs
      };
      return Json(result);
    }

    [HttpGet]
    [Route("GetLastPlayedSongs")]
    public async Task<IHttpActionResult> GetLastPlayedSongs() // Method used for retrieving user's last played songs
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      var result = await _songService.GetLastPlayedSongs(user.Id);
      return Json(result);
    }

    [HttpPost]
    [Route("GetSongsOfArtist")]
    public async Task<IHttpActionResult> GetSongsOfArtist([FromBody]string artist) // Method used for searching similiar songs or artists to user's typed text
    {
      var result = await _songService.GetSongsOfArtist(artist);
      return Json(result);
    }

    [HttpPost]
    [Route("AddSongToPlaylist")]
    public async Task<IHttpActionResult> AddSongToPlaylist(PlaylistSong song) // Method used for adding a song to chosen playlist
    {
      if (song == null) return Json("false");
      var flag = await _songService.AddSongToPLaylist(song);
      return Json(flag.ToString());
    }

    [HttpPost]
    [Route("AddFavoriteSong")]
    public async Task<IHttpActionResult> AddFavoriteSong([FromBody]int songId) // Method user for adding a song to favorites
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (songId <= 0 || user == null) return Json("false");

      var songExist = await _songService.CheckFavoriteSongForExistense(songId, user.Id);
      if (songExist) return Json("true");
      var favSong = new FavoriteSong()
      {
        SongId = songId,
        UserId = user.Id
      };
      await _songService.AddFavoriteSong(favSong);
      return Json("true");
    }

    [HttpPost]
    [Route("RemoveSongFromInstance")]
    public async Task<IHttpActionResult> RemoveSongFromInstance(DeletedInstance instance) // Method used for removing a song from favorites
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (user == null) return Json(false);
      var flag = await _songService.DeleteSongFromInstance(instance.SongId, user.Id, instance.Type, instance.PlaylistId);
      return Json(flag);
    }

    [HttpGet]
    [Route("GetFavoriteSongs")]
    public async Task<IHttpActionResult> GetFavoriteSongs() // Method used for yielding a list of user's favorite songs
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (user == null) return Json(false);
      var songs = await _songService.GetUserFavoriteSongs(user.Id);
      return Json(songs);
    }

    [HttpGet]
    [Route("GetPlaylists")]
    public async Task<IHttpActionResult> GetPlaylists() // Method used for yielding a list of user's playlists
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (user == null) return Json(false);
      var playLists = await _songService.GetUserPlaylists(user.Id);
      return Json(playLists);
    }

    [HttpPost]
    [Route("GetPlaylistSongs")]
    public async Task<IHttpActionResult> GetPlaylistSongs([FromBody]int playlistId) // Method used for yielding a list of user's chosen playlist's songs
    {
      if (playlistId <= 0) return Json(false);
      var songs = await _songService.GetPlaylistSongs(playlistId);
      return Json(songs);
    }

    [HttpGet]
    [Route("GetUploadedSongs")]
    public async Task<IHttpActionResult> GetUploadedSongs() // Method used for yielding a list of user's uploaded songs
    {
      var userId = _userService.GetUserByName(User.Identity.Name).Id;
      var songs = await _songService.GetUploadedSongs(userId);
      return Json(songs);
    }

    [HttpGet]
    [Route("GetTopListenedSongs")]
    public async Task<IHttpActionResult> GetTopListenedSongs() // Method used for yielding a list of most played songs
    {
      var songs = await _songService.GetTopListenedSongs();
      return Json(songs);
    }

    [HttpPost]
    [Route("CreatePlaylist")]
    public async Task<IHttpActionResult> CreatePlaylist(Playlist playlist) // Method used for creating a new user's playlist
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (user == null) return Json(false);
      playlist.Cover = "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNTEyIDUxMiIgd2lkdGg9IjUxMnB4Ij48cGF0aCBkPSJtNTUuMTc1NzgxIDI1NmgtMTUuMTAxNTYyYzAtNTcuNjc1NzgxIDIyLjQ1NzAzMS0xMTEuODk4NDM4IDYzLjI0MjE4Ny0xNTIuNjgzNTk0czk1LjAwNzgxMy02My4yNDIxODcgMTUyLjY4MzU5NC02My4yNDIxODd2MTUuMTAxNTYyYy0xMTAuNzM0Mzc1IDAtMjAwLjgyNDIxOSA5MC4wODk4NDQtMjAwLjgyNDIxOSAyMDAuODI0MjE5em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtMjU2IDQ3MS45MjU3ODF2LTE1LjEwMTU2MmMxMTAuNzM0Mzc1IDAgMjAwLjgyNDIxOS05MC4wODk4NDQgMjAwLjgyNDIxOS0yMDAuODI0MjE5aDE1LjEwNTQ2OWMwIDU3LjY3NTc4MS0yMi40NjA5MzggMTExLjg5ODQzOC02My4yNDYwOTQgMTUyLjY4MzU5NHMtOTUuMDA3ODEzIDYzLjI0MjE4Ny0xNTIuNjgzNTk0IDYzLjI0MjE4N3ptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTQxLjkzMzU5NCAzNjYuNTg5ODQ0Yy0xNy44MDA3ODItMzQuMzg2NzE5LTI2LjgyODEyNS03MS41OTM3NS0yNi44MjgxMjUtMTEwLjU4OTg0NCAwLTY0LjM0Mzc1IDI1LjA1ODU5My0xMjQuODM1OTM4IDcwLjU1ODU5My0xNzAuMzM1OTM4czEwNS45OTIxODgtNzAuNTU4NTkzIDE3MC4zMzU5MzgtNzAuNTU4NTkzYzQ3LjE3MTg3NSAwIDkyLjg1MTU2MiAxMy42Mjg5MDYgMTMyLjA5NzY1NiAzOS40MTc5NjlsOC4yOTY4NzUtMTIuNjI1Yy00MS43MjI2NTYtMjcuNDEwMTU3LTkwLjI2OTUzMS00MS44OTg0MzgtMTQwLjM5NDUzMS00MS44OTg0MzgtNjguMzc1IDAtMTMyLjY2NDA2MiAyNi42Mjg5MDYtMTgxLjAxNTYyNSA3NC45ODQzNzUtNDguMzU1NDY5IDQ4LjM1MTU2My03NC45ODQzNzUgMTEyLjY0MDYyNS03NC45ODQzNzUgMTgxLjAxNTYyNSAwIDQwLjg1MTU2MiA5Ljg2MzI4MSA4MS40OTYwOTQgMjguNTE5NTMxIDExNy41MzUxNTYgMTguMDU0Njg4IDM0Ljg2NzE4OCA0NC4zNjMyODEgNjUuNjEzMjgyIDc2LjA4MjAzMSA4OC45MTQwNjNsOC45NDUzMTMtMTIuMTcxODc1Yy0yOS44NTkzNzUtMjEuOTMzNTk0LTU0LjYyMTA5NC01MC44NzEwOTQtNzEuNjEzMjgxLTgzLjY4NzV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00MTguMDQyOTY5IDU3LjgwODU5NC05LjU2NjQwNyAxMS42ODc1YzU2LjE5MTQwNyA0NS45OTYwOTQgODguNDE3OTY5IDExMy45NzY1NjIgODguNDE3OTY5IDE4Ni41MDM5MDYgMCA2NC4zMzk4NDQtMjUuMDU4NTkzIDEyNC44MzU5MzgtNzAuNTU4NTkzIDE3MC4zMzU5MzhzLTEwNS45OTIxODggNzAuNTU4NTkzLTE3MC4zMzU5MzggNzAuNTU4NTkzYy00MS41NDI5NjkgMC04Mi40OTYwOTQtMTAuNzQ2MDkzLTExOC40MzM1OTQtMzEuMDc4MTI1bC03LjQzNzUgMTMuMTQ4NDM4YzM4LjIwMzEyNSAyMS42MDkzNzUgODEuNzI2NTYzIDMzLjAzNTE1NiAxMjUuODcxMDk0IDMzLjAzNTE1NiA2OC4zNzUgMCAxMzIuNjY0MDYyLTI2LjYyODkwNiAxODEuMDE1NjI1LTc0Ljk4NDM3NSA0OC4zNTU0NjktNDguMzUxNTYzIDc0Ljk4NDM3NS0xMTIuNjQwNjI1IDc0Ljk4NDM3NS0xODEuMDE1NjI1IDAtNzcuMDc0MjE5LTM0LjI0NjA5NC0xNDkuMzEyNS05My45NTcwMzEtMTk4LjE5MTQwNnptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTMwNC4wODU5MzggMjMyLjQxNzk2OWMtMjYuMjU3ODEzIDAtNDcuNjI1IDIxLjM2MzI4MS00Ny42MjUgNDcuNjI1czIxLjM2NzE4NyA0Ny42MjUgNDcuNjI1IDQ3LjYyNWMyNi4yNjE3MTggMCA0Ny42MjUtMjEuMzYzMjgxIDQ3LjYyNS00Ny42MjV2LTE3OS40NTcwMzFsLTEzNS4zMjAzMTMgNTQuMTI4OTA2djE3MC43MjY1NjJjLTguNTE5NTMxLTcuOTc2NTYyLTE5Ljk1NzAzMS0xMi44Nzg5MDYtMzIuNTE5NTMxLTEyLjg3ODkwNi0yNi4yNjE3MTkgMC00Ny42MjUgMjEuMzYzMjgxLTQ3LjYyNSA0Ny42MjVzMjEuMzYzMjgxIDQ3LjYyNSA0Ny42MjUgNDcuNjI1YzI2LjI2MTcxOCAwIDQ3LjYyNS0yMS4zNjMyODEgNDcuNjI1LTQ3LjYyNXYtMTk1LjI0NjA5NGwxMDUuMTA5Mzc1LTQyLjA0Njg3NXYxMjIuNDAyMzQ0Yy04LjUxOTUzMS03Ljk4MDQ2OS0xOS45NTMxMjUtMTIuODc4OTA2LTMyLjUxOTUzMS0xMi44Nzg5MDZ6bS0xMjAuMjE0ODQ0IDE2MC4yODkwNjJjLTE3LjkzMzU5NCAwLTMyLjUxOTUzMi0xNC41ODk4NDMtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkzMzU5NCAxNC41ODU5MzgtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTI5Njg3IDAgMzIuNTE5NTMxIDE0LjU4NTkzNyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTI5Njg4LTE0LjU4OTg0NCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMTIwLjIxNDg0NC04MC4xNDQ1MzFjLTE3LjkyOTY4OCAwLTMyLjUxOTUzMi0xNC41ODU5MzgtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkyOTY4OCAxNC41ODk4NDQtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTMzNTkzIDAgMzIuNTE5NTMxIDE0LjU4OTg0MyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTMzNTkzLTE0LjU4NTkzOCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTM5LjYwOTM3NSAyNzIuNDkyMTg4aDE2LjAzMTI1djE1LjEwMTU2MmgtMTYuMDMxMjV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00NTYuMzU5Mzc1IDIyNC40MDYyNWgxNi4wMzEyNXYxNS4xMDE1NjJoLTE2LjAzMTI1em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNDI0LjMwNDY4OCAyMjQuNDA2MjVoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNzEuNjY3OTY5IDI3Mi40OTIxODhoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48L3N2Zz4K";
      playlist.UserId = user.Id;
      playlist.Created = DateTime.Now;
      var res = await _songService.CreatePlaylist(playlist);
      return Json(res);
    }

    [HttpGet]
    [Route("GetSongs")]
    public async Task<IHttpActionResult> GetSongs() // Method used for yielding all the songs out of the database
    {
      var songs = await _songService.GetAllSongs();
      return Json(songs);
    }
  }
}
