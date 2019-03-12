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
  },
  beforeMount() {
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
          this.isFavorites = false
        }
          break
      }
    }
  }
}
</script>
<style>
.uploader {
  text-align: center;
  margin-bottom: 20px;
}
.empty-box {
  width: 80px;
}
.empty-uploads-archive-block {
    padding: 15px;
    color: #a2a2a2;
    text-align: center;
}

@media only screen and (min-width: 40em) {
  .modal-overlay {
    display: flex;
    align-items: center;
    justify-content: center;
    position: fixed;
    top: -100px;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 5;
    background-color: rgba(0, 0, 0, 0.6);
    opacity: 0;
    visibility: hidden;
    -webkit-backface-visibility: hidden;
    backface-visibility: hidden;
    transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1),
      visibility 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  }
  .modal-overlay.active {
    opacity: 1;
    visibility: visible;
  }
}
.file-uploader {
  position: absolute;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  opacity: 0;
  border-radius: 20px;
  cursor: pointer;
}
/**
 * Modal
 */
.modal {
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  margin: 0 auto;
  background-color: #fff;
  width: 600px;
  max-width: 75rem;
  min-height: 20rem;
  padding: 1rem;
  border-radius: 3px;
  opacity: 0;
  overflow-y: auto;
  visibility: hidden;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  -webkit-backface-visibility: hidden;
  backface-visibility: hidden;
  -webkit-transform: scale(1.2);
  transform: scale(1.2);
  transition: all 0.6s cubic-bezier(0.55, 0, 0.1, 1);
}
.return-block {
  position: absolute;
  top: 5px;
  color: #f39d93;
  display: inline-flex;
  align-items: center;
  cursor: pointer;
  padding-left: 10px;
  animation: fade-in 0.5s;
}
.return-block i {
  margin-right: 10px;
  transition: margin 0.3s;
}
.return-block:hover i {
  margin-left: -5px;
  margin-right: 15px;
}
.modal .close-modal {
  position: absolute;
  cursor: pointer;
  top: 5px;
  right: 15px;
  opacity: 0;
  -webkit-backface-visibility: hidden;
  backface-visibility: hidden;
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1),
    -webkit-transform 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1),
    transform 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1),
    transform 0.6s cubic-bezier(0.55, 0, 0.1, 1),
    -webkit-transform 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition-delay: 0.3s;
}
.modal .close-modal svg {
  width: 1.75em;
  height: 1.75em;
}
.modal .modal-content {
  opacity: 0;
  -webkit-backface-visibility: hidden;
  backface-visibility: hidden;
  transition: opacity 0.6s cubic-bezier(0.55, 0, 0.1, 1);
  transition-delay: 0.3s;
}
.modal.active {
  visibility: visible;
  opacity: 1;
  -webkit-transform: scale(1);
  transform: scale(1);
}
.modal.active .modal-content {
  opacity: 1;
}
.modal.active .close-modal {
  -webkit-transform: translateY(10px);
  transform: translateY(10px);
  opacity: 1;
}
.modal-content h2 {
  padding: 10px 0;
}
.modal-content .input-group {
  padding: 10px 0;
  width: 100%;
}
/**
 * Mobile styling
 */
@media only screen and (max-width: 39.9375em) {
  h1 {
    font-size: 1.5rem;
  }

  .modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    -webkit-overflow-scrolling: touch;
    border-radius: 0;
    -webkit-transform: scale(1.1);
    transform: scale(1.1);
    padding: 0 !important;
    z-index: 999;
  }

  .close-modal {
    right: 20px !important;
    top: 80px !important;
  }
}

