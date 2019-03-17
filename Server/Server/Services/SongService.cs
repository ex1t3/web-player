using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DevOne.Security.Cryptography.BCrypt;
using Server.DAL;
using Server.Models;
using Server.REST;
using System.Threading;
namespace Server.Services
{
  public class SongService
  {
    private readonly WebPlayerDbContext _db;
    private readonly IDbRepository<Song> _dbSong;
    private readonly IDbRepository<UploadedSong> _dbUploadedSong;
    private readonly IDbRepository<Playlist> _dbPlaylist;
    private readonly IDbRepository<PlaylistSong> _dbPlaylistSongs;
    private readonly IDbRepository<FavoriteSong> _dbFavoriteSong;

    public SongService()
    {
      _db = new WebPlayerDbContext();
      _dbSong = new DbRepository<Song>(new DefaultDbFactory());
      _dbUploadedSong = new DbRepository<UploadedSong>(new DefaultDbFactory());
      _dbPlaylist = new DbRepository<Playlist>(new DefaultDbFactory());
      _dbPlaylistSongs = new DbRepository<PlaylistSong>(new DefaultDbFactory());
      _dbFavoriteSong = new DbRepository<FavoriteSong>(new DefaultDbFactory());
    }
    public async Task<Song> AddSong(Song song, int userId, string path)
    {
      if (string.IsNullOrEmpty(song.Name) || string.IsNullOrEmpty(song.Artist)) return null;
      song = CheckForTrashTitles(song);
      var lookUpper = new SongSearchAPI();
      var data = await lookUpper.Search(song.Name, song.Artist);
      if (data != null)
      {
        //song.Name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(data.track_name.ToLower()) ?? song.Name;
        song.Album = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(data.album_name.ToLower()) ?? song.Album;
        // song.Artist = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(data.artist_name.ToLower()) ?? song.Artist;
        if (data.primary_genres.music_genre.Count > 0) song.Genre = data.primary_genres.music_genre[0].music_genre_name ?? song.Genre;
      }
      var songToUpload = _dbSong.Get(x => x.Name == song.Name && x.Artist == song.Artist);
      if (songToUpload == null)
      {
        _dbSong.Add(song);
        _dbUploadedSong.Add(new UploadedSong() { SongId = song.Id, UserId = userId });
        return song;
      }
      File.Delete(path);
      if (_dbUploadedSong.Get(x => x.SongId == songToUpload.Id && x.UserId == userId) != null) return songToUpload;
      _dbUploadedSong.Add(new UploadedSong() { SongId = songToUpload.Id, UserId = userId });
      File.Delete(path);
      return songToUpload;
    }

