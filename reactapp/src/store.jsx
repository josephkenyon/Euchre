import { configureStore } from '@reduxjs/toolkit'
import appStateReducer from './slices/appState/appStateSlice'
import playerStateReducer from './slices/playerState/playerStateSlice'

export const store = configureStore({
  reducer: {
    appState: appStateReducer,
    playerState: playerStateReducer
  },
})
