<template>
<div class="tabs">
  <input type="radio" id="tab1" name="tab-control" checked>
  <input type="radio" id="tab2" name="tab-control">
  <ul>
    <li title="My Playlists"><label for="tab1" role="button"><svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 448.138 448.138" style="enable-background:new 0 0 448.138 448.138;" xml:space="preserve"><path d="M436.768,151.845c-13.152-26.976-35.744-42.08-57.6-56.704C362.88,84.229,347.52,73.925,336.64,59.173l-2.016-2.72c-6.4-8.608-13.696-18.368-14.816-26.56c-1.12-8.288-7.648-14.048-16.928-13.792C294.496,16.677,288,23.653,288,32.069v285.12c-13.408-8.128-29.92-13.12-48-13.12c-44.096,0-80,28.704-80,64s35.904,64,80,64c44.128,0,80-28.704,80-64V181.573c24.032,9.184,63.36,32.576,74.176,87.2c-2.016,2.976-3.936,6.208-6.176,8.736c-5.856,6.624-5.184,16.736,1.44,22.56c6.592,5.888,16.704,5.184,22.56-1.44c20.032-22.752,33.824-58.784,35.968-94.016C449.024,187.237,445.152,168.997,436.768,151.845zs"/><path d="M16,48.069h192c8.832,0,16-7.168,16-16s-7.168-16-16-16H16c-8.832,0-16,7.168-16,16S7.168,48.069,16,48.069z"/><path d="M16,144.069h192c8.832,0,16-7.168,16-16s-7.168-16-16-16H16c-8.832,0-16,7.168-16,16S7.168,144.069,16,144.069z"/><path d="M112,208.069H16c-8.832,0-16,7.168-16,16s7.168,16,16,16h96c8.832,0,16-7.168,16-16S120.832,208.069,112,208.069z"/><path d="M112,304.069H16c-8.832,0-16,7.168-16,16s7.168,16,16,16h96c8.832,0,16-7.168,16-16S120.832,304.069,112,304.069z"/><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g>
</svg><br><span>My Playlists</span></label></li>
    <li title="Favorite Tracks"><label for="tab2" role="button"><svg version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 43.995 43.995" xmlns:xlink="http://www.w3.org/1999/xlink" enable-background="new 0 0 43.995 43.995">
  <g>
    <g>
      <path d="m42.07,24.111c-2.566-2.557-6.369-2.913-8.936-0.356l-2.145,2.137-2.144-2.137c-2.566-2.557-6.369-2.2-8.935,0.356-2.566,2.557-2.566,6.701 0,9.258l.357,.356 10.008,9.97c0.395,0.394 1.035,0.394 1.43,0l10.365-10.326c2.567-2.557 2.567-6.701 0-9.258zm-1.429,7.834l-1.072,1.067-8.578,8.547-9.65-9.614c-1.776-1.771-1.776-4.64 0-6.41 1.654-1.647 4.202-2.223 6.076-0.355l3.574,3.561 3.574-3.561c1.964-1.956 4.422-1.292 6.076,0.355 1.776,1.771 1.776,4.64 7.10543e-15,6.41z"/>
    </g>
  </g>
  <g>
    <g>
      <path fill-rule="evenodd" d="m1,2.005h28c0.552,0 1-0.447 1-1s-0.448-1-1-1h-28c-0.552,0-1,0.447-1,1s0.448,1 1,1zm17,18h-17c-0.552,0-1,0.447-1,1s0.448,1 1,1h17c0.552,0 1-0.447 1-1s-0.448-1-1-1zm11-10h-28c-0.552,0-1,0.447-1,1s0.448,1 1,1h28c0.552,0 1-0.447 1-1s-0.448-1-1-1zm-14,20h-14c-0.552-3.55271e-15-1,0.447-1,1s0.448,1 1,1h14c0.552,0 1-0.447 1-1s-0.448-1-1-1z"/>
    </g>
  </g>
