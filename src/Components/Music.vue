<template>
<div class="tabs">
  <div v-if="isPlaylistOpened && !isFavoriteTracksPageOpened" @click="isPlaylistOpened = false, playListSongsLoaded = false" class="return-block">
           <i class="fas fa-arrow-left"></i>
           <div>Back to playlists</div>
         </div>
  <input type="radio" id="tab1" name="tab-control" checked>
  <input type="radio" id="tab2" name="tab-control">
  <ul>
    <li @click="isFavoriteTracksPageOpened = false" v-bind:title="{'My playlists' : !isPlaylistOpened, currentPlaylistName: isPlaylistOpened}"><label for="tab1" role="button"><svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 448.138 448.138" style="enable-background:new 0 0 448.138 448.138;" xml:space="preserve"><path d="M436.768,151.845c-13.152-26.976-35.744-42.08-57.6-56.704C362.88,84.229,347.52,73.925,336.64,59.173l-2.016-2.72c-6.4-8.608-13.696-18.368-14.816-26.56c-1.12-8.288-7.648-14.048-16.928-13.792C294.496,16.677,288,23.653,288,32.069v285.12c-13.408-8.128-29.92-13.12-48-13.12c-44.096,0-80,28.704-80,64s35.904,64,80,64c44.128,0,80-28.704,80-64V181.573c24.032,9.184,63.36,32.576,74.176,87.2c-2.016,2.976-3.936,6.208-6.176,8.736c-5.856,6.624-5.184,16.736,1.44,22.56c6.592,5.888,16.704,5.184,22.56-1.44c20.032-22.752,33.824-58.784,35.968-94.016C449.024,187.237,445.152,168.997,436.768,151.845zs"/><path d="M16,48.069h192c8.832,0,16-7.168,16-16s-7.168-16-16-16H16c-8.832,0-16,7.168-16,16S7.168,48.069,16,48.069z"/><path d="M16,144.069h192c8.832,0,16-7.168,16-16s-7.168-16-16-16H16c-8.832,0-16,7.168-16,16S7.168,144.069,16,144.069z"/><path d="M112,208.069H16c-8.832,0-16,7.168-16,16s7.168,16,16,16h96c8.832,0,16-7.168,16-16S120.832,208.069,112,208.069z"/><path d="M112,304.069H16c-8.832,0-16,7.168-16,16s7.168,16,16,16h96c8.832,0,16-7.168,16-16S120.832,304.069,112,304.069z"/><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g>