.playlist-block {
  width: 200px;
  text-align: center;
  height: 200px;
  display: flex;
  margin-top: 60px;
  margin-left: 40px;
  margin-bottom: 20px;
  cursor: pointer;
  position: relative;
  transition: 0.3s;
  align-items: center;
  float: left;
  box-shadow: 0px 5px 20px #d2d2d24a;
  animation: fade-in 0.5s;
}
.playlist-block:hover {
  transform: translateY(-5px);
}
.playlist-content {
  width: 100%;
  text-align: left;
  padding: 15px 0;
  z-index: 2;
  position: absolute;
  top: -45px;
  background: #f39d93;
}
.playlist-content .playlist-title {
  margin-block-start: 0em;
  margin-block-end: 0em;
  padding-left: 10px;
}
.playlist-poster {
  position: absolute;
  width: 80%;
  height: 80%;
  left: 50%;
  transform: translateX(-50%);
}
.playlist-poster img {
  width: 100%;
  border-radius: 50%;
}
.create-playlist,
.uploads-playlist {
  transform: none !important;
}
.create-playlist .playlist-content,
.uploads-playlist .playlist-content {
  background: #f6f5f5;
  transition: 0.3s;
}
.create-playlist .playlist-poster,
.uploads-playlist .playlist-poster {
  width: 50%;
  height: 50%;
  transition: 0.3s;
}
.create-playlist:hover .playlist-poster,
.uploads-playlist:hover .playlist-poster {
  width: 60%;
  height: 60%;
}
.create-playlist:hover .playlist-content,
.uploads-playlist:hover .playlist-content {
  background: #f39d93;
}
.tabs {
  left: 50%;
  /* -webkit-transform: translateX(-50%); */
  /* transform: translateX(-50%); */
  /* position: relative; */
  background: white;
  padding-top: 20px;
  padding-bottom: 80px;
  width: 90%;
  height: auto;
  min-width: 240px;
  margin-left: auto;
  margin-right: auto;
}
.tabs input[name="tab-control"] {
  display: none;
}
.tabs .content section h2,
.tabs ul li label {
  font-family: "Niramit", sans-serif;
  font-weight: bold;
  font-size: 18px;
  color: #f39d93;
}
.tabs ul {
  list-style-type: none;
  padding-left: 0;
  display: flex;
  flex-direction: row;
  margin-bottom: 10px;
  justify-content: space-between;
  align-items: flex-end;
  flex-wrap: wrap;
  margin-top: 20px;
}
.tabs ul li {
  box-sizing: border-box;
  flex: 1;
  width: 50%;
  padding: 0 10px;
  text-align: center;
}
.tabs ul li label {
  transition: all 0.3s ease-in-out;
  color: #929daf;
  padding: 5px auto;
  overflow: hidden;
  text-overflow: ellipsis;
  display: block;
  cursor: pointer;
  transition: all 0.2s ease-in-out;
  white-space: nowrap;
  -webkit-touch-callout: none;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}
