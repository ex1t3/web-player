<template>
  <!-- modal -->
  <div v-bind:class="{active: isModalVisible}" class="modal-overlay">
    <div v-bind:class="{active: isModalVisible}" class="modal">
      <a @click="$store.dispatch('updateModalVisibility', false)" class="close-modal">
        <svg viewBox="0 0 20 20">
          <path
            fill="#000000"
            d="M15.898,4.045c-0.271-0.272-0.713-0.272-0.986,0l-4.71,4.711L5.493,4.045c-0.272-0.272-0.714-0.272-0.986,0s-0.272,0.714,0,0.986l4.709,4.711l-4.71,4.711c-0.272,0.271-0.272,0.713,0,0.986c0.136,0.136,0.314,0.203,0.492,0.203c0.179,0,0.357-0.067,0.493-0.203l4.711-4.711l4.71,4.711c0.137,0.136,0.314,0.203,0.494,0.203c0.178,0,0.355-0.067,0.492-0.203c0.273-0.273,0.273-0.715,0-0.986l-4.711-4.711l4.711-4.711C16.172,4.759,16.172,4.317,15.898,4.045z"
          ></path>
        </svg>
      </a>
      <!-- close modal -->
      <div v-if="currentContentType === 0" class="modal-content">
          <h4 class="centered">Name your new playlist</h4>
          <div class="input-group">
            <input ref="playlistName" placeholder="Playlist"
              class="input-field" type="text" required>
          </div>
          <div class="input-group centered">
            <button @click="createPlaylist" class="button-form button-gradient" type="submit">CREATE</button>
          </div>
      </div>
      <div v-if="currentContentType === 1" class="modal-content">
        <h4 class="centered">Choose new name for {{ playlistName }}</h4>
        <div class="input-group">
          <input v-model="playlistName" name="playlistName" placeholder="Playlist"
            class="input-field" type="text" required>
        </div>
        <div class="input-group centered">
          <button @click="editPlaylist" class="button-form button-gradient" type="button">EDIT</button>
          <button @click="deletePlaylist" class="button-form" type="button"><i class="far fa-trash-alt"></i></button>
        </div>
      </div>
      <!-- content -->
    </div>
    <!-- modal -->
  </div>
  <!-- overlay -->
</template>
<script>
// importing libs
import {
  mapGetters
} from 'vuex'
import store from '../store'
import axios from 'axios'
import swal from 'sweetalert'

