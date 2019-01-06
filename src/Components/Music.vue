<template>
<div class="tabs">
  <input type="radio" id="tab1" name="tab-control" checked>
  <input type="radio" id="tab2" name="tab-control">
  <ul>
    <li title="My Playlists"><label for="tab1" role="button"><svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 448.138 448.138" style="enable-background:new 0 0 448.138 448.138;" xml:space="preserve"><path d="M436.768,151.845c-13.152-26.976-35.744-42.08-57.6-56.704C362.88,84.229,347.52,73.925,336.64,59.173l-2.016-2.72c-6.4-8.608-13.696-18.368-14.816-26.56c-1.12-8.288-7.648-14.048-16.928-13.792C294.496,16.677,288,23.653,288,32.069v285.12c-13.408-8.128-29.92-13.12-48-13.12c-44.096,0-80,28.704-80,64s35.904,64,80,64c44.128,0,80-28.704,80-64V181.573c24.032,9.184,63.36,32.576,74.176,87.2c-2.016,2.976-3.936,6.208-6.176,8.736c-5.856,6.624-5.184,16.736,1.44,22.56c6.592,5.888,16.704,5.184,22.56-1.44c20.032-22.752,33.824-58.784,35.968-94.016C449.024,187.237,445.152,168.997,436.768,151.845zs"/><path d="M16,48.069h192c8.832,0,16-7.168,16-16s-7.168-16-16-16H16c-8.832,0-16,7.168-16,16S7.168,48.069,16,48.069z"/><path d="M16,144.069h192c8.832,0,16-7.168,16-16s-7.168-16-16-16H16c-8.832,0-16,7.168-16,16S7.168,144.069,16,144.069z"/><path d="M112,208.069H16c-8.832,0-16,7.168-16,16s7.168,16,16,16h96c8.832,0,16-7.168,16-16S120.832,208.069,112,208.069z"/><path d="M112,304.069H16c-8.832,0-16,7.168-16,16s7.168,16,16,16h96c8.832,0,16-7.168,16-16S120.832,304.069,112,304.069z"/><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g>
</svg><br><span>My Playlists</span></label></li>
    <li @click="loadFavoriteSongs()" title="Favorite Tracks"><label for="tab2" role="button"><svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 43.995 43.995" xmlns:xlink="http://www.w3.org/1999/xlink" enable-background="new 0 0 43.995 43.995">
  <g>
    <g>
      <path d="m42.07,24.111c-2.566-2.557-6.369-2.913-8.936-0.356l-2.145,2.137-2.144-2.137c-2.566-2.557-6.369-2.2-8.935,0.356-2.566,2.557-2.566,6.701 0,9.258l.357,.356 10.008,9.97c0.395,0.394 1.035,0.394 1.43,0l10.365-10.326c2.567-2.557 2.567-6.701 0-9.258zm-1.429,7.834l-1.072,1.067-8.578,8.547-9.65-9.614c-1.776-1.771-1.776-4.64 0-6.41 1.654-1.647 4.202-2.223 6.076-0.355l3.574,3.561 3.574-3.561c1.964-1.956 4.422-1.292 6.076,0.355 1.776,1.771 1.776,4.64 7.10543e-15,6.41z"/>
    </g>
  </g>
  <g>
    <g>
      <path fill-rule="evenodd" d="m1,2.005h28c0.552,0 1-0.447 1-1s-0.448-1-1-1h-28c-0.552,0-1,0.447-1,1s0.448,1 1,1zm17,18h-17c-0.552,0-1,0.447-1,1s0.448,1 1,1h17c0.552,0 1-0.447 1-1s-0.448-1-1-1zm11-10h-28c-0.552,0-1,0.447-1,1s0.448,1 1,1h28c0.552,0 1-0.447 1-1s-0.448-1-1-1zm-14,20h-14c-0.552-3.55271e-15-1,0.447-1,1s0.448,1 1,1h14c0.552,0 1-0.447 1-1s-0.448-1-1-1z"/>
    </g>
  </g>
