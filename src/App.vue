<template>
<div v-bind:class="{ 'sidebar-active': isActiveSidebar }" class="page">
  <Login v-if="!isLoggedIn"/>
  <Sidebar v-if="isLoggedIn"/>
  <Content v-if="isLoggedIn"/>
  <Player v-if="isLoggedIn"/>
</div>
</template>
<script>
import {mapGetters} from 'vuex'
import Sidebar from './Components/Sidebar'
import Login from './Components/Login'
import Player from './Components/Player'
import Content from './Components/Content'
export default {
  data () {
    return {
      message: 'dgdgdg'
    }
  },
  mounted: function () {
    if (window.innerWidth < 700) {
      this.deactSidebar()
    } else {
      this.actSidebar()
    }
  },
  methods: {
    actSidebar () {
      this.$store.dispatch('activateSidebar')
    },
    deactSidebar () {
      this.$store.dispatch('deactivateSidebar')
    }
  },
  name: 'App',
  computed: mapGetters({
    isLoggedIn: 'isLoggedIn',
    isActiveSidebar: 'isActiveSidebar'
  }),
  components: {
    Sidebar,
    Login,
    Player,
    Content
  }
}
</script>
<style>
@import url(https://fonts.googleapis.com/css?family=Montserrat);
body {
  font-family: 'Montserrat', 'Helevtica', sans-serif;
  color: #3a3654;
}
.page {
    position: absolute;
    display: block;
    right: 0;
    left: 0;
    bottom: 0;
    top: 0;
}
</style>
