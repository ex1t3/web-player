<template>
<div class="search-block">
  <div class="search-panel">
    <div class="title-container">
      <h1 class="title">Type the song's name or artist...</h1>
    </div>
    <fieldset class="player-search-container" v-bind:class="{'is-type': isTyping}">
      <input
        v-on:keyup="searching()"
        type="text"
        ref="search"
        placeholder="Search..."
        class="player-search"
      >
      <div class="icons-container">
        <div class="icon-search"></div>
        <div @click="stopSearch()" class="icon-close">
          <div class="x-up"></div>
          <div class="x-down"></div>
        </div>
      </div>
    </fieldset>
  </div>
  <div  v-if="foundedSongs.length > 0 && !nothingWasFound" class="founded-item">
      <h2>Songs</h2>
      <div class="songs">
        <div class="songs-block">
          <SongsTemplate :songs="foundedSongs" :type="3" />
          </div>
      </div>
      <div>View all tracks</div>
    </div>
    <div v-if="foundedArtists.length > 0 && !nothingWasFound"  class="founded-item">
      <h2>Artists</h2>
        <div :key="index" v-for="(item, index) in foundedArtists"></div>
      <div>View all artists</div>
    </div>
    <div class="nothing-found-notification" v-if="nothingWasFound">Sorry, we couldn't found the song or artist you've typed</div>
  </div>
</template>
<script>
let typingTimer
import axios from "axios"
import SongsTemplate from './SongsTemplate'
export default {
  data() {
    return {
      searchVal: "",
      foundedSongs: [],
      foundedArtists: [],
      nothingWasFound: false,
      isTyping: false
    }
  },
  beforeMount() {
    this.$root.$emit("checkScreenWidth")
  },
  mounted() {
    this.$refs.search.focus()
  },
  methods: {
    searching(e) {
      this.isTyping = true
      this.searchVal = this.$refs.search.value
      clearTimeout(typingTimer)
      if (this.searchVal.length >= 3) {
        typingTimer = setTimeout(this.doneTyping, 1000)
      }
    },
    doneTyping() {
      let that = this
      axios({
        method: "POST",
        url: "https://localhost:44304/api/Songs/SearchForSong",
        data: JSON.stringify(that.searchVal),
        headers: {
          "Content-Type": "application/json charset=UTF-8",
          Authorization: "Bearer " + sessionStorage.getItem("access_token")
        }
      })
        .then(function(e) {
          that.foundedSongs = []
          if (e.data.Songs.length > 0) {
            that.foundedSongs = e.data.Songs
            that.nothingWasFound = false
          }
          that.foundedArtists = []
          if (e.data.Artists.length > 0) {
            that.foundedArtists = e.data.Artists
            that.nothingWasFound = false
          }
          if (that.foundedSongs.length == 0 && that.foundedArtists.length == 0) {
            that.nothingWasFound = true
          }
          that.isTyping = false
        })
        .catch(function(e) {
          console.log(e)
          that.$root.$emit("errorHandler", e.response.status)
          that.isTyping = false
        })
    },
    stopSearch() {
      clearTimeout(typingTimer)
      this.isTyping = false
      this.$refs.search.value = ""
    }
  },
  components: {
    SongsTemplate
  }
}
</script>
<style>
.nothing-found-notification {
  display: block;
  text-align: center;
  margin-top: 60px;
}
.founded-item {
  width: 90%;
  margin: 0 auto;
  padding: 10px;
}
@-webkit-keyframes spin {
  0% {
    -webkit-transform: rotate(0deg);
    transform: rotate(0deg);
  }
  100% {
    -webkit-transform: rotate(360deg);
    transform: rotate(360deg);
  }
}
@keyframes spin {
  0% {
    -webkit-transform: rotate(0deg);
    transform: rotate(0deg);
  }
  100% {
    -webkit-transform: rotate(360deg);
    transform: rotate(360deg);
  }
}
@-webkit-keyframes color-1 {
  0% {
    background-color: #f28579;
  }
  100% {
    background-color: #9b8be0;
  }
}
@keyframes color-1 {
  0% {
    background-color: #f28579;
  }
  100% {
    background-color: #9b8be0;
  }
}
::-moz-selection {
  background: #f39d93;
  color: white;
  text-shadow: none;
}
::selection {
  background: #f39d93;
  color: white;
  text-shadow: none;
}

::-webkit-selection {
  background: #f39d93;
  color: white;
  text-shadow: none;
}

.title-container {
  margin: 20px auto;
  text-align: center;
}
.title-container .title {
  margin: 0;
  color: #f39d93;
}
.is-type .title-container .title-down {
  -webkit-transform: translateY(-30px);
  transform: translateY(-30px);
}
.is-type .title-container .title {
  -webkit-transform: translateY(-100%);
  transform: translateY(-100%);
}

.player-search-container {
  position: relative;
  padding: 0;
  margin: 0 auto;
  border: 0;
  width: 60%;
  height: 40px;
}

.icons-container {
  position: absolute;
  top: 11px;
  right: -5px;
  width: 35px;
  height: 35px;
  overflow: hidden;
}