</svg><br><span>Favorite Tracks</span></label></li>
  </ul>
  <div class="slider"><div class="indicator"></div></div>
  <div class="content">
    <section>
      <h2>My Playlists</h2>
      <div v-bind:class="{hidden: isPlaylistOpened}" class="playlists-block">
      <div @click="createDialogVisible = true" class="playlist-block create-playlist">
            <div class="playlist-poster"><img src="../img/plus.svg"/></div>
            <div class="playlist-content">
              <p class="playlist-title">Create playlist</p>
              </div>
          </div>
          <div @click="openPlaylist(playlist.Id)" :key="playlist.Id" v-for="playlist in playlists" class="playlist-block">
            <div class="playlist-poster"><img v-bind:src="playlist.Cover"/></div>
            <div class="playlist-content">
              <p class="playlist-title">{{ playlist.Name }}</p>
              </div>
          </div>
          <input type="file" multiple @change="loadFiles($event)"/>
        </div>
        <div v-bind:class="{hidden: !isPlaylistOpened}" class="playlists-block">
            <div class="songs-block">
            <div class="songs-body">
              <div class="songs-header">
                <div class="songs-header-number">#</div>
                <div class="songs-header-name">Name</div>
                <div class="songs-header-artist">Artist</div>
              </div>
              <div @click="playSong(fav['Id'])" v-bind:class="{'current-song': $main.currentIndex===fav['Id']}" class="songs-item" :key="index" v-for="(fav, index) in playlistSongs">
                <div class="song-number">
                  <div class="song-number-hidden">{{ index + 1}}</div>
                  <button class="player-button-icon"><i class="fas" v-bind:class="{'fa-pause': $main.currentIndex===fav['Id'] && !$main.isPaused, 'fa-play' : $main.isPaused || ($main.currentIndex!==fav['Id'] && !$main.isPaused)}"></i></button>
                </div>
                <div class="song-name">{{ fav['Name'] }}</div>
                <div class="song-title">{{ fav['Artist'] }}</div>
              </div>
            </div>
          </div>
          </div>
      </section>
        <section>
          <h2>Favorite Tracks</h2>
          <div class="songs-block">
            <div class="songs-body">
              <div class="songs-header">
                <div class="songs-header-number">#</div>
                <div class="songs-header-name">Name</div>
                <div class="songs-header-artist">Artist</div>
              </div>
              <div @click="playSong(fav['Id'])" v-bind:class="{'current-song': $main.currentIndex===fav['Id']}" class="songs-item" :key="index" v-for="(fav, index) in favoriteSongs">
                <div class="song-number">
                  <div class="song-number-hidden">{{ index + 1}}</div>
                  <button class="player-button-icon"><i class="fas" v-bind:class="{'fa-pause': $main.currentIndex===fav['Id'] && !$main.isPaused, 'fa-play' : $main.isPaused || ($main.currentIndex!==fav['Id'] && !$main.isPaused)}"></i></button>
                </div>
                <div class="song-name">{{ fav['Name'] }}</div>
                <div class="song-title">{{ fav['Artist'] }}</div>
              </div>
            </div>
          </div>
         </section>
  </div>
  <!-- modal -->
<div v-bind:class="{active: createDialogVisible}" class="modal-overlay">
  <div v-bind:class="{active: createDialogVisible}" class="modal">
    <a @click="createDialogVisible = false" class="close-modal">
      <svg viewBox="0 0 20 20">
        <path fill="#000000" d="M15.898,4.045c-0.271-0.272-0.713-0.272-0.986,0l-4.71,4.711L5.493,4.045c-0.272-0.272-0.714-0.272-0.986,0s-0.272,0.714,0,0.986l4.709,4.711l-4.71,4.711c-0.272,0.271-0.272,0.713,0,0.986c0.136,0.136,0.314,0.203,0.492,0.203c0.179,0,0.357-0.067,0.493-0.203l4.711-4.711l4.71,4.711c0.137,0.136,0.314,0.203,0.494,0.203c0.178,0,0.355-0.067,0.492-0.203c0.273-0.273,0.273-0.715,0-0.986l-4.711-4.711l4.711-4.711C16.172,4.759,16.172,4.317,15.898,4.045z"></path>
      </svg>
    </a><!-- close modal -->

    <div class="modal-content">
      <h2 class="centered">Name your new playlist</h2>
      <div class="input-group">
        <input v-model="playlistName" ref="playlistName" name="playlistName" placeholder="Playlist" class="input-field" type="text" required />
      </div>
      <div class="input-group centered">
        <button @click="createPlaylist()" class="button-form button-gradient" type="submit">CREATE</button>
      </div>
    </div><!-- content -->
  </div><!-- modal -->
