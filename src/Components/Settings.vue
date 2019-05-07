<template>
  <div class="settings-main-block">
    <div class="tabs">
      <input type="radio" id="tab1" name="tab-control" checked>
      <input type="radio" id="tab2" name="tab-control">
      <div>
        <ul>
          <li title="Change Password">
            <label for="tab1" role="button">
              <svg aria-hidden="true" focusable="false" role="img" xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 448 512" class="">
                <path
                  d="M400 256H152V152.9c0-39.6 31.7-72.5 71.3-72.9 40-.4 72.7 32.1 72.7 72v16c0 13.3 10.7 24 24 24h32c13.3 0 24-10.7 24-24v-16C376 68 307.5-.3 223.5 0 139.5.3 72 69.5 72 153.5V256H48c-26.5 0-48 21.5-48 48v160c0 26.5 21.5 48 48 48h352c26.5 0 48-21.5 48-48V304c0-26.5-21.5-48-48-48z"
                  class=""></path>
              </svg>
              <br>
              <span>Change Password</span>
            </label>
          </li>
          <li @click="getActiveSessions" title="Active Sessions">
            <label for="tab2" role="button">
              <svg aria-hidden="true" focusable="false" role="img" xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 384 512" class="">
                <path
                  d="M336 64h-80c0-35.3-28.7-64-64-64s-64 28.7-64 64H48C21.5 64 0 85.5 0 112v352c0 26.5 21.5 48 48 48h288c26.5 0 48-21.5 48-48V112c0-26.5-21.5-48-48-48zM96 424c-13.3 0-24-10.7-24-24s10.7-24 24-24 24 10.7 24 24-10.7 24-24 24zm0-96c-13.3 0-24-10.7-24-24s10.7-24 24-24 24 10.7 24 24-10.7 24-24 24zm0-96c-13.3 0-24-10.7-24-24s10.7-24 24-24 24 10.7 24 24-10.7 24-24 24zm96-192c13.3 0 24 10.7 24 24s-10.7 24-24 24-24-10.7-24-24 10.7-24 24-24zm128 368c0 4.4-3.6 8-8 8H168c-4.4 0-8-3.6-8-8v-16c0-4.4 3.6-8 8-8h144c4.4 0 8 3.6 8 8v16zm0-96c0 4.4-3.6 8-8 8H168c-4.4 0-8-3.6-8-8v-16c0-4.4 3.6-8 8-8h144c4.4 0 8 3.6 8 8v16zm0-96c0 4.4-3.6 8-8 8H168c-4.4 0-8-3.6-8-8v-16c0-4.4 3.6-8 8-8h144c4.4 0 8 3.6 8 8v16z"
                  class=""></path>
              </svg>
              <br>
              <span>Active Sessions</span>
            </label>
          </li>
        </ul>
        <div class="slider">
          <div class="indicator"></div>
        </div>
      </div>
      <div class="content">
        <section>
          <h2>Change Password</h2>
          <form class="content-form">
            <div class="content-input-group">
              <input v-model="currentPassword" type="password" name="CurrentPassword"
                class="content-input" :class="{'input-failed': false}" id="settings_CurrentPassword" />
              <label for="settings_CurrentPassword">Current Password:</label>
            </div>
            <div class="content-input-group">
              <input v-model="newPassword" type="password" name="NewPassword" class="content-input"
                :class="{'input-failed': false}" id="settings_NewPassword" />
              <label for="settings_NewPassword">New Password:</label>
            </div>
            <div class="content-input-group">
              <input v-model="confirmPassword" type="password" name="ConfirmPassword" class="content-input"
                :class="{'input-failed': false}" id="settings_ConfirmPassword" />
              <label for="settings_ConfirmPassword">Confirm Password:</label>
            </div>
            <div class="content-input-group">
              <button @click="changePassword" type="button" class="button-form">CHANGE</button>
            </div>
          </form>
        </section>
        <section>
          <h2>Active Sessions</h2>
          <div class="sessions-block">
            <div class="sessions-header-block">
              <div class="sessions-header-item">#</div>
              <div class="sessions-header-item">IP</div>
              <div class="sessions-header-item">Device Type</div>
              <div class="sessions-header-item">Valid</div>
              <div class="sessions-header-item"></div>
            </div>
            <div :class="{'current-session': index === 0}" class="sessions-content-block" :key="index" v-for="(item, index) in sessions">
              <div class="sessions-content-item">{{ index + 1 }}</div>
              <div class="sessions-content-item"><i title="IP Address" class="mobile-icon fas fa-map-marker-alt"></i> {{ item.IpAddress }}</div>
              <div class="sessions-content-item"><i title="Device type" class="mobile-icon fas fa-tablet-alt"></i> {{ index === 0 ? '[Current Session]' : item.UserAgent }}</div>
              <div class="sessions-content-item"><i title="Valid for" class="mobile-icon fas fa-calendar-check"></i> {{ item.ExpiresIn + ' days' }}</div>
              <div class="sessions-content-item"><i title="Terminate session" @click="terminateSession(index)" class="far fa-trash-alt"></i></div>
            </div>
          </div>
        </section>
      </div>
    </div>
  </div>
