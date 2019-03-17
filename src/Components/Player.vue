<template>
  <div class="main-player-block">
    <div v-bind:class="{hidden: !isAddMenuActive}" class="mobile-layer"></div>
    <div class="main-player-poster"><img v-bind:src="currentPoster" /></div>
    <div class="main-player-add-to">
      <div v-clickOutside="hideAddMenu" class="main-player-add-menu" v-bind:class="{hidden: !isAddMenuActive}">
        <h4>Add to playlist</h4>
        <div class="main-player-add-menu-playlists">
          <div @click="addSongToPlaylist(item.Id)" class="main-player-add-menu-item" :key="item.Id" v-for="item in $main.playlists">
            <div class="main-player-add-menu-item-cover"><img class="main-player-add-menu-item-cover-image" v-bind:src="item.Cover" /></div>
            <div class="main-player-add-menu-item-title">{{ item.Name }}</div>
          </div>
        </div>
      </div>
      <i data-toggler="true" v-bind:class="{'active-icon': isAddMenuActive}" @click="isAddMenuActive = !isAddMenuActive"
        class="fas fa-plus"></i>
      <i @click="favHandler($event)" class="fa-heart" v-bind:class="{fas: favLookUpper($main.currentIndex) > -1, far: favLookUpper($main.currentIndex) < 0}"></i>
    </div>
    <div class="main-player-body">
      <div class="player-buttons">
        <button class="player-button-icon" @click="playPrev()"><i class="fas fa-backward"></i></button>
        <button class="player-button-icon" @click="playSong()" v-bind:class="{hidden : !$main.isPaused}"><i class="fas fa-play"></i></button>
        <button class="player-button-icon" @click="pauseSong()" v-bind:class="{hidden : $main.isPaused}"><i class="fas fa-pause"></i></button>
        <button class="player-button-icon" @click="playNext()"><i class="fas fa-forward"></i></button></div>
      <div class="player-trackline">
        <div class="title">{{ title }}</div>
        <div class="timeline">
          <span class="current-time">{{ currentTime }}</span>
          <span class="total-time">{{ duration }}</span>
          <div @click="onTimeChange($event)" class="slider">
            <div class="progress" v-bind:style="{ width: progressWidth }">
              <div @click="stopProp($event)" class="pin" v-bind:style="{ 'margin-left': progressWidth }"></div>
            </div>
          </div>
        </div>
      </div>
      <div class="mobile-settings"><i class="fas fa-sliders-h"></i></div>
      <div class="player-settings">
        <i @click="muteSong($event)" v-bind:class="{hidden: isVolumeOff}" class="fas fa-volume-up">
          <div @click="stopProp($event)" class="volume-raise-block">
            <div @click="onVolumeChange($event)" class="slider">
              <div class="progress" v-bind:style="{ width: progressVolumeWidth}">
                <div @click="stopProp($event)" class="pin" v-bind:style="{'margin-left' : progressVolumeWidth}"></div>
              </div>
            </div>
          </div>
        </i>
        <i @click="unMuteSong()" v-bind:class="{hidden: !isVolumeOff, 'player-settings-active': isVolumeOff}" class="fas fa-volume-mute"></i>
        <i @click="isActiveQueue = !isActiveQueue" class="fas fa-th-list"></i>
        <i @click="shuffleSongs()" v-bind:class="{'player-settings-active': isShuffled}" class="fas fa-random"></i>
        <i @click="isReplayed=!isReplayed" v-bind:class="{'player-settings-active': isReplayed}" class="fas fa-redo"></i>
      </div>
    </div>
    <div v-bind:class="{'queue-active': isActiveQueue}" class="music-queue-block">
      <div @click="isActiveQueue = !isActiveQueue" class="queue-closer"><i class="fas fa-times"></i></div>
      <div class="queue-body">
        <div class="queue-list">
          <div @click="playDefinedSong(item)" v-bind:class="{'current-song': $main.currentIndex===item}" class="queue-item"
            :key="item" v-for="(item) in shuffleIndexes">
            <button class="queue-item-play-button player-button-icon">
              <i class="fas" :class="{'fa-pause': $main.currentIndex===item && !$main.isPaused, 'fa-play' : $main.isPaused || ($main.currentIndex!==item && !$main.isPaused)}"></i>
            </button>
            <div class="queue-item-cover-block">
              <img :src="songs[lookUpper(item)].AlbumCover"/>
            </div>
            <div class="queue-item-title-block">
              <div class="queue-item-title queue-item-name">{{ songs[lookUpper(item)].Name }}</div>
              <div class="queue-item-title queue-item-artist">{{ songs[lookUpper(item)].Artist }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import store from '../store'
import Vue from 'vue'
import axios from 'axios'
import swal from 'sweetalert'

var rootData = new Vue({
  data: {
    currentIndex: 0,
    isPaused: true,
    playlists: [],
    isMobile: false,
    favoriteSongs: []
  }
})
rootData.install = function () {
  Object.defineProperty(Vue.prototype, '$main', {
    get () { return rootData }
  })
}
Vue.use(rootData)
export default {
  store,
  data () {
    return {
      songs: [],
      duration: '0:00',
      progressWidth: '0%',
      progressVolumeWidth: '100%',
      currentTime: '0:00',
      title: ' Loading ... ',
      isDragged: false,
      songsLength: 0,
      isVolumeOff: false,
      isShuffled: false,
      currentPoster: require('../img/no-cover.svg'),
      volume: 1,
      isReplayed: false,
      shuffleIndexes: [],
      isAddMenuActive: false,
      isActiveQueue: false,
      audio: new Audio()
    }
  },
  directives: {
    clickOutside: {
      bind: function (el, binding, vnode) {
        el.event = function (event) {
          if (!(el.target === event.target || el.contains(event.target) || event.target.hasAttribute('data-toggler'))) {
            vnode.context[binding.expression](event)
          }
        }
        document.body.addEventListener('click', el.event)
      },
      unbind: function (el) {
        document.body.removeEventListener('click', el.event)
      }
    }
  },
  mounted () {
    this.loadSongs()   
    this.loadPlaylists()   
    this.$root.$on('loadPlaylistsRoot', this.loadPlaylists)
    this.addListeners()
    this.$root.$on('loadSongsRoot', this.loadDefinedSongs)
    this.$root.$on('playDefinedSongRoot', this.playDefinedSong)
    this.$root.$on('pauseSongRoot', this.pauseSong)
    this.$root.$on('playSongRoot', this.playSong)
    this.$root.$on('lookUpper', this.lookUpper)
  },
  beforeDestroy () {
    this.$root.$off('loadSongsRoot', this.loadDefinedSongs)
    this.$root.$off('loadPlaylistsRoot', this.loadPlaylists)
    this.$root.$off('playDefinedSongRoot', this.playDefinedSong)
    this.$root.$off('pauseSongRoot', this.pauseSong)
    this.$root.$off('playSongRoot', this.playSong)
    this.$root.$off('lookUpper', this.lookUpper)
    this.removeListeners()
  },
  methods: {
    hideAddMenu () {
      this.isAddMenuActive = false
    },
    userActionsHandler (flag, successfulMessage, errorMessage) {
      if (flag) {
        swal('Yes!', successfulMessage, 'success')
      } else {
        swal('Oops', errorMessage, 'error')
      }
    },
    loadPlaylists () {
      let that = this
      axios({
        method: 'GET',
        url: 'https://localhost:44343/api/Songs/GetPlaylists',
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
        }
      }).then(function (e) {
        that.$main.playlists = e.data
      }).catch(function (e) {
        that.$root.$emit('deactLoadingRoot')
        that.$root.$emit('errorHandler', e.response.status)
      })
    },
    lookUpper (index) {
      return this.songs.map(function (x) { return x.Id }).indexOf(index)
    },
    favLookUpper (index) {
      return this.$main.favoriteSongs.map(function (x) { return x.Id }).indexOf(index)
    },
    addSongToPlaylist(playlistId) {
      let that = this
      let obj = {
        PlaylistId: playlistId,
        SongId: this.$main.currentIndex
      }
      axios({
        method: 'POST',
        url: 'https://localhost:44343/api/Songs/AddSongToPlaylist',
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
        },
        data: obj
      }).then(function (e) {
        that.$root.$emit('notificate', 'success', 'Song successfully added to playlist', 3000)
        let index = that.lookUpper(obj.SongId)
        let song = that.songs[index]
        that.$main.playlists[
          that.$main.playlists
          .map(function (e) {
            return e.Id
          })
          .indexOf(playlistId)
        ].Cover = song.AlbumCover
        that.$root.$emit('addSongToPlaylist', song, obj.PlaylistId)
      }).catch(function (e) {
        that.$root.$emit('errorHandler', e.response.status)
      })
    },
    favHandler (e) {
      let flag = e.target.className === 'fa-heart far'
      let currentIndex = this.$main.currentIndex
      let that = this
      if (flag) {
        axios({
          method: 'POST',
          url: 'https://localhost:44343/api/Songs/AddFavoriteSong',
          data: currentIndex,
          headers: {
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
          }
        }).then(function (e) {
          let song = that.songs[that.lookUpper(currentIndex)]
          that.$main.favoriteSongs.push(song)
          that.$root.$emit('notificate', 'success', 'Song added to favorites', 3000)
        }).catch(function (e) {
          console.log(e)
          that.$root.$emit('errorHandler', e.response.status)
        })
      } else {
        let instance = JSON.stringify({SongId: currentIndex, Type: 2})
        axios({
          method: 'POST',
          url: 'https://localhost:44343/api/Songs/RemoveSongFromInstance',
          data: instance,
          headers: {
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
          }
        }).then(function (e) {
          var index = that.favLookUpper(currentIndex)
          if (index > -1) {
            that.$main.favoriteSongs.splice(index, 1)
          }
          that.$root.$emit('notificate', 'success', 'Song deleted from favorites', 3000)
        }).catch(function (e) {
          console.log(e)
          that.$root.$emit('errorHandler', e.response.status)
        })
      }
    },
    loadDefinedSongs (songs) {
      let that = this
      let temporarySongs = JSON.parse(JSON.stringify(songs));
      if (temporarySongs.length > 0) {
        that.songs = temporarySongs
        that.$main.currentIndex = that.songs[0]['Id']
        that.songsLength = that.songs.length
        that.shuffleIndexes = []
        for (let i = 0; i < that.songsLength; i++) {
          that.shuffleIndexes.push(that.songs[i]['Id'])
        }
        that.preloadSong()
      }
    },
    loadSongs () {
      let that = this
      axios({
        method: 'GET',
        url: 'https://localhost:44343/api/Songs/GetSongs',
        headers: {
          Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
        }
      }).then(function (e) {
        that.songs = e.data
        that.$main.currentIndex = that.songs[0]['Id']
        that.songsLength = that.songs.length
        that.preloadSong()
        that.shuffleIndexes = []
        for (let i = 0; i < that.songsLength; i++) {
          that.shuffleIndexes.push(that.songs[i]['Id'])
        }
      }).catch(function (e) {
        console.log(e)
        that.$root.$emit('errorHandler', e.response.status)
      })
      axios({
        method: 'GET',
        url: 'https://localhost:44343/api/Songs/GetFavoriteSongs',
        headers: {
          Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
        }
      }).then(function (e) {
        that.$main.favoriteSongs = e.data
      }).catch(function (e) {
        console.log(e)
        that.$root.$emit('errorHandler', e.response.status)
      })
      this.audio.preload = 'metadata'
      this.audio.onloadedmetadata = function () {
        that.setDuration()
      }
      this.audio.ontimeupdate = function () {
        that.updateTime()
      }
    },
    handleSpaceClick (e) {
      if (e.target.className === 'player-button-icon') {
        e.preventDefault()
      }
      if (e.target.tagName.toUpperCase() !== 'INPUT') {
        if (e.keyCode === 32) {
          if (this.audio.paused) {
            this.playSong()
          } else {
            this.pauseSong()
          }
        }
      }
    },
    scrollToNextSongInQueue() {
      let currentSong = document.getElementsByClassName('current-song')
      let scrolledBlock = document.getElementsByClassName('queue-body')[0]
      if (currentSong.length > 0) {
        const elementPos = currentSong[0].offsetTop
        scrolledBlock.scrollTop = elementPos
      }
    },
    muteSong (e) {
      e.stopPropagation()
      this.isVolumeOff = !this.isVolumeOff
      this.audio.volume = 0
    },
    unMuteSong () {
      this.isVolumeOff = !this.isVolumeOff
      this.audio.volume = this.volume
    },
    shuffleSongs () {
      this.isShuffled = !this.isShuffled
      if (this.isShuffled) {
        for (let i = this.songsLength - 1; i > 0; i--) {
          const j = Math.floor(Math.random() * (i + 1))
          let x = this.shuffleIndexes[i]
          this.shuffleIndexes[i] = this.shuffleIndexes[j]
          this.shuffleIndexes[j] = x
        }
        this.$main.currentIndex = this.shuffleIndexes[this.shuffleIndexes.indexOf(this.$main.currentIndex)]
      } else this.unShuffleSongs()
    },
    unShuffleSongs () {
      this.$main.currentIndex = this.shuffleIndexes[this.shuffleIndexes.indexOf(this.$main.currentIndex)]
      for (let i = 0; i < this.songsLength; i++) {
        this.shuffleIndexes[i] = this.songs[i]['Id']
      }
    },
    formatTime (time) {
      var min = Math.floor(time / 60)
      var sec = Math.floor(time % 60)
      return min + ':' + ((sec < 10) ? ('0' + sec) : sec)
    },
    formatBackTime (time) {
      return this.audio.duration * time
    },
    formatBackVolume (volume) {
      return volume / 150
    },
    formatProgress (time) {
      return (time / this.audio.duration) * 100 + '%'
    },
    playSong () {
      this.$main.isPaused = false
      if (this.audio.paused) {
        this.audio.play()
      }
    },
    pauseSong () {
      this.$main.isPaused = true
      this.audio.pause()
    },
    playNext () {
      if (this.$main.currentIndex === this.shuffleIndexes[this.songsLength - 1]) {
        this.$main.currentIndex = this.shuffleIndexes[0]
      } else {
        this.$main.currentIndex = this.shuffleIndexes[this.shuffleIndexes.indexOf(this.$main.currentIndex) + 1]
      }
      this.preloadSong()
      this.playSong()
    },
    playPrev () {
      if (this.$main.currentIndex === this.shuffleIndexes[0]) {
        this.$main.currentIndex = this.shuffleIndexes[this.songsLength - 1]
      } else {
        this.$main.currentIndex = this.shuffleIndexes[this.shuffleIndexes.indexOf(this.$main.currentIndex) - 1]
      }
      this.preloadSong()
      this.playSong()
    },
    updateTime () {
      var current = this.audio.currentTime
      this.progressWidth = this.formatProgress(current)
      this.currentTime = this.formatTime(current)
      if (this.audio.ended) {
        if (!this.isReplayed) this.playNext()
        else this.playSong()
      }
    },
    addListeners () {
      document.getElementsByClassName('pin')[0].addEventListener('mousedown', this.mouseDown, false)
      window.addEventListener('mouseup', this.mouseUp, false)
      document.addEventListener('keydown', this.handleSpaceClick, false)
    },
    removeListeners () {
      document.removeEventListener('keydown', this.handleSpaceClick, false)
      window.removeEventListener('mouseup', this.mouseUp, false)
    },
    mouseUp () {
      window.removeEventListener('mousemove', this.onMoveTimeChange, true)
      this.isDragged = false
    },
    mouseDown (e) {
      window.addEventListener('mousemove', this.onMoveTimeChange, true)
      this.isDragged = true
    },
    onMoveTimeChange (e) {
      if (this.isDragged) {
        var x = e.clientX
        var timeline = document.getElementsByClassName('main-player-body')[0]
        var offset = x - timeline.offsetLeft - 1
        this.progressWidth = offset + 'px'
        this.audio.currentTime = this.formatBackTime(offset / timeline.clientWidth)
      }
    },
    onTimeChange (e) {
      var x = e.clientX
      var timeline = document.getElementsByClassName('main-player-body')[0]
      var offset = x - timeline.offsetLeft - 1
      this.progressWidth = offset + 'px'
      this.audio.currentTime = this.formatBackTime(offset / timeline.clientWidth)
    },
    playDefinedSong (index) {
      if (index === this.$main.currentIndex) {
        if (this.$main.isPaused) this.playSong()
        else this.pauseSong()
      } else {
        this.$main.currentIndex = index
        this.preloadSong()
        this.playSong()
        axios({
          method: 'POST',
          url: 'https://localhost:44343/api/Songs/IncreaseActivity',
          headers: {
            'Content-Type': 'application/json',
            Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
          },
          data: index
        }).then(function (e) {
          console.log(e)
        }).catch(function (e) {
          console.log(e)
          that.$root.$emit('errorHandler', e.response.status)
        })
      }
    },
    onVolumeChange (e) {
      var x = e.clientX
      var timeline = document.getElementsByClassName('player-settings')[0]
      var mainLeft = document.getElementsByClassName('main-player-body')[0].offsetLeft
      var offset = x - (timeline.offsetLeft + mainLeft) + 60
      let offsetFromatted = this.formatBackVolume(offset)
      if (offsetFromatted >= 0) {
        this.progressVolumeWidth = offset + 'px'
        this.audio.volume = offsetFromatted
      } else {
        this.progressVolumeWidth = 0
        this.audio.volume = 0
      }
      this.volume = this.audio.volume
    },
    preloadSong () {
      this.progressWidth = 0
      this.audio.volume = this.isVolumeOff ? 0 : this.volume
      let index = this.lookUpper(this.$main.currentIndex)
      if (index != -1) {
        this.audio.src = 'https://localhost:44343/Songs/' + this.songs[index]['Source']
        this.currentPoster = this.songs[index]['AlbumCover']
        this.title = this.songs[index]['Name'] + ' · ' + this.songs[index]['Artist']
        this.scrollToNextSongInQueue()
      }
    },
    setDuration () {
      this.duration = this.formatTime(this.audio.duration)
    },
    stopProp (e) {
      e.stopPropagation()
    }
  }
}
</script>
<style>

 .queue-item.current-song .queue-item-play-button {
   display: block;
 }

 .queue-item.current-song .queue-item-cover-block {
   display: none;
 }

 .queue-item-play-button {
   display: none;
   margin: 0 6.5px;
 }

 .queue-item:hover .queue-item-play-button {
   display: block;
 }

 .queue-item:hover .queue-item-cover-block {
   display: none;
 }

 .main-player-add-to {
  position: relative;
  margin-left: auto;
  font-size: 20px;
  color: #918e9f;
  cursor: pointer;
  display: grid;
}

