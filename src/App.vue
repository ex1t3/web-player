<template>
  <div v-bind:class="{ 'sidebar-active': isActiveSidebar }" class="page">
    <div v-bind:class="{hidden : !isLoading}" class="holder">
      <div class="preloader">
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
      </div>
    </div>
    <div v-if="notifications.length > 0" class="notification-bar">
      <div :key="item.call" v-for="(item) in notifications" class="notification-bar-item">
        <div class="notification-bar-status">
          <i
            class="fas"
            v-bind:class="{'fa-check-circle success':item.notificationStatus === 'success', 'fa-exclamation-circle error': item.notificationStatus === 'error'}"
          ></i>
        </div>
        <div class="notification-bar-title">
          <h6>{{ item.notificationMessage }}</h6>
        </div>
      </div>
    </div>
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
export default {
  data () {
    return {
      message: 'dgdgdg',
      isNotificationActive: false,
      isLoading: false,
      notifications: [],
      notificationCall: 0
    }
  },
  beforeMount () {
    this.$root.$on('actLoadingRoot', this.actLoading)
    this.$root.$on('checkScreenWidth', this.checkScreenWidth)
    this.$root.$on('deactLoadingRoot', this.deactLoading)
    this.$root.$on('notificate', this.notificate)
    this.$root.$on('errorHandler', this.errorHandler)
  },
  beforeDestroy () {
    this.$root.$off('actLoadingRoot', this.actLoading)
    this.$root.$off('checkScreenWidth', this.checkScreenWidth)
    this.$root.$off('deactLoadingRoot', this.deactLoading)
    this.$root.$off('notificate', this.notificate)
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
    },
    notificate (status, message, time) {
      this.notifications.push({
        notificationMessage: message,
        notificationStatus: status,
        call: this.notificationCall++
      })
      let that = this
      setTimeout(function () {
        that.notifications.splice(this.notificationCall - 1, 1)
      }, time)
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
  font-family: "Montserrat", "Helevtica", sans-serif;
  color: #3a3654;
}
.hidden {
  display: none !important;
}
.notification-bar {
  position: fixed;
  z-index: 9999999;
  top: 30px;
  right: 30px;
  width: 200px;
  height: 70px;
}
.notification-bar-item {
  background: #fff;
  width: 200px;
  height: 70px;
  box-shadow: 0 0 20px 11px #a09f9f1a;
  margin-bottom: 35px;
  display: flex;
  align-items: center;
  animation: slideFromRight 0.2s;
}
.notification-bar-item .notification-bar-status {
  font-size: 25px;
  margin-left: 10px;
}
.notification-bar-status .error {
  color: #f7776b;
}
.notification-bar-status .success {
  color: #6bb049;
}
.notification-bar-item .notification-bar-title {
  margin-left: 10px;
}
.page {
  position: absolute;
  display: block;
  right: 0;
  left: 0;
  bottom: 0;
  top: 0;
}
.holder {
  position: fixed;
  left: 0px;
  top: 0px;
  z-index: 3;
  bottom: 0px;
  right: 0px;
  width: 100%;
  height: 100%;
  overflow: hidden;
  background-color: #ffffff;
}
.sidebar-active .holder {
  left: 220px;
}
.centered {
  text-align: center;
}
.preloader {
  /* size */
  width: 100px;
  height: 100px;
  position: fixed;
  left: 50%;
  top: 50%;
  transform: translateX(-50%) translateY(-50%);
  animation: rotatePreloader 2s infinite ease-in;
}

@keyframes rotatePreloader {
  0% {
    transform: translateX(-50%) translateY(-50%) rotateZ(0deg);
  }
  100% {
    transform: translateX(-50%) translateY(-50%) rotateZ(-360deg);
  }
}
.preloader div {
  position: absolute;
  width: 100%;
  height: 100%;
  opacity: 0;
}

.preloader div:before {
  content: "";
  position: absolute;
  left: 50%;
  top: 0%;
  width: 10%;
  height: 10%;
  background-color: #f2857ad4;
  transform: translateX(-50%);
  border-radius: 50%;
}

.preloader div:nth-child(1) {
  transform: rotateZ(0deg);
  animation: rotateCircle1 2s infinite linear;
  z-index: 9;
}

@keyframes rotateCircle1 {
  0% {
    opacity: 0;
  }
  0% {
    opacity: 1;
    transform: rotateZ(36deg);
  }
  7% {
    transform: rotateZ(0deg);
  }
  57% {
    transform: rotateZ(0deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
.preloader div:nth-child(2) {
  transform: rotateZ(36deg);
  animation: rotateCircle2 2s infinite linear;
  z-index: 8;
}

@keyframes rotateCircle2 {
  5% {
    opacity: 0;
  }
  5.0001% {
    opacity: 1;
    transform: rotateZ(0deg);
  }
  12% {
    transform: rotateZ(-36deg);
  }
  62% {
    transform: rotateZ(-36deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
.preloader div:nth-child(3) {
  transform: rotateZ(72deg);
  animation: rotateCircle3 2s infinite linear;
  z-index: 7;
}

@keyframes rotateCircle3 {
  10% {
    opacity: 0;
  }
  10.0002% {
    opacity: 1;
    transform: rotateZ(-36deg);
  }
  17% {
    transform: rotateZ(-72deg);
  }
  67% {
    transform: rotateZ(-72deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
.preloader div:nth-child(4) {
  transform: rotateZ(108deg);
  animation: rotateCircle4 2s infinite linear;
  z-index: 6;
}

@keyframes rotateCircle4 {
  15% {
    opacity: 0;
  }
  15.0003% {
    opacity: 1;
    transform: rotateZ(-72deg);
  }
  22% {
    transform: rotateZ(-108deg);
  }
  72% {
    transform: rotateZ(-108deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
.preloader div:nth-child(5) {
  transform: rotateZ(144deg);
  animation: rotateCircle5 2s infinite linear;
  z-index: 5;
}

@keyframes rotateCircle5 {
  20% {
    opacity: 0;
  }
  20.0004% {
    opacity: 1;
    transform: rotateZ(-108deg);
  }
  27% {
    transform: rotateZ(-144deg);
  }
  77% {
    transform: rotateZ(-144deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
.preloader div:nth-child(6) {
  transform: rotateZ(180deg);
  animation: rotateCircle6 2s infinite linear;
  z-index: 4;
}

@keyframes rotateCircle6 {
  25% {
    opacity: 0;
  }
  25.0005% {
    opacity: 1;
    transform: rotateZ(-144deg);
  }
  32% {
    transform: rotateZ(-180deg);
  }
  82% {
    transform: rotateZ(-180deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
.preloader div:nth-child(7) {
  transform: rotateZ(216deg);
  animation: rotateCircle7 2s infinite linear;
  z-index: 3;
}

@keyframes rotateCircle7 {
  30% {
    opacity: 0;
  }
  30.0006% {
    opacity: 1;
    transform: rotateZ(-180deg);
  }
  37% {
    transform: rotateZ(-216deg);
  }
  87% {
    transform: rotateZ(-216deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
.preloader div:nth-child(8) {
  transform: rotateZ(252deg);
  animation: rotateCircle8 2s infinite linear;
  z-index: 2;
}

@keyframes rotateCircle8 {
  35% {
    opacity: 0;
  }
  35.0007% {
    opacity: 1;
    transform: rotateZ(-216deg);
  }
  42% {
    transform: rotateZ(-252deg);
  }
  92% {
    transform: rotateZ(-252deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
.preloader div:nth-child(9) {
  transform: rotateZ(288deg);
  animation: rotateCircle9 2s infinite linear;
  z-index: 1;
}

@keyframes rotateCircle9 {
  40% {
    opacity: 0;
  }
  40.0008% {
    opacity: 1;
    transform: rotateZ(-252deg);
  }
  47% {
    transform: rotateZ(-288deg);
  }
  97% {
    transform: rotateZ(-288deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
.preloader div:nth-child(10) {
  transform: rotateZ(324deg);
  animation: rotateCircle10 2s infinite linear;
  z-index: 0;
}

@keyframes rotateCircle10 {
  45% {
    opacity: 0;
  }
  45.0009% {
    opacity: 1;
    transform: rotateZ(-288deg);
  }
  52% {
    transform: rotateZ(-324deg);
  }
  102% {
    transform: rotateZ(-324deg);
  }
  100% {
    transform: rotateZ(-324deg);
    opacity: 1;
  }
}
@keyframes slideFromRight {
  0% {
    transform: translateX(250px);
  }
  100% {
    transform: translateX(0);
  }
}
</style>
