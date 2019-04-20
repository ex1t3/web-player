<template>
    <div class="login-form-block">
            <div v-bind:class="{ 'loginActive': loginActive }" class="login-form-content">
                <div class="login-form-header">
                    <div v-on:click="loginActive = true" class="left-header">Log in</div>
                    <div v-on:click="loginActive = false" class="right-header">Sign up</div>
                </div>
                <form class="login-form">
                <div class="login-form-body">
                    <div class="input-group">
                        <label class="label" for="Email">Email</label>
                        <input ref="EmailLogin" class="input-field" type="text" required />
                    </div>
                    <div class="input-group">
                        <label class="label" for="Password">Password</label>
                        <input ref="PasswordLogin" class="input-field" type="password" required />
                    </div>
                    <div class="input-group centered">
                        <button v-on:click="logIn($event)" class="button-form button-gradient" type="submit">LOGIN</button>
                    </div>
                </div>
                    <div class="separator">OR</div>
                    <div class="social-block">
                        <button @click="externalLogIn(0)" type="button" class="button-form button-google"><i class="form-button-brand fab fa-google"></i>GOOGLE</button>
                        <button @click="externalLogIn(1)" type="button" class="button-form button-facebook"><i class="form-button-brand fab fa-facebook"></i>FACEBOOK</button>
                    </div>
                </form>
                <form ref="signup" class="signup-form">
                <div class="login-form-body">
                    <div class="input-group">
                        <label class="label">Fullname</label>
                        <input name="Fullname" class="input-field" type="text" required />
                    </div>
                    <div class="input-group">
                        <label class="label">Email</label>
                        <input name="Email" class="input-field" type="email" required />
                    </div>
                    <div class="input-group">
                        <label class="label">Password</label>
                        <input name="Password" class="input-field" type="password" required />
                    </div>
                    <div class="input-group">
                        <label class="label">confirm password</label>
                        <input name="ConfirmPassword" class="input-field" type="password" required />
                    </div>
                    <div class="input-group centered">
                        <button v-on:click="register($event)" class="button-form button-gradient" type="submit">SIGN UP</button>
                    </div>
                    <!-- <div class="separator">OR</div>
                    <div class="social-block">
                        <button @click="externalLogIn(1)" type="button" class="button-form button-google"><i class="form-button-brand fab fa-google"></i>GOOGLE</button>
                        <button @click="externalLogIn(0)" type="button" class="button-form button-facebook"><i class="form-button-brand fab fa-facebook"></i>FACEBOOK</button>
                    </div> -->
                </div>
                </form>
            </div>
    </div>
</template>
<script>
// Importing necessary components and libs
import {mapGetters} from 'vuex'
import Vue from 'vue'
import store from '../store'
import axios from 'axios'
import swal from 'sweetalert'

// Initializing rootData
var rootData = new Vue({
  data: {
    user: []
  }
})

// Installing $login variable for storing reactive data
rootData.install = function () {
  Object.defineProperty(Vue.prototype, '$login', {
    get () { return rootData }
  })
}
Vue.use(rootData)

