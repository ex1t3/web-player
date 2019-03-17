<template>
  <!-- PLAYLISTS -->
  <section>
    <h2>{{ isPlaylistOpened || isUploadsOpened ? currentPlaylistName : 'My Playlists' }}</h2>
    <div v-bind:class="{hidden: isPlaylistOpened || isUploadsOpened}" class="playlists-block">
      <div @click="createDialogVisible = true" class="playlist-block create-playlist">
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
        @click="openPlaylist(playlist.Id, playlist.Name)"
        :key="playlist.Id"
        v-for="playlist in $main.playlists"
        class="playlist-block"
      >
        <div class="playlist-poster">
          <img v-bind:src="playlist.Cover">
        </div>
        <div class="playlist-content">
          <p class="playlist-title">{{ playlist.Name }}</p>
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
        <!-- modal -->
    <div v-bind:class="{active: createDialogVisible}" class="modal-overlay">
      <div v-bind:class="{active: createDialogVisible}" class="modal">
        <a @click="createDialogVisible = false" class="close-modal">
          <svg viewBox="0 0 20 20">
            <path
              fill="#000000"
              d="M15.898,4.045c-0.271-0.272-0.713-0.272-0.986,0l-4.71,4.711L5.493,4.045c-0.272-0.272-0.714-0.272-0.986,0s-0.272,0.714,0,0.986l4.709,4.711l-4.71,4.711c-0.272,0.271-0.272,0.713,0,0.986c0.136,0.136,0.314,0.203,0.492,0.203c0.179,0,0.357-0.067,0.493-0.203l4.711-4.711l4.71,4.711c0.137,0.136,0.314,0.203,0.494,0.203c0.178,0,0.355-0.067,0.492-0.203c0.273-0.273,0.273-0.715,0-0.986l-4.711-4.711l4.711-4.711C16.172,4.759,16.172,4.317,15.898,4.045z"
            ></path>
          </svg>
        </a>
        <!-- close modal -->
        <div class="modal-content">
          <h2 class="centered">Name your new playlist</h2>
          <div class="input-group">
            <input
              v-model="playlistName"
              ref="playlistName"
              name="playlistName"
              placeholder="Playlist"
              class="input-field"
              type="text"
              required
            >
          </div>
          <div class="input-group centered">
            <button
              @click="createPlaylist"
              class="button-form button-gradient"
              type="submit"
            >CREATE</button>
          </div>
        </div>
        <!-- content -->
      </div>
      <!-- modal -->
    </div>
    <!-- overlay -->
  </section>
