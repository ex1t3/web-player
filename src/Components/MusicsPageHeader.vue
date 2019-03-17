<template>
  <div>
    <div
      v-if="currentPlaylistName !== 'My Playlists' && !isFavoriteTracksPageOpened"
      @click="$root.$emit('openInstance', 3)"
      class="return-block"
    >
      <i class="fas fa-arrow-left"></i>
      <div>Back to playlists</div>
    </div>
    <ul>
      <li
        @click="$root.$emit('openInstance')"
        v-bind:title="currentPlaylistName"
      >
        <label for="tab1" role="button">
          <svg
            version="1.1"
            id="Capa_1"
            xmlns="http://www.w3.org/2000/svg"
            xmlns:xlink="http://www.w3.org/1999/xlink"
            x="0px"
            y="0px"
            viewBox="0 0 448.138 448.138"
            style="enable-background:new 0 0 448.138 448.138;"
            xml:space="preserve"
          >
            <path
              d="M436.768,151.845c-13.152-26.976-35.744-42.08-57.6-56.704C362.88,84.229,347.52,73.925,336.64,59.173l-2.016-2.72c-6.4-8.608-13.696-18.368-14.816-26.56c-1.12-8.288-7.648-14.048-16.928-13.792C294.496,16.677,288,23.653,288,32.069v285.12c-13.408-8.128-29.92-13.12-48-13.12c-44.096,0-80,28.704-80,64s35.904,64,80,64c44.128,0,80-28.704,80-64V181.573c24.032,9.184,63.36,32.576,74.176,87.2c-2.016,2.976-3.936,6.208-6.176,8.736c-5.856,6.624-5.184,16.736,1.44,22.56c6.592,5.888,16.704,5.184,22.56-1.44c20.032-22.752,33.824-58.784,35.968-94.016C449.024,187.237,445.152,168.997,436.768,151.845zs"
            ></path>
            <path
              d="M16,48.069h192c8.832,0,16-7.168,16-16s-7.168-16-16-16H16c-8.832,0-16,7.168-16,16S7.168,48.069,16,48.069z"
            ></path>
            <path
              d="M16,144.069h192c8.832,0,16-7.168,16-16s-7.168-16-16-16H16c-8.832,0-16,7.168-16,16S7.168,144.069,16,144.069z"
            ></path>
            <path
              d="M112,208.069H16c-8.832,0-16,7.168-16,16s7.168,16,16,16h96c8.832,0,16-7.168,16-16S120.832,208.069,112,208.069z"
            ></path>
            <path
              d="M112,304.069H16c-8.832,0-16,7.168-16,16s7.168,16,16,16h96c8.832,0,16-7.168,16-16S120.832,304.069,112,304.069z"
            ></path>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
            <g></g>
          </svg>
          <br>
          <span>{{ currentPlaylistName }}</span>
        </label>
      </li>
      <li @click="$root.$emit('openInstance', 2)" title="Favorite Tracks">
        <label for="tab2" role="button">
          <svg
            version="1.1"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 43.995 43.995"
            xmlns:xlink="http://www.w3.org/1999/xlink"
            enable-background="new 0 0 43.995 43.995"
          >
            <g>
              <g>
                <path
                  d="m42.07,24.111c-2.566-2.557-6.369-2.913-8.936-0.356l-2.145,2.137-2.144-2.137c-2.566-2.557-6.369-2.2-8.935,0.356-2.566,2.557-2.566,6.701 0,9.258l.357,.356 10.008,9.97c0.395,0.394 1.035,0.394 1.43,0l10.365-10.326c2.567-2.557 2.567-6.701 0-9.258zm-1.429,7.834l-1.072,1.067-8.578,8.547-9.65-9.614c-1.776-1.771-1.776-4.64 0-6.41 1.654-1.647 4.202-2.223 6.076-0.355l3.574,3.561 3.574-3.561c1.964-1.956 4.422-1.292 6.076,0.355 1.776,1.771 1.776,4.64 7.10543e-15,6.41z"
                ></path>
              </g>
            </g>
            <g>
              <g>
                <path
                  fill-rule="evenodd"
                  d="m1,2.005h28c0.552,0 1-0.447 1-1s-0.448-1-1-1h-28c-0.552,0-1,0.447-1,1s0.448,1 1,1zm17,18h-17c-0.552,0-1,0.447-1,1s0.448,1 1,1h17c0.552,0 1-0.447 1-1s-0.448-1-1-1zm11-10h-28c-0.552,0-1,0.447-1,1s0.448,1 1,1h28c0.552,0 1-0.447 1-1s-0.448-1-1-1zm-14,20h-14c-0.552-3.55271e-15-1,0.447-1,1s0.448,1 1,1h14c0.552,0 1-0.447 1-1s-0.448-1-1-1z"
                ></path>
              </g>
            </g>
          </svg>
          <br>
          <span>Favorite Tracks</span>
        </label>
      </li>
    </ul>
    <div class="slider">
      <div class="indicator"></div>
    </div>
  </div>
</template>
<script>
export default {
  props: ['isFavoriteTracksPageOpened', 'isPlaylistOpened', 'isUploadsOpened', 'currentPlaylistName']
}
</script>
<style scoped>
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

</style>