.main-player-add-to i {
  padding: 10px;
}

.main-player-add-menu {
  position: absolute;
  height: 350px;
  width: 300px;
  background: #fff;
  display: block;
  box-shadow: 1px -1px 20px 1px rgba(0, 0, 0, 0.14901960784313725);
  bottom: 90px;
  font-size: 14px;
  text-align: center;
  padding: 10px;
  padding-bottom: 0;
  cursor: default;
  left: -3px;
  border-radius: 4px;
  animation: fade-in 0.5s;
}

.main-player-add-menu::before,
.main-player-add-menu::after {
  left: 10px;
  width: 0;
  border-style: solid;
  border-width: 0 11px 8px;
  content: "";
  transform: rotateX(180deg);
  position: absolute;
  height: auto;
}

.main-player-add-menu::before {
  top: 359px;
  z-index: 1;
  border-color: transparent transparent #fff;
}

.main-player-add-menu::after {
  top: 360px;
  border-color: transparent transparent #cecece;
}

.main-player-add-menu-playlists {
  display: block;
  overflow: auto;
  height: 290px;
}

.main-player-add-menu-item {
  display: flex;
  align-items: center;
  padding: 5px;
  cursor: pointer;
  margin: 5px 0;
}

.main-player-add-menu-item:hover {
  background: #f6f6f7;
}