</div><!-- overlay -->
</div>
</template>
<script>
import universalParse from 'id3-parser/lib/universal'
import axios from 'axios'
import swal from 'sweetalert'
export default {
  data () {
    return {
      favoriteSongs: [],
      playlists: [],
      playlistSongs: [],
      isPlaylistOpened: false,
      favoriteSongsLoaded: false,
      createDialogVisible: false,
      playlistName: ''
    }
  },
  beforeMount () {
    this.loadPlaylists()
    this.loadFavoriteSongs()
  },
  methods: {
    userActionsHandler (flag, successfulMessage, errorMessage) {
      if (flag) {
        swal('Yes!', successfulMessage, 'success')
      } else {
        swal('Oops', errorMessage, 'error')
      }
    },
    loadPlaylists () {
      this.$root.$emit('actLoadingRoot')
      let that = this
      axios({
        method: 'GET',
        url: 'https://localhost:44304/api/Songs/GetPlaylists',
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
        }
      }).then(function (e) {
        that.$root.$emit('deactLoadingRoot')
        that.playlists = e.data
      }).catch(function (e) {
        that.$root.$emit('deactLoadingRoot')
        swal('Oops', 'Some error happened!', 'error')
      })
    },
    loadFavoriteSongs () {
      this.$root.$emit('actLoadingRoot')
      let that = this
      axios({
        method: 'GET',
        url: 'https://localhost:44304/api/Songs/GetFavoriteSongs',
        headers: {
          Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
        }
      }).then(function (e) {
        that.favoriteSongs = e.data
      })
      this.$root.$emit('deactLoadingRoot')
    },
    createPlaylist () {
      let that = this
      let obj = {Name: this.playlistName}
      if (this.playlistName !== '') {
        axios({
          method: 'POST',
          url: 'https://localhost:44304/api/Songs/CreatePlaylist',
          headers: {
            'Content-Type': 'application/json; charset=UTF-8',
            Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
          },
          data: obj
        }).then(function (e) {
          that.createDialogVisible = false
          that.userActionsHandler(e.data > 0, 'Playlist successfully created', 'Playlist can\'t be created')
          if (e.data > 0) {
            that.playlists.push({Id: e.data, Name: obj.Name, Cover: 'data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pg0KPCEtLSBHZW5lcmF0b3I6IEFkb2JlIElsbHVzdHJhdG9yIDE5LjAuMCwgU1ZHIEV4cG9ydCBQbHVnLUluIC4gU1ZHIFZlcnNpb246IDYuMDAgQnVpbGQgMCkgIC0tPg0KPHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHhtbG5zOnhsaW5rPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5L3hsaW5rIiB2ZXJzaW9uPSIxLjEiIGlkPSJDYXBhXzEiIHg9IjBweCIgeT0iMHB4IiB2aWV3Qm94PSIwIDAgNTUgNTUiIHN0eWxlPSJlbmFibGUtYmFja2dyb3VuZDpuZXcgMCAwIDU1IDU1OyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSIgd2lkdGg9IjUxMnB4IiBoZWlnaHQ9IjUxMnB4Ij4NCjxwYXRoIGQ9Ik01Mi42NiwwLjI0OWMtMC4yMTYtMC4xODktMC41MDEtMC4yNzUtMC43ODktMC4yNDFsLTMxLDQuMDExQzIwLjM3Myw0LjA4NCwyMCw0LjUwNywyMCw1LjAxdjYuMDE3djQuMjEydjI1LjM4NCAgQzE4LjE3NCwzOC40MjgsMTUuMjczLDM3LDEyLDM3Yy01LjUxNCwwLTEwLDQuMDM3LTEwLDlzNC40ODYsOSwxMCw5czEwLTQuMDM3LDEwLTljMC0wLjIzMi0wLjAxOS0wLjQ2LTAuMDM5LTAuNjg3ICBDMjEuOTc0LDQ1LjI0OCwyMiw0NS4xODksMjIsNDUuMTIxVjE2LjExOGwyOS0zLjc1M3YxOC4yNTdDNDkuMTc0LDI4LjQyOCw0Ni4yNzMsMjcsNDMsMjdjLTUuNTE0LDAtMTAsNC4wMzctMTAsOXM0LjQ4Niw5LDEwLDkgIGM1LjQ2NCwwLDkuOTEzLTMuOTY2LDkuOTkzLTguODY3YzAtMC4wMTMsMC4wMDctMC4wMjQsMC4wMDctMC4wMzdWMTEuMjI3VjcuMDE2VjFDNTMsMC43MTIsNTIuODc2LDAuNDM4LDUyLjY2LDAuMjQ5eiBNMTIsNTMgIGMtNC40MTEsMC04LTMuMTQxLTgtN3MzLjU4OS03LDgtN3M4LDMuMTQxLDgsN1MxNi40MTEsNTMsMTIsNTN6IE00Myw0M2MtNC40MTEsMC04LTMuMTQxLTgtN3MzLjU4OS03LDgtN3M4LDMuMTQxLDgsNyAgUzQ3LjQxMSw0Myw0Myw0M3ogTTIyLDE0LjEwMXYtMy4wNzRWNS44ODlsMjktMy43NTJ2NC44Nzl2My4zMzJMMjIsMTQuMTAxeiIgZmlsbD0iI2Q2ZDZkNiIvPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPGc+DQo8L2c+DQo8Zz4NCjwvZz4NCjxnPg0KPC9nPg0KPC9zdmc+DQo='})
          }
        }).catch(function (e) {
          that.createDialogVisible = false
          swal('Oops', 'Some error happened!', 'error')
        })
      } else {
        swal('Hmmm', 'Playlist name can\'t be empty', 'warning')
      }
    },
    openPlaylist (id) {
      let that = this
      this.$root.$emit('actLoadingRoot')
      axios({
        method: 'GET',
        url: 'https://localhost:44304/api/Songs/GetPlaylistSongs',
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
        },
        data: id
      }).then(function (e) {
        that.playlistSongs = e.data
        that.isPlaylistOpened = true
        that.$root.$emit('actLoadingRoot')
      }).catch(function (e) {
        swal('Oops', 'Some error happened', 'error')
      })
    },
    loadFiles (event) {
      let files = event.target.files
      let that = this
      let songs = []
      let name = ''
      let artist = ''
      let genre = ''
      let cover = ''
      let src = ''
      let album = ''
      let length = ''
      let year = ''
      for (let i = 0, p = Promise.resolve(); i < files.length; i++) {
        let file = files[i]
        p = p.then(_ => universalParse(require('../songs/' + file.name)).then(tag => {
          var image = tag.image
          if (image) {
            var base64String = ''
            for (var i = 0; i < image.data.length; i++) {
              base64String += String.fromCharCode(image.data[i])
            }
            var base64 = 'data:' + image.mime + ';base64,' + window.btoa(base64String)
            cover = base64
          } else {
            cover = require('../img/no-cover.svg')
          }
          name = tag.title
          artist = tag.artist
          genre = tag.genre
          year = tag.year
          length = tag.length
          album = tag.album
          src = file.name
          songs.push({
            Name: name,
            Artist: artist,
            Genre: genre,
            AlbumCover: cover,
            Source: src,
            Year: year,
            Duration: length,
            Album: album
          })
          if (songs.length === files.length) {
            axios({
              method: 'POST',
              url: 'https://localhost:44304/api/Songs/UploadSongs',
              data: JSON.parse(JSON.stringify(songs)),
              headers: {
                'Content-Type': 'application/json; charset=UTF-8',
                Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
              }
            }).then(function (e) {
              that.userActionsHandler(e.data, 'Songs have been successfully uploaded!', 'Songs haven\'t been uploaded')
            })
          }
        }
        ))
      }
    },
    playSong (index) {
      if (index === this.$main.currentIndex && !this.$main.isPaused) {
        this.$root.$emit('pauseSongRoot')
        return 0
      }
      if (index === this.$main.currentIndex) {
        this.$root.$emit('playSongRoot')
        return 0
      }
      if (index !== this.$main.currentIndex) {
        if (!this.favoriteSongsLoaded) {
          this.$root.$emit('loadSongsRoot', this.favoriteSongs)
          this.favoriteSongsLoaded = true
        }
        this.$root.$emit('playDefinedSongRoot', index)
        return 0
      }
    }
  }
}
</script>
<style>
@media only screen and (min-width: 40em) {
  .modal-overlay {
    display: flex;
    align-items: center;
    justify-content: center;
    position: fixed;
    top: -100px;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 5;
    background-color: rgba(0, 0, 0, 0.6);
    opacity: 0;
    visibility: hidden;
    -webkit-backface-visibility: hidden;
            backface-visibility: hidden;
    transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1), visibility 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  }
  .modal-overlay.active {
    opacity: 1;
    visibility: visible;
  }
}
/**
 * Modal
 */