</svg><br><span>Favorite Tracks</span></label></li>
  </ul>
  <div class="slider"><div class="indicator"></div></div>
  <div class="content">
    <section>
      <h2>My Playlists</h2>
          <div class="playlist-block">
            <div class="playlist-poster"><img src="../img/pc.svg"/></div>
            <div class="playlist-content">
              <p class="playlist-title">Playlist #1</p>
              </div>
          </div>
          <div class="playlist-block">
            <div class="playlist-poster"><img src="../img/pc.svg"/></div>
            <div class="playlist-content">
              <p class="playlist-title">GYM</p>
              </div>
          </div>
      </section>
        <section>
          <h2>Favorite Tracks</h2>
          <div class="favorite-songs-block">
            <div class="favorite-songs-body">
              <div class="favorite-songs-header">
                <div class="favorite-songs-header-number">#</div>
                <div class="favorite-songs-header-name">Name</div>
                <div class="favorite-songs-header-artist">Artist</div>
              </div>
              <div @click="playSong(fav['index'])" v-bind:class="{'current-song': $main.currentIndex===fav['index']}" class="favorite-songs-item" :key="index" v-for="(fav, index) in songs">
                <div class="favorite-song-number">
                  <div class="favorite-song-number-hidden">{{ index + 1}}</div>
                  <button class="player-button-icon"><i class="fas" v-bind:class="{'fa-pause': $main.currentIndex===fav['index'] && !$main.isPaused, 'fa-play' : $main.isPaused || ($main.currentIndex!==fav['index'] && !$main.isPaused)}"></i></button>
                </div>
                <div class="favorite-song-name">{{ fav['name'] }}</div>
                <div class="favorite-song-title">{{ fav['artist'] }}</div>
              </div>
            </div>
          </div>
         </section>
  </div>