.main-player-add-menu-item-title {
  margin-left: 10px;
}

.main-player-add-menu-item-cover {
  height: 40px;
  width: 40px;
}

.main-player-add-menu-item-cover-image {
  width: 100%;
}

.music-queue-block {
  position: absolute;
  transform: translateY(500px);
  transition: .5s;
  bottom: 0;
  z-index: 999;
  width: 100%;
  background: #ffffff;
  box-shadow: 0 5px 20px 0px #00000040;
}

.queue-item-cover-block {
  max-width: 30px;
  margin: 0 10px;
}

.queue-item-cover-block img {
  width: 100%;
}

.queue-item-title-block {
  display: block;
  margin-left: 5px;
}

.queue-body {
  max-height: 500px;
  min-height: 200px;
  overflow-y: auto;
}

.queue-active.music-queue-block {
  transform: translateY(0px);
}

.music-queue-block:not(.queue-active) .queue-closer {
  transform: rotate(360deg);
  transition: .5s;
  top: 0;
}

.queue-closer {
  position: absolute;
  right: 5px;
  padding: 0px 5px 0px 5px;
  top: -40px;
  background: #e3e3e76e;
  cursor: pointer;
  width: auto;
  height: auto;
  font-size: 25px;
  color: #f39d93;
}

.queue-list {
  width: 100%;
  margin: 0 auto;
}
.queue-item {
  padding: 10px 5px 10px 5px;
  cursor: pointer;
  color: #3a3654;
  display: flex;
  align-items: center;
  border-bottom: 1px solid #ebeaee;
}

