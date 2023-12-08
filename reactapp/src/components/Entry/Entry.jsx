import '../../App.css'
import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setConnection, setGameName, setPlayerName } from '../../slices/appState/appStateSlice';
import { setAllyState, setCurrentBid, setDisplayedCards, setHand, setHasState, setHighlightPlayer, setIsReady, setLastBid, setLeftOpponentState, setRightOpponentState, setRoundBidResults, setShowBiddingBox,
    setShowCollectButton,
    setShowLastBid, setShowPlayButton, setShowReady, setShowSwapPosition, setShowTricksTaken, setShowTrumpIndicator, setShowTrumpSelection, setTeamIndex, setTeamOneName, setTeamOneScoreLog, setTeamOneTricksTaken, setTeamTwoName, setTeamTwoScoreLog, setTeamTwoTricksTaken, setTrickState } from '../../slices/playerState/playerStateSlice';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import ConnectionService from '../../services/connectionService';
import { toast } from 'react-toastify';
import { TailSpin } from 'react-loader-spinner';

export default function Entry() {
    const dispatch = useDispatch()
  
    const gameName = useSelector((state) => state.appState.gameName)
    const playerName = useSelector((state) => state.appState.playerName)

    const [submitting, setSubmitting] = useState(false)

    const joinGame = async (e) => {
        let connection;

        try {
            e.preventDefault();

            setSubmitting(true)

            connection = new HubConnectionBuilder()
                .withUrl("https://www.playpinochle.games:7177/hub")
                .configureLogging(LogLevel.Information)
                .build();
        } catch (err) {
            console.error(err)
            sendMessage(err.toString())
            console.log(err)
        } finally {
            setSubmitting(false)
        }

        connection.on("ErrorMessage", (message) => {
            console.error(message)
        })

        connection.on("SendMessage", (messageObject) => {
            const { content, code } = messageObject
            sendMessage(content, code)
        })

        connection.on("UpdatePlayerState", (newState) => {
            dispatch(setTeamOneName(newState.teamOneScoreList.shift()))
            dispatch(setTeamTwoName(newState.teamTwoScoreList.shift()))
            dispatch(setTeamOneScoreLog(newState.teamOneScoreList))
            dispatch(setTeamTwoScoreLog(newState.teamTwoScoreList))
            dispatch(setRoundBidResults(newState.roundBidResults))
            dispatch(setTeamIndex(newState.teamIndex))
            dispatch(setLastBid(newState.lastBid))
            dispatch(setCurrentBid(newState.currentBid))
            dispatch(setHighlightPlayer(newState.highlightPlayer))
            dispatch(setIsReady(newState.isReady))
            dispatch(setHasState(true))
            dispatch(setShowReady(newState.showReady))
            dispatch(setShowSwapPosition(newState.showSwapPosition))
            dispatch(setShowLastBid(newState.showLastBid))
            dispatch(setShowBiddingBox(newState.showBiddingBox))
            dispatch(setShowTrumpSelection(newState.showTrumpSelection))
            dispatch(setShowTrumpIndicator(newState.showTrumpIndicator))
            dispatch(setShowTricksTaken(newState.showTricksTaken))
            dispatch(setTeamOneTricksTaken(newState.teamOneTricksTaken))
            dispatch(setTeamTwoTricksTaken(newState.teamTwoTricksTaken))
            dispatch(setShowPlayButton(newState.showPlayButton))
            dispatch(setShowCollectButton(newState.showCollectButton))
            dispatch(setHand(newState.hand))
            dispatch(setDisplayedCards(newState.displayedCards))
            dispatch(setTrickState(newState.trickState))
            dispatch(setAllyState(newState.allyState))
            dispatch(setLeftOpponentState(newState.leftOpponentState))
            dispatch(setRightOpponentState(newState.rightOpponentState))
        })

        connection.onclose(_ => {
            ConnectionService.setConnection(null)
            
            dispatch(setConnection(false))
            dispatch(setHasState(false))
        })

        try {
            await connection.start()
        } catch (e) {
            console.error("Unable to connect to the server.")
            return;
        }

        ConnectionService.setConnection(connection)
        dispatch(setConnection(true))

        try {
            await connection.invoke("JoinGame", { GameName: gameName, PlayerName: playerName })
        } catch (e) {
            console.error("An error occurred trying to join the game.")
        }
    }

    function sendMessage(message, code) {
        const className = code == 0 ? 'blue-team-div' : code == 1 ? 'green-team-div' : 'error-div'

        const props = {
            position: "top-left",
            autoClose: 3000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: false,
            className: className + ' message-div',
            icon: '',
            progress: undefined,
            theme: "colored"
        }

        if (code == 0) {
            toast.info(message, props);
        } else if (code == 1) {
            toast.success(message, props);
        } else if (code == 2) {
            toast.error(message, props);
        }
    }

    return (
        <form className="vertical-div entry-div" onSubmit={joinGame}>
            <div className="entry-title-div">Euchre</div>
            <input className="entry-input" type="text" value={gameName} placeholder="Enter a game name"
                onChange={event => dispatch(setGameName(event.target.value.toUpperCase()))}/>

            <input className="entry-input" type="text" value={playerName} placeholder="Enter your player name"
                onChange={event => dispatch(setPlayerName(event.target.value))}/>

            <button className="entry-button" disabled={submitting} type="submit">
                Join Game
                <TailSpin
                    height="30"
                    width="30"
                    color="#ffffff"
                    ariaLabel="tail-spin-loading"
                    radius="1"
                    wrapperStyle={{}}
                    wrapperClass=""
                    visible={submitting}/>
            </button>
        </form>
    );
}

