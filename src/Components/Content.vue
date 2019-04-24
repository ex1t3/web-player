<template>
    <div class="content-wrapper">
        <Home v-if="isHomePage"/>
        <Music v-if="isMusicPage"/>
        <Search v-if="isSearchPage"/>
        <div v-if="isSettingsPage">Settings</div>
        <Profile v-if="isProfilePage"/>
    </div>
</template>
<script>
import store from '../store'
import Music from '../Components/Music'
import Search from '../Components/Search'
import Profile from '../Components/Profile'
import Home from '../Components/Home'
import {mapGetters} from 'vuex'

// Exporting data for current template
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
    Search,
    Profile,
    Home
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
    transition: .3s;
    margin-top: 70px;
    padding-bottom: 140px;
    overflow: hidden;
    width: 100%;
}
@media (max-width: 700px) {
  .sidebar-active .content-wrapper {
    filter: blur(7px);
    position: fixed;
  }
}
</style>