.modal {
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  margin: 0 auto;
  background-color: #fff;
  width: 600px;
  max-width: 75rem;
  min-height: 20rem;
  padding: 1rem;
  border-radius: 3px;
  opacity: 0;
  overflow-y: auto;
  visibility: hidden;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  -webkit-backface-visibility: hidden;
          backface-visibility: hidden;
  -webkit-transform: scale(1.2);
          transform: scale(1.2);
  transition: all 0.6s cubic-bezier(0.55, 0, 0.1, 1);
}
.modal .close-modal {
  position: absolute;
  cursor: pointer;
  top: 5px;
  right: 15px;
  opacity: 0;
  -webkit-backface-visibility: hidden;
          backface-visibility: hidden;
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1), -webkit-transform 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1), transform 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1), transform 0.6s cubic-bezier(0.55, 0, 0.1, 1), -webkit-transform 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition-delay: 0.3s;
}
.modal .close-modal svg {
  width: 1.75em;
  height: 1.75em;
}
.modal .modal-content {
  opacity: 0;
  -webkit-backface-visibility: hidden;
          backface-visibility: hidden;
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition-delay: 0.3s;
}
.modal.active {
  visibility: visible;
  opacity: 1;
  -webkit-transform: scale(1);
          transform: scale(1);
}
.modal.active .modal-content {
  opacity: 1;
}
.modal.active .close-modal {
  -webkit-transform: translateY(10px);
          transform: translateY(10px);
  opacity: 1;
}
.modal-content h2 {
  padding: 10px 0;
}
.modal-content .input-group {
  padding: 10px 0;
  width: 100%;
}
/**
 * Mobile styling
 */