.queue-item-title {
  margin: 5px 0;
}

.queue-item-name {
  font-size: 12px;
}

.queue-item-artist {
  font-size: 10px;
}

.queue-item.current-song {
  background: rgba(160, 158, 173, 0.2901960784313726)
}

.queue-item:hover {
  background: rgba(160, 158, 173, 0.2901960784313726);
  transition: .5s
}

.queue-item .player-button-icon {
  padding: 0 !important;
}

.main-player-block {
  position: fixed;
  -webkit-align-items: center;
  align-items: center;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  bottom: 0;
  animation: fade-in 0.5s;
  height: 100px;
  width: 100%;
  left: 0;
  right: 0;
  background: #fff;
  z-index: 999999;
  box-shadow: 0 -3px 15px 0px rgba(51, 51, 51, 0.1)
}

.player-button-icon {
  background: none;
  border: none;
  border-radius: 50%;
  padding: 10px;
  font-size: 15px;
  color: #3a3654;
  cursor: pointer;
}

.main-player-body {
  width: 80%;
  position: relative;
  margin: 0 auto;
  margin-top: 10px;
}

.main-player-poster {
  width: 70px;
  height: 70px;
  position: relative;
  margin-left: auto
}

.main-player-poster img {
  width: 100%;
  height: 100%;
}

