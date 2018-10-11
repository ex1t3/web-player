<template>
<div class="absolute-items">
  <div class="sidebar">
    <div v-bind:class="{ 'burger-active': isActiveSidebar }" v-on:click="toggleSidebar()" class="sidebar-burger">
      <span class="burger-inner"></span>
    </div>
    <div class="sidebar-body">
      <div class="sidebar-legend">
        <a>AUDIO</a>
     </div>
     <ul>
        <li><a>Home</a></li>
        <li><a>My music</a></li>
        <li><a>Playlists</a></li>
        <li><a>Search</a></li>
      </ul>
    </div>
  </div>
  <div class="main-player-block">
    <div class="player-buttons"><i class="fas fa-step-backward"></i><i class="fas fa-play"></i><i class="fas fa-step-forward"></i></div>
    <div class="player-trackline">dgdgdgdg</div>
    <div class="player-settings">dgdgdgdg</div>
  </div>
  </div>
</template>
<script>
import store from '../store'
import { mapGetters } from 'vuex'
export default {
  store,
  data () {
    return {
      message: 'dgdgdg'
    }
  },
  methods: {
    toggleSidebar () {
      if (this.isActiveSidebar) {
        this.$store.dispatch('deactivateSidebar')
      } else {
        this.$store.dispatch('activateSidebar')
      }
    }
  },
  computed: mapGetters({
    isActiveSidebar: 'isActiveSidebar'
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
    transition: .3s;
    margin-top: 40px;
    animation: slide-out-body 1s;
}
@media (max-width: 700px) {
  .sidebar-active .content-wrapper {
    left: -220px;
    background: linear-gradient(125deg, white, #625abb73);
    filter: blur(10px);
  }
}
.sidebar-active .content-wrapper {
  animation: slide-in 1s;
}
.sidebar-active {
  left: 220px;
}
.sidebar-active .sidebar-body ul li:nth-child(1) {
  animation: slide-in 1.1s;
}
.sidebar-active .sidebar-body ul li:nth-child(2) {
  animation: slide-in 1.3s;
}
.sidebar-active .sidebar-body ul li:nth-child(3) {
  animation: slide-in 1.5s;
}
.sidebar-active .sidebar-body ul li:nth-child(4) {
  animation: slide-in 1.6s;
}

ul {
  display: block;
  list-style-type: disc;
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
  animation: slide-in 1s;
  transform: translateX(0);
}
.page:not(.sidebar-active) .sidebar {
  animation: slide-out 1s;
}
.page:not(.sidebar-active) .sidebar-burger {
  animation: roll-out 1s;
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
  animation: roll-in 1s;
  cursor: pointer;
  width: 40px;
  height: 40px;
}
.burger-inner {
  top: 50%;
  margin-top: -2px;
  transition-timing-function: cubic-bezier(.55,.055,.675,.19);
  transition-duration: .22s;
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
  transition-duration: .15s;
  transition-property: transform;
  border-radius: 4px;
  background-color: #d3b0d1;
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
  transition-delay: .12s;
  transition-timing-function: cubic-bezier(.215,.61,.355,1);
  transform: rotate(225deg);
}
.sidebar-burger.burger-active span:before {
  top: 0;
  transition: top .1s ease-out,opacity .1s ease-out .12s;
  opacity: 0;
}
.sidebar-burger.burger-active span:after {
  bottom: 0;
  transition: bottom .1s ease-out,transform .22s cubic-bezier(.215,.61,.355,1) .12s;
  transform: rotate(-90deg);
}
.sidebar li a {
  font-size: 30px;
  font-family: 'Niramit', sans-serif;
  cursor: pointer;
  color: #232121;
  transition: .3s ease-in;
}
.sidebar li a::after {
  content: "";
  width: 0;
  opacity: 0;
  position: absolute;
  left: 5px;
  margin-top: 28px;
  height: 2px;
  background: rgba(255, 255, 255, 0.61);
  transition: .3s ease-in;
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
.sidebar-legend:hover a:before{
  width: 110px;
  left: 50px;
}
.sidebar-legend a {
  font-size: 40px;
  cursor: pointer;
  letter-spacing: 10px;
  font-family: Nova Round,sans-serif;
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
  transition: .3s ease-in;
}
.sidebar-legend a:after {
  content: "web";
  top: 85px;
  position: absolute;
  color: #fff;
  font-size: 10px;
  font-family: Round Nova,sans-serif;
  text-shadow: none;
  left: 85px;
}
.main-player-block {
    position: fixed;
    -webkit-align-items: center;
    align-items: center;
    display: -webkit-flex;
    display: -ms-flexbox;
    display: flex;
    bottom: 0;
    height: 80px;
    width: 100%;
    left: 0;
    right: 0;
    background: #fff;
    z-index: 999999;
    box-shadow: 0 -3px 15px 0px rgba(51, 51, 51, 0.1);
}
.player-buttons {
    width: 160px;
    padding: 0 20px;
    -webkit-align-items: center;
    align-items: center;
    display: -webkit-flex;
    display: -ms-flexbox;
    display: flex;
}
.player-buttons i {
  font-size: 25px;
  margin: 0 20px;
  cursor: pointer;
  color: rgb(58, 54, 84)
}
@keyframes slide-in {
  from {transform: translateX(-300px);}
    to {transform: translateX(0px);}
}
@keyframes slide-out-body {
  10% {transform: translateX(50px);}
  100% {transform: translateX(0px);}
}
@keyframes slide-out {
  from {transform: translateX(0px);}
    to {transform: translateX(-220px);}
}
@keyframes roll-in {
from {transform: translateX(-0px) rotate(-360deg);}
to {transform: translateX(0px) rotate(0);}
}
@keyframes roll-out {
from {transform: translateX(0px) rotate(360deg);}
to {transform: translateX(-0px) rotate(0);}
}
@keyframes slide-up {
  from {transform: translateY(900px);}
    to {transform: translateY(0px);}
}
</style>
