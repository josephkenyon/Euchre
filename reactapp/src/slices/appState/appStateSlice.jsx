import { createSlice } from '@reduxjs/toolkit'

const initialState = {
    connection: null,
    gameName: '',
    playerName: '',
    loading: false,
    showLog: false
}

export const appStateSlice = createSlice({
  name: 'appState',
  initialState,
  reducers: {
    setConnection: (state, action) => {
        state.connection = action.payload
    },
    setGameName: (state, action) => {
        state.gameName = action.payload
    },
    setPlayerName: (state, action) => {
        state.playerName = action.payload
    },
    setLoading: (state, action) => {
        state.loading = action.payload
    },
    setShowLog: (state, action) => {
        state.showLog = action.payload
    }
  },
})

export const { setConnection, setGameName, setPlayerName, setLoading, setShowLog } = appStateSlice.actions

export default appStateSlice.reducer