.player-trackline {
  top: 15px;
  padding: 0;
  width: 100%;
  position: absolute;
  margin: 0 auto;
  left: 0;
  right: 0
}

.timeline {
  position: relative;
  display: flex;
  align-items: center
}

.player-settings {
  position: absolute;
  top: 30px;
  right: 0
}

.mobile-settings {
  display: none;
}

.player-settings i,
.mobile-settings i {
  cursor: pointer;
  color: rgba(58, 54, 84, 0.56);
  padding: 6px;
  border-radius: 50%;
  position: relative;
}

.main-player-add-to i:hover {
  background: #ebeaed;
  border-radius: 50%;
}

.main-player-add-to .active-icon {
  background: #ebeaed;
  border-radius: 50%;
}

.main-player-add-to .fa-plus {
  margin-left: 2px;
}

.main-player-add-to .fa-heart {
  margin-bottom: -2px;
}

.player-buttons .player-button-icon:hover i {
  background: rgba(58, 54, 84, 0.1);
}

.player-buttons .player-button-icon i {
  vertical-align: middle;
  border-radius: 50%;
  text-align: center;
  padding: 15px;
}

.player-button-icon:nth-child(1) i,
.player-button-icon:nth-child(4) i {
  width: 17px;
  height: 17px;
  line-height: 17px;
  padding: 10px;
}

