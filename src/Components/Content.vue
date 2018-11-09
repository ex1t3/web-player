<template>
    <div class="content-wrapper">
        <div v-if="isHomePage">Home</div>
        <Music v-if="isMusicPage"/>
        <Search v-if="isSearchPage"/>
        <div v-if="isSettingsPage">Settings</div>
        <div v-if="isProfilePage">Profile</div>
    </div>
</template>
<script>
import store from '../store'
import Music from '../Components/Music'
import Search from '../Components/Search'
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
    isSettingsPage: 'isSettingsPage',
    isSearchPage: 'isSearchPage',
    isProfilePage: 'isProfilePage'
  }),
  components: {
    Music,
    Search
  }
}
</script>
<style>
.content-wrapper {
    position: relative;
    display: block;
    right: 0;
    top: 0;
    bottom: 0;
    z-index: 1;
    padding-left: 15px;
    transition: .3s;
    margin-top: 70px;
}
@media (max-width: 700px) {
  .sidebar-active .content-wrapper {
    filter: blur(7px);
    position: fixed;
  }
}
</style>
