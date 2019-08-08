<template>
  <div class="songs-body">
    <div v-if="!$main.isMobile" class="songs-header">
      <div class="songs-header-number">#</div>
      <div class="songs-header-name">Name</div>
      <div class="songs-header-artist">Artist</div>
      <div class="songs-header-album">Album</div>
      <div v-if="type < 3" class="songs-header-sorting">Sort
        <div class="menu-layer"></div>
        <ul class="sorting-menu">
          <li @click="changeSortType(0)" :class="{'current-sort-type': currentSortType === 0}" class="sorting-menu-item"><i class="fas fa-check-double"></i>Default</li>
          <li @click="changeSortType(1)" :class="{'current-sort-type': currentSortType === 1}" class="sorting-menu-item"><i class="fas fa-check-double"></i>A-Z</li>
          <li @click="changeSortType(2)" :class="{'current-sort-type': currentSortType === 2}" class="sorting-menu-item"><i class="fas fa-check-double"></i>Z-A</li>
          <li @click="changeSortType(3)" :class="{'current-sort-type': currentSortType === 3}" class="sorting-menu-item"><i class="fas fa-check-double"></i>Last added</li>
        </ul>
      </div>
    </div>
    <div
      @click="playSong(item['Id'])"
      v-bind:class="{'current-song': $main.currentIndex===item['Id']}"
      class="songs-item"
      :key="index"
      v-for="(item, index) in slicedArrayOfSongs "
    >
      <div v-if="$main.isMobile" class="song-cover">
        <img v-bind:src="item.AlbumCover" class="song-cover-image">
      </div>
      <div v-if="!$main.isMobile" class="song-number">
        <div class="song-number-hidden">{{ index + 1}}</div>
        <button class="player-button-icon">
          <i
            class="fas"
            v-bind:class="{'fa-pause': $main.currentIndex === item['Id'] && !$main.isPaused, 'fa-play' : $main.isPaused || ($main.currentIndex!==item['Id'] && !$main.isPaused)}"
          ></i>
        </button>
      </div>
      <div class="song-name">{{ item.Name }}</div>
      <div class="song-artist">{{ item.Artist }}</div>
      <div class="song-album">{{ item.Album }}</div>
      <div class="mobile-song-title" v-if="$main.isMobile">
        <div class="mobile-song-name">{{ item.Name }}</div>
        <div class="mobile-song-artist">{{ item.Artist }}</div>
        <div class="mobile-song-album">{{ item.Album }}</div>
      </div>
      <div v-if="type < 3" class="song-remove-icon">
        <i @click="removeSong($event, index, item.Id, type)" class="far fa-trash-alt"></i>
      </div>
    </div>
  </div>
</template>
<script>
import axios from 'axios'
import { mapGetters } from 'vuex'
import store from '../store'

