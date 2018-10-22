<template>
<div v-bind:class="{ 'sidebar-active': isActiveSidebar }" class="page">
  <Login v-if="!isLoggedIn"/>
  <Sidebar v-if="isLoggedIn"/>
  <Player v-if="isLoggedIn"/>
  <Content v-if="isLoggedIn"/>
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
html, body {
  font-family: 'Niramit', sans-serif;
}
.page {
  position: absolute;
  display: block;
}
</style>
