using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using Server.Models;
using Server.Services;
using Server.SessionHandlers;

namespace Server.Controllers
{
  [SessionAuthorize]
  [RoutePrefix("api/Songs")]
  public class SongsController : ApiController
  {
    private readonly SongService _songService;
    private readonly UserService _userService;

    public SongsController()
    {
      this._songService = new SongService();
      this._userService = new UserService();
    }

    [HttpPost]
    [Route("UploadSongs")]
    public async Task<IHttpActionResult> UploadSongs(List<Song> songs)
    {
      if (songs.Capacity <= 0) return Json("false");
      foreach (var song in songs)
      {
        await _songService.AddSong(song);
      }

      return Json("true");

    }
    [HttpPost]
    [Route("IncreaseActivity")]
    public async Task<IHttpActionResult> IncreaseActivity([FromBody]int songId)
    {
      if (songId <= 0) return Json("false");
      await _songService.IncreaseActivity(songId);
      return Json("true");

    }

    [HttpPost]
    [Route("AddFavoriteSong")]
    public async Task<IHttpActionResult> AddFavoriteSong([FromBody]int songId)
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (songId <= 0 || user == null) return Json("false");

      var songExist = await _songService.CheckFavoriteSongForExistense(songId, user.Id);
      if (!songExist)
      {
        var favSong = new FavoriteSong()
        {
          SongId = songId,
          UserId = user.Id
        };
        await _songService.AddFavoriteSong(favSong);
      }
      return Json("true");
    }

    [HttpPost]
    [Route("DeleteFavoriteSong")]
    public async Task<IHttpActionResult> DeleteFavoriteSong([FromBody]int songId)
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (songId <= 0 || user == null) return Json("false");
      var flag = await _songService.Delete(songId, user.Id);
      return Json(flag);
    }

    [HttpGet]
    [Route("GetFavoriteSongs")]
    public async Task<IHttpActionResult> GetFavoriteSongs()
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (user == null) return Json(false);
      var songs = await _songService.GetUserFavoriteSongs(user.Id);
      return Json(songs);
    }

    [HttpGet]
    [Route("GetPlaylists")]
    public async Task<IHttpActionResult> GetPlaylists()
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (user == null) return Json(false);
      var playLists = await _songService.GetUserPlaylists(user.Id);
      return Json(playLists);
    }

    [HttpGet]
    [Route("GetPlaylistSongs")]
    public async Task<IHttpActionResult> GetPlaylistSongs(int playlistId)
    {
      if (playlistId <= 0) return Json(false);
      var songs = await _songService.GetPlaylistSongs(playlistId);
      return Json(songs);

    }

    [HttpPost]
    [Route("CreatePlaylist")]
    public async Task<IHttpActionResult> CreatePlaylist(Playlist playlist)
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (user == null) return Json(false);
      playlist.Cover = "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pg0KPCEtLSBHZW5lcmF0b3I6IEFkb2JlIElsbHVzdHJhdG9yIDE5LjAuMCwgU1ZHIEV4cG9ydCBQbHVnLUluIC4gU1ZHIFZlcnNpb246IDYuMDAgQnVpbGQgMCkgIC0tPg0KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHhtbG5zOnhsaW5rPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5L3hsaW5rIiB2ZXJzaW9uPSIxLjEiIGlkPSJDYXBhXzEiIHg9IjBweCIgeT0iMHB4IiB2aWV3Qm94PSIwIDAgNTUgNTUiIHN0eWxlPSJlbmFibGUtYmFja2dyb3VuZDpuZXcgMCAwIDU1IDU1OyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSIgd2lkdGg9IjUxMnB4IiBoZWlnaHQ9IjUxMnB4Ij4NCjxwYXRoIGQ9Ik01Mi42NiwwLjI0OWMtMC4yMTYtMC4xODktMC41MDEtMC4yNzUtMC43ODktMC4yNDFsLTMxLDQuMDExQzIwLjM3Myw0LjA4NCwyMCw0LjUwNywyMCw1LjAxdjYuMDE3djQuMjEydjI1LjM4NCAgQzE4LjE3NCwzOC40MjgsMTUuMjczLDM3LDEyLDM3Yy01LjUxNCwwLTEwLDQuMDM3LTEwLDlzNC40ODYsOSwxMCw5czEwLTQuMDM3LDEwLTljMC0wLjIzMi0wLjAxOS0wLjQ2LTAuMDM5LTAuNjg3ICBDMjEuOTc0LDQ1LjI0OCwyMiw0NS4xODksMjIsNDUuMTIxVjE2LjExOGwyOS0zLjc1M3YxOC4yNTdDNDkuMTc0LDI4LjQyOCw0Ni4yNzMsMjcsNDMsMjdjLTUuNTE0LDAtMTAsNC4wMzctMTAsOXM0LjQ4Niw5LDEwLDkgIGM1LjQ2NCwwLDkuOTEzLTMuOTY2LDkuOTkzLTguODY3YzAtMC4wMTMsMC4wMDctMC4wMjQsMC4wMDctMC4wMzdWMTEuMjI3VjcuMDE2VjFDNTMsMC43MTIsNTIuODc2LDAuNDM4LDUyLjY2LDAuMjQ5eiBNMTIsNTMgIGMtNC40MTEsMC04LTMuMTQxLTgtN3MzLjU4OS03LDgtN3M4LDMuMTQxLDgsN1MxNi40MTEsNTMsMTIsNTN6IE00Myw0M2MtNC40MTEsMC04LTMuMTQxLTgtN3MzLjU4OS03LDgtN3M4LDMuMTQxLDgsNyAgUzQ3LjQxMSw0Myw0Myw0M3ogTTIyLDE0LjEwMXYtMy4wNzRWNS44ODlsMjktMy43NTJ2NC44Nzl2My4zMzJMMjIsMTQuMTAxeiIgZmlsbD0iI2Q2ZDZkNiIvPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPC9zdmc+DQo=";
      playlist.UserId = user.Id;
      playlist.Created = DateTime.Now;
      var res = await _songService.CreatePlaylist(playlist);
      return Json(res);
    }

    [HttpGet]
    [Route("GetFavoriteSongsIndexes")]
    public async Task<IHttpActionResult> GetFavoriteSongsIndexes()
    {
      var user = _userService.GetUserByName(User.Identity.Name);
      if (user == null) return Json(false);
      var songs = await _songService.GetUserFavoriteSongs(user.Id);
      var indexes = new int[songs.Capacity];
      foreach (var song in songs.Select((value, i) => new { i, value }))
      {
        indexes[song.i] = song.value.Id;
      }
      return Json(indexes);
    }

    [HttpGet]
    [Route("GetSongs")]
    public async Task<IHttpActionResult> GetSongs()
    {
      var songs = await _songService.GetAllSongs();
      return Json(songs);
    }
  }
}
