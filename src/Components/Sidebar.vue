<template>
<div class="absolute-items">
  <div class="sidebar">
    <div v-bind:class="{ 'burger-active': isActiveSidebar }" @click="toggleSidebar()" class="sidebar-burger">
      <span class="burger-inner"></span>
    </div>
    <div class="sidebar-body">
      <div class="sidebar-legend">
        <a>AUDIO</a>
     </div>
     <ul>
        <li><a>Home</a></li>
        <li><a>My music</a></li>
        <li><a>Playlists</a></li>
        <li><a>Search</a></li>
      </ul>
    </div>
  </div>
  <div class="main-player-block">
    <div class="main-player-poster"><img v-bind:src="currentPoster"/></div>
    <div class="main-player-body">
    <div class="player-buttons">
      <button class="player-button-icon" @click="playPrev()"><i class="fas fa-backward"></i></button>
      <button class="player-button-icon" @click="playSong()" v-bind:class="{hidden : !isPaused}" ><i class="fas fa-play"></i></button>
      <button class="player-button-icon" @click="pauseSong()" v-bind:class="{hidden : isPaused}" ><i class="fas fa-pause"></i></button>
      <button class="player-button-icon" @click="playNext()"><i class="fas fa-forward"></i></button></div>
    <div class="player-trackline">
      <div class="title">{{ title }}</div>
      <div class="timeline">
      <span class="current-time">{{ currentTime }}</span>
      <span class="total-time">{{ duration }}</span>
      <div @click="onTimeChange($event)" class="slider">
       <div class="progress" v-bind:style="{ width: progressWidth }">
         <div @click="stopProp($event)" class="pin" id="progress-pin" v-bind:style="{ 'margin-left': progressWidth }"></div>
      </div>
      </div>
     </div>
     </div>
    <div class="player-settings">
      <i @click="muteSong($event)" v-bind:class="{hidden: isVolumeOff}" class="fas fa-volume-up">
        <div @click="stopProp($event)" class="volume-raise-block">
          <div @click="onVolumeChange($event)" class="slider">
            <div class="progress" v-bind:style="{ width: progressVolumeWidth }">
              <div @click="stopProp($event)" class="pin" id="progress-pin" v-bind:style="{ 'margin-left': progressVolumeWidth }"></div>
            </div>
          </div>
        </div>
      </i>
      <i @click="unMuteSong()" v-bind:class="{hidden: !isVolumeOff, 'player-settings-active': isVolumeOff}" class="fas fa-volume-mute"></i>
      <i @click="shuffleSongs()" v-bind:class="{'player-settings-active': isShuffled}" class="fas fa-random"></i>
      <i @click="isReplayed=!isReplayed" v-bind:class="{'player-settings-active': isReplayed}" class="fas fa-redo"></i>
    </div>
  </div>
  </div>
  </div>
