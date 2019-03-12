<template>
  <div v-if="notifications.length > 0" class="notification-bar">
    <div :key="item.call" v-for="(item) in notifications" class="notification-bar-item">
      <div class="notification-bar-status">
        <i
          class="fas"
          v-bind:class="{'fa-check-circle success':item.notificationStatus === 'success', 'fa-exclamation-circle error': item.notificationStatus === 'error'}"
        ></i>
      </div>
      <div class="notification-bar-title">
        <h6>{{ item.notificationMessage }}</h6>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      notifications: []
    }
  },
  beforeMount() {
    this.$root.$on('notificate', this.notificate)
  },
  beforeDestroy() {
    this.$root.$off('notificate', this.notificate)     
  },
  methods: {
    notificate (status, message, time) {
      this.notifications.push({
        notificationMessage: message,
        notificationStatus: status,
        call: this.notificationCall++
      })
      let that = this
      setTimeout(function () {
        that.notifications.splice(this.notificationCall - 1, 1)
      }, time)
    }
  }
}
</script>
<style scoped>
.notification-bar {
  position: fixed;
  z-index: 9999999;
  top: 30px;
  right: 30px;
  width: 200px;
  height: 70px;
}
.notification-bar-item {
  background: #fff;
  width: 200px;
  height: 70px;
  box-shadow: 0 0 20px 11px #a09f9f1a;
  margin-bottom: 35px;
  display: flex;
  align-items: center;
  animation: slideFromRight 0.2s;
}
.notification-bar-item .notification-bar-status {
  font-size: 25px;
  margin-left: 10px;
}
.notification-bar-status .error {
  color: #f7776b;
}
.notification-bar-status .success {
  color: #6bb049;
}
.notification-bar-item .notification-bar-title {
  margin-left: 10px;
}
</style>
