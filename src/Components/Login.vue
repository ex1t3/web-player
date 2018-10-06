<template>
    <div class="login-form-block">
        <form>
            <div class="login-form-content">
                <div class="login-form-header">Log In</div>
                <div class="login-form-body">
                    <div class="input-group">
                        <label class="label" for="Username">Username</label>
                        <input class="input-field" type="text" required />
                    </div>
                    <div class="input-group">
                        <label class="label" for="Password">Password</label>
                        <input placeholder="" class="input-field" type="password" required />
                    </div>
                    <div class="input-group centered">
                        <button v-on:click="logIn()" class="button-gradient" type="submit">LOGIN</button>
                    </div>
                </div>
                <div class="login-form-footer"></div>
            </div>
        </form>
    </div>
</template>
<script>
import { mapGetters } from 'vuex'
import store from '../store'
import axios from 'axios'
export default {
  methods: {
    logIn() {
        this.$store.dispatch('logIn')
    }
  },
  beforeMount(){
    //this.$store.dispatch('logIn')
  },
  computed: mapGetters ({
    isLoggedIn: 'isLoggedIn'  
    }),
}
</script>

<style>
.input-group {
    width: 60%;
    margin: 25px auto;
}
.input-group .label {
    font-size: 16px;
    text-transform: uppercase;
}
.input-group .input-field {
    outline: none;
    display: block;
    width: 100%;
    border: 0;
    border-bottom: 1px solid rgba(31, 28, 236, 0.44);
    box-sizing: border-box;
    padding: 5px 5px;
    color: rgba(0, 0, 0, 0.6);
    font-family: inherit;
    font-size: inherit;
    font-weight: 500;
    line-height: inherit;
    transition: 0.3s ease;

}
.input-group .input-field:active, 
.input-group .input-field:focus {
    border-bottom: 2px solid rgba(31, 28, 236, 0.44);
}
.input-group .button-gradient:hover {
    background: linear-gradient(135deg, #9670e7 0%, #f57f52 100%);
    box-shadow: 0px 11px 8em rgba(167, 151, 255, 0.63);
}
.input-group .button-gradient {
    color: white;
    border-radius: 20px;
    border: 0;
    background: none;
    border-radius: 20px;
    padding: 1.2em 5em;
    letter-spacing: 0.35em;
    font-size: 0.7em;
    transition: background-color 0.3s, box-shadow 0.3s, color 0.3s;
    margin: 1em;
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
.login-form-block {
  position: absolute;
  right: 0;
  left: 0;
  bottom: 0;
  top: 0;
  width: 100%;
  height: 100%;
  z-index: 9999999999;
}
.login-form-header {
  padding: 20px 35px;
  border-bottom: 1px solid #f1f1f1;
  font-style: initial;
  font-weight: 300;
  font-size: 23px;
  text-transform: uppercase;
  background: -webkit-linear-gradient(
    rgb(243, 133, 120),
    rgba(31, 28, 236, 0.75)
  );
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}
.login-form-content {
  position: relative;
  margin: auto auto;
  width: 50%;
  bottom: -50%;
  min-height: 400px;
  max-width: 500px;
  min-width: 300px;
  border-radius: 5px;
  border: 1px solid rgba(111, 111, 111, 0.01);
  -moz-box-shadow: 0 19px 38px rgba(0, 0, 0, 0.18), 0 15px 12px rgba(0, 0, 0, 0);
  box-shadow: 0 19px 38px rgba(0, 0, 0, 0.18), 0 15px 12px rgba(0, 0, 0, 0);
}
</style>
