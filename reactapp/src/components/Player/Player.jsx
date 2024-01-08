import '../../App.css'
import React from 'react';
import ReadyBox from './Readybox/ReadyBox';
import { useSelector } from 'react-redux';
import ConnectionService from '../../services/connectionService';

export default function Player({ playerStateName, ally }) {
    const name = useSelector((state) => state.playerState[playerStateName].name)
    const showReady = useSelector((state) => state.playerState[playerStateName].showReady)
    const showPassed = useSelector((state) => state.playerState[playerStateName].passed)
    const showDealer = useSelector((state) => state.playerState[playerStateName].isDealer)
    const teamIndex = useSelector((state) => state.playerState.teamIndex)
    const highlightPlayer = useSelector((state) => state.playerState[playerStateName].highlightPlayer)
    const showSwapPosition = useSelector((state) => state.playerState.showSwapPosition)

    const highlightClassName = highlightPlayer ? 'highlight-player' : ''

    let thisPlayerTeamIndex = 0;
    if ((teamIndex == 1 && ally) || (teamIndex == 0 && !ally)) {
        thisPlayerTeamIndex = 1;
    }

    const swapPosition = () => {
        ConnectionService.getConnection().invoke("SwapPlayerPosition", name)
    }

    return (
        <div className="vertical-div">
            <div className='horizontal-div'>
                { name && showSwapPosition ? <button className="swap-player-button me-3" onClick={() => swapPosition()}> {'Swap'} </button> : null }
                <div className={highlightClassName + " player-name-div " + ((thisPlayerTeamIndex == 0) ? 'blue-team-div' : 'green-team-div')}>
                    {name || "Waiting for player"}
                </div>
                { showReady ? <ReadyBox playerStateName={playerStateName}/> : null }
                { showDealer ? <div className="last-bid-div me-2">Dealer</div> : null }
                { showPassed ? <div className="last-bid-div">Passed</div> : null }
            </div>
        </div>
    )
}
