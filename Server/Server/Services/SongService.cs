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
      if (data != null) {
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
      var domains = new string[] {".me", ".org", ".com", ".net", ".fm", ".top"};
      foreach (var domain in domains)
      {
        if (song.Name.Contains(domain))
        {
          song.Name = song.Name.Split('(')[0];
        }
        if (song.Album.Contains(domain))
        {
          song.Album = song.Album.Split('(')[0];
        }
        if (song.Artist.Contains(domain))
        {
         song.Artist = song.Artist.Split('(')[0];
        }
        if (song.Genre.Contains(domain))
        {
         song.Artist = song.Artist.Split('(')[0];
        }
      }

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
      var artistsStartedWithTypedText = new string[] {};
      var artistsContainedTypedText = new string[] {};
      var artistsEndedWithTypedText = new string[] {};
      foreach (var typedtext in keyWords)
      {
        artistsStartedWithTypedText = artistsStartedWithTypedText.Union(_dbSong.GetMany(x => x.Artist.StartsWith(typedtext)).Select(x=>x.Artist).ToArray()).ToArray();
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

    public async Task<bool> CheckFavoriteSongForExistense(int songId, int userId)
    {
      return _db.FavoriteSongs.Any(x => x.SongId == songId && x.UserId == userId);
    }
    public async Task<bool> Delete(int songId, int userId)
    {
      _dbFavoriteSong.Delete(x => x.SongId == songId && x.UserId == userId);
      return true;
    }
    public async Task<Song> GetSongById(int id)
    {
      return _dbSong.Get(x => x.Id == id);
    }
  }
}