    public Song CheckForTrashTitles(Song song)
    {
      var domains = new string[] { ".me", ".org", ".com", ".net", ".fm", ".top", ".music" }; // Need to be improved
      var symbols = new string[] { " ", "[", "(" };
      foreach (var domain in domains)
      {
        if (song.Name.Contains(domain))
        {
          foreach (var symbol in symbols)
          {
            var index = song.Name.LastIndexOf(symbol);
            if (index != -1)
            {
              song.Name = song.Name.Substring(0, index);
            }
          }
          song.AlbumCover =
            "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNTEyIDUxMiIgd2lkdGg9IjUxMnB4Ij48cGF0aCBkPSJtNTUuMTc1NzgxIDI1NmgtMTUuMTAxNTYyYzAtNTcuNjc1NzgxIDIyLjQ1NzAzMS0xMTEuODk4NDM4IDYzLjI0MjE4Ny0xNTIuNjgzNTk0czk1LjAwNzgxMy02My4yNDIxODcgMTUyLjY4MzU5NC02My4yNDIxODd2MTUuMTAxNTYyYy0xMTAuNzM0Mzc1IDAtMjAwLjgyNDIxOSA5MC4wODk4NDQtMjAwLjgyNDIxOSAyMDAuODI0MjE5em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtMjU2IDQ3MS45MjU3ODF2LTE1LjEwMTU2MmMxMTAuNzM0Mzc1IDAgMjAwLjgyNDIxOS05MC4wODk4NDQgMjAwLjgyNDIxOS0yMDAuODI0MjE5aDE1LjEwNTQ2OWMwIDU3LjY3NTc4MS0yMi40NjA5MzggMTExLjg5ODQzOC02My4yNDYwOTQgMTUyLjY4MzU5NHMtOTUuMDA3ODEzIDYzLjI0MjE4Ny0xNTIuNjgzNTk0IDYzLjI0MjE4N3ptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTQxLjkzMzU5NCAzNjYuNTg5ODQ0Yy0xNy44MDA3ODItMzQuMzg2NzE5LTI2LjgyODEyNS03MS41OTM3NS0yNi44MjgxMjUtMTEwLjU4OTg0NCAwLTY0LjM0Mzc1IDI1LjA1ODU5My0xMjQuODM1OTM4IDcwLjU1ODU5My0xNzAuMzM1OTM4czEwNS45OTIxODgtNzAuNTU4NTkzIDE3MC4zMzU5MzgtNzAuNTU4NTkzYzQ3LjE3MTg3NSAwIDkyLjg1MTU2MiAxMy42Mjg5MDYgMTMyLjA5NzY1NiAzOS40MTc5NjlsOC4yOTY4NzUtMTIuNjI1Yy00MS43MjI2NTYtMjcuNDEwMTU3LTkwLjI2OTUzMS00MS44OTg0MzgtMTQwLjM5NDUzMS00MS44OTg0MzgtNjguMzc1IDAtMTMyLjY2NDA2MiAyNi42Mjg5MDYtMTgxLjAxNTYyNSA3NC45ODQzNzUtNDguMzU1NDY5IDQ4LjM1MTU2My03NC45ODQzNzUgMTEyLjY0MDYyNS03NC45ODQzNzUgMTgxLjAxNTYyNSAwIDQwLjg1MTU2MiA5Ljg2MzI4MSA4MS40OTYwOTQgMjguNTE5NTMxIDExNy41MzUxNTYgMTguMDU0Njg4IDM0Ljg2NzE4OCA0NC4zNjMyODEgNjUuNjEzMjgyIDc2LjA4MjAzMSA4OC45MTQwNjNsOC45NDUzMTMtMTIuMTcxODc1Yy0yOS44NTkzNzUtMjEuOTMzNTk0LTU0LjYyMTA5NC01MC44NzEwOTQtNzEuNjEzMjgxLTgzLjY4NzV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00MTguMDQyOTY5IDU3LjgwODU5NC05LjU2NjQwNyAxMS42ODc1YzU2LjE5MTQwNyA0NS45OTYwOTQgODguNDE3OTY5IDExMy45NzY1NjIgODguNDE3OTY5IDE4Ni41MDM5MDYgMCA2NC4zMzk4NDQtMjUuMDU4NTkzIDEyNC44MzU5MzgtNzAuNTU4NTkzIDE3MC4zMzU5MzhzLTEwNS45OTIxODggNzAuNTU4NTkzLTE3MC4zMzU5MzggNzAuNTU4NTkzYy00MS41NDI5NjkgMC04Mi40OTYwOTQtMTAuNzQ2MDkzLTExOC40MzM1OTQtMzEuMDc4MTI1bC03LjQzNzUgMTMuMTQ4NDM4YzM4LjIwMzEyNSAyMS42MDkzNzUgODEuNzI2NTYzIDMzLjAzNTE1NiAxMjUuODcxMDk0IDMzLjAzNTE1NiA2OC4zNzUgMCAxMzIuNjY0MDYyLTI2LjYyODkwNiAxODEuMDE1NjI1LTc0Ljk4NDM3NSA0OC4zNTU0NjktNDguMzUxNTYzIDc0Ljk4NDM3NS0xMTIuNjQwNjI1IDc0Ljk4NDM3NS0xODEuMDE1NjI1IDAtNzcuMDc0MjE5LTM0LjI0NjA5NC0xNDkuMzEyNS05My45NTcwMzEtMTk4LjE5MTQwNnptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTMwNC4wODU5MzggMjMyLjQxNzk2OWMtMjYuMjU3ODEzIDAtNDcuNjI1IDIxLjM2MzI4MS00Ny42MjUgNDcuNjI1czIxLjM2NzE4NyA0Ny42MjUgNDcuNjI1IDQ3LjYyNWMyNi4yNjE3MTggMCA0Ny42MjUtMjEuMzYzMjgxIDQ3LjYyNS00Ny42MjV2LTE3OS40NTcwMzFsLTEzNS4zMjAzMTMgNTQuMTI4OTA2djE3MC43MjY1NjJjLTguNTE5NTMxLTcuOTc2NTYyLTE5Ljk1NzAzMS0xMi44Nzg5MDYtMzIuNTE5NTMxLTEyLjg3ODkwNi0yNi4yNjE3MTkgMC00Ny42MjUgMjEuMzYzMjgxLTQ3LjYyNSA0Ny42MjVzMjEuMzYzMjgxIDQ3LjYyNSA0Ny42MjUgNDcuNjI1YzI2LjI2MTcxOCAwIDQ3LjYyNS0yMS4zNjMyODEgNDcuNjI1LTQ3LjYyNXYtMTk1LjI0NjA5NGwxMDUuMTA5Mzc1LTQyLjA0Njg3NXYxMjIuNDAyMzQ0Yy04LjUxOTUzMS03Ljk4MDQ2OS0xOS45NTMxMjUtMTIuODc4OTA2LTMyLjUxOTUzMS0xMi44Nzg5MDZ6bS0xMjAuMjE0ODQ0IDE2MC4yODkwNjJjLTE3LjkzMzU5NCAwLTMyLjUxOTUzMi0xNC41ODk4NDMtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkzMzU5NCAxNC41ODU5MzgtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTI5Njg3IDAgMzIuNTE5NTMxIDE0LjU4NTkzNyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTI5Njg4LTE0LjU4OTg0NCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMTIwLjIxNDg0NC04MC4xNDQ1MzFjLTE3LjkyOTY4OCAwLTMyLjUxOTUzMi0xNC41ODU5MzgtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkyOTY4OCAxNC41ODk4NDQtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTMzNTkzIDAgMzIuNTE5NTMxIDE0LjU4OTg0MyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTMzNTkzLTE0LjU4NTkzOCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTM5LjYwOTM3NSAyNzIuNDkyMTg4aDE2LjAzMTI1djE1LjEwMTU2MmgtMTYuMDMxMjV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00NTYuMzU5Mzc1IDIyNC40MDYyNWgxNi4wMzEyNXYxNS4xMDE1NjJoLTE2LjAzMTI1em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNDI0LjMwNDY4OCAyMjQuNDA2MjVoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNzEuNjY3OTY5IDI3Mi40OTIxODhoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48L3N2Zz4K";
        }
        if (song.Album.Contains(domain))
        {
          foreach (var symbol in symbols)
          {
            var index = song.Album.LastIndexOf(symbol);
            if (index != -1)
            {
              song.Name = song.Album.Substring(0, index);
            }
          }
          song.AlbumCover =
            "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNTEyIDUxMiIgd2lkdGg9IjUxMnB4Ij48cGF0aCBkPSJtNTUuMTc1NzgxIDI1NmgtMTUuMTAxNTYyYzAtNTcuNjc1NzgxIDIyLjQ1NzAzMS0xMTEuODk4NDM4IDYzLjI0MjE4Ny0xNTIuNjgzNTk0czk1LjAwNzgxMy02My4yNDIxODcgMTUyLjY4MzU5NC02My4yNDIxODd2MTUuMTAxNTYyYy0xMTAuNzM0Mzc1IDAtMjAwLjgyNDIxOSA5MC4wODk4NDQtMjAwLjgyNDIxOSAyMDAuODI0MjE5em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtMjU2IDQ3MS45MjU3ODF2LTE1LjEwMTU2MmMxMTAuNzM0Mzc1IDAgMjAwLjgyNDIxOS05MC4wODk4NDQgMjAwLjgyNDIxOS0yMDAuODI0MjE5aDE1LjEwNTQ2OWMwIDU3LjY3NTc4MS0yMi40NjA5MzggMTExLjg5ODQzOC02My4yNDYwOTQgMTUyLjY4MzU5NHMtOTUuMDA3ODEzIDYzLjI0MjE4Ny0xNTIuNjgzNTk0IDYzLjI0MjE4N3ptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTQxLjkzMzU5NCAzNjYuNTg5ODQ0Yy0xNy44MDA3ODItMzQuMzg2NzE5LTI2LjgyODEyNS03MS41OTM3NS0yNi44MjgxMjUtMTEwLjU4OTg0NCAwLTY0LjM0Mzc1IDI1LjA1ODU5My0xMjQuODM1OTM4IDcwLjU1ODU5My0xNzAuMzM1OTM4czEwNS45OTIxODgtNzAuNTU4NTkzIDE3MC4zMzU5MzgtNzAuNTU4NTkzYzQ3LjE3MTg3NSAwIDkyLjg1MTU2MiAxMy42Mjg5MDYgMTMyLjA5NzY1NiAzOS40MTc5NjlsOC4yOTY4NzUtMTIuNjI1Yy00MS43MjI2NTYtMjcuNDEwMTU3LTkwLjI2OTUzMS00MS44OTg0MzgtMTQwLjM5NDUzMS00MS44OTg0MzgtNjguMzc1IDAtMTMyLjY2NDA2MiAyNi42Mjg5MDYtMTgxLjAxNTYyNSA3NC45ODQzNzUtNDguMzU1NDY5IDQ4LjM1MTU2My03NC45ODQzNzUgMTEyLjY0MDYyNS03NC45ODQzNzUgMTgxLjAxNTYyNSAwIDQwLjg1MTU2MiA5Ljg2MzI4MSA4MS40OTYwOTQgMjguNTE5NTMxIDExNy41MzUxNTYgMTguMDU0Njg4IDM0Ljg2NzE4OCA0NC4zNjMyODEgNjUuNjEzMjgyIDc2LjA4MjAzMSA4OC45MTQwNjNsOC45NDUzMTMtMTIuMTcxODc1Yy0yOS44NTkzNzUtMjEuOTMzNTk0LTU0LjYyMTA5NC01MC44NzEwOTQtNzEuNjEzMjgxLTgzLjY4NzV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00MTguMDQyOTY5IDU3LjgwODU5NC05LjU2NjQwNyAxMS42ODc1YzU2LjE5MTQwNyA0NS45OTYwOTQgODguNDE3OTY5IDExMy45NzY1NjIgODguNDE3OTY5IDE4Ni41MDM5MDYgMCA2NC4zMzk4NDQtMjUuMDU4NTkzIDEyNC44MzU5MzgtNzAuNTU4NTkzIDE3MC4zMzU5MzhzLTEwNS45OTIxODggNzAuNTU4NTkzLTE3MC4zMzU5MzggNzAuNTU4NTkzYy00MS41NDI5NjkgMC04Mi40OTYwOTQtMTAuNzQ2MDkzLTExOC40MzM1OTQtMzEuMDc4MTI1bC03LjQzNzUgMTMuMTQ4NDM4YzM4LjIwMzEyNSAyMS42MDkzNzUgODEuNzI2NTYzIDMzLjAzNTE1NiAxMjUuODcxMDk0IDMzLjAzNTE1NiA2OC4zNzUgMCAxMzIuNjY0MDYyLTI2LjYyODkwNiAxODEuMDE1NjI1LTc0Ljk4NDM3NSA0OC4zNTU0NjktNDguMzUxNTYzIDc0Ljk4NDM3NS0xMTIuNjQwNjI1IDc0Ljk4NDM3NS0xODEuMDE1NjI1IDAtNzcuMDc0MjE5LTM0LjI0NjA5NC0xNDkuMzEyNS05My45NTcwMzEtMTk4LjE5MTQwNnptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTMwNC4wODU5MzggMjMyLjQxNzk2OWMtMjYuMjU3ODEzIDAtNDcuNjI1IDIxLjM2MzI4MS00Ny42MjUgNDcuNjI1czIxLjM2NzE4NyA0Ny42MjUgNDcuNjI1IDQ3LjYyNWMyNi4yNjE3MTggMCA0Ny42MjUtMjEuMzYzMjgxIDQ3LjYyNS00Ny42MjV2LTE3OS40NTcwMzFsLTEzNS4zMjAzMTMgNTQuMTI4OTA2djE3MC43MjY1NjJjLTguNTE5NTMxLTcuOTc2NTYyLTE5Ljk1NzAzMS0xMi44Nzg5MDYtMzIuNTE5NTMxLTEyLjg3ODkwNi0yNi4yNjE3MTkgMC00Ny42MjUgMjEuMzYzMjgxLTQ3LjYyNSA0Ny42MjVzMjEuMzYzMjgxIDQ3LjYyNSA0Ny42MjUgNDcuNjI1YzI2LjI2MTcxOCAwIDQ3LjYyNS0yMS4zNjMyODEgNDcuNjI1LTQ3LjYyNXYtMTk1LjI0NjA5NGwxMDUuMTA5Mzc1LTQyLjA0Njg3NXYxMjIuNDAyMzQ0Yy04LjUxOTUzMS03Ljk4MDQ2OS0xOS45NTMxMjUtMTIuODc4OTA2LTMyLjUxOTUzMS0xMi44Nzg5MDZ6bS0xMjAuMjE0ODQ0IDE2MC4yODkwNjJjLTE3LjkzMzU5NCAwLTMyLjUxOTUzMi0xNC41ODk4NDMtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkzMzU5NCAxNC41ODU5MzgtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTI5Njg3IDAgMzIuNTE5NTMxIDE0LjU4NTkzNyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTI5Njg4LTE0LjU4OTg0NCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMTIwLjIxNDg0NC04MC4xNDQ1MzFjLTE3LjkyOTY4OCAwLTMyLjUxOTUzMi0xNC41ODU5MzgtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkyOTY4OCAxNC41ODk4NDQtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTMzNTkzIDAgMzIuNTE5NTMxIDE0LjU4OTg0MyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTMzNTkzLTE0LjU4NTkzOCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTM5LjYwOTM3NSAyNzIuNDkyMTg4aDE2LjAzMTI1djE1LjEwMTU2MmgtMTYuMDMxMjV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00NTYuMzU5Mzc1IDIyNC40MDYyNWgxNi4wMzEyNXYxNS4xMDE1NjJoLTE2LjAzMTI1em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNDI0LjMwNDY4OCAyMjQuNDA2MjVoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNzEuNjY3OTY5IDI3Mi40OTIxODhoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48L3N2Zz4K";
        }
        if (song.Artist.Contains(domain))
        {
          foreach (var symbol in symbols)
          {
            var index = song.Artist.LastIndexOf(symbol);
            if (index != -1)
            {
              song.Artist = song.Name.Substring(0, index);
            }
          }
          song.AlbumCover =
            "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNTEyIDUxMiIgd2lkdGg9IjUxMnB4Ij48cGF0aCBkPSJtNTUuMTc1NzgxIDI1NmgtMTUuMTAxNTYyYzAtNTcuNjc1NzgxIDIyLjQ1NzAzMS0xMTEuODk4NDM4IDYzLjI0MjE4Ny0xNTIuNjgzNTk0czk1LjAwNzgxMy02My4yNDIxODcgMTUyLjY4MzU5NC02My4yNDIxODd2MTUuMTAxNTYyYy0xMTAuNzM0Mzc1IDAtMjAwLjgyNDIxOSA5MC4wODk4NDQtMjAwLjgyNDIxOSAyMDAuODI0MjE5em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtMjU2IDQ3MS45MjU3ODF2LTE1LjEwMTU2MmMxMTAuNzM0Mzc1IDAgMjAwLjgyNDIxOS05MC4wODk4NDQgMjAwLjgyNDIxOS0yMDAuODI0MjE5aDE1LjEwNTQ2OWMwIDU3LjY3NTc4MS0yMi40NjA5MzggMTExLjg5ODQzOC02My4yNDYwOTQgMTUyLjY4MzU5NHMtOTUuMDA3ODEzIDYzLjI0MjE4Ny0xNTIuNjgzNTk0IDYzLjI0MjE4N3ptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTQxLjkzMzU5NCAzNjYuNTg5ODQ0Yy0xNy44MDA3ODItMzQuMzg2NzE5LTI2LjgyODEyNS03MS41OTM3NS0yNi44MjgxMjUtMTEwLjU4OTg0NCAwLTY0LjM0Mzc1IDI1LjA1ODU5My0xMjQuODM1OTM4IDcwLjU1ODU5My0xNzAuMzM1OTM4czEwNS45OTIxODgtNzAuNTU4NTkzIDE3MC4zMzU5MzgtNzAuNTU4NTkzYzQ3LjE3MTg3NSAwIDkyLjg1MTU2MiAxMy42Mjg5MDYgMTMyLjA5NzY1NiAzOS40MTc5NjlsOC4yOTY4NzUtMTIuNjI1Yy00MS43MjI2NTYtMjcuNDEwMTU3LTkwLjI2OTUzMS00MS44OTg0MzgtMTQwLjM5NDUzMS00MS44OTg0MzgtNjguMzc1IDAtMTMyLjY2NDA2MiAyNi42Mjg5MDYtMTgxLjAxNTYyNSA3NC45ODQzNzUtNDguMzU1NDY5IDQ4LjM1MTU2My03NC45ODQzNzUgMTEyLjY0MDYyNS03NC45ODQzNzUgMTgxLjAxNTYyNSAwIDQwLjg1MTU2MiA5Ljg2MzI4MSA4MS40OTYwOTQgMjguNTE5NTMxIDExNy41MzUxNTYgMTguMDU0Njg4IDM0Ljg2NzE4OCA0NC4zNjMyODEgNjUuNjEzMjgyIDc2LjA4MjAzMSA4OC45MTQwNjNsOC45NDUzMTMtMTIuMTcxODc1Yy0yOS44NTkzNzUtMjEuOTMzNTk0LTU0LjYyMTA5NC01MC44NzEwOTQtNzEuNjEzMjgxLTgzLjY4NzV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00MTguMDQyOTY5IDU3LjgwODU5NC05LjU2NjQwNyAxMS42ODc1YzU2LjE5MTQwNyA0NS45OTYwOTQgODguNDE3OTY5IDExMy45NzY1NjIgODguNDE3OTY5IDE4Ni41MDM5MDYgMCA2NC4zMzk4NDQtMjUuMDU4NTkzIDEyNC44MzU5MzgtNzAuNTU4NTkzIDE3MC4zMzU5MzhzLTEwNS45OTIxODggNzAuNTU4NTkzLTE3MC4zMzU5MzggNzAuNTU4NTkzYy00MS41NDI5NjkgMC04Mi40OTYwOTQtMTAuNzQ2MDkzLTExOC40MzM1OTQtMzEuMDc4MTI1bC03LjQzNzUgMTMuMTQ4NDM4YzM4LjIwMzEyNSAyMS42MDkzNzUgODEuNzI2NTYzIDMzLjAzNTE1NiAxMjUuODcxMDk0IDMzLjAzNTE1NiA2OC4zNzUgMCAxMzIuNjY0MDYyLTI2LjYyODkwNiAxODEuMDE1NjI1LTc0Ljk4NDM3NSA0OC4zNTU0NjktNDguMzUxNTYzIDc0Ljk4NDM3NS0xMTIuNjQwNjI1IDc0Ljk4NDM3NS0xODEuMDE1NjI1IDAtNzcuMDc0MjE5LTM0LjI0NjA5NC0xNDkuMzEyNS05My45NTcwMzEtMTk4LjE5MTQwNnptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTMwNC4wODU5MzggMjMyLjQxNzk2OWMtMjYuMjU3ODEzIDAtNDcuNjI1IDIxLjM2MzI4MS00Ny42MjUgNDcuNjI1czIxLjM2NzE4NyA0Ny42MjUgNDcuNjI1IDQ3LjYyNWMyNi4yNjE3MTggMCA0Ny42MjUtMjEuMzYzMjgxIDQ3LjYyNS00Ny42MjV2LTE3OS40NTcwMzFsLTEzNS4zMjAzMTMgNTQuMTI4OTA2djE3MC43MjY1NjJjLTguNTE5NTMxLTcuOTc2NTYyLTE5Ljk1NzAzMS0xMi44Nzg5MDYtMzIuNTE5NTMxLTEyLjg3ODkwNi0yNi4yNjE3MTkgMC00Ny42MjUgMjEuMzYzMjgxLTQ3LjYyNSA0Ny42MjVzMjEuMzYzMjgxIDQ3LjYyNSA0Ny42MjUgNDcuNjI1YzI2LjI2MTcxOCAwIDQ3LjYyNS0yMS4zNjMyODEgNDcuNjI1LTQ3LjYyNXYtMTk1LjI0NjA5NGwxMDUuMTA5Mzc1LTQyLjA0Njg3NXYxMjIuNDAyMzQ0Yy04LjUxOTUzMS03Ljk4MDQ2OS0xOS45NTMxMjUtMTIuODc4OTA2LTMyLjUxOTUzMS0xMi44Nzg5MDZ6bS0xMjAuMjE0ODQ0IDE2MC4yODkwNjJjLTE3LjkzMzU5NCAwLTMyLjUxOTUzMi0xNC41ODk4NDMtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkzMzU5NCAxNC41ODU5MzgtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTI5Njg3IDAgMzIuNTE5NTMxIDE0LjU4NTkzNyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTI5Njg4LTE0LjU4OTg0NCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMTIwLjIxNDg0NC04MC4xNDQ1MzFjLTE3LjkyOTY4OCAwLTMyLjUxOTUzMi0xNC41ODU5MzgtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkyOTY4OCAxNC41ODk4NDQtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTMzNTkzIDAgMzIuNTE5NTMxIDE0LjU4OTg0MyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTMzNTkzLTE0LjU4NTkzOCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTM5LjYwOTM3NSAyNzIuNDkyMTg4aDE2LjAzMTI1djE1LjEwMTU2MmgtMTYuMDMxMjV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00NTYuMzU5Mzc1IDIyNC40MDYyNWgxNi4wMzEyNXYxNS4xMDE1NjJoLTE2LjAzMTI1em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNDI0LjMwNDY4OCAyMjQuNDA2MjVoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNzEuNjY3OTY5IDI3Mi40OTIxODhoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48L3N2Zz4K";
        }
        if (song.Genre.Contains(domain))
        {
          foreach (var symbol in symbols)
          {
            var index = song.Genre.LastIndexOf(symbol);
            if (index != -1)
            {
              song.Genre = song.Genre.Substring(0, index);
            }
          }
          song.AlbumCover =
            "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNTEyIDUxMiIgd2lkdGg9IjUxMnB4Ij48cGF0aCBkPSJtNTUuMTc1NzgxIDI1NmgtMTUuMTAxNTYyYzAtNTcuNjc1NzgxIDIyLjQ1NzAzMS0xMTEuODk4NDM4IDYzLjI0MjE4Ny0xNTIuNjgzNTk0czk1LjAwNzgxMy02My4yNDIxODcgMTUyLjY4MzU5NC02My4yNDIxODd2MTUuMTAxNTYyYy0xMTAuNzM0Mzc1IDAtMjAwLjgyNDIxOSA5MC4wODk4NDQtMjAwLjgyNDIxOSAyMDAuODI0MjE5em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtMjU2IDQ3MS45MjU3ODF2LTE1LjEwMTU2MmMxMTAuNzM0Mzc1IDAgMjAwLjgyNDIxOS05MC4wODk4NDQgMjAwLjgyNDIxOS0yMDAuODI0MjE5aDE1LjEwNTQ2OWMwIDU3LjY3NTc4MS0yMi40NjA5MzggMTExLjg5ODQzOC02My4yNDYwOTQgMTUyLjY4MzU5NHMtOTUuMDA3ODEzIDYzLjI0MjE4Ny0xNTIuNjgzNTk0IDYzLjI0MjE4N3ptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTQxLjkzMzU5NCAzNjYuNTg5ODQ0Yy0xNy44MDA3ODItMzQuMzg2NzE5LTI2LjgyODEyNS03MS41OTM3NS0yNi44MjgxMjUtMTEwLjU4OTg0NCAwLTY0LjM0Mzc1IDI1LjA1ODU5My0xMjQuODM1OTM4IDcwLjU1ODU5My0xNzAuMzM1OTM4czEwNS45OTIxODgtNzAuNTU4NTkzIDE3MC4zMzU5MzgtNzAuNTU4NTkzYzQ3LjE3MTg3NSAwIDkyLjg1MTU2MiAxMy42Mjg5MDYgMTMyLjA5NzY1NiAzOS40MTc5NjlsOC4yOTY4NzUtMTIuNjI1Yy00MS43MjI2NTYtMjcuNDEwMTU3LTkwLjI2OTUzMS00MS44OTg0MzgtMTQwLjM5NDUzMS00MS44OTg0MzgtNjguMzc1IDAtMTMyLjY2NDA2MiAyNi42Mjg5MDYtMTgxLjAxNTYyNSA3NC45ODQzNzUtNDguMzU1NDY5IDQ4LjM1MTU2My03NC45ODQzNzUgMTEyLjY0MDYyNS03NC45ODQzNzUgMTgxLjAxNTYyNSAwIDQwLjg1MTU2MiA5Ljg2MzI4MSA4MS40OTYwOTQgMjguNTE5NTMxIDExNy41MzUxNTYgMTguMDU0Njg4IDM0Ljg2NzE4OCA0NC4zNjMyODEgNjUuNjEzMjgyIDc2LjA4MjAzMSA4OC45MTQwNjNsOC45NDUzMTMtMTIuMTcxODc1Yy0yOS44NTkzNzUtMjEuOTMzNTk0LTU0LjYyMTA5NC01MC44NzEwOTQtNzEuNjEzMjgxLTgzLjY4NzV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00MTguMDQyOTY5IDU3LjgwODU5NC05LjU2NjQwNyAxMS42ODc1YzU2LjE5MTQwNyA0NS45OTYwOTQgODguNDE3OTY5IDExMy45NzY1NjIgODguNDE3OTY5IDE4Ni41MDM5MDYgMCA2NC4zMzk4NDQtMjUuMDU4NTkzIDEyNC44MzU5MzgtNzAuNTU4NTkzIDE3MC4zMzU5MzhzLTEwNS45OTIxODggNzAuNTU4NTkzLTE3MC4zMzU5MzggNzAuNTU4NTkzYy00MS41NDI5NjkgMC04Mi40OTYwOTQtMTAuNzQ2MDkzLTExOC40MzM1OTQtMzEuMDc4MTI1bC03LjQzNzUgMTMuMTQ4NDM4YzM4LjIwMzEyNSAyMS42MDkzNzUgODEuNzI2NTYzIDMzLjAzNTE1NiAxMjUuODcxMDk0IDMzLjAzNTE1NiA2OC4zNzUgMCAxMzIuNjY0MDYyLTI2LjYyODkwNiAxODEuMDE1NjI1LTc0Ljk4NDM3NSA0OC4zNTU0NjktNDguMzUxNTYzIDc0Ljk4NDM3NS0xMTIuNjQwNjI1IDc0Ljk4NDM3NS0xODEuMDE1NjI1IDAtNzcuMDc0MjE5LTM0LjI0NjA5NC0xNDkuMzEyNS05My45NTcwMzEtMTk4LjE5MTQwNnptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTMwNC4wODU5MzggMjMyLjQxNzk2OWMtMjYuMjU3ODEzIDAtNDcuNjI1IDIxLjM2MzI4MS00Ny42MjUgNDcuNjI1czIxLjM2NzE4NyA0Ny42MjUgNDcuNjI1IDQ3LjYyNWMyNi4yNjE3MTggMCA0Ny42MjUtMjEuMzYzMjgxIDQ3LjYyNS00Ny42MjV2LTE3OS40NTcwMzFsLTEzNS4zMjAzMTMgNTQuMTI4OTA2djE3MC43MjY1NjJjLTguNTE5NTMxLTcuOTc2NTYyLTE5Ljk1NzAzMS0xMi44Nzg5MDYtMzIuNTE5NTMxLTEyLjg3ODkwNi0yNi4yNjE3MTkgMC00Ny42MjUgMjEuMzYzMjgxLTQ3LjYyNSA0Ny42MjVzMjEuMzYzMjgxIDQ3LjYyNSA0Ny42MjUgNDcuNjI1YzI2LjI2MTcxOCAwIDQ3LjYyNS0yMS4zNjMyODEgNDcuNjI1LTQ3LjYyNXYtMTk1LjI0NjA5NGwxMDUuMTA5Mzc1LTQyLjA0Njg3NXYxMjIuNDAyMzQ0Yy04LjUxOTUzMS03Ljk4MDQ2OS0xOS45NTMxMjUtMTIuODc4OTA2LTMyLjUxOTUzMS0xMi44Nzg5MDZ6bS0xMjAuMjE0ODQ0IDE2MC4yODkwNjJjLTE3LjkzMzU5NCAwLTMyLjUxOTUzMi0xNC41ODk4NDMtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkzMzU5NCAxNC41ODU5MzgtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTI5Njg3IDAgMzIuNTE5NTMxIDE0LjU4NTkzNyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTI5Njg4LTE0LjU4OTg0NCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMTIwLjIxNDg0NC04MC4xNDQ1MzFjLTE3LjkyOTY4OCAwLTMyLjUxOTUzMi0xNC41ODU5MzgtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkyOTY4OCAxNC41ODk4NDQtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTMzNTkzIDAgMzIuNTE5NTMxIDE0LjU4OTg0MyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTMzNTkzLTE0LjU4NTkzOCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTM5LjYwOTM3NSAyNzIuNDkyMTg4aDE2LjAzMTI1djE1LjEwMTU2MmgtMTYuMDMxMjV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00NTYuMzU5Mzc1IDIyNC40MDYyNWgxNi4wMzEyNXYxNS4xMDE1NjJoLTE2LjAzMTI1em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNDI0LjMwNDY4OCAyMjQuNDA2MjVoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNzEuNjY3OTY5IDI3Mi40OTIxODhoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48L3N2Zz4K";
        }
      }

      if (song.Name.Trim() == "" || song.Artist.Trim() == "") return null;
      return song;
    }
    public async Task<string> AddFavoriteSong(FavoriteSong song)
    {
      _dbFavoriteSong.Add(song);
      return song.Id.ToString();
    }
    public async Task<bool> AddSongToPLaylist(PlaylistSong song)
    {
      var flag = _db.PlaylistSongs.Any(x => x.PlaylistId == song.PlaylistId && x.SongId == song.SongId);
      if (flag) return true;
      _dbPlaylistSongs.Add(song);
      await UpdatePlaylist(song);
      return song.Id > 0;

    }
    public async Task<bool> UpdatePlaylist(PlaylistSong song)
    {
      var songCover = _dbSong.Get(x => x.Id == song.SongId).AlbumCover;
      var playlist = _dbPlaylist.Get(x => x.Id == song.PlaylistId);
      if (playlist == null || songCover == null) return false;
      playlist.Cover = songCover;
      _dbPlaylist.Update(playlist);
      return true;
    }
    public async Task<List<Song>> GetAllSongs()
    {
      return _dbSong.GetAll().ToList();
    }
    public async Task<List<Song>> GetUserFavoriteSongs(int userId)
    {
      var favorites = _dbFavoriteSong.GetMany(x => x.UserId == userId).ToList();
      var songs = new List<Song>();
      if (favorites.Capacity <= 0) return songs;
      foreach (var fav in favorites)
      {
        songs.Add(await GetSongById(fav.SongId));
      }
      return songs;
    }
    public async Task<List<Song>> GetSearchedSongs(string[] keyWords)
    {
      var songsStartedWithTypedText = new List<Song>();
      var songsContainedTypedText = new List<Song>();
      var songsEndedWithTypedText = new List<Song>();
      foreach (var typedtext in keyWords)
      {
        songsStartedWithTypedText = songsStartedWithTypedText.Union(_dbSong.GetMany(x => x.Name.StartsWith(typedtext)).ToList()).ToList();
        songsContainedTypedText = songsContainedTypedText.Union(_dbSong.GetMany(x => x.Name.Contains(typedtext)).ToList()).ToList();
        songsEndedWithTypedText = songsEndedWithTypedText.Union(_dbSong.GetMany(x => x.Name.EndsWith(typedtext)).ToList()).ToList();
      }
      var songsToReturn = songsStartedWithTypedText.Union(songsContainedTypedText).Union(songsEndedWithTypedText)
        .ToList();
      return songsToReturn;
    }
    public async Task<string[]> GetSearchedArtists(string[] keyWords)
    {
      var artistsStartedWithTypedText = new string[] { };
      var artistsContainedTypedText = new string[] { };
      var artistsEndedWithTypedText = new string[] { };
      foreach (var typedtext in keyWords)
      {
        artistsStartedWithTypedText = artistsStartedWithTypedText.Union(_dbSong.GetMany(x => x.Artist.StartsWith(typedtext)).Select(x => x.Artist).ToArray()).ToArray();
        artistsContainedTypedText = artistsContainedTypedText.Union(_dbSong.GetMany(x => x.Artist.Contains(typedtext)).Select(x => x.Artist).ToArray()).ToArray();
        artistsEndedWithTypedText = artistsEndedWithTypedText.Union(_dbSong.GetMany(x => x.Artist.EndsWith(typedtext)).Select(x => x.Artist).ToArray()).ToArray();
      }

      var artistsToReturn = artistsEndedWithTypedText.Union(artistsContainedTypedText).Union(artistsEndedWithTypedText)
        .ToArray();
      return artistsToReturn;
    }
    public async Task<List<Playlist>> GetUserPlaylists(int userId)
    {
      var playlists = _dbPlaylist.GetMany(x => x.UserId == userId).ToList();
      return playlists;
    }
    public async Task<List<Song>> GetPlaylistSongs(int playlistId)
    {
      var playlistSongs = _dbPlaylistSongs.GetMany(x => x.PlaylistId == playlistId).ToList();
      var songs = new List<Song>();
      foreach (var item in playlistSongs)
      {
        songs.Add(_dbSong.Get(x => x.Id == item.SongId));
      }
      return songs;
    }
    public async Task<List<Song>> GetUploadedSongs(int userId)
    {
      var uploadedSongs = _dbUploadedSong.GetMany(x => x.UserId == userId).ToList();
      var songs = new List<Song>();
      foreach (var item in uploadedSongs)
      {
        songs.Add(_dbSong.Get(x => x.Id == item.SongId));
      }
      return songs;
    }

