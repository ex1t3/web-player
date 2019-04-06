<template>
  <!-- PLAYLISTS -->
  <section>
    <h2>{{ isPlaylistOpened || isUploadsOpened ? currentPlaylistName : 'My Playlists' }}</h2>
    <div v-bind:class="{hidden: isPlaylistOpened || isUploadsOpened}" class="playlists-block">
      <!-- CREATE PLAYLIST -->
      <div @click="$store.dispatch('updateModalVisibility', true), $store.dispatch('updateCurrentContentType', 0)" class="playlist-block create-playlist">
        <div class="playlist-poster">
          <img src="../img/plus.svg">
        </div>
        <div class="playlist-content">
          <p class="playlist-title">Create playlist</p>
        </div>
      </div>
      <!-- UPLOADS BANNER -->
      <div @click="openUploads" class="playlist-block uploads-playlist">
        <div class="playlist-poster">
          <img src="../img/uploads.svg">
        </div>
        <div class="playlist-content">
          <p class="playlist-title">My Uploads</p>
        </div>
      </div>
      <!-- PLAYLISTS BANNER -->
      <div
        :key="index"
        v-for="(playlist, index) in $main.playlists"
        class="playlist-block"
      >
        <div @click="openPlaylist(playlist.Id, playlist.Name)" class="playlist-poster">
          <img v-bind:src="playlist.Cover">
        </div>
        <div class="playlist-content">
          <p class="playlist-title">{{ playlist.Name }}</p>
          <i @click="$store.dispatch('updateModalVisibility', true), $store.dispatch('updateCurrentContentType', 1), $store.dispatch('updateSelectedPlaylistIndex', index)" :title="'Edit ' + playlist.Name" class="fas fa-ellipsis-v"></i>
        </div>
      </div>
    </div>
    <!-- UPLOADS BLOCK -->
    <div v-bind:class="{hidden: !isUploadsOpened || isPlaylistOpened}">
      <div class="empty-uploads-archive-block" v-if="uploadedSongs.length == 0">
        <img class="empty-box" src="../img/empty-box.svg">
        <h3>You haven't uploaded any song yet,
          <br>but it is never too late to do this
        </h3>
        <button class="button-form button-google">
          <i class="fas fa-cloud-upload-alt"></i> UPLOAD SONGS
          <input
            id="songUploader"
            class="file-uploader"
            type="file"
            accept="audio/*"
            multiple
            @change="loadFiles($event)"
          >
        </button>
      </div>
      <div v-if="uploadedSongs.length > 0" class="songs-block">
        <div class="uploader">
          <button class="button-form button-google">
            <i class="fas fa-cloud-upload-alt"></i> UPLOAD SONGS
            <input
              id="songUploader"
              class="file-uploader"
              type="file"
              accept="audio/*"
              multiple
              @change="loadFiles($event)"
            >
          </button>
        </div>
        <SongsTemplate v-if="pageType === 0" :songs="uploadedSongs" :type="0"/>
      </div>
    </div>
    <!-- PLAYLIST BLOCK -->
    <div :class="{hidden: !isPlaylistOpened || isUploadsOpened}">
      <div v-if="playlistSongs.length == 0">This playlist is empty</div>
      <div v-if="playlistSongs.length > 0" class="songs-block">
        <SongsTemplate v-if="pageType === 1" :playlistId="currentPlaylist" :songs="playlistSongs" :type="1"/>
      </div>
    </div>
  <Modal />
  </section>
</template>
<script>
// importing components
import SongsTemplate from "./SongsTemplate"
import Modal from "./Modal"

// importing libs
import store from '../store'
import axios from "axios"
import swal from "sweetalert"

