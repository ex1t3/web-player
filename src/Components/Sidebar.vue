<template>
  <div class="absolute-items">
    <div class="profiler-round">
      <img :src="$login.user.Photo"/>
      <div class="profile-menu">      
        <ul class="profile-link-list">
          <div class="profile-menu-layer"></div>
          <li @click="openTemplate('setProfilePage')"><i class="fas profile-item-icon fa-user"></i>Profile</li>
          <li @click="openTemplate('setSettingsPage')"><i class="fas profile-item-icon fa-cog"></i>Settings</li>
          <li @click="logOut()"><i class="fas profile-item-icon fa-sign-out-alt"></i>Logout</li>
        </ul>
      </div>
    </div>
    <div class="sidebar">
      <div
        v-bind:class="{ 'burger-active': isActiveSidebar }"
        @click="toggleSidebar()"
        class="sidebar-burger"
      >
        <span class="burger-inner"></span>
      </div>
      <div class="sidebar-body">
        <div class="sidebar-legend">
          <a>AUDIO</a>
        </div>
        <ul>
          <li>
            <a @click="openTemplate('setHomePage')">Home</a>
          </li>
          <li>
            <a @click="openTemplate('setMusicPage')">My music</a>
          </li>
          <li>
            <a @click="openTemplate('setSearchPage')">Search</a>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>
<script>
import store from '../store';
import axios from 'axios';
import {
  mapGetters
} from 'vuex';

// Exporting data for current template
export default {
  store,
  methods: {
    openTemplate(template) {
      this.$store.dispatch(template)
    },
    toggleSidebar() {
      if (this.isActiveSidebar) {
        this.$store.dispatch('deactivateSidebar')
      } else {
        this.$store.dispatch('activateSidebar')
      }
    },
    logOut() {
      var that = this
      let token = localStorage.getItem('access_token')
      this.$store.dispatch('updateModalVisibility', false)
      that.$root.$emit('pauseSongRoot')
      if (token == null) {
        that.$store.dispatch('logOut')
      } else {
        localStorage.removeItem('access_token')
        axios({
            method: 'POST',
            url: 'https://localhost:44343/api/Account/Logout',
            headers: {
              Authorization: 'Bearer ' + token
            }
          })
          .then(function (e) {
            that.$store.dispatch('logOut')
          })
          .catch(function (e) {
            that.$root.$emit('errorHandler', e)
          })
      }
    }
  },
  computed: mapGetters({
    isActiveSidebar: 'isActiveSidebar'
  })
}
</script>
<style>
.profile-link-list {
  margin-top: 1px;
  right: 0;
  position: absolute;
  background: #fff;
  text-align: left;
  width: 130px;
  border-radius: 5px;
  animation: fade-in .5s;
  box-shadow: 3px 2px 20px rgba(0, 0, 0, 0.14901960784313725);
}

.profile-link-list::before {
  right: 20px;
  width: 0;
  border-style: solid;
  border-width: 0 11px 7px;
  content: "";
  position: absolute;
  height: auto;
  top: -7px;
  border-color: transparent transparent #fff;
}

.profile-link-list li {
  padding: 10px;
  border-bottom: 1px solid rgba(200, 200, 202, 0.2);
  cursor: pointer;
}

.profile-link-list li:hover {
  background: rgba(200, 200, 202, 0.1);
  border-bottom: 1px solid transparent;
}

.profile-item-icon {
  margin-right: 7px;
}

.profile-menu-layer {
      position: absolute;
    width: 100%;
    height: 30px;
    top: -30px;
    background: transparent;
}

.profile-menu {
  margin-top: 67px;
  display: none;
}

.profiler-round:hover .profile-menu {
  display: block;
}

.profiler-round {
  position: absolute;
  right: 20px;
  top: 17px;
  width: 60px;
  height: 60px;
  align-items: center;
  text-align: center;
  border-radius: 50%;
  cursor: pointer;
  background-color: #ddd;
  box-shadow: 0px 5px 20px rgba(221, 221, 221, 0.7607843137254902);
  z-index: 999;
}

.profiler-round img {
  position: absolute;
  height: 100%;
  width: 100%;
  right: 0;
  border-radius: 50%
}

.sidebar-active {
  left: 220px;
}

.sidebar-active .sidebar-body ul li:nth-child(1) {
  animation: slide-in 0.5s;
}

.sidebar-active .sidebar-body ul li:nth-child(2) {
  animation: slide-in 0.6s;
}

.sidebar-active .sidebar-body ul li:nth-child(3) {
  animation: slide-in 0.7s;
}

.sidebar-active .sidebar-body ul li:nth-child(4) {
  animation: slide-in 0.8s;
}

ul {
  display: block;
  list-style-type: none;
  margin-block-start: 1em;
  margin-block-end: 1em;
  margin-inline-start: 0px;
  margin-inline-end: 0px;
  padding-inline-start: 0px;
  margin-top: 80px;
}

.sidebar ul li {
  list-style: none;
  padding-left: 20px;
  padding-bottom: 10px;
  border-bottom: 1px solid rgba(0, 0, 0, 0.07);
}

