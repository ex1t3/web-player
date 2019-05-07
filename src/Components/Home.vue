<template>
  <div class="home-page-main-block">
    <div v-if="isPageLoaded && !isAllTopTracksOpened" class="home-page-content-block">
      <div v-if="lastPlayedSongs.length > 0">
        <h4>Your last played Songs</h4>
        <div class="home-songs-items-block last-played-songs">
          <div @click="playSong(item['Id'], 0)" class="home-songs-item" :key="index"
            v-for="(item, index) in lastPlayedSongs">
            <div class="home-songs-item--cover">
              <img v-bind:src="item.AlbumCover" class="last-played-cover-image">
            </div>
            <div class="home-songs-item--title">
              <div class="home-songs-item--name">{{ item.Name }}</div>
              <div class="home-songs-item--artist">{{ item.Artist }}</div>
              <div class="home-songs-item--album">{{ item.Album }}</div>
            </div>
          </div>
        </div>
      </div>
      <h4>Top-10 most popular over AudioWeb</h4>
      <div class="home-songs-items-block">
        <div @click="playSong(item['Id'], 1)" class="home-songs-item"
          :class="{'current-home-songs-item': $main.currentIndex===item['Id']}" :key="index"
          v-for="(item, index) in topListenedSongs.slice(0,10)">
          <div class="home-songs-item--cover">
            <img v-bind:src="item.AlbumCover" class="last-played-cover-image">
          </div>
          <div class="home-songs-item--title">
            <div class="home-songs-item--name">{{ item.Name }}</div>
            <div class="home-songs-item--artist">{{ item.Artist }}</div>
            <div class="home-songs-item--album">{{ item.Album }}</div>
          </div>
        </div>
      </div>
      <div @click="isAllTopTracksOpened = true" class="move-to-block">
        <div>View top 100 tracks</div>
        <i class="fas fa-arrow-right"></i>
      </div>
    </div>
    <div v-if="isAllTopTracksOpened">
      <div @click="isAllTopTracksOpened = false" class="return-block">
        <i class="fas fa-arrow-left"></i>
        <div>Back to Home page</div>
      </div>
      <SongsTemplate :type="3" :songs="topListenedSongs" />
    </div>
  </div>
</template>
<script>
import axios from 'axios'
import {
  mapGetters
} from 'vuex'
import store from '../store'
import SongsTemplate from './SongsTemplate'

// Exporting data for current template
export default {
  data() {
    return {
      isPageLoaded: false,
      isAllTopTracksOpened: false
    }
  },
  components: {
    SongsTemplate
  },
  computed: mapGetters({
    lastPlayedSongs: 'lastPlayedSongs',
    topListenedSongs: 'topListenedSongs'
  }),
  beforeMount () {
    this.$root.$emit('checkScreenWidth')
    this.$root.$emit('actLoadingRoot')
  },
  mounted () {
    this.checkForDataLoaded()
  },
  methods: {
    checkForDataLoaded () {
      setTimeout (() => {
        if (this.topListenedSongs.length > 0) {
          setTimeout(() => {
            this.$root.$emit('deactLoadingRoot')
            this.isPageLoaded = true
          }, 100)
        } else {
          this.checkForDataLoaded()
        }
      }, 200)
    },
    playSong (index, type) {
      switch (true) {
        case index === this.$main.currentIndex && !this.$main.isPaused:
          {
            this.$root.$emit('pauseSongRoot')
            return 0
          }
        case index === this.$main.currentIndex:
          {
            this.$root.$emit('playSongRoot')
            return 0
          }
        case index !== this.$main.currentIndex:
          {
            if (type === 0) this.$root.$emit('loadSongsRoot', this.lastPlayedSongs)
            else this.$root.$emit('loadSongsRoot', this.topListenedSongs)
            this.$main.isPaused = true
            this.$root.$emit('playDefinedSongRoot', index)
            break
          }
      }
    }
  }
}
</script>
<style scoped>
.home-page-main-block {
  width: 95%;
  margin: 0 auto;
}

.home-page-content-block {
  display: grid;
  animation: fade-in .5s;
}

.return-block, .move-to-block {
  color: #f39d93;
  display: inline-flex;
  align-items: center;
  cursor: pointer;
  font-size: 14px;
  margin-left: 10px;
}
.return-block {
  margin: 0px 0px 10px 0px;
}
.return-block i {
  margin-right: 10px;
  transition: margin 0.3s;
}
.return-block:hover i {
  margin-left: -5px;
  margin-right: 15px;
}
.move-to-block i {
  margin-left: 10px;
  transition: margin 0.3s;
}
.move-to-block:hover i {
   margin: 0px -5px 0px 15px;
}

.home-songs-items-block {
    display: block;
}

.home-songs-item:hover {
  background: #f7f7f8;
}

.current-home-songs-item {
  background: #ececef;
}

.home-songs-item:hover,
.current-home-songs-item {
  cursor: pointer;
  box-shadow: 8px 4px 20px #e4e4e473 !important;
}

.home-songs-item {
  float: left;
  position: relative;
  margin: 0px 25px 15px 0px;
  display: flex;
  align-items: center;
  font-size: 13px;
  border-radius: 3px;
  min-width: 150px;
  height: 70px;
  animation: fade-in .3s;
  padding: 10px 15px;
  box-shadow: 0 0 20px #dddddd5e;
}


.home-songs-item .home-songs-item--name,
.home-songs-item .home-songs-item--album,
.home-songs-item .home-songs-item--artist {
  width: 100%;
  padding: 3px 0;
}

.home-songs-item--artist {
  font-size: 10px;
}

.home-songs-item--album {
  font-size: 8px;
  color: #ccc !important;
  font-style: italic;
  font-weight: 100 !important;
}

.home-songs-item--cover {
  width: 60px;
  position: absolute;
  left: 10px;
}

.home-songs-item .last-played-cover-image {
  width: 100%;
}

.home-songs-item--title {
display: block;
    font-size: 14px;
    margin-left: 70px;
    width: calc(100% - 70px) !important;
}

.home-songs-item--name {
  font-size: 12px;
}

@media (max-width: 700px) {
  .home-songs-items-block {
    overflow-x: scroll;
    overflow-y: hidden;
    height: 100px;
    width: 100%;
    white-space: nowrap;
  }
  .home-songs-item {
    float: none;
    display: inline-block;
    min-width: 90px;
    height: 55px
  }
  .home-songs-item--cover {
      width: 40px;
      top: 17px;
  }
  .home-songs-item--title {
    margin-left: 45px;
    width: calc(100% - 45px) !important;
  }
  .home-page-main-block {
    display: block;
  }
  .home-page-content-block {
    display: block;
  }
}

@-webkit-keyframes fade-in {
  0% {
    opacity: 0;
  }

  100% {
    opacity: 1;
  }
}

@keyframes fade-in {
  0% {
    opacity: 0
  }

  100% {
    opacity: 1;
  }
}
</style>