</template>
<script>
import store from '../store'
import { mapGetters } from 'vuex'
import universalParse from 'id3-parser/lib/universal'
export default {
  store,
  data () {
    return {
      message: 'dgdgdg',
      songs: [],
      duration: '0:00',
      progressWidth: '0%',
      progressVolumeWidth: '50%',
      currentTime: '0:00',
      title: ' - ',
      isDragged: false,
      isPaused: true,
      songsLength: 0,
      isVolumeOff: false,
      isShuffled: false,
      currentPoster: require('../img/no-cover.png'),
      volume: 0.5,
      isReplayed: false,
      shuffleIndexes: [],
      currentIndex: 0,
      audio: new Audio()
    }
  },
  mounted () {
    this.loadSongs()
    this.preloadSong()
    this.addListeners()
  },
  methods: {
    loadSongs () {
      this.songs = [
        { src: require('../songs/1.mp3'),
          artist: '0',
          name: 'Wow'
        },
        { src: require('../songs/2.mp3'),
          artist: '1',
          name: 'Disdain'
        },
        { src: require('../songs/3.mp3'),
          artist: '2',
          name: 'Base'
        },
        { src: require('../songs/4.mp3'),
          artist: '3',
          name: 'Fireproof'
        },
        { src: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/NF%20-%20Let%20You%20Down.mp3',
          artist: '4',
          name: 'Im not the only one'
        }
      ]
      this.audio.preload = 'metadata'
      var that = this
      this.audio.onloadedmetadata = function () {
        that.setDuration()
      }
      this.audio.ontimeupdate = function () {
        that.updateTime()
      }
      this.songsLength = this.songs.length
      for (let i = 0; i < this.songsLength; i++) {
        this.shuffleIndexes.push(i)
      }
    },
    handleSpaceClick (e) {
      e.preventDefault()
      if (e.keyCode === 32) {
        if (this.isPaused) {
          this.playSong()
        } else {
          this.pauseSong()
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
    unShuffleSongs () {
      this.currentIndex = this.shuffleIndexes[this.currentIndex]
      for (let i = 0; i < this.songsLength; i++) {
        this.shuffleIndexes[i] = i
      }
    },
    toggleSidebar () {
      if (this.isActiveSidebar) {
        this.$store.dispatch('deactivateSidebar')
      } else {
        this.$store.dispatch('activateSidebar')
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
      this.isPaused = false
      if (this.audio.paused) {
        this.audio.play()
      }
    },
    pauseSong () {
      this.isPaused = true
      var audio = this.audio
      audio.pause()
    },
    playNext () {
      if (this.currentIndex === this.songsLength - 1) this.currentIndex = 0
      else this.currentIndex++
      this.preloadSong()
      this.playSong()
      console.log(this.currentIndex)
    },
    playPrev () {
      if (this.currentIndex === 0) this.currentIndex = this.songsLength - 1
      else this.currentIndex--
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
      document.getElementById('progress-pin').addEventListener('mousedown', this.mouseDown, false)
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
      this.audio.src = this.songs[this.shuffleIndexes[this.currentIndex]]['src']
      universalParse(this.audio.src).then(tag => {
        var image = tag.image
        if (image) {
          var base64String = ''
          for (var i = 0; i < image.data.length; i++) {
            base64String += String.fromCharCode(image.data[i])
          }
          var base64 = 'data:' + image.mime + ';base64,' +
                window.btoa(base64String)
          this.currentPoster = base64
        } else {
          this.currentPoster = require('../img/no-cover.png')
        }
        this.title = tag.title + ' - ' + tag.artist
      })
    },
    setDuration () {
      this.duration = this.formatTime(this.audio.duration)
    },
    stopProp (e) {
      e.stopPropagation()
    },
    shuffleSongs () {
      this.isShuffled = !this.isShuffled
      if (this.isShuffled) {
        var currentSongBeforeShuffle = this.shuffleIndexes[this.currentIndex]
        for (let i = this.songsLength - 1; i > 0; i--) {
          const j = Math.floor(Math.random() * (i + 1))
          let x = this.shuffleIndexes[i]
          this.shuffleIndexes[i] = this.shuffleIndexes[j]
          this.shuffleIndexes[j] = x
        }
        var currentSongIndex = this.shuffleIndexes.indexOf(currentSongBeforeShuffle)
        this.shuffleIndexes[currentSongIndex] = this.shuffleIndexes[0]
        this.shuffleIndexes[0] = currentSongBeforeShuffle
        this.currentIndex = 0
      } else this.unShuffleSongs()
    }
  },
  computed: mapGetters({
    isActiveSidebar: 'isActiveSidebar'
  })
}
</script>
<style>
.content-wrapper {
    position: relative;
    display: block;
    right: 0;
    top: 0;
    bottom: 0;
    min-height: 100%;
    z-index: 1;
    transition: .3s;
    margin-top: 40px;
    animation: slide-out-body 1s;
}
.sidebar-active .content-wrapper {
  animation: slide-in 1s;
}
.sidebar-active {
  left: 220px;
}
.sidebar-active .sidebar-body ul li:nth-child(1) {
  animation: slide-in 1.1s;
}
.sidebar-active .sidebar-body ul li:nth-child(2) {
  animation: slide-in 1.3s;
}
.sidebar-active .sidebar-body ul li:nth-child(3) {
  animation: slide-in 1.5s;
}
.sidebar-active .sidebar-body ul li:nth-child(4) {
  animation: slide-in 1.6s;
}
.hidden {
  display: none;
}
ul {
  display: block;
  list-style-type: disc;
  margin-block-start: 1em;
  margin-block-end: 1em;
  margin-inline-start: 0px;
  margin-inline-end: 0px;
  padding-inline-start: 0px;
  margin-top: 80px;
}
.sidebar ul li {
  list-style: none;
  padding-left: 20px;
  padding-bottom: 10px;
  border-bottom: 1px solid rgba(0, 0, 0, 0.07);
}
.sidebar ul li:last-child {
  border: none;
}
.sidebar-active .sidebar {
  animation: slide-in 1s;
  transform: translateX(0);
}
.page:not(.sidebar-active) .sidebar {
  animation: slide-out 1s;
}
.page:not(.sidebar-active) .sidebar-burger {
  animation: roll-out 1s;
}
.sidebar {
  -ms-flex-direction: column;
  -webkit-flex-direction: column;
  background: linear-gradient(rgb(243, 133, 120), rgba(31, 28, 236, 0.5));
  bottom: 0px;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  flex-direction: column;
  left: 0;
  position: fixed;
  top: 0;
  transform: translateX(-220px);
  z-index: 100;
  width: 220px;
}
.sidebar-burger {
  position: absolute;
  left: 235px;
  top: 10px;
  animation: roll-in 1s;
  cursor: pointer;
  width: 40px;
  height: 40px;
}
.burger-inner {
  top: 50%;
  margin-top: -2px;
  transition-timing-function: cubic-bezier(.55,.055,.675,.19);
  transition-duration: .22s;
}
.sidebar-burger span,
.sidebar-burger span::before,
.sidebar-burger span::after {
  position: absolute;
  cursor: pointer;
  display: block;
  width: 40px;
  height: 4px;
  transition-timing-function: ease;
  transition-duration: .15s;
  transition-property: transform;
  border-radius: 4px;
  background-color: #d3b0d1;
}
.burger-inner:after,
.burger-inner:before {
    content: "";
}
.burger-inner:before {
  top: -10px;
}
.burger-inner:after {
  bottom: -10px;
}
.sidebar-burger.burger-active span {
  transition-delay: .12s;
  transition-timing-function: cubic-bezier(.215,.61,.355,1);
  transform: rotate(225deg);
}
.sidebar-burger.burger-active span:before {
  top: 0;
  transition: top .1s ease-out,opacity .1s ease-out .12s;
  opacity: 0;
}
.sidebar-burger.burger-active span:after {
  bottom: 0;
  transition: bottom .1s ease-out,transform .22s cubic-bezier(.215,.61,.355,1) .12s;
  transform: rotate(-90deg);
}
.sidebar li a {
  font-size: 30px;
  font-family: 'Niramit', sans-serif;
  cursor: pointer;
  color: #3a3654;
  transition: .3s ease-in;
}
.sidebar li a::after {
  content: "";
  width: 0;
  opacity: 0;
  position: absolute;
  left: 5px;
  margin-top: 28px;
  height: 2px;
  background: rgba(255, 255, 255, 0.61);
  transition: .3s ease-in;
}
.sidebar li a:hover,
.sidebar li a:active,
.sidebar li a:focus,
.sidebar li a:visited {
  margin-left: 10px;
}
.sidebar li a:hover:after,
.sidebar li a:visited:after,
.sidebar li a:active:after,
.sidebar li a:focus:after {
  opacity: 1;
  width: 10px;
}
.sidebar-legend {
  text-align: center;
  padding: 25px;
  cursor: pointer;
}
.sidebar-legend:hover a:before{
  width: 110px;
  left: 50px;
}
.sidebar-legend a {
  font-size: 40px;
  cursor: pointer;
  letter-spacing: 10px;
  font-family: 'Nova Round', sans-serif;
  color: #ffffff;
  text-shadow: 4px 1px 0 #ffa79c;
}
.sidebar-legend a:before {
  content: "";
  position: absolute;
  width: 80px;
  margin-top: 50px;
  left: 65px;
  height: 1px;
  background: #fdfdfd;
  transition: .3s ease-in;
}
.sidebar-legend a:after {
  content: "web";
  top: 85px;
  position: absolute;
  color: #fff;
  font-size: 10px;
  font-family: Round Nova,sans-serif;
  text-shadow: none;
  left: 85px;
}
.main-player-block {
    position: fixed;
    -webkit-align-items: center;
    align-items: center;
    display: -webkit-flex;
    display: -ms-flexbox;
    display: flex;
    bottom: 0;
    height: 100px;
    width: 100%;
    left: 0;
    right: 0;
    background: #fff;
    z-index: 999999;
    box-shadow: 0 -3px 15px 0px rgba(51, 51, 51, 0.1)
}
.main-player-block .player-button-icon {
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
    background: #3a3f68;
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
    width: 160px;
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
.player-settings i {
  cursor: pointer;
  color: rgba(58, 54, 84, 0.56);
  padding: 6px;
  border-radius: 50%;
  position: relative;
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
@media (min-width: 1500px) {
  .main-player-poster {
    margin: 0 auto;
  }
}
@media (max-width: 700px) {
  .sidebar-active .content-wrapper {
    left: -220px;
    background: linear-gradient(125deg, white, #625abb73);
    filter: blur(10px);
  }
  .player-settings {
    display: none;
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
}
@keyframes slide-in {
  from {transform: translateX(-300px);}
    to {transform: translateX(0px);}
}
@keyframes slide-out-body {
  10% {transform: translateX(50px);}
  100% {transform: translateX(0px);}
}
@keyframes slide-out {
  from {transform: translateX(0px);}
    to {transform: translateX(-220px);}
}
@keyframes roll-in {
from {transform: translateX(-0px) rotate(-360deg);}
to {transform: translateX(0px) rotate(0);}
}
@keyframes roll-out {
from {transform: translateX(0px) rotate(360deg);}
to {transform: translateX(-0px) rotate(0);}
}
@keyframes slide-up {
  from {transform: translateY(900px);}
    to {transform: translateY(0px);}
}
</style>