</div>
</template>
<script>
export default {
  data () {
    return {
      songs: []
    }
  },
  mounted () {
    this.loadSongs()
    this.$root.$emit('loadSongsRoot', this.songs)
  },
  methods: {
    loadSongs () {
      this.songs = [
        { src: require('../songs/1.mp3'),
          artist: '0',
          name: 'gfgfg',
          index: '12'
        },
        { src: require('../songs/2.mp3'),
          artist: '1',
          name: 'fggfgf',
          index: '11'
        },
        { src: require('../songs/3.mp3'),
          artist: '2',
          name: 'fgfg',
          index: '13'
        },
        { src: require('../songs/4.mp3'),
          artist: '3',
          name: 'gffg',
          index: '14'
        },
        { src: 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/308622/NF%20-%20Let%20You%20Down.mp3',
          artist: '4',
          name: 'fgfgg',
          index: '15'
        }
      ]
    },
    playSong (index) {
      if (index === this.$main.currentIndex && !this.$main.isPaused) {
        this.$root.$emit('pauseSongRoot')
      } else {    
        this.$root.$emit('playDefinedSongRoot', index)
      }
    }
  }
}
</script>
<style>
.favorite-songs-item .player-button-icon,
.favorite-songs-item .player-button-icon i {
    background: none;
    padding: 0;
}
.favorite-songs-item:hover {
  cursor: pointer;
  background: #efeff17d;
}
.favorite-songs-item.current-song {
  background: #efeff17d;
}
.favorite-song-number .player-button-icon {
  display: none;
}
.favorite-songs-item:hover .favorite-song-number .player-button-icon {
  display: block;
}
.favorite-songs-item:hover .favorite-song-number .favorite-song-number-hidden {
  display: none;
}
.favorite-songs-item.current-song .favorite-song-number .player-button-icon {
  display: block;
}
.favorite-songs-item.current-song .favorite-song-number .favorite-song-number-hidden {
  display: none;
}
.favorite-songs-item.current-song div,
.favorite-songs-item.current-song button {
  color: #f39d93;
}
.favorite-songs-header {
  display: flex;
  margin: 10px;
  align-items: center;
}
.favorite-songs-item {
    display: flex;
    align-items: center;
    height: 50px;
    position: relative;
    padding: 5px;
}
.favorite-songs-item::after {
    content: '';
    height: 1px;
    background: #f4f4f4;
    top: 0px;
    width: calc(100% - 12px);
    position: absolute;
    left: 6px;
    right: 6px;
}
.favorite-songs-block {
  width: 100%;
}
.favorite-songs-header,
.favorite-songs-item {
  width: 100%;
}
.favorite-songs-header div,
.favorite-songs-item div{
  width: 20%;
}
.playlist-block {
    width: 200px;
    text-align: center;
    height: 200px;
    display: flex;
    margin-top: 60px;
    margin-left: 40px;
    cursor: pointer;
    position: relative;
    transition: .3s;
    align-items: center;
    float: left;
    box-shadow: 0px 5px 20px #d2d2d24a;
}
.playlist-block:hover {
  transform: translateY(-5px)
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
}
.tabs {
left: 50%;
    -webkit-transform: translateX(-50%);
    transform: translateX(-50%);
    position: relative;
    background: white;
    padding: 20px;
    padding-bottom: 80px;
    width: 90%;
    height: auto;
    min-width: 240px;
}
.tabs input[name="tab-control"] {
  display: none;
}
.tabs .content section h2,
.tabs ul li label {
  font-family: 'Niramit', sans-serif;
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
.tabs ul li label:hover, .tabs ul li label:focus, .tabs ul li label:active {
  outline: 0;
  color: #bec5cf;
}
.tabs ul li label:hover svg, .tabs ul li label:focus svg, .tabs ul li label:active svg {
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
.tabs input[name="tab-control"]:nth-of-type(1):checked ~ ul > li:nth-child(1) > label {
  cursor: default;
  color: #f39d93;
}
.tabs input[name="tab-control"]:nth-of-type(1):checked ~ ul > li:nth-child(1) > label svg {
  fill: #f39d93;
}
@media (max-width: 700px) {
  .playlist-block {
    float: none;
    margin-left: auto;
    margin-right: auto;
    width: 190px;
    height: 150px;
    margin-top: 70px;
  }
  .playlist-poster img {
    width: 80%;
  }
}
@media (max-width: 600px) {
  .tabs input[name="tab-control"]:nth-of-type(1):checked ~ ul > li:nth-child(1) > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(1):checked ~ .slider {
  -webkit-transform: translateX(0%);
          transform: translateX(0%);
}
.tabs input[name="tab-control"]:nth-of-type(1):checked ~ .content > section:nth-child(1) {
  display: block;
}
.tabs input[name="tab-control"]:nth-of-type(2):checked ~ ul > li:nth-child(2) > label {
  cursor: default;
  color: #f39d93;
}
.tabs input[name="tab-control"]:nth-of-type(2):checked ~ ul > li:nth-child(2) > label svg {
  fill: #f39d93;
}
@media (max-width: 600px) {
  .tabs input[name="tab-control"]:nth-of-type(2):checked ~ ul > li:nth-child(2) > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(2):checked ~ .slider {
  -webkit-transform: translateX(100%);
          transform: translateX(100%);
}
.tabs input[name="tab-control"]:nth-of-type(2):checked ~ .content > section:nth-child(2) {
  display: block;
}
.tabs input[name="tab-control"]:nth-of-type(3):checked ~ ul > li:nth-child(3) > label {
  cursor: default;
  color: #f39d93;
}
.tabs input[name="tab-control"]:nth-of-type(3):checked ~ ul > li:nth-child(3) > label svg {
  fill: #f39d93;
}
@media (max-width: 600px) {
  .tabs input[name="tab-control"]:nth-of-type(3):checked ~ ul > li:nth-child(3) > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(3):checked ~ .slider {
  -webkit-transform: translateX(200%);
          transform: translateX(200%);
}
.tabs input[name="tab-control"]:nth-of-type(3):checked ~ .content > section:nth-child(3) {
  display: block;
}
.tabs input[name="tab-control"]:nth-of-type(4):checked ~ ul > li:nth-child(4) > label {
  cursor: default;
  color: #f39d93;
}
.tabs input[name="tab-control"]:nth-of-type(4):checked ~ ul > li:nth-child(4) > label svg {
  fill: #f39d93;
}
@media (max-width: 600px) {
  .tabs input[name="tab-control"]:nth-of-type(4):checked ~ ul > li:nth-child(4) > label {
    background: rgba(0, 0, 0, 0.08);
  }
}
.tabs input[name="tab-control"]:nth-of-type(4):checked ~ .slider {
  -webkit-transform: translateX(300%);
          transform: translateX(300%);
}
.tabs input[name="tab-control"]:nth-of-type(4):checked ~ .content > section:nth-child(4) {
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