// Exporting data for current template
export default {
  props: ['songs', // Array of songs
    'type', // Songs template type
    'playlistId' // Current playlist ID 
  ],
  data() {
    return {
      slicedArrayOfSongs: [],
      countOfScrollTimes: 1,
      defaultSortedSongs: [],
      workingSongs: [],
      defaultCountOfItemsToLoad: 10
    }
  },
  computed: mapGetters({
    currentSortType: 'currentSortType'
  }),
  mounted() {
    this.workingSongs = this.songs.slice()
    this.defaultSortedSongs = this.workingSongs.slice()
    document.getElementsByClassName('page')[0].scrollTop = 0
    let isSorted = this.sortSongs()
    this.sliceFunction(this.defaultCountOfItemsToLoad)
    this.addListeners()
  },
  watch: {
    songs: function () {
      this.workingSongs = this.songs.slice()
      this.defaultSortedSongs = this.workingSongs.slice()
      let isSorted = this.sortSongs()
      this.sliceFunction(this.defaultCountOfItemsToLoad * this.countOfScrollTimes)
    },
    type: function () {
      this.countOfScrollTimes = 1
    }
  },
  methods: {
    addListeners() {
      let that = this
      if (this.workingSongs.length > 0) {
        window.onscroll = function() {
          that.scrollFunction()  
        }
      }
    },
    scrollFunction() {
      let scrollElement = document.getElementsByClassName('page')[0]
      let scrollHeight = scrollElement.scrollHeight
      let scrollPosition = window.innerHeight + window.scrollY
      if (scrollPosition === scrollHeight) {
        this.countTimesFunction()
      }
    },
    countTimesFunction() {
      this.countOfScrollTimes++
      let totalItemsToLoad = this.defaultCountOfItemsToLoad * this.countOfScrollTimes
      this.sliceFunction(totalItemsToLoad)
    },
    changeSortType (sortType) {
      if (sortType === this.currentSortType) return 0
      this.$store.dispatch('updateCurrentSortType', sortType)
      this.sortSongs()
      this.sliceFunction(this.defaultCountOfItemsToLoad * this.countOfScrollTimes)
    },
    sortSongs () {
      if (this.type >= 3) return
      switch (this.currentSortType) {
        // Default
        case 0: {
          this.workingSongs = this.defaultSortedSongs.slice()
          break
        }
        // Reverse of default **LastAdded**
        case 3: {
          this.workingSongs.reverse()
          break
        }
        // A - Z
        case 1: {
          this.workingSongs.sort(function (a, b) {
            var x = a.Name.toLowerCase()
            var y = b.Name.toLowerCase()
            return x < y ? -1 : x > y ? 1 : 0;
          })
          break
        }
        // Z - A
        case 2: {
          this.workingSongs.sort(function (a, b) {
            var x = a.Name.toLowerCase()
            var y = b.Name.toLowerCase()
            return x > y ? -1 : x < y ? 1 : 0;
          })
          break
        }
      
        default:
          break
      }
      return true
    },
    sliceFunction (count) {
      this.slicedArrayOfSongs = this.workingSongs.slice(0, count)
    },
    playSong (index) {
      switch (true) {
        case index === this.$main.currentIndex && !this.$main.isPaused: {
          this.$root.$emit('pauseSongRoot')
          return 0
        }
        case index === this.$main.currentIndex: {
          this.$root.$emit('playSongRoot')
          return 0
        }
        case index !== this.$main.currentIndex: {
          this.$root.$emit('loadSongsRoot', this.songs)
          this.$main.isPaused = true
          this.$root.$emit('playDefinedSongRoot', index)
          break
        } 
      }
    },
    removeSong(e, index, songId, type) {
      // Removes song from a chosen data array
      e.stopPropagation()
      let instance = JSON.stringify({SongId: songId, Type: type, PlaylistId: this.playlistId})
      let that = this
      axios({
        method: 'POST',
        url: 'https://localhost:44343/api/Songs/RemoveSongFromInstance',
        data: instance,
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          Authorization: 'Bearer ' + localStorage.getItem('access_token')
        }
      }).then(function (e) {
        that.removedSongHandler(type, index)
      }).catch(function (e) {
        console.log(e)
        that.$root.$emit('errorHandler', e)
      })
    },
    removedSongHandler(type, index) {
      switch (type) {
        // 0 - Uploaded songs
        case 0: {
          this.songs.splice(index, 1)
          this.sliceFunction(this.defaultCountOfItemsToLoad * this.countOfScrollTimes)
          this.$root.$emit('notificate', 'success', 'Song deleted from uploaded', 3000)
          return true
        }
        // 1 - Playlist songs
        case 1: {
          this.songs.splice(index, 1)
          this.sliceFunction(this.defaultCountOfItemsToLoad * this.countOfScrollTimes)
          this.$root.$emit('notificate', 'success', 'Song deleted from playlist', 3000)  
          return true
        }
        // 2 - Favorite songs
        case 2: {
          this.$main.favoriteSongs.splice(index, 1)
          this.$root.$emit('notificate', 'success', 'Song deleted from favorites', 3000)       
          return true
        }
        default: {
          return false
        }
      }
      this.sliceFunction(this.defaultCountOfItemsToLoad * this.countOfScrollTimes)
    }
  }
}
</script>
<style scoped>
.sorting-menu {
  display: none !important;
  animation: fade-in .5s;
  position: absolute;
  z-index: 2;
  background: #fff;
  width: 120px;
  margin-top: 20px;
  right: 0;
  display: block;
  box-shadow: 0 0 20px 1px #ddddddb8;
}