.player-button-icon:nth-child(1) i {
  width: 17px;
  height: 17px;
  line-height: 17px;
  padding: 10px;
  padding-right: 12px;
  padding-left: 8px;
}

.player-button-icon:nth-child(2) i {
  width: 22px;
  height: 22px;
  line-height: 22px;
  /* background: rgba(58, 54, 84, 0.1); */
  padding: 15px;
}

.player-button-icon:nth-child(3) i {
  width: 22px;
  height: 22px;
  line-height: 22px;
  background: rgba(58, 54, 84, 0.1);
  padding: 15px;
  padding-left: 13px;
  padding-right: 17px;
}

.player-buttons {
  width: 190px;
  padding: 0;
  margin: 0 auto;
  margin-top: 25px;
  -webkit-align-items: center;
  align-items: center;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
}

.player-buttons button:nth-child(2),
.player-buttons button:nth-child(3) {
  font-size: 30px;
}

.current-time {
  left: 0;
}

.current-time,
.total-time {
  position: relative;
  top: -12px;
  font-size: 11px;
  cursor: initial;
  color: #3a3654;
  width: 20px;
  font-family: 'Niramit', sans-serif
}

.total-time {
  margin-left: calc(100% - 45px);
}

.main-player-block .pin {
  background-color: #3a3654;
  border-radius: 8px;
  height: 8px;
  position: absolute;
  pointer-events: all;
  top: -3px;
  width: 8px;
  -webkit-box-shadow: 0px 1px 1px 0px rgba(0, 0, 0, 0.32);
  -moz-box-shadow: 0px 1px 1px 0px rgba(0, 0, 0, 0.32);
  box-shadow: 0px 1px 1px 0px rgba(0, 0, 0, 0.32);
  -webkit-transition: transform 0.25s ease;
  -moz-transition: transform 0.25s ease;
  -ms-transition: transform 0.25s ease;
  -o-transition: transform 0.25s ease;
  transition: transform 0.25s ease
}

