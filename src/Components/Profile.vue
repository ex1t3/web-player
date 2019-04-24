<template>
  <div class="profile-main-block">
     <div class="profile-form">
       <form>
         <div class="input-group">
           <label for="profile_Email">Email:</label>
           <input type="email" class="content-input" :class="{'input-failed': !isEmailValid}" :value="$login.user.Email" id="profile_Email" />
         </div>
         <div class="input-group">
           <label for="profile_Fullname">Fullname:</label>
           <input type="text" class="content-input" :value="$login.user.Fullname" id="profile_Fullname" />
         </div>
         <div class="input-group">
           <label for="profile_Email">Email:</label>
           <input type="email" class="content-input" id="profile_Email" />
         </div>
         <div class="input-group">
           <label for="profile_Email">Email:</label>
           <input type="email" class="content-input" id="profile_Email" />
         </div>
         <div class="input-group">
           <label for="profile_Email">Email:</label>
           <input type="email" class="content-input" id="profile_Email" />
         </div>
         <div class="input-group">
         <button type="button" class="button-form">SAVE</button>
         </div>
       </form>
      </div> 
  </div>
</template>
<script>
export default {
  data () {
    return {
      isEmailValid: this.checkEmailValidity()
    }
  },
  methods: {
    checkEmailValidity () {
      let re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      this.isEmailValid = re.test(String(this.$login.user.Email).toLowerCase())
    }
  }
}
</script>
<style scoped>
.content-input {
    font-size: 20px;
    margin-top: 10px;
    color: #3a3654;
    border-radius: 5px;
    box-shadow: 0 6px 10px 0 rgba(0, 0, 0, 0.1);
    border: 0;
    padding: 22px 18px;
    width: 100%;
}
.input-failed {
  box-shadow: 0 6px 10px 0 rgba(255, 0, 0, 0.21)
}
.input-group {
  width: 50%;
  margin: 15px auto;
  text-align: center;
}
.input-group label {
  display: block;
  text-align: left;
}
.content-input:active,
.content-input:focus {
  box-shadow: 0 6px 20px 0 #7f68ff4a;
}
</style>

