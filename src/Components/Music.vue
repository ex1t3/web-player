<template>
  <div class="tabs">
    <input type="radio" id="tab1" name="tab-control" checked>
    <input type="radio" id="tab2" name="tab-control">
    <MusicsPageHeader :currentPlaylistName="currentPlaylist" :isFavoriteTracksPageOpened="isFavorites" :isUploadsOpened="isUploads" :isPlaylistOpened="isPlaylist"/>   
    <div class="content">
      <Playlists :pageType="currentPageType" :isUploadsOpened="isUploads" :isPlaylistOpened="isPlaylist"/>
      <Favorites :pageType="currentPageType" />
    </div>
  </div>
</template>
<script>
import {mapGetters} from 'vuex'
import Playlists from './Playlists'
import MusicsPageHeader from './MusicsPageHeader'
import Favorites from './Favorites'
import universalParse from "id3-parser/lib/universal"

// Exporting data for current template
export default {
  data() {
    return {
      isUploads: false,
      isPlaylist: false,
      isFavorites: false,
      currentPageType: -1,
      currentPlaylist: 'My Playlists'
    }
  },
  components: {
    MusicsPageHeader,
    Playlists,
    Favorites
  },
  mounted() {
    window.addEventListener("resize", this.checkScreenWidth)
    this.$root.$emit('deactLoadingRoot')
  },
  beforeMount() {
    this.$root.$emit('actLoadingRoot')
    this.checkScreenWidth()
    this.$root.$on("openInstance", this.openInstance)
  },
  beforeDestroy() {
    this.$root.$off("addSongToPlaylist", this.addSongToPlaylist)
    this.$root.$off("openInstance", this.openInstance)
    window.removeEventListener("resize", this.checkScreenWidth)
  },
  methods: {
    checkScreenWidth() {
      this.$root.$emit('checkScreenWidth')
    },
    openInstance(type, name) {
      switch (type) {
        // Uploads page
        case 0: {
          this.isUploads = true
          this.isFavorites = false
          this.isPlaylist = false
          this.currentPageType = 0
          this.currentPlaylist = name
        }
        break
        // Playlist page
        case 1: {
          this.isUploads = false
          this.isFavorites = false
          this.isPlaylist = true
          this.currentPageType = 1
          this.currentPlaylist = name
        }
        break
        // Favorites page
        case 2: {
          this.currentPageType = 2
          this.isFavorites = true
        }
        break
        // Exit playlists
        case 3: {
          this.isUploads = false
          this.isFavorites = false
          this.isPlaylist = false
          this.currentPageType = -1
          this.currentPlaylist = 'My Playlists'
        }
        // Back to playlists || uploads
        default:
        {
          this.currentPageType = this.currentPlaylist === 'My Uploads' ? 0 : 1
          this.isFavorites = false
        }
          break
      }
    }
  }
}
</script>