</svg><br><span>{{ isPlaylistOpened ? currentPlaylistName : 'My Playlists' }}</span></label></li>
    <li @click="isFavoriteTracksPageOpened = true" title="Favorite Tracks"><label for="tab2" role="button"><svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 43.995 43.995" xmlns:xlink="http://www.w3.org/1999/xlink" enable-background="new 0 0 43.995 43.995">
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
      <h2> {{ isPlaylistOpened ? currentPlaylistName : 'My Playlists' }} </h2>
      <div v-bind:class="{hidden: isPlaylistOpened}" class="playlists-block">          
           <button class="button-form button-google"><i class="fas fa-cloud-upload-alt"></i> UPLOAD SONGS
             <input id="songUploader" class="file-uploader" type="file" accept="audio/*" multiple @change="loadFiles($event)"/>
             </button>
      <div @click="createDialogVisible = true" class="playlist-block create-playlist">
            <div class="playlist-poster"><img src="../img/plus.svg"/></div>
            <div class="playlist-content">
              <p class="playlist-title">Create playlist</p>
              </div>
          </div>
          <div @click="openPlaylist(playlist.Id, playlist.Name)" :key="playlist.Id" v-for="playlist in $main.playlists" class="playlist-block">
            <div class="playlist-poster"><img v-bind:src="playlist.Cover"/></div>
            <div class="playlist-content">
              <p class="playlist-title">{{ playlist.Name }}</p>
              </div>
          </div>  
        </div>
        <div v-bind:class="{hidden: !isPlaylistOpened}" class="playlists-block">        
          <div v-if="playlistSongs.length == 0">This playlist is empty</div>
            <div v-if="playlistSongs.length > 0" class="songs-block">
            <div class="songs-body">
              <div v-if="!isMobile" class="songs-header">
                <div class="songs-header-number">#</div>
                <div class="songs-header-name">Name</div>
                <div class="songs-header-artist">Artist</div>
              </div>
              <div @click="playSong(item['Id'])" v-bind:class="{'current-song': $main.currentIndex===item['Id']}" class="songs-item" :key="index" v-for="(item, index) in playlistSongs">
                <div v-if="isMobile" class="song-cover"><img v-bind:src="item.AlbumCover" class="song-cover-image"/></div>
                <div v-if="!isMobile" class="song-number">
                  <div class="song-number-hidden">{{ index + 1}}</div>
                  <button class="player-button-icon"><i class="fas" v-bind:class="{'fa-pause': $main.currentIndex===item['Id'] && !$main.isPaused, 'fa-play' : $main.isPaused || ($main.currentIndex!==item['Id'] && !$main.isPaused)}"></i></button>
                </div>
                <div class="song-name">{{ item.Name }}</div>
                <div class="song-artist">{{ item.Artist }}</div>
                <div class="mobile-song-title" v-if="isMobile">
                  <div class="mobile-song-name">{{ item.Name }}</div>
                  <div class="mobile-song-artist">{{ item.Artist }}</div>
                </div>
              </div>
            </div>
          </div>
          </div>
      </section>
        <section>
          <h2>Favorite Tracks</h2>
          <div v-if="$main.favoriteSongs.length == 0">You don't have any favorite songs</div>
          <div v-if="$main.favoriteSongs.length > 0" class="songs-block">
            <div class="songs-body">
              <div v-if="!isMobile" class="songs-header">
                <div class="songs-header-number">#</div>
                <div class="songs-header-name">Name</div>
                <div class="songs-header-artist">Artist</div>
              </div>
              <div @click="playSong(item['Id'])" v-bind:class="{'current-song': $main.currentIndex===item['Id']}" class="songs-item" :key="index" v-for="(item, index) in $main.favoriteSongs">
                <div v-if="isMobile" class="song-cover"><img v-bind:src="item.AlbumCover" class="song-cover-image"/></div>
                <div v-if="!isMobile" class="song-number">
                  <div class="song-number-hidden">{{ index + 1}}</div>
                  <button class="player-button-icon"><i class="fas" v-bind:class="{'fa-pause': $main.currentIndex===item['Id'] && !$main.isPaused, 'fa-play' : $main.isPaused || ($main.currentIndex!==item['Id'] && !$main.isPaused)}"></i></button>
                </div>
                <div class="song-name">{{ item['Name'] }}</div>
                <div class="song-artist">{{ item['Artist'] }}</div>
                <div class="mobile-song-title" v-if="isMobile">
                  <div class="mobile-song-name">{{ item.Name }}</div>
                  <div class="mobile-song-artist">{{ item.Artist }}</div>
                </div>
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
      playlistSongs: [],
      isMobile: false,
      favoriteSongs: [],
      isPlaylistOpened: false,
      favoriteSongsLoaded: false,
      playListSongsLoaded: false,
      createDialogVisible: false,
      playlistName: '',
      isFavoriteTracksPageOpened: false,
      currentPlaylist: 0,
      currentPlaylistName: ''
    }
  },
  mounted () {
    this.$root.$on('addSongToPlaylist', this.addSongToPlaylist)
    window.addEventListener('resize', this.checkScreenWisdth)
  },
  beforeMount () {
    // this.loadPlaylists()
    // this.loadFavoriteSongs()
    this.checkScreenWisdth()
  },
  beforeDestroy () {
    this.$root.$off('addSongToPlaylist', this.addSongToPlaylist)
  },
  methods: {
    checkScreenWisdth () {
      if (window.innerWidth < 700) {
        this.isMobile = true
        this.deactSidebar()
      } else {
        this.actSidebar()
        this.isMobile = false
      }
    },
    actSidebar () {
      this.$store.dispatch('activateSidebar')
    },
    deactSidebar () {
      this.$store.dispatch('deactivateSidebar')
    },
    addSongToPlaylist(song, playlistId) {
      if (this.currentPlaylist == playlistId) {
        this.playlistSongs.push(song)
      }
      this.$main.playlists[this.$main.playlists.map(function (e) { return e.Id }).indexOf(playlistId)].Cover = song.AlbumCover
    },
    userActionsHandler (flag, successfulMessage, errorMessage) {
      if (flag) {
        swal('Yes!', successfulMessage, 'success')
      } else {
        swal('Oops', errorMessage, 'error')
      }
    },
    // loadPlaylists () {
    //   this.$root.$emit('actLoadingRoot')
    //   let that = this
    //   axios({
    //     method: 'GET',
    //     url: 'https://localhost:44304/api/Songs/GetPlaylists',
    //     headers: {
    //       'Content-Type': 'application/json; charset=UTF-8',
    //       Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
    //     }
    //   }).then(function (e) {
    //     that.$root.$emit('deactLoadingRoot')
    //     that.playlists = e.data
    //   }).catch(function (e) {
    //     that.$root.$emit('deactLoadingRoot')
    //     swal('Oops', 'Some error happened!', 'error')
    //   })
    // },
    createPlaylist () {
      let that = this
      let obj = {Name: this.playlistName}
      if (this.playlistName !== '') {
        let flag = this.$main.playlists.find(el => el.Name === this.playlistName) !== undefined
        if (flag) {
          this.$root.$emit('notificate', 'error', 'Such playlist already exists', 3000)
          return 0
        }
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
          that.$root.$emit('notificate', 'success', 'Playlist successfully created', 3000)
          if (e.data > 0) {
            that.$main.playlists.push({Id: e.data, Name: obj.Name, Cover: 'data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNTEyIDUxMiIgd2lkdGg9IjUxMnB4Ij48cGF0aCBkPSJtNTUuMTc1NzgxIDI1NmgtMTUuMTAxNTYyYzAtNTcuNjc1NzgxIDIyLjQ1NzAzMS0xMTEuODk4NDM4IDYzLjI0MjE4Ny0xNTIuNjgzNTk0czk1LjAwNzgxMy02My4yNDIxODcgMTUyLjY4MzU5NC02My4yNDIxODd2MTUuMTAxNTYyYy0xMTAuNzM0Mzc1IDAtMjAwLjgyNDIxOSA5MC4wODk4NDQtMjAwLjgyNDIxOSAyMDAuODI0MjE5em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtMjU2IDQ3MS45MjU3ODF2LTE1LjEwMTU2MmMxMTAuNzM0Mzc1IDAgMjAwLjgyNDIxOS05MC4wODk4NDQgMjAwLjgyNDIxOS0yMDAuODI0MjE5aDE1LjEwNTQ2OWMwIDU3LjY3NTc4MS0yMi40NjA5MzggMTExLjg5ODQzOC02My4yNDYwOTQgMTUyLjY4MzU5NHMtOTUuMDA3ODEzIDYzLjI0MjE4Ny0xNTIuNjgzNTk0IDYzLjI0MjE4N3ptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTQxLjkzMzU5NCAzNjYuNTg5ODQ0Yy0xNy44MDA3ODItMzQuMzg2NzE5LTI2LjgyODEyNS03MS41OTM3NS0yNi44MjgxMjUtMTEwLjU4OTg0NCAwLTY0LjM0Mzc1IDI1LjA1ODU5My0xMjQuODM1OTM4IDcwLjU1ODU5My0xNzAuMzM1OTM4czEwNS45OTIxODgtNzAuNTU4NTkzIDE3MC4zMzU5MzgtNzAuNTU4NTkzYzQ3LjE3MTg3NSAwIDkyLjg1MTU2MiAxMy42Mjg5MDYgMTMyLjA5NzY1NiAzOS40MTc5NjlsOC4yOTY4NzUtMTIuNjI1Yy00MS43MjI2NTYtMjcuNDEwMTU3LTkwLjI2OTUzMS00MS44OTg0MzgtMTQwLjM5NDUzMS00MS44OTg0MzgtNjguMzc1IDAtMTMyLjY2NDA2MiAyNi42Mjg5MDYtMTgxLjAxNTYyNSA3NC45ODQzNzUtNDguMzU1NDY5IDQ4LjM1MTU2My03NC45ODQzNzUgMTEyLjY0MDYyNS03NC45ODQzNzUgMTgxLjAxNTYyNSAwIDQwLjg1MTU2MiA5Ljg2MzI4MSA4MS40OTYwOTQgMjguNTE5NTMxIDExNy41MzUxNTYgMTguMDU0Njg4IDM0Ljg2NzE4OCA0NC4zNjMyODEgNjUuNjEzMjgyIDc2LjA4MjAzMSA4OC45MTQwNjNsOC45NDUzMTMtMTIuMTcxODc1Yy0yOS44NTkzNzUtMjEuOTMzNTk0LTU0LjYyMTA5NC01MC44NzEwOTQtNzEuNjEzMjgxLTgzLjY4NzV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00MTguMDQyOTY5IDU3LjgwODU5NC05LjU2NjQwNyAxMS42ODc1YzU2LjE5MTQwNyA0NS45OTYwOTQgODguNDE3OTY5IDExMy45NzY1NjIgODguNDE3OTY5IDE4Ni41MDM5MDYgMCA2NC4zMzk4NDQtMjUuMDU4NTkzIDEyNC44MzU5MzgtNzAuNTU4NTkzIDE3MC4zMzU5MzhzLTEwNS45OTIxODggNzAuNTU4NTkzLTE3MC4zMzU5MzggNzAuNTU4NTkzYy00MS41NDI5NjkgMC04Mi40OTYwOTQtMTAuNzQ2MDkzLTExOC40MzM1OTQtMzEuMDc4MTI1bC03LjQzNzUgMTMuMTQ4NDM4YzM4LjIwMzEyNSAyMS42MDkzNzUgODEuNzI2NTYzIDMzLjAzNTE1NiAxMjUuODcxMDk0IDMzLjAzNTE1NiA2OC4zNzUgMCAxMzIuNjY0MDYyLTI2LjYyODkwNiAxODEuMDE1NjI1LTc0Ljk4NDM3NSA0OC4zNTU0NjktNDguMzUxNTYzIDc0Ljk4NDM3NS0xMTIuNjQwNjI1IDc0Ljk4NDM3NS0xODEuMDE1NjI1IDAtNzcuMDc0MjE5LTM0LjI0NjA5NC0xNDkuMzEyNS05My45NTcwMzEtMTk4LjE5MTQwNnptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTMwNC4wODU5MzggMjMyLjQxNzk2OWMtMjYuMjU3ODEzIDAtNDcuNjI1IDIxLjM2MzI4MS00Ny42MjUgNDcuNjI1czIxLjM2NzE4NyA0Ny42MjUgNDcuNjI1IDQ3LjYyNWMyNi4yNjE3MTggMCA0Ny42MjUtMjEuMzYzMjgxIDQ3LjYyNS00Ny42MjV2LTE3OS40NTcwMzFsLTEzNS4zMjAzMTMgNTQuMTI4OTA2djE3MC43MjY1NjJjLTguNTE5NTMxLTcuOTc2NTYyLTE5Ljk1NzAzMS0xMi44Nzg5MDYtMzIuNTE5NTMxLTEyLjg3ODkwNi0yNi4yNjE3MTkgMC00Ny42MjUgMjEuMzYzMjgxLTQ3LjYyNSA0Ny42MjVzMjEuMzYzMjgxIDQ3LjYyNSA0Ny42MjUgNDcuNjI1YzI2LjI2MTcxOCAwIDQ3LjYyNS0yMS4zNjMyODEgNDcuNjI1LTQ3LjYyNXYtMTk1LjI0NjA5NGwxMDUuMTA5Mzc1LTQyLjA0Njg3NXYxMjIuNDAyMzQ0Yy04LjUxOTUzMS03Ljk4MDQ2OS0xOS45NTMxMjUtMTIuODc4OTA2LTMyLjUxOTUzMS0xMi44Nzg5MDZ6bS0xMjAuMjE0ODQ0IDE2MC4yODkwNjJjLTE3LjkzMzU5NCAwLTMyLjUxOTUzMi0xNC41ODk4NDMtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkzMzU5NCAxNC41ODU5MzgtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTI5Njg3IDAgMzIuNTE5NTMxIDE0LjU4NTkzNyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTI5Njg4LTE0LjU4OTg0NCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMTIwLjIxNDg0NC04MC4xNDQ1MzFjLTE3LjkyOTY4OCAwLTMyLjUxOTUzMi0xNC41ODU5MzgtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkyOTY4OCAxNC41ODk4NDQtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTMzNTkzIDAgMzIuNTE5NTMxIDE0LjU4OTg0MyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTMzNTkzLTE0LjU4NTkzOCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTM5LjYwOTM3NSAyNzIuNDkyMTg4aDE2LjAzMTI1djE1LjEwMTU2MmgtMTYuMDMxMjV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00NTYuMzU5Mzc1IDIyNC40MDYyNWgxNi4wMzEyNXYxNS4xMDE1NjJoLTE2LjAzMTI1em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNDI0LjMwNDY4OCAyMjQuNDA2MjVoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNzEuNjY3OTY5IDI3Mi40OTIxODhoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48L3N2Zz4K'})
          }
        }).catch(function (e) {
          that.createDialogVisible = false
          console.log(e)
          that.$root.$emit('errorHandler', e.response.status)
        })
      } else {
        this.$root.$emit('notificate', 'error', 'Playlist name is required', 3000)
      }
    },
    openPlaylist (id, name) {
      let that = this
      this.$root.$emit('actLoadingRoot')
      axios({
        method: 'POST',
        url: 'https://localhost:44304/api/Songs/GetPlaylistSongs',
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
        },
        data: id
      }).then(function (e) {
        that.currentPlaylist = id
        that.currentPlaylistName = name
        that.playlistSongs = e.data
        that.isPlaylistOpened = true
        that.$root.$emit('deactLoadingRoot')
      }).catch(function (e) {
        that.$root.$emit('deactLoadingRoot')
        that.$root.$emit('errorHandler', e.response.status)
      })
    },
    loadFiles (event) {
      let files = event.target.files
      let that = this
      for (var i = 0, f = Promise.resolve(); i < files.length; i++) {
        let data = new FormData()
        data.append("NewSong", files[i])
        f = f.then(_=> axios({
          method: 'POST',
          url: 'https://localhost:44304/api/Songs/UploadSong',
          data: data,
          headers: {
            'Content-Type': 'application/json; charset=UTF-8',
            Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
          }
        }).then(function (e) {
          if (!e.data) return 0
          console.log(e.data)       
        }).catch(function (e) {

        })
      )}           
    },
    playSong (index) {
      switch (true) {
        case index === this.$main.currentIndex && !this.$main.isPaused : {
          this.$root.$emit('pauseSongRoot')
          return 0
        }
        case index === this.$main.currentIndex : {
          this.$root.$emit('playSongRoot')
          return 0
        }
        case index !== this.$main.currentIndex : {
          if (this.isFavoriteTracksPageOpened) {
            if (!this.favoriteSongsLoaded) {
              this.$root.$emit('loadSongsRoot', this.$main.favoriteSongs)
              this.favoriteSongsLoaded = true
              this.playListSongsLoaded = false           
            }        
          } else {
            if (!this.playListSongsLoaded) {
              this.$root.$emit('loadSongsRoot', this.playlistSongs)
              this.playListSongsLoaded = true
              this.favoriteSongsLoaded = false
            }
          }
          this.$main.isPaused = true
          this.$root.$emit('playDefinedSongRoot', index)
        }            
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
.file-uploader {
    position: absolute;
    width: 100%;
    height: 100%;
    top: 0;
    left: 0;
    opacity: 0;
    border-radius: 20px;
    cursor: pointer;
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
.return-block {
    position: relative;
    color: #f39d93;
    display: inline-flex;
    align-items: center;
    cursor: pointer;
    padding-left: 10px;
    animation: fade-in 0.5s;
}
.return-block i {
  margin-right: 10px;
  transition: margin 0.3s;
}
.return-block:hover i {
  margin-left: -5px;
  margin-right: 15px;
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
  background:#f7f7f8;
}
.songs-item.current-song {
  background: #f7f7f8;
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
     color: #6b687f;
    font-weight: 600;
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
    padding: 10px 10px;
    margin: 25px 0;
    box-shadow: 0 0 20px #dddddd5e;
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
    margin-bottom: 20px;
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
  border-radius: 50%;
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
  .song-name, .song-artist {
    display: none;
  }
  .playlist-poster img {
    width: 80%;
  }
  .songs-item {
    padding: 25px 0;
    margin: 15px 0;
  }
  .songs-item .mobile-song-name, .songs-item .mobile-song-artist {
    width: 100%;
    padding: 7px 0;
  }
  .mobile-song-artist {
    font-size: 12px;
  }
  .songs-item .song-cover {
    width: 60px;
    position: absolute;
    left: 15px;
  }
  .songs-item .song-cover .song-cover-image {
    width: 100%;
    border-radius: 50%;
  }
  .mobile-song-title {
    display: block;
    margin-left: 100px;
    width: calc(100% - 100px) !important;
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
