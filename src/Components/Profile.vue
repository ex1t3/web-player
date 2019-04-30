<template>
  <div class="profile-main-block">
    <div class="profile-form">
      <form>
        <div class="input-group">
          <label for="profile_Email">Email:</label>
          <input @keyup="checkEmailValidity" v-model="$login.user.Email" type="email" name="Email" class="content-input"
            :class="{'input-failed': !isEmailValid}" id="profile_Email" />
        </div>
        <div class="input-group">
          <label for="profile_Fullname">Fullname:</label>
          <input v-model="$login.user.Fullname" type="text" name="Fullname" class="content-input"
            :class="{'input-failed': !checkStringValidity($login.user.Fullname)}" id="profile_Fullname" />
        </div>
        <div class="input-group">
          <label class="radio-label" for="profile_Gender">Gender:</label>
          <span @click="$login.user.IsMale = true" class="radio-block" :class="{'checked-radio': $login.user.IsMale}">Male
            <input class="radio-item" type="radio" name="Gender"
              id="profile_Male" />
            <i class="fas fa-check"></i>
          </span>
          <span @click="$login.user.IsMale = false" class="radio-block" :class="{'checked-radio': !$login.user.IsMale}">Female
            <input class="radio-item" type="radio" name="Gender"
              id="profile_Female" />
            <i class="fas fa-check"></i>
          </span>
        </div>
        <div class="input-group">
          <label for="profile_Country">Country:</label>
          <input  v-model="$login.user.Country" type="text" name="Country" class="content-input"
            :class="{'input-failed': !checkStringValidity($login.user.Country)}" id="profile_Country" />
        </div>
        <div class="input-group">
          <button @click="updateUser" type="button" class="button-form">SAVE</button>
        </div>
      </form>
    </div>
  </div>
</template>
<script>

// importing libraries and tools
import axios from 'axios'

export default {
  data () {
    return {
      isEmailValid: this.checkEmailValidity(),
    }
  },
  mounted () {
    this.checkEmailValidity()
    this.$root.$emit('deactLoadingRoot')
  },
  methods: {
    checkEmailValidity () {
      let re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      this.isEmailValid = re.test(String(this.$login.user.Email).toLowerCase())
      return this.isEmailValid
    },
    checkStringValidity (value) {
      if (value == null) return false
      if (value.length >= 3) return true
      else return false
    },
    updateUser () {
      let that = this
      if (!this.checkEmailValidity() || !this.checkStringValidity(this.$login.user.Fullname) || !this.checkStringValidity(this.$login.user.Country)) {
        return this.$root.$emit('notificate', 'error', 'All values should be filled correctly', 3000)
      }
      axios({
        method: 'POST',
        url: 'https://localhost:44343/api/Account/UpdateUser',
        data: that.$login.user,
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization' : 'Bearer ' + sessionStorage.getItem('access_token')
        }
      }).then(function (e) {
        if (e.data) {
          that.$root.$emit('notificate', 'success', 'Profile successfully updated', 3000)
        }
      }).catch(function (e) {
        
      })
    }
  }
}
</script>
<style scoped>
.profile-main-block {
  animation: fade-in .5s
}
</style>

