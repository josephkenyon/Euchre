import '../../../App.css'
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setShowLog } from '../../../slices/appState/appStateSlice';
import TrumpIndicator from '../TrumpIndicator/TrumpIndicator';
import GameScore from '../GameScore/GameScore';
import TricksTaken from '../TricksTaken/TricksTaken';
import ConnectionService from '../../../services/connectionService';

export default function GameScoreHeader() {
    const dispatch = useDispatch()

    const showGameLog = useSelector(state => state.appState.showLog)
    const hideGameLog = !showGameLog

    
    const showTrumpIndicator = useSelector(state => state.playerState.showTrumpIndicator)
    const showTricksTaken = useSelector(state => state.playerState.showTricksTaken)

    return (
        <div className="score-header-div horizontal-div">
            <div className="horizontal-div">
                <div className="show-log-button-div">
                    {hideGameLog
                        ? <button onClick={() => dispatch(setShowLog(true))}>
                            Show Score Sheet {">>"}
                        </button> : null}
                </div>

                <button className="error-div" onClick={() => ConnectionService.closeConnection()}>
                    Leave Game
                </button>
            </div>

            {showTrumpIndicator && hideGameLog ? <TrumpIndicator /> : null}
            {showTricksTaken && hideGameLog ? <TricksTaken /> : null}

            <GameScore />
        </div>
    )
}
