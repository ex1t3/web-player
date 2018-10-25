<template>
    <div class="content-wrapper">
        <div v-if="isHomePage">Home</div>
        <div v-if="isMusicPage">Music</div>
        <div v-if="isPlaylistsPage">Playlists</div>
        <div v-if="isSettingsPage">Settings</div>
        <div v-if="isProfilePage">Profile</div>
    </div>
</template>
<script>
import store from '../store'
import {mapGetters} from 'vuex'

export default {
  store,
  data () {
    return {
    }
  },
  computed: mapGetters({
    isHomePage: 'isHomePage',
    isMusicPage: 'isMusicPage',
    isPlaylistsPage: 'isPlaylistsPage',
    isSettingsPage: 'isSettingsPage',
    isProfilePage: 'isProfilePage'
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
    padding-left: 15px;
    transition: .3s;
    margin-top: 40px;
}
@media (max-width: 700px) {
  .sidebar-active .content-wrapper {
    left: -220px;
    background: linear-gradient(125deg, white, #625abb73);
    filter: blur(10px);
  }
}
</style>
