<template>
  <div class="home-page-main-block"></div>
</template>
<script>
export default {
  data() {
    return {}
  },
  beforeMount() {
    this.$root.$emit("checkScreenWidth")
  }
}
</script>
