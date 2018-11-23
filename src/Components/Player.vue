<template>
  <div class="main-player-block">
    <div class="main-player-poster"><img v-bind:src="currentPoster"/></div>
    <div class="main-player-add-to"><i class="fas fa-plus"></i><i class="far fa-heart"></i></div>
    <div class="main-player-body">
    <div class="player-buttons">
      <button class="player-button-icon" @click="playPrev()"><i class="fas fa-backward"></i></button>
      <button class="player-button-icon" @click="playSong()" v-bind:class="{hidden : !$main.isPaused}" ><i class="fas fa-play"></i></button>
      <button class="player-button-icon" @click="pauseSong()" v-bind:class="{hidden : $main.isPaused}" ><i class="fas fa-pause"></i></button>
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
            <div class="progress" v-bind:style="{ width: progressVolumeWidth }">
              <div @click="stopProp($event)" class="pin" v-bind:style="{ 'margin-left': progressVolumeWidth }"></div>
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
        <div @click="playDefinedSong(item)" v-bind:class="{'current-song': $main.currentIndex===item}" class="queue-item" :key="item" v-for="(item) in shuffleIndexes">
          <button class="player-button-icon">
            <i class="fas" v-bind:class="{'fa-pause': $main.currentIndex===item && !$main.isPaused, 'fa-play' : $main.isPaused || ($main.currentIndex!==item && !$main.isPaused)}"></i>
          </button>
          <div class="queue-item-title">{{ songs[lookUpper(item)].name }}</div>
          <div class="queue-item-title">{{ songs[lookUpper(item)].artist }}</div>
        </div>
      </div>
    </div>
  </div>
  </div>
