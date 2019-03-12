<template>
  <div class="songs-body">
    <div v-if="!$main.isMobile" class="songs-header">
      <div class="songs-header-number">#</div>
      <div class="songs-header-name">Name</div>
      <div class="songs-header-artist">Artist</div>
      <div class="songs-header-album">Album</div>
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
            v-bind:class="{'fa-pause': $main.currentIndex===item['Id'] && !$main.isPaused, 'fa-play' : $main.isPaused || ($main.currentIndex!==item['Id'] && !$main.isPaused)}"
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
      <div v-if="type != 3" class="song-remove-icon">
        <i @click="removeSong($event, index, item.Id, type)" class="far fa-trash-alt"></i>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  props: ["songs", "type"],
  data() {
    return {
      favoriteSongsLoaded: false,
      slicedArrayOfSongs: [],
      countOfScrollTimes: 1,
      defaultCountOfItemsToLoad: 10
    }
  },
  mounted() {
    this.sliceFunction(this.defaultCountOfItemsToLoad)
    this.addListeners()
  },
  watch: {
    songs: function () {
      this.sliceFunction(this.defaultCountOfItemsToLoad * this.countOfScrollTimes)
    },
    type: function () {
      this.countOfScrollTimes = 1
    }
  },
  methods: {
    addListeners() {
      let that = this
      if (this.songs.length > 0) {
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
    sliceFunction(count) {
      this.slicedArrayOfSongs = this.songs.slice(0, count)
    },
    playSong(index) {
      switch (true) {
        case index === this.$main.currentIndex && !this.$main.isPaused: {
          this.$root.$emit("pauseSongRoot")
          return 0
        }
        case index === this.$main.currentIndex: {
          this.$root.$emit("playSongRoot")
          return 0
        }
        case index !== this.$main.currentIndex:
          {
            switch (this.type) {
              // Favorite songs
              case 2: {
                if (!this.favoriteSongsLoaded) {
                  this.$root.$emit("loadSongsRoot", this.$main.favoriteSongs)
                  this.favoriteSongsLoaded = true
                }
                break
              }
              // Playlist, Uploaded & Founded songs
              case 0: 
              case 1: 
              case 3: {
                this.$root.$emit("loadSongsRoot", this.songs)
                break
              }
            }
          }
          this.$main.isPaused = true
          this.$root.$emit("playDefinedSongRoot", index)
      }
    },
    removeSong(e, index, id, type) {
      // Removes song from a chosen data array
      e.stopPropagation()
      switch (type) {
        // 0 - Uploaded & Playlist songs
        case 0: 
        case 1: {
          this.songs.splice(index, 1)
          this.sliceFunction(this.defaultCountOfItemsToLoad * this.countOfScrollTimes)
          break
        }
        // 2 - Favorite songs
        case 2: {
          this.$main.favoriteSongs.splice(index, 1)
          break
        }
        default: {
          break
        }
      }
    }
  }
}
</script>
<style scoped>
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
  align-items: center;
}
.songs-item {
  display: flex;
  align-items: center;
  font-size: 14px;
  height: 50px;
  position: relative;
  padding: 10px 10px;
  margin: 25px 0;
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
.songs-block {
  width: 100%;
}
.songs-header,
.songs-item {
  width: 100%;
}
.songs-header div,
.songs-item div {
  width: 20%;
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
