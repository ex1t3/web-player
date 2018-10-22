import {
  OPEN_HOME,
  OPEN_PROFILE,
  OPEN_MUSIC,
  OPEN_PLAYLISTS,
  OPEN_SETTINGS,
  LOG_IN,
  LOG_OUT,
  START_LOADING,
  STOP_LOADING,
  ACTIVE_SIDEBAR,
  DEACTIVE_SIDEBAR
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
  isHomePage: false,
  isSettingsPage: false,
  isProfilePage: false,
  isMusicPage: false,
  isPlaylistsPage: false,
  is: false,
  isActiveSidebar: false

}

// initialize main store getters
const getters = {
  isLoading: state => state.isLoading,
  isLoggedIn: state => state.isLoggedIn,
  isActiveSidebar: state => state.isActiveSidebar,
  isHomePage: state => state.isHomePage,
  isSettingsPage: state => state.isSettingsPage,
  isProfilePage: state => state.isProfilePage,
  isMusicPage: state => state.isMusicPage,
  isPlaylistsPage: state => state.isPlaylistsPage
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
  setProfilePage ({commit}, data) {
    commit(OPEN_PROFILE, data)
  },
  setSettingsPage ({commit}, data) {
    commit(OPEN_SETTINGS, data)
  },
  setMusicPage ({commit}, data) {
    commit(OPEN_MUSIC, data)
  },
  setPlaylistsPage ({commit}, data) {
    commit(OPEN_PLAYLISTS, data)
  },
  startLoading ({commit}, data) {
    commit(START_LOADING, data)
  },
  stopLoading ({commit}, data) {
    commit(STOP_LOADING, data)
  },
  activateSidebar ({commit}, data) {
    commit(ACTIVE_SIDEBAR, data)
  },
  deactivateSidebar ({commit}, data) {
    commit(DEACTIVE_SIDEBAR, data)
  }
}
const mutations = {
  [START_LOADING] (state) {
    state.LoadingVisability = true
  },
  [STOP_LOADING] (state) {
    state.LoadingVisability = false
  },
  [ACTIVE_SIDEBAR] (state) {
    state.isActiveSidebar = true
  },
  [DEACTIVE_SIDEBAR] (state) {
    state.isActiveSidebar = false
  },
  [LOG_IN] (state) {
    state.isLoggedIn = true
  },
  [LOG_OUT] (state) {
    state.isLoggedIn = false
  },
  [OPEN_HOME] (state) {
    state.isHomePage = true
    state.isProfilePage = false
    state.isSettingsPage = false
    state.isMusicPage = false
    state.isPlaylistsPage = false
  },
  [OPEN_PLAYLISTS] (state) {
    state.isHomePage = false
    state.isProfilePage = false
    state.isSettingsPage = false
    state.isMusicPage = false
    state.isPlaylistsPage = true
  },
  [OPEN_MUSIC] (state) {
    state.isHomePage = false
    state.isProfilePage = false
    state.isSettingsPage = false
    state.isMusicPage = true
    state.isPlaylistsPage = false
  },
  [OPEN_PROFILE] (state) {
    state.isHomePage = false
    state.isProfilePage = true
    state.isSettingsPage = false
    state.isMusicPage = false
    state.isPlaylistsPage = false
  },
  [OPEN_SETTINGS] (state) {
    state.isHomePage = false
    state.isProfilePage = false
    state.isSettingsPage = true
    state.isMusicPage = false
    state.isPlaylistsPage = false
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
