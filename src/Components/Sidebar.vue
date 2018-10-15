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
    <div class="main-player-body">
    <div class="player-buttons">
      <button class="player-button-icon" @click="playPrev()"><i class="fas fa-backward"></i></button>
      <button class="player-button-icon" @click="playSong()" v-bind:class="{hidden : isPaused}" ><i class="fas fa-play"></i></button>
      <button class="player-button-icon" @click="pauseSong()" v-bind:class="{hidden : isPlayed}" ><i class="fas fa-pause"></i></button>
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
      <i class="fas fa-volume-up"></i>
      <i @click="shuffleSongs()" v-bind:class="{shuffled: isShuffled}" class="fas fa-random"></i>
      <i class="fas fa-redo"></i>
    </div>
  </div>
  </div>
  </div>
</template>
<script>
import store from '../store'
import { mapGetters } from 'vuex'
export default {
  store,
  data () {
    return {
      message: 'dgdgdg',
      songs: [],
      duration: '0:00',
      progressWidth: '0%',
      currentTime: '0:00',
      title: ' - ',
      isPlayed: true,
      isDragged: false,
      isPaused: false,
      songsLength: 0,
      isShuffled: false,
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
        { src: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/Post%20Malone%20-%20I%20Fall%20Apart.mp3',
          artist: '0',
          name: 'Wow'
        },
        { src: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/Post%20Malone%20-%20rockstar%20ft.%2021%20Savage%20(1).mp3',
          artist: '1',
          name: 'Disdain'
        },
        { src: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/Marshmello%20-%20Silence%20ft.%20Khalid.mp3',
          artist: '2',
          name: 'Base'
        },
        { src: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/VAX%20-%20Fireproof%20Feat%20Teddy%20Sky.mp3',
          artist: '3',
          name: 'Fireproof'
        },
        { src: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/NF%20-%20Let%20You%20Down.mp3',
          artist: '4',
          name: 'Im not the only one'
        }
      ]
      this.songsLength = this.songs.length
      for (let i = 0; i < this.songsLength; i++) {
        this.shuffleIndexes.push(i)
      }
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
    formatProgress (time) {
      return (time / this.audio.duration) * 100 + '%'
    },
    playSong () {
      this.isPlayed = false
      this.isPaused = true
      if (this.audio.paused) {
        this.audio.play()
        var that = this
        this.audio.onloadedmetadata = function () {
          that.setDuration()
        }
      }
    },
    playNext () {
      if (this.currentIndex === this.songsLength - 1) this.currentIndex = 0
      else this.currentIndex++
      this.preloadSong()
      this.playSong()
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
      if (current === this.audio.duration) {
        this.playNext()
      }
    },
    addListeners () {
      document.getElementById('progress-pin').addEventListener('mousedown', this.mouseDown, false)
      window.addEventListener('mouseup', this.mouseUp, false)
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
        var offset = x - timeline.offsetLeft
        this.progressWidth = offset + 'px'
        this.audio.currentTime = this.formatBackTime(offset / timeline.clientWidth)
      }
    },
    onTimeChange (e) {
      var x = e.clientX
      var timeline = document.getElementsByClassName('main-player-body')[0]
      var offset = x - timeline.offsetLeft
      this.progressWidth = offset + 'px'
      this.audio.currentTime = this.formatBackTime(offset / timeline.clientWidth)
    },
    preloadSong () {
      this.progressWidth = 0
      this.audio.src = this.songs[this.shuffleIndexes[this.currentIndex]]['src']
      this.title = this.songs[this.shuffleIndexes[this.currentIndex]]['name'] + ' - ' + this.songs[this.shuffleIndexes[this.currentIndex]]['artist']
      this.audio.preload = 'metadata'
      var that = this
      this.audio.onloadedmetadata = function () {
        that.setDuration()
      }
      this.audio.ontimeupdate = function () {
        that.updateTime()
      }
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
    },
    pauseSong () {
      this.isPlayed = true
      this.isPaused = false
      var audio = this.audio
      audio.pause()
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
    box-shadow: 0 -3px 15px 0px rgba(51, 51, 51, 0.1);
}
.main-player-body {
  width: 80%;
  position: absolute;
  margin: 0 auto;
  left: 0;
  right: 0;
}
.player-trackline {
    top: 15px;
    padding: 0;
    width: 100%;
    position: absolute;
    margin: 0 auto;
    left: 0;
    right: 0;
}
.timeline {
  position: relative;
  display: flex;
  align-items: center;
}
.player-settings {
  margin: 0 20px;
  position: absolute;
  top: 30px;
  right: 0;
}
.main-player-block .player-button-icon {
    background: none;
    border: none;
    border-radius: 50%;
    padding: 10px;
    font-size: 20px;
    color: #3a3654;
    cursor: pointer;
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
.player-trackline .pin {
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
.player-trackline .slider {
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
.player-trackline .slider:hover {
  height: 5px;
  border-top-right-radius: 5px;
  border-bottom-right-radius: 5px;
}
.player-trackline .slider:hover .progress {
  border-top-left-radius: 5px;
  border-bottom-left-radius: 5px;
}
.player-trackline .slider:hover .pin {
  height: 11px;
  width: 11px
}
.player-trackline .pin:active,
.player-trackline .pin:focus {
    -moz-transform: scale(1.5);
    -o-transform: scale(1.5);
    -ms-transform: scale(1.5);
    -webkit-transform: scale(1.5);
    transform: scale(1.5)
}
.shuffled {
  color: #3a3654 !important;
  background: #ebeaed;
}
.player-settings i {
  cursor: pointer;
  color: rgba(58, 54, 84, 0.56);
  padding: 6px;
  border-radius: 50%;
}
.player-settings i:hover {
  background: #ebeaed;
}
:focus {
  outline: none
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
    height: 100px;
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
