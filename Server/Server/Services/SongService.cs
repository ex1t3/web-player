using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DevOne.Security.Cryptography.BCrypt;
using Server.DAL;
using Server.Models;
namespace Server.Services
{
  public class SongService
  {
    private readonly WebPlayerDbContext _db;
    private readonly IDbRepository<Song> _dbSong;
    private readonly IDbRepository<Playlist> _dbPlaylist;
    private readonly IDbRepository<PlaylistSongs> _dbPlaylistSongs;
    private readonly IDbRepository<FavoriteSong> _dbFavoriteSong;

    public SongService()
    {
      _db = new WebPlayerDbContext();
      _dbSong = new DbRepository<Song>(new DefaultDbFactory());
      _dbPlaylist = new DbRepository<Playlist>(new DefaultDbFactory());
      _dbPlaylistSongs = new DbRepository<PlaylistSongs>(new DefaultDbFactory());
      _dbFavoriteSong = new DbRepository<FavoriteSong>(new DefaultDbFactory());
    }
    public async Task<bool> AddSong(Song song)
    {
      if (string.IsNullOrEmpty(song.Name) || string.IsNullOrEmpty(song.Artist)) return false;
      if (_dbSong.Get(x => x.Name == song.Name && x.Artist == song.Artist) != null) return false;
      _dbSong.Add(song);
      return true;
    }
    public async Task<string> AddFavoriteSong(FavoriteSong song)
    {
      _dbFavoriteSong.Add(song);
      return song.Id.ToString();
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
        songs.Add(GetSongById(fav.SongId));
      }
      return songs;
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
        songs.Add(_dbSong.Get(x=>x.Id==item.SongId));
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

    public async Task<bool> CheckFavoriteSongForExistense(int songId, int userId)
    {
      return _db.FavoriteSongs.Any(x => x.SongId == songId && x.UserId == userId);
    }
    public async Task<bool> Delete(int songId, int userId)
    {
      _dbFavoriteSong.Delete(x => x.SongId == songId && x.UserId == userId);
      return true;
    }
    public Song GetSongById(int id)
    {
      return _dbSong.Get(x => x.Id == id);
    }
  }
}
