<template>
    <div class="search-panel">
        Search
        <form>
            <input v-on:keydown="searching()" id="SearchInput" v-model="searchVal" required="required" class="player-input" placeholder="Type here..." ref="search"/>
        </form>
    </div>
</template>
<script>
export default {
  data () {
    return {
      searchVal: ''
    }
  },
    methods: {
      searching () {
        this.$refs.search.value = this.searchVal
      }
    }
}
</script>
<style>
.player-input {
    padding: 12px;
    border: 1px solid #ebeaed;
    font-size: 25px;
    width: 50%;
    border-radius: 2px;
    transition: box-shadow .3s;
}
.player-input:active,
.player-input:focus {
    box-shadow: 0px 0px 20px #9f8be02e
}
.search-panel {
  padding-left: 20px;
}
</style>
