<template>
  <div v-bind:class="{ 'sidebar-active': isActiveSidebar }" class="page">
    <Loader v-bind:class="{hidden : !isLoading}"/>
    <Notifications />
    <Login v-if="!isLoggedIn"/>
    <Sidebar v-if="isLoggedIn"/>
    <Player v-if="isLoggedIn"/>
    <Content v-if="isLoggedIn"/>
  </div>
</template>
<script>
import {mapGetters} from 'vuex'
import swal from 'sweetalert'
import Sidebar from './Components/Sidebar'
import Login from './Components/Login'
import Player from './Components/Player'
import Content from './Components/Content'
import Notifications from './Components/Notifications'
import Loader from './Components/Loader'
export default {
  data () {
    return {
      isNotificationActive: false,
      isLoading: false
    }
  },
  beforeMount () {
    this.$root.$on('actLoadingRoot', this.actLoading)
    this.$root.$on('checkScreenWidth', this.checkScreenWidth)
    this.$root.$on('deactLoadingRoot', this.deactLoading)
    this.$root.$on('errorHandler', this.errorHandler)
  },
  beforeDestroy () {
    this.$root.$off('actLoadingRoot', this.actLoading)
    this.$root.$off('checkScreenWidth', this.checkScreenWidth)
    this.$root.$off('deactLoadingRoot', this.deactLoading)
    this.$root.$off('errorHandler', this.errorHandler)
  },
  methods: {
    checkScreenWidth() {
      if (window.innerWidth < 700) {
        this.$main.isMobile = true
        this.deactSidebar()
      } else {
        this.actSidebar()
        this.$main.isMobile = false
      }
    },
    actSidebar () {
      this.$store.dispatch('activateSidebar')
    },
    deactSidebar () {
      this.$store.dispatch('deactivateSidebar')
    },
    actLoading () {
      this.isLoading = true
    },
    deactLoading () {
      this.isLoading = false
    },
    errorHandler (data) {
      let that = this
      switch (data) {
        case 401 : {
          swal({
            titleText: 'Oops',
            text: 'Session has expired. You will be automatically located to Login form.',
            icon: 'warning',
            timer: '3000',
            buttons: false
          }).then(function () {
            that.$root.$emit('pauseSongRoot')
            that.$store.dispatch('logOut')
          })
          break
        }
        case 400 : {
          swal({
            titleText: 'Oops',
            text: 'Incorrect user data.',
            icon: 'error'
          })
          break
        }
        default : swal({
            titleText: 'Hmm',
            text: 'Some unknown problem happenned. Page will be automatically reloaded.',
            icon: 'error',
            timer: '4000',
            buttons: false
          }).then(function () {
            window.location.reload()
          })
      }
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
    Content,
    Notifications,
    Loader
  }
}
</script>
<style>
@import url(https://fonts.googleapis.com/css?family=Montserrat);
body {
  font-family: "Montserrat", "Helevtica", sans-serif;
  color: #3a3654;
}
.hidden {
  display: none !important;
}
.swal-text {
  text-align: center;
}
.page {
  position: absolute;
  display: block;
  right: 0;
  left: 0;
  bottom: 0;
  top: 0;
}
.centered {
  text-align: center;
}
</style>