</template>
<script>
import store from '../store'
import Vue from 'vue'
import universalParse from 'id3-parser/lib/universal'
var indexes = new Vue({
  data: {
    currentIndex: 0,
    isPaused: true
  }
})
indexes.install = function () {
  Object.defineProperty(Vue.prototype, '$main', {
    get () { return indexes }
  })
}
Vue.use(indexes)
// import { mapGetters } from 'vuex'
export default {
  store,
  data () {
    return {
      songs: [],
      duration: '0:00',
      progressWidth: '0%',
      progressVolumeWidth: '50%',
      currentTime: '0:00',
      title: ' · ',
      isDragged: false,
      songsLength: 0,
      isVolumeOff: false,
      isShuffled: false,
      currentPoster: require('../img/no-cover.svg'),
      volume: 0.5,
      isReplayed: false,
      shuffleIndexes: [],
      isActiveQueue: false,
      audio: new Audio()
    }
  },
  mounted () {
    this.loadSongs()
    this.$main.currentIndex = this.songs[0]['index']
    this.preloadSong()
    this.addListeners()
    this.$root.$on('loadSongs', (songs) => {
      this.loadSongs(songs)
    })
    this.$root.$on('playDefinedSong', (index) => {
      this.playDefinedSong(index)
    })
    this.$root.$on('pauseSong', () => {
      this.pauseSong()
    })
  },
  methods: {
    lookUpper (index) {
      return this.songs.map(function (x){ return x.index }).indexOf(index)
    },
    loadSongs (songs) {
      if (songs !== undefined) {
        this.songs = songs
      } else {
        this.songs = [
          { src: '../media/1.mp3',
            artist: '0',
            name: 'song1',
            index: '12'
          },
          { src: '../media/2.mp3',
            artist: '1',
            name: 'song2',
            index: '22'
          },
          { src: '../media/3.mp3',
            artist: '2',
            name: 'song3',
            index: '4334'
          },
          { src: '../media/4.mp3',
            artist: '3',
            name: 'song4',
            index: '5454'
          },
          { src: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/NF%20-%20Let%20You%20Down.mp3',
            artist: '4',
            name: 'song5',
            index: '5353'
          }
        ]
      }
      this.audio.preload = 'metadata'
      var that = this
      this.audio.onloadedmetadata = function () {
        that.setDuration()
      }
      this.audio.ontimeupdate = function () {
        that.updateTime()
      }
      this.songsLength = this.songs.length
      this.shuffleIndexes = []
      for (let i = 0; i < this.songsLength; i++) {
        this.shuffleIndexes.push(this.songs[i]['index'])
      }
    },
    handleSpaceClick (e) {
      if (e.target.tagName.toUpperCase() !== 'INPUT' && e.target.tagName.toUpperCase() !== 'BUTTON') {
        if (e.keyCode === 32) {
          if (this.audio.paused) {
            this.playSong()
          } else {
            this.pauseSong()
          }
        }
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
        this.shuffleIndexes[i] = this.songs[i]['index']
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
      var audio = this.audio
      audio.pause()
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
      }
    },
    onVolumeChange (e) {
      var x = e.clientX
      var timeline = document.getElementsByClassName('player-settings')[0]
      var mainLeft = document.getElementsByClassName('main-player-body')[0].offsetLeft
      var offset = x - (timeline.offsetLeft + mainLeft) + 60
      if (this.formatBackVolume(offset) >= 0) {
        this.progressVolumeWidth = offset + 'px'
        this.audio.volume = this.formatBackVolume(offset)
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
      this.audio.src = this.songs[index]['src']
      universalParse(this.audio.src).then(tag => {
        var image = tag.image
        if (image) {
          var base64String = ''
          for (var i = 0; i < image.data.length; i++) {
            base64String += String.fromCharCode(image.data[i])
          }
          var base64 = 'data:' + image.mime + ';base64,' + window.btoa(base64String)
          this.currentPoster = base64
        } else {
          this.currentPoster = require('../img/no-cover.svg')
        }
        this.title = tag.title + ' · ' + tag.artist
      })
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
.queue-body {
  max-height: 500px;
  overflow-y: auto;
}
.queue-active.music-queue-block {
  transform: translateY(0px);
}
.music-queue-block:not(.queue-active) .queue-closer{
  transform: rotate(360deg);
  transition: .5s;
  top: 0;
}
.queue-closer {
    position: absolute;
    right: 20px;
    top: 20px;
    top: -50px;
    cursor: pointer;
    width: auto;
    height: auto;
    font-size: 25px;
    color: #908d9e;
}
.queue-list {
  width: 100%;
  margin: 0 auto;
}
.queue-item {
    padding: 10px 15px;
    cursor: pointer;
    color: #3a3654;
    display: flex;
    align-items: center;
    border-bottom: 1px solid #ebeaee;
}
.queue-item-title {
  margin-left: 10px;
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
    font-size: 20px;
    color: #3a3654;
    cursor: pointer
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
    border-radius: 5px;
    position: relative;
    margin-left: auto
}
.main-player-poster img {
    width: 100%;
    height: 100%;
    border-radius: 5px
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
.player-settings i, .mobile-settings i {
  cursor: pointer;
  color: rgba(58, 54, 84, 0.56);
  padding: 6px;
  border-radius: 50%;
  position: relative;
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
.player-button-icon:nth-child(1) i{
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
    background: rgba(58, 54, 84, 0.1);
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
.current-time, .total-time {
    position: relative;
    top: -12px;
    font-size: 12px;
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
    width: 150px;
    border-radius: 50px;
    bottom: 40px;
    right: 0;
    margin-left: -71px;
    left: 0;
    display: flex;
    visibility: hidden;
    align-items: center;
    box-shadow: 0px 5px 20px #00000026;
    padding: 0 10px;
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
.volume-raise-block .slider{
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
    background: #dedee2;
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
  .player-settings i.fa-volume-up {
    display: none;
  }
  .mobile-settings:hover + .player-settings,
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
  .main-player-add-to {
    position: absolute;
    display: block;
    width: 95%;
    margin-top: 20px;
    left: 50%;
    transform: translate(-50%, 0);
    height: 0;
    z-index: 2;
  }
  .main-player-add-to i {
    position: absolute;
    padding: 4px;
  }
  .main-player-add-to i:nth-child(2) {
    right: 0px;
  }
}
</style>