@media only screen and (max-width: 39.9375em) {
  h1 {
    font-size: 1.5rem;
  }

  .modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    -webkit-overflow-scrolling: touch;
    border-radius: 0;
    -webkit-transform: scale(1.1);
            transform: scale(1.1);
    padding: 0 !important;
    z-index: 999;
  }

  .close-modal {
    right: 20px !important;
    top: 80px !important;
  }
}
.songs-item .player-button-icon,
.songs-item .player-button-icon i {
    background: none;
    padding: 0;
}
.songs-item:hover {
  cursor: pointer;
  background: #efeff17d;
}
.songs-item.current-song {
  background: #efeff17d;
}
.song-number .player-button-icon {
  display: none;
}
.songs-item:hover .song-number .player-button-icon {
  display: block;
}
.songs-item:hover .song-number .song-number-hidden {
  display: none;
}
.songs-item.current-song .song-number .player-button-icon {
  display: block;
}
.songs-item.current-song .song-number .song-number-hidden {
  display: none;
}
.songs-item.current-song div,
.songs-item.current-song button {
  color: #f39d93;
}
.songs-header {
  display: flex;
  margin: 10px;
  align-items: center;
}
.songs-item {
    display: flex;
    align-items: center;
    height: 50px;
    position: relative;
    padding: 5px;
}
.songs-item::after {
    content: '';
    height: 1px;
    background: #f4f4f4;
    top: 0px;
    width: calc(100% - 12px);
    position: absolute;
    left: 6px;
    right: 6px;
}
.songs-block {
  width: 100%;
}
.songs-header,
.songs-item {
  width: 100%;
}
.songs-header div,
.songs-item div{
  width: 20%;
}
.playlist-block {
    width: 200px;
    text-align: center;
    height: 200px;
    display: flex;
    margin-top: 60px;
    margin-left: 40px;
    cursor: pointer;
    position: relative;
    transition: .3s;
    align-items: center;
    float: left;
    box-shadow: 0px 5px 20px #d2d2d24a;
    animation: fade-in 0.5s;
}
.playlist-block:hover {
  transform: translateY(-5px)
}
.playlist-content {
    width: 100%;
    text-align: left;
    padding: 15px 0;
    z-index: 2;
    position: absolute;
    top: -45px;
    background: #f39d93;
}
.playlist-content .playlist-title {
    margin-block-start: 0em;
    margin-block-end: 0em;
    padding-left: 10px;
}
.playlist-poster {
    position: absolute;
    width: 80%;
    height: 80%;
    left: 50%;
    transform: translateX(-50%);
}
.playlist-poster img {
  width: 100%;
}
.create-playlist {
  transform: none !important;
}
.create-playlist .playlist-content {
  background: #f6f5f5;
  transition: .3s;
}
.create-playlist .playlist-poster {
  width: 50%;
  height: 50%;
  transition: .3s;
}
.create-playlist:hover .playlist-poster{
  width: 60%;
  height: 60%;
}
.create-playlist:hover .playlist-content{
  background: #f39d93;
}
.tabs {
    left: 50%;
    /* -webkit-transform: translateX(-50%); */
    /* transform: translateX(-50%); */
    /* position: relative; */
    background: white;
    padding: 20px;
    padding-bottom: 80px;
    width: 90%;
    height: auto;
    min-width: 240px;
    margin-left: auto;
    margin-right: auto;
}
.tabs input[name="tab-control"] {
  display: none;
}
.tabs .content section h2,
.tabs ul li label {
  font-family: 'Niramit', sans-serif;
  font-weight: bold;
  font-size: 18px;
  color: #f39d93;
}
.tabs ul {
  list-style-type: none;
  padding-left: 0;
  display: flex;
  flex-direction: row;
  margin-bottom: 10px;
  justify-content: space-between;
  align-items: flex-end;
  flex-wrap: wrap;
  margin-top: 20px;
}
.tabs ul li {
    box-sizing: border-box;
    flex: 1;
    width: 50%;
    padding: 0 10px;
    text-align: center;
}
.tabs ul li label {
  transition: all 0.3s ease-in-out;
  color: #929daf;
  padding: 5px auto;
  overflow: hidden;
  text-overflow: ellipsis;
  display: block;
  cursor: pointer;
  transition: all 0.2s ease-in-out;
  white-space: nowrap;
  -webkit-touch-callout: none;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}