.player-trackline .title {
  position: absolute;
  top: 55%;
  font-family: 'Niramit', sans-serif;
  font-size: 14px;
}

.main-player-block .slider {
  width: 100%;
  cursor: pointer;
  background: rgba(58, 54, 84, 0.31);
  height: 2px;
  position: absolute
}

.progress {
  background: #3a3654;
  height: 100%
}

.main-player-block .slider:hover {
  height: 5px;
  border-top-right-radius: 5px;
  border-bottom-right-radius: 5px;
}

.main-player-block .slider:hover .progress {
  border-top-left-radius: 5px;
  border-bottom-left-radius: 5px;
}

.main-player-block .slider:hover .pin {
  height: 11px;
  width: 11px
}

.main-player-block .pin:active,
.main-player-block .pin:focus {
  -moz-transform: scale(1.5);
  -o-transform: scale(1.5);
  -ms-transform: scale(1.5);
  -webkit-transform: scale(1.5);
  transform: scale(1.5)
}

.player-settings-active {
  color: #3a3654 !important;
  background: #ebeaed;
}

.player-settings i:hover {
  background: #ebeaed;
}

.volume-raise-block {
  position: absolute;
  height: 50px;
  background: #ffffff;
  width: 170px;
  border-radius: 50px;
  bottom: 40px;
  right: 0;
  margin-left: -71px;
  left: 0;
  display: flex;
  visibility: hidden;
  align-items: center;
  box-shadow: 0px 5px 20px #00000026;
  transition: visibility 0.2s;
}

.volume-raise-block::before {
  content: "";
  position: absolute;
  top: 100%;
  left: 50%;
  margin-left: -5px;
  border-width: 5px;
  border-style: solid;
  border-color: #fff transparent transparent transparent;
}

.player-settings i:nth-child(1):hover .volume-raise-block {
  visibility: visible;
}

.volume-raise-block .slider {
  width: 90%;
  margin: 0 auto;
  left: 0;
  right: 0;
  position: absolute;
}

:focus {
  outline: none
}

@media (min-width: 1000px) {
  .main-player-poster {
    margin-left: auto;
  }

  .main-player-body {
    margin: 0 auto;
  }
}

@media (max-width: 700px) {
  .player-settings {
    display: none;
    top: -72px;
    right: -4.5px;
    background: #f1f1f1;
    border-radius: 13px;
    animation: fade-in 0.5s;
    padding: 5px;
    padding-bottom: 36px;
  }

  .player-settings i:not(:nth-child(3)) {
    margin-top: 5px;
  }

  .mobile-settings {
    display: block;
    position: absolute;
    top: 30px;
    z-index: 2;
    right: 0
  }

  .player-trackline .title {
    font-size: 12px;
  }

  .player-settings i.fa-volume-up {
    display: none;
  }

  .mobile-settings:hover+.player-settings,
  .player-settings:hover {
    display: grid;
  }

  .main-player-block {
    height: 120px;
  }

  .main-player-body {
    margin-top: 0;
  }

  .player-buttons {
    margin-top: 45px;
  }

  .main-player-poster {
    width: 45px;
    height: 45px;
  }

  .sidebar ul {
    margin-top: 40px;
  }
}

@media (max-width: 500px) {
  .main-player-poster {
    display: none;
  }

  .main-player-body {
    width: 95%;
  }

  .mobile-layer {
    position: fixed;
    left: 0px;
    top: 0px;
    z-index: 2;
    bottom: 0px;
    right: 0px;
    width: 100%;
    height: 100%;
    background-color: #9494944a;
  }

  .main-player-add-to {
    position: absolute;
    display: block;
    width: 95%;
    margin-top: 20px;
    left: 50%;
    transform: translate(-50%, 0);
    height: 0;
    z-index: 3;
  }

  .main-player-add-to i {
    position: absolute;
    padding: 4px;
  }

  .main-player-add-to i:nth-child(2) {
    right: 0px;
  }

  .main-player-add-menu {
    right: 50%;
    transform: translate(50%, -10%);
    height: 400px;
    left: auto;
  }

  .main-player-add-menu::before,
  .main-player-add-menu::after {
    display: none;
  }
}

</style>
