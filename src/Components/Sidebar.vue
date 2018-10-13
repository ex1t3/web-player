<template>
<div class="absolute-items">
  <div class="sidebar">
    <div v-bind:class="{ 'burger-active': isActiveSidebar }" v-on:click="toggleSidebar()" class="sidebar-burger">
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
    <div class="player-buttons">
      <i v-on:click="playPrev()" class="fas fa-backward"></i>
      <i v-on:click="playSong()" v-bind:class="{hidden : isPaused}" class="fas fa-play"></i>
      <i v-on:click="pauseSong()" v-bind:class="{hidden : isPlayed}" class="fas fa-pause"></i>
      <i v-on:click="playNext()" class="fas fa-forward"></i></div>
    <div class="player-trackline">
      <div class="timeline">
      <span class="current-time">{{ currentTime }}</span>
      <span class="total-time">{{ duration }}</span>
      <div v-on:click="onTimeChange($event)" class="slider">
       <div class="progress" v-bind:style="{ width: progressWidth }">
         <div @mousedown="onMouseDown()" @mousemove="onMoveTimeChange($event)" class="pin" id="progress-pin" v-bind:style="{ 'margin-left': progressWidth }"></div>
      </div>
      </div>
     </div>
     </div>
    <div class="player-settings">dgdgdgdg</div>
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
      isPlayed: true,
      isPaused: false,
      isDragged: false,
      currentIndex: 0,
      audio: new Audio()
    }
  },
  mounted () {
    this.loadSongs()
    this.preloadSong()
    window.addEventListener('mouseup', this.onMouseUp)
  },
  methods: {
    loadSongs () {
      this.songs = [
        { name: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/Post%20Malone%20-%20I%20Fall%20Apart.mp3',
          artist: 'Black'
        },
        { name: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/Post%20Malone%20-%20rockstar%20ft.%2021%20Savage%20(1).mp3',
          artist: 'Black'
        }
      ]
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
      if (this.currentIndex === this.songs.length - 1) this.currentIndex = 0
      else this.currentIndex++
      this.preloadSong()
      this.playSong()
    },
    playPrev () {
      console.log(this.songs.length)
      if (this.currentIndex === 0) this.currentIndex = this.songs.length - 1
      else this.currentIndex--
      this.preloadSong()
      this.playSong()
    },
    updateTime () {
      var current = this.audio.currentTime
      this.progressWidth = this.formatProgress(current)
      this.currentTime = this.formatTime(current)
      if (current === this.audio.duration) {
        this.pauseSong()
      }
    },
    onMouseDown () {
      this.isDragged = true
    },
    onMouseUp () {
      this.isDragged = false
    },
    onMoveTimeChange (e) {
      if (this.isDragged) {
        var x = e.clientX
        console.log(x)
        this.progressWidth = x / 200  + '%'
      }
    },
    onTimeChange (e) {
      var x = e.clientX
      var offset = e.target.parentElement.offsetLeft === 0 ? e.target.parentElement.parentElement.offsetLeft : e.target.parentElement.offsetLeft
      offset = x - offset
      this.progressWidth = offset + 'px'
      this.audio.currentTime = this.formatBackTime(offset / e.target.parentElement.clientWidth) 
    },
    preloadSong () {
      this.audio.src = this.songs[this.currentIndex]['name']
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
@media (max-width: 700px) {
  .sidebar-active .content-wrapper {
    left: -220px;
    background: linear-gradient(125deg, white, #625abb73);
    filter: blur(10px);
  }
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
  color: #232121;
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
  font-family: Nova Round,sans-serif;
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
    height: 80px;
    width: 100%;
    left: 0;
    right: 0;
    background: #fff;
    z-index: 999999;
    box-shadow: 0 -3px 15px 0px rgba(51, 51, 51, 0.1);
}
.player-trackline {
    padding: 0 40px;
    width: 80%;
    max-width: 800px;
    margin: 0 auto;
}
.timeline {
  position: relative;
  display: flex;
  align-items: center;
}
.player-settings {
  padding-right: 20px;
}
.player-buttons {
    width: 160px;
    padding: 0 20px;
    -webkit-align-items: center;
    align-items: center;
    display: -webkit-flex;
    display: -ms-flexbox;
    display: flex;
}
.player-buttons i {
  font-size: 20px;
  margin: 0 20px;
  cursor: pointer;
  color: rgb(58, 54, 84)
}
.player-buttons i:nth-child(2),
.player-buttons i:nth-child(3) {
  font-size: 30px;
}
.player-buttons i:first-child {
  margin-right: 20px;
  margin-left: 15px;
}
.current-time {
    left: 0;
}
.current-time, .total-time {
    position: relative;
    top: -10px;
    font-size: 10px;
    cursor: initial;
}
.total-time {
    margin-left: calc(100% - 41px);
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
  height: 4px;
  border-radius: 5px
}
.player-trackline .slider:hover .pin {
  height: 10px;
  width: 10px
}
.player-trackline .pin:active,
.player-trackline .pin:focus {
    -moz-transform: scale(1.5);
    -o-transform: scale(1.5);
    -ms-transform: scale(1.5);
    -webkit-transform: scale(1.5);
    transform: scale(1.5)
}
:focus {
  outline: none
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