</template>
<script>
// importing necessary components and libs
import axios from 'axios'
import swal from 'sweetalert'

export default {
  data () {
    return {
      currentTabIndex: 0,
      currentPassword: '',
      newPassword: '',
      sessions: [],
      confirmPassword: ''
    }
  },
  mounted () {
    this.$root.$emit('deactLoadingRoot')
    this.getActiveSessions()
  },
  methods: {
    terminateSession(index) {
      let that = this
      if (index === 0) {
        swal({
          title: 'Are you sure?',
          text: 'This will log you out from current session',
          icon: 'warning',
          buttons: true,
          dangerMode: true,
        }).then((willLogOut) => {
          if (willLogOut) {
            axios({
              method: 'POST',
              url: 'https://localhost:44343/api/Account/Logout',
              headers: {
                Authorization: 'Bearer ' + localStorage.getItem('access_token')
              }
            }).then(function (e) {
              localStorage.removeItem('access_token')
              that.$store.dispatch('logOut')
            }).catch(function (e) {
              that.$root.$emit('errorHandler', e)
            })
          }
        })
      } else {
        let item = this.sessions[index].Id
        axios({
          method: 'POST',
          url: 'https://localhost:44343/api/Account/TerminateSession',
          data: item,
          headers: {
            'Content-Type': 'application/json; charset=UTF-8',
            Authorization: 'Bearer ' + localStorage.getItem('access_token')
          }
        }).then(function (e) {
          that.sessions.splice(index, 1)
          that.$root.$emit('notificate', 'success', 'Session successfully terminated', 3000)
        }).catch(function (e) {
         // that.$root.$emit('errorHandler', e)
        })
      }
    },
    getActiveSessions () {
      let that = this    
      axios({
        method: 'GET',
        url: 'https://localhost:44343/api/Account/GetActiveSessions',
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization' : 'Bearer ' + localStorage.getItem('access_token')
        }
      }).then(function (e) {
        that.sessions = e.data
      }).catch(function (e) {
        
      })
    },
    changePassword () {
      let that = this
      if (this.newPassword.length < 4) {
        this.$root.$emit('notificate', 'error', 'Password should be at least 4 symbols', 3000)
        return false
      }
      if (this.newPassword !== this.confirmPassword) {
        this.$root.$emit('notificate', 'error', 'Passwords don\'t match', 3000)   
        return false    
      }
      let obj = {
        CurrentPassword: this.currentPassword,
        NewPassword: this.newPassword,
        ConfirmPassword: this.confirmPassword
      }
      axios({
        method: 'POST',
        url: 'https://localhost:44343/api/Account/ChangePassword',
        data: obj,
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization' : 'Bearer ' + localStorage.getItem('access_token')
        }
      }).then(function (e) {
        if (e.data) {
          that.$root.$emit('notificate', 'success', 'Password successfully changed', 3000)
        } else {
          that.$root.$emit('notificate', 'error', 'Wrong current password', 3000)       
        }
      }).catch(function (e) {
        
      })
    }
  }
}
</script>
<style scoped>
.mobile-icon {
  display: none;
}


.sessions-header-block,
.sessions-content-block {
    display: flex;
    padding: 20px 5px;
    align-items: center;
    min-height: 50px;
    position: relative;
}

.sessions-header-item,
.sessions-content-item {
  width: 15%;
}

.sessions-header-item:first-of-type,
.sessions-content-item:first-of-type,
.sessions-header-item:last-of-type,
.sessions-content-item:last-of-type {
  width: 30px;
}

.sessions-content-item:nth-child(3),
.sessions-header-item:nth-child(3) {
  width: 70%
}

.sessions-content-block {
    margin-bottom: 30px;
    box-shadow: 0 0px 17px 8px #dddddd38;
}

.sessions-content-item {
  padding: 0 2px;
  font-size: 14px;
}

.sessions-content-item .fa-trash-alt {
  cursor: pointer;
}

.current-session {
  background: rgba(58, 54, 84, 0.05);
  margin-bottom: 35px;
  box-shadow: 0 0px 20px 8px #dddddd38;
}

@media (max-width: 700px) {
  .sessions-header-block {
    display: none;
  }

  .sessions-content-block {
    font-size: 10px;
    display: block;
    padding: 10px 5px;
    margin: 20px 0;
  }

  .mobile-icon {
    display: initial;
    margin: 0 10px;
  }

  .sessions-content-item {
    font-size: 12px;
    margin: 15px 0;
    width: 90% !important;
  }

  .sessions-content-item i {
    font-size: 14px;
  }

  .sessions-content-item:last-of-type {
    position: absolute;
    right: 10px;
    top: 50%;
    transform: translateY(-50%);
    font-size: 15px;
    width: 20px !important;
    margin: 0;
  }

  .sessions-content-item:nth-child(3) {
    display: flex;
    align-items: center;
  }

  .sessions-content-block:not(.current-session) .sessions-content-item:nth-child(3) {
    font-size: 11px;
  }

  .sessions-content-item:first-of-type {
    display: none;
  }
}
</style>