</template>
<script>
import SongsTemplate from "./SongsTemplate"
import axios from "axios"
import swal from "sweetalert"
export default {
  props: ['isPlaylistOpened', 'isUploadsOpened', 'currentPlaylistName', 'pageType'],
  data() {
    return {
      playlistSongs: [],
      uploadedSongs: [],
      createDialogVisible: false,
      playlistName: "",
      currentPlaylist: 0
    }
  },
  components: {
    SongsTemplate
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
    createPlaylist() {
      let that = this
      let obj = { Name: this.playlistName }
      if (this.playlistName !== "") {
        let flag =
          this.$main.playlists.find(el => el.Name === this.playlistName) !==
          undefined
        if (flag) {
          this.$root.$emit(
            "notificate",
            "error",
            "Such playlist already exists",
            3000
          )
          return 0
        }
        axios({
          method: "POST",
          url: "https://localhost:44343/api/Songs/CreatePlaylist",
          headers: {
            "Content-Type": "application/json; charset=UTF-8",
            Authorization: "Bearer " + sessionStorage.getItem("access_token")
          },
          data: obj
        })
          .then(function(e) {
            that.createDialogVisible = false
            that.$root.$emit(
              "notificate",
              "success",
              "Playlist successfully created",
              3000
            )
            if (e.data > 0) {
              that.$main.playlists.push({
                Id: e.data,
                Name: obj.Name,
                Cover:
                  "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNTEyIDUxMiIgd2lkdGg9IjUxMnB4Ij48cGF0aCBkPSJtNTUuMTc1NzgxIDI1NmgtMTUuMTAxNTYyYzAtNTcuNjc1NzgxIDIyLjQ1NzAzMS0xMTEuODk4NDM4IDYzLjI0MjE4Ny0xNTIuNjgzNTk0czk1LjAwNzgxMy02My4yNDIxODcgMTUyLjY4MzU5NC02My4yNDIxODd2MTUuMTAxNTYyYy0xMTAuNzM0Mzc1IDAtMjAwLjgyNDIxOSA5MC4wODk4NDQtMjAwLjgyNDIxOSAyMDAuODI0MjE5em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtMjU2IDQ3MS45MjU3ODF2LTE1LjEwMTU2MmMxMTAuNzM0Mzc1IDAgMjAwLjgyNDIxOS05MC4wODk4NDQgMjAwLjgyNDIxOS0yMDAuODI0MjE5aDE1LjEwNTQ2OWMwIDU3LjY3NTc4MS0yMi40NjA5MzggMTExLjg5ODQzOC02My4yNDYwOTQgMTUyLjY4MzU5NHMtOTUuMDA3ODEzIDYzLjI0MjE4Ny0xNTIuNjgzNTk0IDYzLjI0MjE4N3ptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTQxLjkzMzU5NCAzNjYuNTg5ODQ0Yy0xNy44MDA3ODItMzQuMzg2NzE5LTI2LjgyODEyNS03MS41OTM3NS0yNi44MjgxMjUtMTEwLjU4OTg0NCAwLTY0LjM0Mzc1IDI1LjA1ODU5My0xMjQuODM1OTM4IDcwLjU1ODU5My0xNzAuMzM1OTM4czEwNS45OTIxODgtNzAuNTU4NTkzIDE3MC4zMzU5MzgtNzAuNTU4NTkzYzQ3LjE3MTg3NSAwIDkyLjg1MTU2MiAxMy42Mjg5MDYgMTMyLjA5NzY1NiAzOS40MTc5NjlsOC4yOTY4NzUtMTIuNjI1Yy00MS43MjI2NTYtMjcuNDEwMTU3LTkwLjI2OTUzMS00MS44OTg0MzgtMTQwLjM5NDUzMS00MS44OTg0MzgtNjguMzc1IDAtMTMyLjY2NDA2MiAyNi42Mjg5MDYtMTgxLjAxNTYyNSA3NC45ODQzNzUtNDguMzU1NDY5IDQ4LjM1MTU2My03NC45ODQzNzUgMTEyLjY0MDYyNS03NC45ODQzNzUgMTgxLjAxNTYyNSAwIDQwLjg1MTU2MiA5Ljg2MzI4MSA4MS40OTYwOTQgMjguNTE5NTMxIDExNy41MzUxNTYgMTguMDU0Njg4IDM0Ljg2NzE4OCA0NC4zNjMyODEgNjUuNjEzMjgyIDc2LjA4MjAzMSA4OC45MTQwNjNsOC45NDUzMTMtMTIuMTcxODc1Yy0yOS44NTkzNzUtMjEuOTMzNTk0LTU0LjYyMTA5NC01MC44NzEwOTQtNzEuNjEzMjgxLTgzLjY4NzV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00MTguMDQyOTY5IDU3LjgwODU5NC05LjU2NjQwNyAxMS42ODc1YzU2LjE5MTQwNyA0NS45OTYwOTQgODguNDE3OTY5IDExMy45NzY1NjIgODguNDE3OTY5IDE4Ni41MDM5MDYgMCA2NC4zMzk4NDQtMjUuMDU4NTkzIDEyNC44MzU5MzgtNzAuNTU4NTkzIDE3MC4zMzU5MzhzLTEwNS45OTIxODggNzAuNTU4NTkzLTE3MC4zMzU5MzggNzAuNTU4NTkzYy00MS41NDI5NjkgMC04Mi40OTYwOTQtMTAuNzQ2MDkzLTExOC40MzM1OTQtMzEuMDc4MTI1bC03LjQzNzUgMTMuMTQ4NDM4YzM4LjIwMzEyNSAyMS42MDkzNzUgODEuNzI2NTYzIDMzLjAzNTE1NiAxMjUuODcxMDk0IDMzLjAzNTE1NiA2OC4zNzUgMCAxMzIuNjY0MDYyLTI2LjYyODkwNiAxODEuMDE1NjI1LTc0Ljk4NDM3NSA0OC4zNTU0NjktNDguMzUxNTYzIDc0Ljk4NDM3NS0xMTIuNjQwNjI1IDc0Ljk4NDM3NS0xODEuMDE1NjI1IDAtNzcuMDc0MjE5LTM0LjI0NjA5NC0xNDkuMzEyNS05My45NTcwMzEtMTk4LjE5MTQwNnptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTMwNC4wODU5MzggMjMyLjQxNzk2OWMtMjYuMjU3ODEzIDAtNDcuNjI1IDIxLjM2MzI4MS00Ny42MjUgNDcuNjI1czIxLjM2NzE4NyA0Ny42MjUgNDcuNjI1IDQ3LjYyNWMyNi4yNjE3MTggMCA0Ny42MjUtMjEuMzYzMjgxIDQ3LjYyNS00Ny42MjV2LTE3OS40NTcwMzFsLTEzNS4zMjAzMTMgNTQuMTI4OTA2djE3MC43MjY1NjJjLTguNTE5NTMxLTcuOTc2NTYyLTE5Ljk1NzAzMS0xMi44Nzg5MDYtMzIuNTE5NTMxLTEyLjg3ODkwNi0yNi4yNjE3MTkgMC00Ny42MjUgMjEuMzYzMjgxLTQ3LjYyNSA0Ny42MjVzMjEuMzYzMjgxIDQ3LjYyNSA0Ny42MjUgNDcuNjI1YzI2LjI2MTcxOCAwIDQ3LjYyNS0yMS4zNjMyODEgNDcuNjI1LTQ3LjYyNXYtMTk1LjI0NjA5NGwxMDUuMTA5Mzc1LTQyLjA0Njg3NXYxMjIuNDAyMzQ0Yy04LjUxOTUzMS03Ljk4MDQ2OS0xOS45NTMxMjUtMTIuODc4OTA2LTMyLjUxOTUzMS0xMi44Nzg5MDZ6bS0xMjAuMjE0ODQ0IDE2MC4yODkwNjJjLTE3LjkzMzU5NCAwLTMyLjUxOTUzMi0xNC41ODk4NDMtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkzMzU5NCAxNC41ODU5MzgtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTI5Njg3IDAgMzIuNTE5NTMxIDE0LjU4NTkzNyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTI5Njg4LTE0LjU4OTg0NCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMTIwLjIxNDg0NC04MC4xNDQ1MzFjLTE3LjkyOTY4OCAwLTMyLjUxOTUzMi0xNC41ODU5MzgtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkyOTY4OCAxNC41ODk4NDQtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTMzNTkzIDAgMzIuNTE5NTMxIDE0LjU4OTg0MyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTMzNTkzLTE0LjU4NTkzOCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTM5LjYwOTM3NSAyNzIuNDkyMTg4aDE2LjAzMTI1djE1LjEwMTU2MmgtMTYuMDMxMjV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00NTYuMzU5Mzc1IDIyNC40MDYyNWgxNi4wMzEyNXYxNS4xMDE1NjJoLTE2LjAzMTI1em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNDI0LjMwNDY4OCAyMjQuNDA2MjVoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNzEuNjY3OTY5IDI3Mi40OTIxODhoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48L3N2Zz4K"
              })
            }
          })
          .catch(function(e) {
            that.createDialogVisible = false
            that.$root.$emit("errorHandler", e.response.status)
          })
      } else {
        this.$root.$emit(
          "notificate",
          "error",
          "Playlist name is required",
          3000
        )
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
          that.$root.$emit("errorHandler", e.response.status)
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
          that.$root.$emit("errorHandler", e.response.status)
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
  .playlist-block.uploads-playlist .playlist-poster,
  .playlist-block.create-playlist .playlist-poster {
    margin-top: 25px;
  }
  .playlist-content {
    padding: 10px 0;
  }
}
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
    transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1),
      visibility 0.6s cubic-bezier(0.55, 0, 0.1, 1);
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
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1),
    -webkit-transform 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1),
    transform 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1),
    transform 0.6s cubic-bezier(0.55, 0, 0.1, 1),
    -webkit-transform 0.6s cubic-bezier(0.55, 0, 0.1, 1);
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
</style>