// Exporting data for current template
export default {
  store,
  data () {
    return {
      loginActive: true
    }
  },
  methods: {
    postLogin() {
      let that = this
      axios({
        method: 'GET',
        url: 'https://localhost:44343/api/Account/CheckToken',
        headers: {
          Authorization: 'Bearer ' + sessionStorage.getItem('access_token')
        }
      }).then(function (e) {
        that.$root.$emit('deactLoadingRoot')
        that.$login.user = e.data
        that.$store.dispatch('logIn')
        that.$store.dispatch('setHomePage')
      }).catch(function (e) {
        that.$root.$emit('deactLoadingRoot')
      })
    },
    logIn (e) {
      this.$root.$emit('actLoadingRoot')
      e.preventDefault()
      let loginData = {
        Email: this.$refs.EmailLogin.value,
        Password: this.$refs.PasswordLogin.value
      }
      if (loginData.Username !== '' && loginData.Password !== '') {
        let that = this
        axios({
          method: 'POST',
          url: 'https://localhost:44343/api/Account/Login',
          data: loginData,
          headers: {
            'Content-Type': 'application/json; charset=UTF-8'
          }
        }).then(function (response) {
          sessionStorage.setItem('access_token', response.data.access_token)
          that.postLogin()
        }).catch(function (e) {
          that.$root.$emit('deactLoadingRoot')
          that.$root.$emit('errorHandler', e)
        })
      } else {
        swal('Oops', 'Username and Password cannot be empty', 'warning')
      }
      this.$root.$emit('deactLoadingRoot')
    },
    externalLogIn (provider) {
      this.$root.$emit('actLoadingRoot')
      axios({
        type: 'GET',
        url: 'https://localhost:44343/api/Account/ExternalLogins?returnUrl=%2F&generateState=true'
      }).then(function (e) {
        window.location = 'https://localhost:44343' + e.data[provider].url
      })
      this.$root.$emit('deactLoadingRoot')
    },
    register (e) {
      this.$root.$emit('actLoadingRoot')
      let that = this
      e.preventDefault()
      let form = this.$refs.signup
      let data = {}
      for (let i = 0; i <= 3; i++) {
        data[form[i].name] = form[i].value
      }
      if (data.Fullname === '' || data.Email === '' || data.Password === '') {
        that.$root.$emit('deactLoadingRoot')
        swal('Oops', 'All fields should be filled', 'error')
        return
      }
      if (data.Password !== data.ConfirmPassword) {
        that.$root.$emit('deactLoadingRoot')
        swal('Oops', 'Passwords don\'t match', 'error')
        return
      }
      axios({
        method: 'POST',
        url: 'https://localhost:44343/api/Account/Register',
        data: data,
        headers: {
          'Content-Type': 'application/json; charset=UTF-8'
        }
      }).then(function (response) {
        sessionStorage.setItem('access_token', response.data.access_token)
        that.postLogin()
      }).catch(function (e) {
        that.$root.$emit('deactLoadingRoot')
        console.log(e)
        that.$root.$emit('errorHandler', e)
      })
    },
    updateUser () {
      let that = this
      axios({
        method: 'POST',
        url: 'https://localhost:44343/api/Account/UpdateUser',
        data: that.$login.user,
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization' : 'Bearer ' + sessionStorage.getItem('access_token')
        }
      })
    },
    getFacebookPicture () {
      let that = this
      window.fbAsyncInit = function () {
          FB.init({
            appId: '204022623821507',
            cookie: true,
            xfbml: true,
            version: 'v3.2'
          })

          FB.getLoginStatus(function (response) {
            let userId = response.authResponse.userID
            let src = 'https://graph.facebook.com/' + userId + '/picture'
            that.$login.user.Photo = src
            that.updateUser()
          })
      }
    },
    getGooglePicture() {
      let that = this
      // 2. Initialize the JavaScript client library.
      gapi.client.init({
        'apiKey': 'AIzaSyD7FsAX3ZcWVKDJwBldCDkqu97j9r7L9ts',
        // Your API key will be automatically added to the Discovery Document URLs.
        'discoveryDocs': ['https://people.googleapis.com/$discovery/rest'],
        // clientId and scope are optional if auth is not required.
        'clientId': '598694976413-t3qat6cf9r4vh7gh24tvt7ebn66f5vu7.apps.googleusercontent.com',
        'scope': 'profile',
      }).then(function () {
        // 3. Initialize and make the API request.
        return gapi.client.people.people.get({
          'resourceName': 'people/me',
          'personFields': 'photos'
        })
      }).then(function (response) {
        that.$login.user.Photo = response.result.photos[0].url
        that.updateUser()
      }, function (reason) {
        console.log('Error: ' + reason.result.error.message);
      })
    }
  },
  beforeMount() {
    if (window.location.hash) {
      this.$root.$emit('actLoadingRoot')
      let that = this
      var url = window.location.hash.substring(1)
      history.replaceState(null, null, ' ')
      const params = new URLSearchParams(url)
      let paramObj = {}
      for (var value of params.keys()) {
        paramObj[value] = params.get(value)
      }
      var keyNames = Object.keys(paramObj)
      console.log(keyNames)
      switch (keyNames[0]) {
        case 'access_token':
          {
            sessionStorage.setItem('access_token', paramObj.access_token)
            if (keyNames[1] === 'register_facebook') {
              (function (d, s, id) {
                let js, fjs = d.getElementsByTagName(s)[0]
                if (d.getElementById(id)) {
                  return
                }
                js = d.createElement(s)
                js.id = id;
                js.src = "https://connect.facebook.net/en_US/sdk.js"
                fjs.parentNode.insertBefore(js, fjs)
              }(document, 'script', 'facebook-jssdk'))
              if (typeof FB == 'undefined') {
                setTimeout(() => {
                  this.getFacebookPicture()
                }, 500)
              } else {
                this.getFacebookPicture()
              }
            }
            if (keyNames[1] === 'register_google') {
              (function (d, s, id) {
                let js, fjs = d.getElementsByTagName(s)[0]
                if (d.getElementById(id)) {
                  return
                }
                js = d.createElement(s)
                js.id = id
                js.src = "https://apis.google.com/js/api.js"
                fjs.parentNode.insertBefore(js, fjs)
              }(document, 'script', 'google-jssdk'))
              if (typeof gapi == 'undefined') {
                setTimeout(() => {
                  gapi.load('client', this.getGooglePicture)
                }, 500)
              } else {
                gapi.load('client', this.getGooglePicture)
              }          
            }
            break
          }
        case 'error_login':
          {
            let arr = paramObj.error_login.replace(/_/g, ' ')
            swal('Error', arr, 'error')
            break
          }
      }
      this.$root.$emit('deactLoadingRoot')
    }
  },
  mounted () {
    let that = this
    this.$root.$emit('actLoadingRoot')
    let token = sessionStorage.getItem('access_token')
    if (token !== null) {
      this.postLogin()
    } else {
      this.$root.$emit('deactLoadingRoot')
    }
  },
  computed: mapGetters({
    isLoggedIn: 'isLoggedIn'
  })
}
</script>
<style>
.swal-button:focus {
    box-shadow: none !important;
}
.swal-modal {
    border-radius: 0px !important;
}
.social-block {
    text-align: center;
}
.separator {
    position: relative;
    text-align: center;
    width: 50px;
    color: #b775b3;
    margin: 0px auto;
    margin-top: -10px;
}
.loginActive .separator {
    position: relative;
    text-align: center;
    width: 50px;
    margin: 15px auto;
    color: #b775b3;
}
.separator:before {
    content: "";
    width: 12px;
    height: 1px;
    background: #b775b3;
    position: absolute;
    top: 15px;
    right: 0;
}
.separator:after {
    content: "";
    width: 12px;
    height: 1px;
    background: #b775b3;
    position: absolute;
    top: 15px;
    left: 0;
}
.login-form, .signup-form {
  transition: 0.3s;
  position: absolute;
  font-family: 'Niramit', sans-serif;
}
.loginActive .login-form {
    width: 100%;
    opacity: 1;
    transform: translateX(0);
}
.login-form-content:not(.loginActive) {
    min-height: 550px;
}
.login-form-content:not(.loginActive) .signup-form {
    width: 100%;
    opacity: 1;
    transform: translateX(0);
}
.loginActive .signup-form {
  transform: translateX(500px);
  opacity: 0;
  width: 0;
}
.login-form-content:not(.loginActive) .login-form {
  transform: translateX(-500px);
  opacity: 0;
  width: 0;
}