.sidebar ul li:last-child {
  border: none;
}

.sidebar-active .sidebar {
  animation: slide-in 0.5s;
  transform: translateX(0);
}

.page:not(.sidebar-active) .sidebar {
  animation: slide-out 0.5s;
}

.page:not(.sidebar-active) .sidebar-burger {
  animation: roll-out 0.5s;
}

.sidebar {
  -ms-flex-direction: column;
  -webkit-flex-direction: column;
  background: linear-gradient(rgb(243, 133, 120), rgba(31, 28, 236, 0.5));
  bottom: 0px;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  flex-direction: column;
  left: 0;
  position: fixed;
  top: 0;
  transform: translateX(-220px);
  z-index: 100;
  width: 220px;
}

.sidebar-burger {
  position: absolute;
  left: 235px;
  top: 10px;
  animation: roll-in 0.5s;
  cursor: pointer;
  width: 40px;
  height: 40px;
}

@media (max-width: 700px) {
  .sidebar-burger {
    background: rgba(255, 255, 255, 0.7098039215686275)
  }
}

.burger-inner {
  top: 50%;
  margin-top: -2px;
  transition-timing-function: cubic-bezier(0.55, 0.055, 0.675, 0.19);
  transition-duration: 0.22s;
}

.sidebar-burger span,
.sidebar-burger span::before,
.sidebar-burger span::after {
  position: absolute;
  cursor: pointer;
  display: block;
  width: 40px;
  height: 4px;
  transition-timing-function: ease;
  transition-duration: 0.15s;
  transition-property: transform;
  border-radius: 4px;
  background-color: #f39d93;
}

.burger-inner:after,
.burger-inner:before {
  content: "";
}

.burger-inner:before {
  top: -10px;
}

.burger-inner:after {
  bottom: -10px;
}

.sidebar-burger.burger-active span {
  transition-delay: 0.12s;
  transition-timing-function: cubic-bezier(0.215, 0.61, 0.355, 1);
  transform: rotate(225deg);
}

.sidebar-burger.burger-active span:before {
  top: 0;
  transition: top 0.1s ease-out, opacity 0.1s ease-out 0.12s;
  opacity: 0;
}

.sidebar-burger.burger-active span:after {
  bottom: 0;
  transition: bottom 0.1s ease-out,
    transform 0.22s cubic-bezier(0.215, 0.61, 0.355, 1) 0.12s;
  transform: rotate(-90deg);
}

.sidebar li a {
  font-size: 30px;
  font-family: "Niramit", sans-serif;
  cursor: pointer;
  color: #3a3654;
  transition: 0.3s ease-in;
}

.sidebar li a::after {
  content: "";
  width: 0;
  opacity: 0;
  position: absolute;
  left: 5px;
  margin-top: 28px;
  height: 2px;
  background: rgba(58, 54, 84, 0.4);
  transition: 0.3s ease-in;
}

.sidebar li a:hover,
.sidebar li a:active,
.sidebar li a:focus,
.sidebar li a:visited {
  margin-left: 10px;
}

.sidebar li a:hover:after,
.sidebar li a:visited:after,
.sidebar li a:active:after,
.sidebar li a:focus:after {
  opacity: 1;
  width: 10px;
}

.sidebar-legend {
  text-align: center;
  padding: 25px;
  cursor: pointer;
}

.sidebar-legend:hover a:before {
  width: 110px;
  left: 50px;
}

.sidebar-legend a {
  font-size: 40px;
  cursor: pointer;
  letter-spacing: 10px;
  font-family: "Nova Round", sans-serif;
  color: #ffffff;
  text-shadow: 4px 1px 0 #ffa79c;
}

.sidebar-legend a:before {
  content: "";
  position: absolute;
  width: 80px;
  margin-top: 50px;
  left: 65px;
  height: 1px;
  background: #fdfdfd;
  transition: 0.3s ease-in;
}

.sidebar-legend a:after {
  content: "web";
  top: 85px;
  position: absolute;
  color: #fff;
  font-size: 10px;
  font-family: Round Nova, sans-serif;
  text-shadow: none;
  left: 85px;
}

@keyframes slide-in {
  from {
    transform: translateX(-300px);
  }

  to {
    transform: translateX(0px);
  }
}

@keyframes slide-out-body {
  10% {
    transform: translateX(50px);
  }

  100% {
    transform: translateX(0px);
  }
}

@keyframes slide-out {
  from {
    transform: translateX(0px);
  }

  to {
    transform: translateX(-220px);
  }
}

@keyframes roll-in {
  from {
    transform: translateX(-0px) rotate(-360deg);
  }

  to {
    transform: translateX(0px) rotate(0);
  }
}

@keyframes roll-out {
  from {
    transform: translateX(0px) rotate(360deg);
  }

  to {
    transform: translateX(-0px) rotate(0);
  }
}

@keyframes slide-up {
  from {
    transform: translateY(900px);
  }

  to {
    transform: translateY(0px);
  }
}

@media (max-width: 700px) {
  .profiler-round {
    right: 10px;
    top: 7px;
  }
}
</style>
