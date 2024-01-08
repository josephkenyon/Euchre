import { createSlice } from '@reduxjs/toolkit'

const initialState = {
    teamOneName: "",
    teamTwoName: "",
    teamOneScoreLog: [],
    teamTwoScoreLog: [],
    roundBidResults: [],
    teamIndex: 0,
    currentBid: 0,
    highlightPlayer: false,
    hasState: false,
    alone: false,
    isReady: false,
    showReady: false,
    showPassed: false,
    showIsDealer: false,
    showSwapPosition: false,
    showBiddingBox: false,
    showPickItUp: false,
    showGoingUnder: false,
    showPlayButton: false,
    showPassButton: false,
    showDiscardButton: false,
    showCollectButton: false,
    showTrumpSelection: false,
    showTrumpIndicator: false,
    showTricksTaken: false,
    teamOneTricksTaken: 0,
    teamTwoTricksTaken: 0,
    hasSelectedCard: false,
    selectedCardIds: [],
    hand: [],
    trumpCard: null,
    trickState: {},
    allyState: {},
    leftOpponentState: {},
    rightOpponentState: {}
}

export const playerStateSlice = createSlice({
  name: 'playerState',
  initialState,
  reducers: {
    setTeamOneName: (state, action) => {
        state.teamOneName = action.payload
    },
    setTeamTwoName: (state, action) => {
        state.teamTwoName = action.payload
    },
    setTeamOneScoreLog: (state, action) => {
        state.teamOneScoreLog = action.payload
    },
    setTeamTwoScoreLog: (state, action) => {
        state.teamTwoScoreLog = action.payload
    },
    setRoundBidResults: (state, action) => {
        state.roundBidResults = action.payload
    },
    setTeamIndex: (state, action) => {
        state.teamIndex = action.payload
    },
    setCurrentBid: (state, action) => {
        state.currentBid = action.payload
    },
    setHighlightPlayer: (state, action) => {
        state.highlightPlayer = action.payload
    },
    setHasState: (state, action) => {
        state.hasState = action.payload
    },
    setAlone: (state, action) => {
        state.alone = action.payload
    },
    setIsReady: (state, action) => {
        state.isReady = action.payload
    },
    setShowReady: (state, action) => {
        state.showReady = action.payload
    },
    setShowSwapPosition: (state, action) => {
        state.showSwapPosition = action.payload
    },
    setShowPassed: (state, action) => {
        state.showPassed = action.payload
    },
    setShowIsDealer: (state, action) => {
        state.showIsDealer = action.payload
    },
    setShowBiddingBox: (state, action) => {
        state.showBiddingBox = action.payload
    },
    setShowPickItUp: (state, action) => {
        state.showPickItUp = action.payload
    },
    setTrumpCard: (state, action) => {
        state.trumpCard = action.payload
    },
    setShowGoingUnder: (state, action) => {
        state.showGoingUnder = action.payload
    },
    setShowTrumpSelection: (state, action) => {
        state.showTrumpSelection = action.payload
    },
    setShowTrumpIndicator: (state, action) => {
        state.showTrumpIndicator = action.payload
    },
    setShowTricksTaken: (state, action) => {
        state.showTricksTaken = action.payload
    },
    setTeamOneTricksTaken: (state, action) => {
        state.teamOneTricksTaken = action.payload
    },
    setTeamTwoTricksTaken: (state, action) => {
        state.teamTwoTricksTaken = action.payload
    },
    setShowPlayButton: (state, action) => {
        state.showPlayButton = action.payload
    },
    setShowPassButton: (state, action) => {
        state.showPassButton = action.payload
    },
    setShowDiscardButton: (state, action) => {
        state.showDiscardButton = action.payload
    },
    setShowCollectButton: (state, action) => {
        state.showCollectButton = action.payload
    },
    setHand: (state, action) => {
        state.hand = action.payload
        state.selectedCardIds = []
    },
    setTrickState: (state, action) => {
        state.trickState = action.payload
    },
    setAllyState: (state, action) => {
        state.allyState = action.payload
    },
    setLeftOpponentState: (state, action) => {
        state.leftOpponentState = action.payload
    },
    setRightOpponentState: (state, action) => {
        state.rightOpponentState = action.payload
    },
    selectCard: (state, action) => {
        const newHand = state.hand

        state.selectedCardIds = [];
        state.hasSelectedCard = false;

        if (state.showGoingUnder) {
            newHand.forEach(card => {
                if (card.id == action.payload) {
                    card.selected = !card.selected;
                }
            });

            newHand.forEach(card => {
                if (card.selected) {
                    state.selectedCardIds.push(card.id);
                    state.hasSelectedCard = true;
                }
            });
        } else {
            newHand.forEach(card => {
                if (card.id == action.payload) {
                    card.selected = !card.selected;
                    state.hasSelectedCard = card.selected;
                    state.selectedCardIds = [card.selected ? card.id : -1]
                } else {
                    card.selected = false;
                }
            });
        }
    
        state.selectedCardIds = state.selectedCardIds.filter(id => id != -1)

        state.hand = newHand
    },
  },
})

export const { setTeamOneName, setTeamTwoName, setTeamOneScoreLog, setTeamTwoScoreLog, setRoundBidResults, setTeamIndex, setCurrentBid, setHighlightPlayer, setHasState, setAlone, setIsReady,
    setShowReady, setShowSwapPosition, setShowPassed, setShowIsDealer, setShowBiddingBox, setShowPickItUp, setTrumpCard, setShowGoingUnder, setShowTrumpSelection, setShowTrumpIndicator, setShowTricksTaken, setTeamOneTricksTaken, setTeamTwoTricksTaken,
    setHand, setShowDiscardButton, setShowPlayButton, setShowPassButton, setShowCollectButton, setTrickState, setAllyState, setLeftOpponentState, setRightOpponentState, selectCard } = playerStateSlice.actions

export default playerStateSlice.reducer