.input-group {
    width: 60%;
    margin: 25px auto;
}
.input-group .label {
    font-size: 16px;
    text-transform: lowercase;
    position: relative;
    pointer-events: none;
}
.input-group .input-field {
    outline: none;
    display: block;
    width: 100%;
    border: 0;
    box-shadow: 0 1px 0 0 rgba(157, 113, 220, 0.6);
    box-sizing: border-box;
    color: rgba(0, 0, 0, 0.6);
    font-family: inherit;
    font-size: inherit;
    font-weight: 500;
    line-height: inherit;
    margin-top: -5px;
    transition: 0.5s box-shadow, border-bottom;
}
.input-group .input-field:active,
.input-group .input-field:focus {
    border-bottom: 1px transparent;
    box-shadow: 0 2px 0 0 rgba(160, 113, 216, 0.62)
}
.input-group .button-gradient:hover {
    background: linear-gradient(335deg, #9670e7 0%, #f57f52 100%);
}
.input-group .button-gradient:focus,
.input-group .button-gradient:active {
    background: #fff;
    color: #000;
}
.input-group .button-gradient {
    color: white;
    background: linear-gradient(195deg, #9670e7 0%, #e87d67 100%);
}
button {
    display: inline-block;
    zoom: 1;
    outline: 0;
    line-height: normal;
    white-space: nowrap;
    vertical-align: baseline;
    text-align: center;
    cursor: pointer;
    -webkit-user-drag: none;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
}
.button-form {
    border-radius: 20px;
    border: 0;
    position: relative;
    background: none;
    border-radius: 20px;
    padding: 1.2em 5em;
    letter-spacing: 0.35em;
    font-size: 0.7em;
    transition: background-color 0.3s, box-shadow 0.3s, color 0.3s;
    margin: 1em;
    border: 1px solid;
}
.button-google {
    border: 1px solid rgba(219, 68, 55, 0.42);
    color: #db4437;
    padding-right: 3em;
}
.button-google:hover {
    box-shadow: 1px 1px rgba(195, 94, 35, 0.4);
}
.button-facebook:hover {
    box-shadow: 1px 1px rgba(80, 73, 169, 0.4);
}
.button-facebook {
    color: #443ca7;
    padding-right: 2em;
    border: 1px solid rgba(80, 73, 169, 0.4);
}
.form-button-brand {
    position: absolute;
    left: 20px;
}
.login-form-block {
    position: fixed;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    display: flex;
}
.login-form-header {
  border-bottom: 1px solid #f1f1f1;
  display: flex;
}
.left-header, .right-header {
    padding: 20px 0;
    text-align: center;
    width: 50%;
    cursor: pointer;
    left: 0;
    background: -webkit-linear-gradient(30deg, rgb(243, 133, 120), rgba(31, 28, 236, 0.88));
    -webkit-background-clip: text;
    background-clip: text;
    -webkit-text-fill-color: transparent;
    font-style: initial;
    font-weight: 300;
    font-size: 23px;
    text-transform: uppercase;
}
.right-header {
    border-left: 1px solid #e6e6e6;
}
.login-form-content {
  position: relative;
  margin: auto auto;
  width: 50%;
  min-height: 500px;
  max-width: 500px;
  min-width: 300px;
  transition: 0.3s min-height;
  overflow: hidden;
  animation: fade-in 1s;
  border-radius: 2px;
  border: 1px solid rgba(111, 111, 111, 0.01);
  -moz-box-shadow: 0 19px 38px rgba(0, 0, 0, 0.18), 0 15px 12px rgba(0, 0, 0, 0);
  box-shadow: 0 19px 38px rgba(0, 0, 0, 0.18), 0 15px 12px rgba(0, 0, 0, 0);
}
.login-form-body {
    position: relative;
    top: 10px;
}
@keyframes fade-in {
    from{opacity: 0;}
    to{opacity:1;}
}
@media (max-width: 800px) {
    .login-form-content:not(.loginActive) {
        min-height: 580px;
    }
    .login-form-content {
        min-height: 580px;
    }
    .button-form {
        margin: 1em 0;
    }
}
</style>
