<template>
  <!-- FAVORITE TRACKS -->
  <section>
    <h2>Favorite Tracks</h2>
    <div v-if="$main.favoriteSongs.length == 0">You don't have any favorite tracks</div>
    <div v-if="$main.favoriteSongs.length > 0 && pageType === 2" class="songs-block">
      <SongsTemplate :songs="$main.favoriteSongs" :type="pageType"/>
    </div>
  </section>
</template>
<script>
import SongsTemplate from './SongsTemplate.vue'
export default {
    props: ['pageType'],
    components: {
        SongsTemplate
    }
}
</script>

