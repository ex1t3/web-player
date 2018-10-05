<template>
<div class="sidebar">
  <div class="sidebar-body">
    <ul>
      <li>Home</li>
      <li>My music</li>
      <li>Playlists</li>
      <li>Search</li>
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
}
ul li {
  list-style: none;
}
.sidebar {
  min-width: 400px;
  display: block;
  height: 100%;
}
.sidebar-body {
  background-color: lightgrey;
}
</style>