.sorting-menu-item {
  list-style: none;
  width: calc(100% - 10px);
  text-align: left;
  padding: 10px 0px 10px 10px;
  border-bottom: 1px solid rgb(249, 249, 249);
  display: flex;
  align-items: center;
}

.current-sort-type {
  padding-left: 25px;
  width: calc(100% - 25px);
}

.sorting-menu-item i {
  display: none;
}

.current-sort-type i {
  display: initial;
  position: absolute;
  left: 7px;
}

.menu-layer {
  display: block;
  position: absolute;
  background: transparent;
  width: 100% !important;
  height: 10px;
  left: 0;
  bottom: -10px;
}

.songs-header-sorting:hover .sorting-menu {
  display: block !important;
}

.song-remove-icon {
  font-size: 18px;
  position: absolute;
  right: 0;
  margin-right: 15px !important;
  color: #929daf !important;
  width: auto !important;
}

.song-remove-icon i {
  float: right;
}

.song-number,
.songs-header-number {
  width: 50px !important;
}

.songs-item .player-button-icon,
.songs-item .player-button-icon i {
  background: none;
  padding: 0;
}

.songs-item:hover {
  cursor: pointer;
  background: #f7f7f8;
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
  font-size: 14px;
  align-items: center;
  position: relative;
}

.songs-item {
  display: flex;
  align-items: center;
  font-size: 13px;
  height: 50px;
  position: relative;
  padding: 10px 10px;
  margin: 15px 0;
  box-shadow: 0 0 20px #dddddd5e;
}

.songs-item::after {
  content: "";
  height: 1px;
  background: #f4f4f4;
  top: 0px;
  width: calc(100% - 12px);
  position: absolute;
  left: 6px;
  right: 6px;
}

.songs-header-sorting {
  position: absolute;
  right: 0;
  text-align: center;
  padding: 10px;
  border-bottom: 1px solid;
  max-width: 30px;
  cursor: pointer;
  font-size: 12px;
  margin-top: -5px;
}

.songs-block {
  width: 100%;
}

.songs-header,
.songs-item {
  width: 100%;
}

.songs-header div,
.songs-item div {
  width: 30%;
}

@media (max-width: 700px) {

  .song-name,
  .song-album,
  .song-artist {
    display: none;
  }

  .playlist-poster img {
    width: 80%;
  }

  .songs-item {
    padding: 25px 0;
    margin: 15px 0;
    height: 35px;
  }

  .songs-item .mobile-song-name,
  .songs-item .mobile-song-album,
  .songs-item .mobile-song-artist {
    width: 100%;
    padding: 3px 0;
  }

  .mobile-song-artist {
    font-size: 10px;
  }

  .mobile-song-album {
    font-size: 8px;
    color: #ccc !important;
    font-style: italic;
    font-weight: 100 !important;
  }

  .songs-item .song-cover {
    width: 60px;
    position: absolute;
    left: 15px;
  }

  .songs-item .song-cover .song-cover-image {
    width: 100%;
  }

  .mobile-song-title {
    display: block;
    font-size: 14px;
    margin-left: 90px;
    width: calc(100% - 90px) !important;
  }

  .mobile-song-name {
    font-size: 12px;
  }
}
</style>