    public async Task<bool> IncreaseActivity(int songId)
    {
      var song = _dbSong.Get(x => x.Id == songId);
      song.Activity += 0.0001;
      _dbSong.Update(song);
      return true;
    }

    public async Task<int> CreatePlaylist(Playlist playlist)
    {
      _dbPlaylist.Add(playlist);
      return playlist.Id;
    }

    public async Task<List<Song>> GetSongsOfArtist(string artist)
    {
      return _dbSong.GetMany(x => x.Artist == artist).ToList();
    }

    public async Task<bool> CheckFavoriteSongForExistense(int songId, int userId)
    {
      return _db.FavoriteSongs.Any(x => x.SongId == songId && x.UserId == userId);
    }
    public async Task<bool> DeleteSongFromInstance(int songId, int userId, int type, int playlistId)
    {
      switch (type)
      {
        case 0:
          {
            _dbUploadedSong.Delete(x => x.SongId == songId && x.UserId == userId); 
            return true;
          }
        case 1:
          {
            var playlist = _dbPlaylist.Get(x => x.UserId == userId && x.Id == playlistId);
            _dbPlaylistSongs.Delete(x => x.SongId == songId && x.PlaylistId == playlist.Id);
            return true;
          }
        case 2:
          {
            _dbFavoriteSong.Delete(x => x.SongId == songId && x.UserId == userId);
            return true;
          }
      }

      return false;
    }
    public async Task<Song> GetSongById(int id)
    {
      return _dbSong.Get(x => x.Id == id);
    }
  }
}
