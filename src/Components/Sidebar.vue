<template>
<div class="sidebar">
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
</template>
<script>
export default {
  data () {
    return {
      message: 'dgdgdg'
    }
  }
}
</script>
<style>
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
ul li {
  list-style: none;
  padding-left: 20px;
  padding-bottom: 10px;
}
.sidebar-legend {
  text-align: center;
  padding: 25px;
  cursor: pointer;
}
.sidebar-legend:hover a:before{
  width: 110px;
  left: 
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
  margin-top: 10px;
  position: absolute;
  color: #fff;
  font-size: 10px;
  font-family: Round Nova,sans-serif;
  text-shadow: none;
  left: 85px;
}
.sidebar {
  -ms-flex-direction: column;
  -webkit-flex-direction: column;
  background-color: rgb(243, 133, 120);
  bottom: 80px;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;
  flex-direction: column;
  left: 0;
  position: fixed;
  top: 0;
  width: 220px;
  z-index: 100;
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
</style>
