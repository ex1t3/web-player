<template>
  <div class="home-page-main-block">
    <h4>Your last played Songs</h4>
    <div class="last-played-items-block">
      <div @click="playSong(item['Id'])" class="last-played-item" :class="{'current-last-played-item': $main.currentIndex===item['Id']}"
        :key="index" v-for="(item, index) in lastPlayedSongs">
        <div class="last-played-item--cover">
          <img v-bind:src="item.AlbumCover" class="last-played-cover-image">
        </div>
        <div class="last-played-item--title">
          <div class="last-played-item--name">{{ item.Name }}</div>
          <div class="last-played-item--artist">{{ item.Artist }}</div>
          <div class="last-played-item--album">{{ item.Album }}</div>
        </div>
      </div>
    </div>
  </div>
  </div>
</template>
<script>
import axios from 'axios'
import {mapGetters} from 'vuex'
import store from '../store'

// Exporting data for current template
export default {
  data() {
    return {
      styles: {
        background: 'none',
      }
    }
  },
  computed: mapGetters({
    lastPlayedSongs: 'lastPlayedSongs'
  }),
  beforeMount() {
    this.$root.$emit("checkScreenWidth")   
  },
  methods: {
    playSong(index) {
      switch (true) {
        case index === this.$main.currentIndex && !this.$main.isPaused:
          {
            this.$root.$emit("pauseSongRoot")
            return 0
          }
        case index === this.$main.currentIndex:
          {
            this.$root.$emit("playSongRoot")
            return 0
          }
        case index !== this.$main.currentIndex:
          {
            this.$root.$emit("loadSongsRoot", this.lastPlayedSongs)
            this.$main.isPaused = true
            this.$root.$emit("playDefinedSongRoot", index)
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

.last-played-items-block {
    display: block;
}

.last-played-item:hover {
  background: #f7f7f8;
}

.current-last-played-item {
  background: #ececef;
}

.last-played-item:hover,
.current-last-played-item {
  cursor: pointer;
  box-shadow: 8px 4px 20px #e4e4e473 !important;
}

.last-played-item {
  float: left;
  position: relative;
  margin: 0px 25px 15px 0px;
  display: flex;
  align-items: center;
  font-size: 13px;
  min-width: 150px;
  height: 70px;
  padding: 10px 15px;
  animation: fade-in .5s;
  box-shadow: 0 0 20px #dddddd5e;
}

.last-played-item .last-played-item--name,
.last-played-item .last-played-item--album,
.last-played-item .last-played-item--artist {
  width: 100%;
  padding: 3px 0;
}

.last-played-item--artist {
  font-size: 10px;
}

.last-played-item--album {
  font-size: 8px;
  color: #ccc !important;
  font-style: italic;
  font-weight: 100 !important;
}

.last-played-item--cover {
  width: 60px;
  position: absolute;
  left: 10px;
}

.last-played-item .last-played-cover-image {
  width: 100%;
}

.last-played-item--title {
display: block;
    font-size: 14px;
    margin-left: 70px;
    width: calc(100% - 70px) !important;
}

.last-played-item--name {
  font-size: 12px;
}

@media (max-width: 700px) {
  .last-played-items-block {
    overflow-x: scroll;
    overflow-y: hidden;
    height: 100px;
    width: 100%;
    white-space: nowrap;
  }
  .last-played-item {
    float: none;
    display: inline-block;
    min-width: 90px;
    height: 55px
  }
  .last-played-item--cover {
      width: 40px;
      top: 17px;
  }
  .last-played-item--title {
    margin-left: 45px;
    width: calc(100% - 45px) !important;
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
