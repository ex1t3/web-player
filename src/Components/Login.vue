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
                        <label class="label" for="Username">Username</label>
                        <input id="UsernameLogin" class="input-field" type="text" required />
                    </div>
                    <div class="input-group">
                        <label class="label" for="Password">Password</label>
                        <input id="PasswordLogin" class="input-field" type="password" required />
                    </div>
                    <div class="input-group centered">
                        <button v-on:click="logIn()" class="button-form button-gradient" type="submit">LOGIN</button>
                    </div>
                </div>
                    <div class="separator">OR</div>
                    <div class="social-block">
                        <button type="button" class="button-form button-google"><i class="form-button-brand fab fa-google"></i>GOOGLE</button>
                        <button type="button" class="button-form button-facebook"><i class="form-button-brand fab fa-facebook"></i>FACEBOOK</button>
                    </div>
                </form>
                <form class="signup-form">
                <div class="login-form-body">
                    <div class="input-group">
                        <label class="label" for="Username">Username</label>
                        <input id="UsernameSignup" class="input-field" type="text" required />
                    </div>
                    <div class="input-group">
                        <label class="label" for="EmailSignup">Email</label>
                        <input id="EmailSignup" class="input-field" type="email" required />
                    </div>
                    <div class="input-group">
                        <label class="label" for="PasswordSignup">Password</label>
                        <input id="PasswordSignup" class="input-field" type="password" required />
                    </div>
                    <div class="input-group">
                        <label class="label" for="ConfirmPasswordSignup">Confrim password</label>
                        <input id="ConfirmPasswordSignup" class="input-field" type="password" required />
                    </div>
                    <div class="input-group centered">
                        <button v-on:click="register()" class="button-form button-gradient" type="submit">SIGN UP</button>
                    </div>
                    <div class="separator">OR</div>
                    <div class="social-block">
                        <button type="button" class="button-form button-google"><i class="form-button-brand fab fa-google"></i>GOOGLE</button>
                        <button type="button" class="button-form button-facebook"><i class="form-button-brand fab fa-facebook"></i>FACEBOOK</button>
                    </div>
                </div>
                </form>
            </div>
    </div>
</template>
<script>
import {mapGetters} from 'vuex'
import store from '../store'
// import axios from 'axios'
export default {
  store,
  data () {
    return {
      loginActive: true
    }
  },
  methods: {
    logIn () {
      this.$store.dispatch('logIn')
    },
    register () {
      this.$store.dispatch('logIn')
    }
  },
  beforeMount () {
    // this.$store.dispatch('logIn')
  },
  computed: mapGetters({
    isLoggedIn: 'isLoggedIn'
  })
}
</script>
<style>
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
.login-form .input-group {
    margin: 25px auto;
}
.input-group {
    width: 60%;
    margin: 10px auto;
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
    box-shadow: 0 1px 0 0 rgba(31, 28, 236, 0.41);
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
    box-shadow: 0 2px 0 0 rgba(31, 28, 236, 0.68);
}
.input-group .button-gradient:hover {
    background: linear-gradient(335deg, #9670e7 0%, #f57f52 100%);
    box-shadow: 0px 11px 8em rgba(167, 151, 255, 0.63);
}
.input-group .button-gradient {
    color: white;
    background: linear-gradient(195deg, #9670e7 0%, #e87d67 100%);
}
.centered {
    text-align: center;
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
    background: -webkit-linear-gradient(      rgb(243, 133, 120),      rgba(31, 28, 236, 0.75)    );
    -webkit-background-clip: text;
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
}
</style>