.icon-close {
  position: absolute;
  top: 2px;
  left: 2px;
  width: 75%;
  height: 75%;
  opacity: 0;
  cursor: pointer;
  -webkit-transform: translateX(-200%);
  transform: translateX(-200%);
  border-radius: 50%;
  transition: opacity 0.25s ease,
    -webkit-transform 0.43s cubic-bezier(0.694, 0.048, 0.335, 1);
  transition: opacity 0.25s ease,
    transform 0.43s cubic-bezier(0.694, 0.048, 0.335, 1);
  transition: opacity 0.25s ease,
    transform 0.43s cubic-bezier(0.694, 0.048, 0.335, 1),
    -webkit-transform 0.43s cubic-bezier(0.694, 0.048, 0.335, 1);
}
.icon-close:before {
  content: "";
  border-radius: 50%;
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  opacity: 0;
  border: 2px solid transparent;
  border-top-color: #f39d93;
  border-left-color: #f39d93;
  border-bottom-color: #f39d93;
  transition: opacity 0.2s ease;
}
.icon-close .x-up {
  position: relative;
  width: 100%;
  height: 50%;
}
.icon-close .x-up:before {
  content: "";
  position: absolute;
  bottom: 2px;
  left: 3px;
  width: 50%;
  height: 2px;
  background-color: #f39d93;
  -webkit-transform: rotate(45deg);
  transform: rotate(45deg);
}
.icon-close .x-up:after {
  content: "";
  position: absolute;
  bottom: 2px;
  right: 0px;
  width: 50%;
  height: 2px;
  background-color: #f39d93;
  -webkit-transform: rotate(-45deg);
  transform: rotate(-45deg);
}
.icon-close .x-down {
  position: relative;
  width: 100%;
  height: 50%;
}
.icon-close .x-down:before {
  content: "";
  position: absolute;
  top: 5px;
  left: 4px;
  width: 50%;
  height: 2px;
  background-color: #f39d93;
  -webkit-transform: rotate(-45deg);
  transform: rotate(-45deg);
}
.icon-close .x-down:after {
  content: "";
  position: absolute;
  top: 5px;
  right: 1px;
  width: 50%;
  height: 2px;
  background-color: #f39d93;
  -webkit-transform: rotate(45deg);
  transform: rotate(45deg);
}
.is-type .icon-close:before {
  opacity: 1;
  -webkit-animation: spin 0.85s infinite;
  animation: spin 0.85s infinite;
}
.is-type .icon-close .x-up:before,
.is-type .icon-close .x-up:after {
  -webkit-animation: color-1 0.85s infinite;
  animation: color-1 0.85s infinite;
}
.is-type .icon-close .x-up:after {
  -webkit-animation-delay: 0.3s;
  animation-delay: 0.3s;
}
.is-type .icon-close .x-down:before,
.is-type .icon-close .x-down:after {
  -webkit-animation: color-1 0.85s infinite;
  animation: color-1 0.85s infinite;
}
.is-type .icon-close .x-down:before {
  -webkit-animation-delay: 0.2s;
  animation-delay: 0.2s;
}
.is-type .icon-close .x-down:after {
  -webkit-animation-delay: 0.1s;
  animation-delay: 0.1s;
}

.icon-search {
  position: relative;
  top: 5px;
  left: 8px;
  width: 50%;
  height: 50%;
  opacity: 1;
  border-radius: 50%;
  border: 3px solid #f9c8c3;
  transition: opacity 0.25s ease,
    -webkit-transform 0.43s cubic-bezier(0.694, 0.048, 0.335, 1);
  transition: opacity 0.25s ease,
    transform 0.43s cubic-bezier(0.694, 0.048, 0.335, 1);
  transition: opacity 0.25s ease,
    transform 0.43s cubic-bezier(0.694, 0.048, 0.335, 1),
    -webkit-transform 0.43s cubic-bezier(0.694, 0.048, 0.335, 1);
}
.icon-search:after {
  content: "";
  position: absolute;
  bottom: -9px;
  right: -2px;
  width: 4px;
  border-radius: 3px;
  -webkit-transform: rotate(-45deg);
  transform: rotate(-45deg);
  height: 10px;
  background-color: #f9c8c3;
}

.player-search {
  border: 0;
  width: 100%;
  height: 100%;
  padding: 10px 20px;
  background: white;
  border-radius: 3px;
  box-shadow: 0px 8px 15px rgba(75, 72, 72, 0.1);
  transition: all 0.4s ease;
  margin-left: -20px;
  position: relative;
  font-family: "Niramit", sans-serif;
}
.player-search:focus {
  outline: none;
  box-shadow: 0px 9px 20px rgba(75, 72, 72, 0.3);
}
.player-search:focus + .icons-container .icon-close {
  opacity: 1;
  -webkit-transform: translateX(0);
  transform: translateX(0);
}
.player-search:focus + .icons-container .icon-search {
  opacity: 0;
  -webkit-transform: translateX(200%);
  transform: translateX(200%);
}

.search-panel {
  padding-left: 20px;
  padding-top: 20px;
}
@media (max-width: 700px) {
  .search-panel .title {
    font-size: 16px;
  }
}
</style>