.tabs ul li label br {
  display: none;
}
.tabs ul li label svg {
  fill: #929daf;
  height: 1.2em;
  vertical-align: middle;
  margin-right: 0.2em;
  transition: all 0.2s ease-in-out;
}
.tabs ul li label:hover,
.tabs ul li label:focus,
.tabs ul li label:active {
  outline: 0;
  color: #bec5cf;
}
.tabs ul li label:hover svg,
.tabs ul li label:focus svg,
.tabs ul li label:active svg {
  fill: #bec5cf;
}
.tabs .slider {
  position: relative;
  width: 50%;
  transition: all 0.33s cubic-bezier(0.38, 0.8, 0.32, 1.07);
}
.tabs .slider .indicator {
  position: relative;
  width: 80%;
  margin: 0 auto;
  height: 4px;
  background: #f39d93;
  border-radius: 1px;
}
.tabs .content {
  margin-top: 30px;
}
.tabs .content section {
  display: none;
  -webkit-animation-name: content;
  animation-name: content;
  -webkit-animation-direction: normal;
  animation-direction: normal;
  -webkit-animation-duration: 0.3s;
  animation-duration: 0.3s;
  -webkit-animation-timing-function: ease-in-out;
  animation-timing-function: ease-in-out;
  -webkit-animation-iteration-count: 1;
  animation-iteration-count: 1;
  line-height: 1.4;
}
.tabs .content section h2 {
  color: #f39d93;
  display: none;
}
.tabs .content section h2::after {
  content: "";
  position: relative;
  display: block;
  width: 30px;
  height: 3px;
  background: #f39d93;
  margin-top: 5px;
  left: 1px;
}
.tabs
  input[name="tab-control"]:nth-of-type(1):checked
  ~ div
  ul
  > li:nth-child(1)
  > label {
  cursor: default;
  color: #f39d93;
}
.tabs
  input[name="tab-control"]:nth-of-type(1):checked
  ~ div
  ul
  > li:nth-child(1)
  > label
  svg {
  fill: #f39d93;
}
@media (max-width: 700px) {
  .playlists-block {
    overflow-x: scroll;
    overflow-y: hidden;
    height: 220px;
    width: 100%;
    white-space: nowrap;
    font-size: 12px;
  }
  .playlist-block {
    float: none;
    margin-right: 5px;
    width: 120px;
    margin-left: 0;
    height: 110px;
    display: inline-block;
  }
  .playlist-poster {
    margin-top: 10px;
  }
  .playlist-block.uploads-playlist .playlist-poster,
  .playlist-block.create-playlist .playlist-poster {
    margin-top: 25px;
  }
  .playlist-content {
    padding: 10px 0;
  }
}
@media (max-width: 600px) {
  .tabs
    input[name="tab-control"]:nth-of-type(1):checked
    ~ div
    ul
    > li:nth-child(1)
    > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(1):checked ~ div .slider {
  -webkit-transform: translateX(0%);
  transform: translateX(0%);
}
.tabs
  input[name="tab-control"]:nth-of-type(1):checked
  ~ .content
  > section:nth-child(1) {
  display: block;
}
.tabs
  input[name="tab-control"]:nth-of-type(2):checked
  ~ div
  ul
  > li:nth-child(2)
  > label {
  cursor: default;
  color: #f39d93;
}
.tabs
  input[name="tab-control"]:nth-of-type(2):checked
  ~ div
  ul
  > li:nth-child(2)
  > label
  svg {
  fill: #f39d93;
}
@media (max-width: 600px) {
  .tabs
    input[name="tab-control"]:nth-of-type(2):checked
    ~ div
    ul
    > li:nth-child(2)
    > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(2):checked ~ div .slider {
  -webkit-transform: translateX(100%);
  transform: translateX(100%);
}
.tabs
  input[name="tab-control"]:nth-of-type(2):checked
  ~ .content
  > section:nth-child(2) {
  display: block;
}
.tabs
  input[name="tab-control"]:nth-of-type(3):checked
  ~ div
  ul
  > li:nth-child(3)
  > label {
  cursor: default;
  color: #f39d93;
}
.tabs
  input[name="tab-control"]:nth-of-type(3):checked
  ~ div
  ul
  > li:nth-child(3)
  > label
  svg {
  fill: #f39d93;
}
@media (max-width: 600px) {
  .tabs
    input[name="tab-control"]:nth-of-type(3):checked
    ~ div
    ul
    > li:nth-child(3)
    > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(3):checked ~ div .slider {
  -webkit-transform: translateX(200%);
  transform: translateX(200%);
}
.tabs
  input[name="tab-control"]:nth-of-type(3):checked
  ~ .content
  > section:nth-child(3) {
  display: block;
}
.tabs
  input[name="tab-control"]:nth-of-type(4):checked
  ~ div
  ul
  > li:nth-child(4)
  > label {
  cursor: default;
  color: #f39d93;
}
.tabs
  input[name="tab-control"]:nth-of-type(4):checked
  ~ div
  ul
  > li:nth-child(4)
  > label
  svg {
  fill: #f39d93;
}
@media (max-width: 600px) {
  .tabs
    input[name="tab-control"]:nth-of-type(4):checked
    ~ div
    ul
    > li:nth-child(4)
    > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(4):checked ~ div .slider {
  -webkit-transform: translateX(300%);
  transform: translateX(300%);
}
.tabs
  input[name="tab-control"]:nth-of-type(4):checked
  ~ .content
  > section:nth-child(4) {
  display: block;
}
@-webkit-keyframes content {
  from {
    opacity: 0;
    -webkit-transform: translateY(5%);
    transform: translateY(5%);
  }
  to {
    opacity: 1;
    -webkit-transform: translateY(0%);
    transform: translateY(0%);
  }
}
@keyframes content {
  from {
    opacity: 0;
    -webkit-transform: translateY(5%);
    transform: translateY(5%);
  }
  to {
    opacity: 1;
    -webkit-transform: translateY(0%);
    transform: translateY(0%);
  }
}
@media (max-width: 1000px) {
  .tabs ul li label {
    white-space: initial;
  }
  .tabs ul li label br {
    display: initial;
  }
  .tabs ul li label svg {
    height: 1.5em;
  }
}
@media (max-width: 600px) {
  .tabs ul li label {
    padding: 5px;
    border-radius: 5px;
  }
  .tabs ul li label span {
    display: none;
  }
  .tabs .slider {
    display: none;
  }
  .tabs .content {
    margin-top: 20px;
  }
  .tabs .content section h2 {
    display: block;
  }
}
</style>