// Exporting data for current template
export default {
  data() {
    return {
      playlistName: ''
    }
  },
  computed: mapGetters({
    isModalVisible: 'isModalVisible',
    currentContentType: 'currentContentType',
    selectedPlaylistIndex: 'selectedPlaylistIndex'
  }),
  mounted() {
    if (this.$main.playlists.length > 0) this.playlistName = this.$main.playlists[this.selectedPlaylistIndex].Name
  },
  watch: {
    selectedPlaylistIndex: function () {
      this.playlistName = this.$main.playlists[this.selectedPlaylistIndex].Name
    }
  },
  methods: {
    createPlaylist() {
      let that = this
      let obj = {
        Name: this.$refs.playlistName.value
      }
      if (this.checkPlaylistValidity(obj.Name)) {
        axios({
          method: 'POST',
          url: 'https://localhost:44343/api/Songs/CreatePlaylist',
          headers: {
            'Content-Type': 'application/json; charset=UTF-8',
            Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
          },
          data: obj
        })
        .then(function (e) {
          that.$store.dispatch('updateModalVisibility', false)
          that.$root.$emit(
            'notificate',
            'success',
            'Playlist successfully created',
            3000
          )
          if (e.data > 0) {
            that.$main.playlists.push({
              Id: e.data,
              Name: obj.Name,
              UserId: that.$login.user.Id,
              Cover: 'data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgNTEyIDUxMiIgd2lkdGg9IjUxMnB4Ij48cGF0aCBkPSJtNTUuMTc1NzgxIDI1NmgtMTUuMTAxNTYyYzAtNTcuNjc1NzgxIDIyLjQ1NzAzMS0xMTEuODk4NDM4IDYzLjI0MjE4Ny0xNTIuNjgzNTk0czk1LjAwNzgxMy02My4yNDIxODcgMTUyLjY4MzU5NC02My4yNDIxODd2MTUuMTAxNTYyYy0xMTAuNzM0Mzc1IDAtMjAwLjgyNDIxOSA5MC4wODk4NDQtMjAwLjgyNDIxOSAyMDAuODI0MjE5em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtMjU2IDQ3MS45MjU3ODF2LTE1LjEwMTU2MmMxMTAuNzM0Mzc1IDAgMjAwLjgyNDIxOS05MC4wODk4NDQgMjAwLjgyNDIxOS0yMDAuODI0MjE5aDE1LjEwNTQ2OWMwIDU3LjY3NTc4MS0yMi40NjA5MzggMTExLjg5ODQzOC02My4yNDYwOTQgMTUyLjY4MzU5NHMtOTUuMDA3ODEzIDYzLjI0MjE4Ny0xNTIuNjgzNTk0IDYzLjI0MjE4N3ptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTQxLjkzMzU5NCAzNjYuNTg5ODQ0Yy0xNy44MDA3ODItMzQuMzg2NzE5LTI2LjgyODEyNS03MS41OTM3NS0yNi44MjgxMjUtMTEwLjU4OTg0NCAwLTY0LjM0Mzc1IDI1LjA1ODU5My0xMjQuODM1OTM4IDcwLjU1ODU5My0xNzAuMzM1OTM4czEwNS45OTIxODgtNzAuNTU4NTkzIDE3MC4zMzU5MzgtNzAuNTU4NTkzYzQ3LjE3MTg3NSAwIDkyLjg1MTU2MiAxMy42Mjg5MDYgMTMyLjA5NzY1NiAzOS40MTc5NjlsOC4yOTY4NzUtMTIuNjI1Yy00MS43MjI2NTYtMjcuNDEwMTU3LTkwLjI2OTUzMS00MS44OTg0MzgtMTQwLjM5NDUzMS00MS44OTg0MzgtNjguMzc1IDAtMTMyLjY2NDA2MiAyNi42Mjg5MDYtMTgxLjAxNTYyNSA3NC45ODQzNzUtNDguMzU1NDY5IDQ4LjM1MTU2My03NC45ODQzNzUgMTEyLjY0MDYyNS03NC45ODQzNzUgMTgxLjAxNTYyNSAwIDQwLjg1MTU2MiA5Ljg2MzI4MSA4MS40OTYwOTQgMjguNTE5NTMxIDExNy41MzUxNTYgMTguMDU0Njg4IDM0Ljg2NzE4OCA0NC4zNjMyODEgNjUuNjEzMjgyIDc2LjA4MjAzMSA4OC45MTQwNjNsOC45NDUzMTMtMTIuMTcxODc1Yy0yOS44NTkzNzUtMjEuOTMzNTk0LTU0LjYyMTA5NC01MC44NzEwOTQtNzEuNjEzMjgxLTgzLjY4NzV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00MTguMDQyOTY5IDU3LjgwODU5NC05LjU2NjQwNyAxMS42ODc1YzU2LjE5MTQwNyA0NS45OTYwOTQgODguNDE3OTY5IDExMy45NzY1NjIgODguNDE3OTY5IDE4Ni41MDM5MDYgMCA2NC4zMzk4NDQtMjUuMDU4NTkzIDEyNC44MzU5MzgtNzAuNTU4NTkzIDE3MC4zMzU5MzhzLTEwNS45OTIxODggNzAuNTU4NTkzLTE3MC4zMzU5MzggNzAuNTU4NTkzYy00MS41NDI5NjkgMC04Mi40OTYwOTQtMTAuNzQ2MDkzLTExOC40MzM1OTQtMzEuMDc4MTI1bC03LjQzNzUgMTMuMTQ4NDM4YzM4LjIwMzEyNSAyMS42MDkzNzUgODEuNzI2NTYzIDMzLjAzNTE1NiAxMjUuODcxMDk0IDMzLjAzNTE1NiA2OC4zNzUgMCAxMzIuNjY0MDYyLTI2LjYyODkwNiAxODEuMDE1NjI1LTc0Ljk4NDM3NSA0OC4zNTU0NjktNDguMzUxNTYzIDc0Ljk4NDM3NS0xMTIuNjQwNjI1IDc0Ljk4NDM3NS0xODEuMDE1NjI1IDAtNzcuMDc0MjE5LTM0LjI0NjA5NC0xNDkuMzEyNS05My45NTcwMzEtMTk4LjE5MTQwNnptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTMwNC4wODU5MzggMjMyLjQxNzk2OWMtMjYuMjU3ODEzIDAtNDcuNjI1IDIxLjM2MzI4MS00Ny42MjUgNDcuNjI1czIxLjM2NzE4NyA0Ny42MjUgNDcuNjI1IDQ3LjYyNWMyNi4yNjE3MTggMCA0Ny42MjUtMjEuMzYzMjgxIDQ3LjYyNS00Ny42MjV2LTE3OS40NTcwMzFsLTEzNS4zMjAzMTMgNTQuMTI4OTA2djE3MC43MjY1NjJjLTguNTE5NTMxLTcuOTc2NTYyLTE5Ljk1NzAzMS0xMi44Nzg5MDYtMzIuNTE5NTMxLTEyLjg3ODkwNi0yNi4yNjE3MTkgMC00Ny42MjUgMjEuMzYzMjgxLTQ3LjYyNSA0Ny42MjVzMjEuMzYzMjgxIDQ3LjYyNSA0Ny42MjUgNDcuNjI1YzI2LjI2MTcxOCAwIDQ3LjYyNS0yMS4zNjMyODEgNDcuNjI1LTQ3LjYyNXYtMTk1LjI0NjA5NGwxMDUuMTA5Mzc1LTQyLjA0Njg3NXYxMjIuNDAyMzQ0Yy04LjUxOTUzMS03Ljk4MDQ2OS0xOS45NTMxMjUtMTIuODc4OTA2LTMyLjUxOTUzMS0xMi44Nzg5MDZ6bS0xMjAuMjE0ODQ0IDE2MC4yODkwNjJjLTE3LjkzMzU5NCAwLTMyLjUxOTUzMi0xNC41ODk4NDMtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkzMzU5NCAxNC41ODU5MzgtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTI5Njg3IDAgMzIuNTE5NTMxIDE0LjU4NTkzNyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTI5Njg4LTE0LjU4OTg0NCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMTIwLjIxNDg0NC04MC4xNDQ1MzFjLTE3LjkyOTY4OCAwLTMyLjUxOTUzMi0xNC41ODU5MzgtMzIuNTE5NTMyLTMyLjUxOTUzMSAwLTE3LjkyOTY4OCAxNC41ODk4NDQtMzIuNTE5NTMxIDMyLjUxOTUzMi0zMi41MTk1MzEgMTcuOTMzNTkzIDAgMzIuNTE5NTMxIDE0LjU4OTg0MyAzMi41MTk1MzEgMzIuNTE5NTMxIDAgMTcuOTMzNTkzLTE0LjU4NTkzOCAzMi41MTk1MzEtMzIuNTE5NTMxIDMyLjUxOTUzMXptMCAwIiBmaWxsPSIjZDFkMWQxIi8+PHBhdGggZD0ibTM5LjYwOTM3NSAyNzIuNDkyMTg4aDE2LjAzMTI1djE1LjEwMTU2MmgtMTYuMDMxMjV6bTAgMCIgZmlsbD0iI2QxZDFkMSIvPjxwYXRoIGQ9Im00NTYuMzU5Mzc1IDIyNC40MDYyNWgxNi4wMzEyNXYxNS4xMDE1NjJoLTE2LjAzMTI1em0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNDI0LjMwNDY4OCAyMjQuNDA2MjVoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48cGF0aCBkPSJtNzEuNjY3OTY5IDI3Mi40OTIxODhoMTYuMDI3MzQzdjE1LjEwMTU2MmgtMTYuMDI3MzQzem0wIDAiIGZpbGw9IiNkMWQxZDEiLz48L3N2Zz4K'
            })
          }
        })
        .catch(function (e) {
          that.$store.dispatch('updateModalVisibility', false)
          that.$root.$emit('errorHandler', e)
        })
      }   
    },
    deletePlaylist() {
      let that = this
      swal({
          title: 'Are you sure?',
          text: 'Once deleted, you will not be able to listen to songs from this playlist',
          icon: 'warning',
          buttons: true,
          dangerMode: true,
        })
        .then((willDelete) => {
          if (willDelete) {
            let obj = this.$main.playlists[that.selectedPlaylistIndex]
            axios({
              method: 'POST',
              url: 'https://localhost:44343/api/Songs/DeletePlaylist',
              headers: {
                'Content-Type': 'application/json; charset=UTF-8',
                Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
              },
              data: JSON.stringify(obj)
            }).then(function (e) {
              that.$main.playlists.splice(that.selectedPlaylistIndex, 1)
              that.$root.$emit(
                'notificate',
                'success',
                that.playlistName + ' successfully deleted!',
                3000
              )
            }).catch(function (e) {
              that.$root.$emit('errorHandler', e)
            })
            this.$store.dispatch('updateModalVisibility', false)
          }
        })
    },
    editPlaylist () {
      if (this.checkPlaylistValidity(this.playlistName)) {
        let that = this
        let oldName = this.$main.playlists[this.selectedPlaylistIndex].Name
        this.$main.playlists[this.selectedPlaylistIndex].Name = this.playlistName
        let obj = this.$main.playlists[this.selectedPlaylistIndex]
        this.$store.dispatch('updateModalVisibility', false)
        axios({
          method: 'POST',
          url: 'https://localhost:44343/api/Songs/EditPlaylist',
          headers: {
            'Content-Type': 'application/json; charset=UTF-8',
            Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
          },
          data: JSON.stringify(obj)
        }).then(function (e) {
          that.$root.$emit(
            'notificate',
            'success',
            '\'' + oldName + '\'' + ' changed to ' + '\'' + that.playlistName + '\'',
            3000
          )
        }).catch(function (e) {
          that.$root.$emit('errorHandler', e)
        })
        
      }
    },
    checkPlaylistValidity (name) {
      if (name === '') {
        this.$root.$emit(
          'notificate',
          'error',
          'Playlist name is required',
          3000
        )
        return false;
      }
      let flag =
        this.$main.playlists.find(el => el.Name === name) !==
        undefined
      if (flag) {
        this.$root.$emit(
          'notificate',
          'error',
          'Such playlist already exists',
          3000
        )
        return false
      }
      return true
    }
  }
}
</script>
<style scoped>
@media only screen and (min-width: 40em) {
  .modal-overlay {
    display: flex;
    align-items: center;
    justify-content: center;
    position: fixed;
    top: -100px;
    bottom: 0;
    left: 0;
    width: 100%;
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