import {
  OPEN_HOME,
  LOG_IN,
  LOG_OUT,
  START_LOADING,
  STOP_LOADING
} from './mutation-types'
import Vue from 'vue'
// import axios from 'axios'
import Vuex from 'vuex'
import createLogger from 'vuex/dist/logger'
const debug = process.env.NODE_ENV !== 'production'

// set up vuex data store
Vue.use(Vuex)

// initialize main store state
const state = {
  isLoading: false,
  isLoggedIn: false,
  isHomePage: false

}

// initialize main store getters
const getters = {
  isLoading: state => state.isLoading,
  isLoggedIn: state => state.isLoggedIn,
  isHomePage: state => state.isHomePage
}
// initialize store actions
const actions = {
  logIn ({commit}, data) {
    commit(LOG_IN, data)
  },
  logOut ({commit}, data) {
    commit(LOG_OUT, data)
  },
  setHomePage ({commit}, data) {
    commit(OPEN_HOME, data)
  },
  startLoading ({commit}, data) {
    commit(START_LOADING, data)
  },
  stopLoading ({commit}, data) {
    commit(STOP_LOADING, data)
  }
}
const mutations = {
  [START_LOADING] (state) {
    state.LoadingVisability = true
  },
  [STOP_LOADING] (state) {
    state.LoadingVisability = false
  },
  [LOG_IN] (state) {
    state.isLoggedIn = true
  },
  [LOG_OUT] (state) {
    state.isLoggedIn = false
  },
  [OPEN_HOME] (state) {
    state.isHomePage = true
  }
}
// instantiate vuex store
const store = new Vuex.Store({
  state,
  getters,
  actions,
  mutations,
  strict: false,
  plugins: debug ? [createLogger()] : []
})
export default store