// Exporting data for current template
export default {
  props: ['isPlaylistOpened', 'isUploadsOpened', 'currentPlaylistName', 'pageType'],
  data() {
    return {
      playlistSongs: [],
      uploadedSongs: [],
      createDialogVisible: false,
      editDialogVisible: false,
      currentPlaylist: 0
    }
  },
  components: {
    SongsTemplate,
    Modal
  },
  beforeMount() {
    this.$root.$on("addSongToPlaylist", this.addSongToPlaylist)
  },
  beforeDestroy() {
    this.$root.$off("addSongToPlaylist", this.addSongToPlaylist)
  },
  methods: {
    addSongToPlaylist(song, playlistId) {
      if (this.currentPlaylist == playlistId) {
        this.playlistSongs.push(song)
      }
    },
    userActionsHandler(flag, successfulMessage, errorMessage) {
      if (flag) {
        swal("Yes!", successfulMessage, "success")
      } else {
        swal("Oops", errorMessage, "error")
      }
    },
    openUploads() {
      let that = this
      this.$root.$emit('openInstance', 0, 'My Uploads')
      this.$root.$emit("actLoadingRoot")
      axios({
        method: "GET",
        url: "https://localhost:44343/api/Songs/GetUploadedSongs",
        headers: {
          "Content-Type": "application/json; charset=UTF-8",
          Authorization: "Bearer " + sessionStorage.getItem("access_token")
        }
      })
        .then(function(e) {
          that.uploadedSongs = e.data
          that.isPlaylistOpened = false
          that.isUploadsOpened = true
          that.$root.$emit("deactLoadingRoot")
        })
        .catch(function(e) {
          that.$root.$emit("deactLoadingRoot")
          that.$root.$emit("errorHandler", e)
        })
    },
    openPlaylist(id, name) {
      let that = this
      this.$root.$emit('openInstance', 1, name)
      this.$root.$emit("actLoadingRoot")
      axios({
        method: "POST",
        url: "https://localhost:44343/api/Songs/GetPlaylistSongs",
        headers: {
          "Content-Type": "application/json; charset=UTF-8",
          Authorization: "Bearer " + sessionStorage.getItem("access_token")
        },
        data: id
      })
        .then(function(e) {
          that.currentPlaylist = id
          that.playlistSongs = e.data
          that.isPlaylistOpened = true
          that.isUploadsOpened = false
          that.$root.$emit("deactLoadingRoot")
        })
        .catch(function(e) {
          that.$root.$emit("deactLoadingRoot")
          that.$root.$emit("errorHandler", e)
        })
    },
    loadFiles(event) {
      let files = event.target.files
      let that = this
      for (var i = 0, f = Promise.resolve(); i < files.length; i++) {
        let data = new FormData()
        data.append("NewSong", files[i])
        f = f.then(_ =>
          axios({
            method: "POST",
            url: "https://localhost:44343/api/Songs/UploadSong",
            data: data,
            headers: {
              "Content-Type": "application/json; charset=UTF-8",
              Authorization: "Bearer " + sessionStorage.getItem("access_token")
            }
          })
            .then(function(e) {
              if (!e.data) return 0
              let index = that.uploadedSongs.map(function(obj) { return obj.Id }).indexOf(e.data.Id)
              if (index != -1) { 
                that.uploadedSongs.splice(index, 1)
              }
              that.uploadedSongs.unshift(e.data)
            })
            .catch(function(e) {})
        )
      }
    }
  }
}
</script>
<style scoped>
.playlist-content i {
  position: absolute;
  top: 30%;
  right: 6%;
  cursor: pointer;
}
.uploader {
  text-align: center;
  margin-bottom: 20px;
}
.empty-box {
  width: 80px;
}
.empty-uploads-archive-block {
    padding: 15px;
    color: #a2a2a2;
    text-align: center;
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
.playlist-block {
  width: 200px;
  text-align: center;
  height: 200px;
  display: flex;
  margin: 60px 40px 20px 0px;
  cursor: pointer;
  position: relative;
  transition: 0.3s;
  align-items: center;
  float: left;
  box-shadow: 0px 5px 20px #d2d2d24a;
  animation: fade-in 0.5s;
}
.playlist-block:hover {
  transform: translateY(-5px);
}
.playlist-content {
  width: 100%;
  text-align: left;
  padding: 15px 0;
  z-index: 2;
  position: absolute;
  top: -45px;
  cursor: initial;
  background: #f39d93;
}
.playlist-content .playlist-title {
  margin-block-start: 0em;
  margin-block-end: 0em;
  padding-left: 10px;
  width: 72%;
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
}
.playlist-poster {
  position: absolute;
  width: 100%;
  height: 100%;
  left: 50%;
  transform: translateX(-50%);
}
.playlist-poster img {
width: 80%;
    border-radius: 50%;
    margin-top: 11%;
}
.create-playlist,
.uploads-playlist {
  transform: none !important;
}
.create-playlist .playlist-content,
.uploads-playlist .playlist-content {
  background: #f6f5f5;
  transition: 0.3s;
}
.create-playlist .playlist-poster,
.uploads-playlist .playlist-poster {
  width: 50%;
  height: 50%;
  transition: 0.3s;
}
.create-playlist:hover .playlist-poster,
.uploads-playlist:hover .playlist-poster {
  width: 60%;
  height: 60%;
}
.create-playlist:hover .playlist-content,
.uploads-playlist:hover .playlist-content {
  background: #f39d93;
}
@media (max-width: 700px) {
  .playlists-block {
    overflow-x: scroll;
    overflow-y: hidden;
    height: 220px;
    width: 100%;
    white-space: nowrap;
    font-size: 12px;
  }
  .playlist-block {
    float: none;
    margin-right: 5px;
    width: 120px;
    margin-left: 0;
    height: 110px;
    display: inline-block;
  }
  .playlist-poster {
    margin-top: 2px;
  }
  .playlist-poster img {
    margin-top: 0;
  }
  .playlist-block.uploads-playlist .playlist-poster,
  .playlist-block.create-playlist .playlist-poster {
    margin-top: 25px;
  }
  .playlist-content {
    padding: 10px 0;
  }
}
</style>
