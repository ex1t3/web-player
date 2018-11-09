<template>
    <div>
        Search
        <form>
            <input type="text" ref="search"/>
        </form>
    </div>
</template>
<script>
export default {
    data () {
      return {

      }
    },
    mounted () {
      this.$refs.search.focus()
    }
}
</script>