.tabs ul li label br {
  display: none;
}
.tabs ul li label svg {
  fill: #929daf;
  height: 1.2em;
  vertical-align: middle;
  margin-right: 0.2em;
  transition: all 0.2s ease-in-out;
}
.tabs ul li label:hover, .tabs ul li label:focus, .tabs ul li label:active {
  outline: 0;
  color: #bec5cf;
}
.tabs ul li label:hover svg, .tabs ul li label:focus svg, .tabs ul li label:active svg {
  fill: #bec5cf;
}
.tabs .slider {
  position: relative;
  width: 50%;
  transition: all 0.33s cubic-bezier(0.38, 0.8, 0.32, 1.07);
}
.tabs .slider .indicator {
position: relative;
    width: 80%;
    margin: 0 auto;
    height: 4px;
    background: #f39d93;
    border-radius: 1px;
}
.tabs .content {
  margin-top: 30px;
}
.tabs .content section {
  display: none;
  -webkit-animation-name: content;
          animation-name: content;
  -webkit-animation-direction: normal;
          animation-direction: normal;
  -webkit-animation-duration: 0.3s;
          animation-duration: 0.3s;
  -webkit-animation-timing-function: ease-in-out;
          animation-timing-function: ease-in-out;
  -webkit-animation-iteration-count: 1;
          animation-iteration-count: 1;
  line-height: 1.4;
}
.tabs .content section h2 {
  color: #f39d93;
  display: none;
}
.tabs .content section h2::after {
  content: "";
  position: relative;
  display: block;
  width: 30px;
  height: 3px;
  background: #f39d93;
  margin-top: 5px;
  left: 1px;
}
.tabs input[name="tab-control"]:nth-of-type(1):checked ~ ul > li:nth-child(1) > label {
  cursor: default;
  color: #f39d93;
}
.tabs input[name="tab-control"]:nth-of-type(1):checked ~ ul > li:nth-child(1) > label svg {
  fill: #f39d93;
}
@media (max-width: 700px) {
  .playlist-block {
    float: none;
    margin-left: auto;
    margin-right: auto;
    width: 190px;
    height: 150px;
    margin-top: 70px;
  }
  .playlist-poster img {
    width: 80%;
  }
}
@media (max-width: 600px) {
  .tabs input[name="tab-control"]:nth-of-type(1):checked ~ ul > li:nth-child(1) > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(1):checked ~ .slider {
  -webkit-transform: translateX(0%);
          transform: translateX(0%);
}
.tabs input[name="tab-control"]:nth-of-type(1):checked ~ .content > section:nth-child(1) {
  display: block;
}
.tabs input[name="tab-control"]:nth-of-type(2):checked ~ ul > li:nth-child(2) > label {
  cursor: default;
  color: #f39d93;
}
.tabs input[name="tab-control"]:nth-of-type(2):checked ~ ul > li:nth-child(2) > label svg {
  fill: #f39d93;
}
@media (max-width: 600px) {
  .tabs input[name="tab-control"]:nth-of-type(2):checked ~ ul > li:nth-child(2) > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(2):checked ~ .slider {
  -webkit-transform: translateX(100%);
          transform: translateX(100%);
}
.tabs input[name="tab-control"]:nth-of-type(2):checked ~ .content > section:nth-child(2) {
  display: block;
}
.tabs input[name="tab-control"]:nth-of-type(3):checked ~ ul > li:nth-child(3) > label {
  cursor: default;
  color: #f39d93;
}
.tabs input[name="tab-control"]:nth-of-type(3):checked ~ ul > li:nth-child(3) > label svg {
  fill: #f39d93;
}
@media (max-width: 600px) {
  .tabs input[name="tab-control"]:nth-of-type(3):checked ~ ul > li:nth-child(3) > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(3):checked ~ .slider {
  -webkit-transform: translateX(200%);
          transform: translateX(200%);
}
.tabs input[name="tab-control"]:nth-of-type(3):checked ~ .content > section:nth-child(3) {
  display: block;
}
.tabs input[name="tab-control"]:nth-of-type(4):checked ~ ul > li:nth-child(4) > label {
  cursor: default;
  color: #f39d93;
}
.tabs input[name="tab-control"]:nth-of-type(4):checked ~ ul > li:nth-child(4) > label svg {
  fill: #f39d93;
}
@media (max-width: 600px) {
  .tabs input[name="tab-control"]:nth-of-type(4):checked ~ ul > li:nth-child(4) > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(4):checked ~ .slider {
  -webkit-transform: translateX(300%);
          transform: translateX(300%);
}
.tabs input[name="tab-control"]:nth-of-type(4):checked ~ .content > section:nth-child(4) {
  display: block;
}
@-webkit-keyframes content {
  from {
    opacity: 0;
    -webkit-transform: translateY(5%);
            transform: translateY(5%);
  }
  to {
    opacity: 1;
    -webkit-transform: translateY(0%);
            transform: translateY(0%);
  }
}
@keyframes content {
  from {
    opacity: 0;
    -webkit-transform: translateY(5%);
            transform: translateY(5%);
  }
  to {
    opacity: 1;
    -webkit-transform: translateY(0%);
            transform: translateY(0%);
  }
}
@media (max-width: 1000px) {
  .tabs ul li label {
    white-space: initial;
  }
  .tabs ul li label br {
    display: initial;
  }
  .tabs ul li label svg {
    height: 1.5em;
  }
}
@media (max-width: 600px) {
  .tabs ul li label {
    padding: 5px;
    border-radius: 5px;
  }
  .tabs ul li label span {
    display: none;
  }
  .tabs .slider {
    display: none;
  }
  .tabs .content {
    margin-top: 20px;
  }
  .tabs .content section h2 {
    display: block;
  }
}

</style>
