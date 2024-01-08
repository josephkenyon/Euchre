import '../../App.css'
import React from 'react';
import ReadyBox from './Readybox/ReadyBox';
import BiddingBox from './BiddingBox/BiddingBox';
import TrumpSelectionBox from './TrumpSelectionBox/TrumpSelectionBox';
import Hand from './Hand/Hand';
import { useSelector } from 'react-redux';
import PlayCardButton from './PlayCardButton/PlayCardButton';
import CollectTrickButton from './PlayCardButton/CollectTrickButton';
import DiscardButton from './PlayCardButton/DiscardButton';

export default function HeroPlayer() {
    const name = useSelector((state) => state.appState.playerName)
    const hasState = useSelector((state) => state.playerState.hasState)
    const showReady = useSelector((state) => state.playerState.showReady)
    const showBiddingBox = useSelector((state) => state.playerState.showBiddingBox)
    const showTrumpSelection = useSelector((state) => state.playerState.showTrumpSelection)
    const showPlayButton = useSelector((state) => state.playerState.showPlayButton)
    const showDiscardButton = useSelector((state) => state.playerState.showDiscardButton)
    const showCollectButton = useSelector((state) => state.playerState.showCollectButton)
    const showPassed = useSelector((state) => state.playerState.showPassed)
    const showIsDealer = useSelector((state) => state.playerState.showIsDealer)
    const teamIndex = useSelector((state) => state.playerState.teamIndex)
    const highlightMyself = useSelector((state) => state.playerState.highlightPlayer)

    const highlightClassName = highlightMyself ? 'highlight-player' : ''

    return (
        <div className='hundred-percent-div'>
            { hasState ?
                <div className="vertical-div hero-player-state-div">
                    <div className="horizontal-div">
                        <div className={highlightClassName + " player-name-div mb-2 " + ((teamIndex == 0) ? 'blue-team-div' : 'green-team-div')}>
                            {name || "Waiting for player"}
                        </div>
                        { showIsDealer ? <div className="last-bid-div me-2">Dealer</div> : null }
                        { showPassed ? <div className="last-bid-div">Passed</div> : null }
                        { showReady ? <ReadyBox /> : null }
                     </div>

                    { showBiddingBox ? <BiddingBox/> : null }
                    { showTrumpSelection ? <TrumpSelectionBox /> : null }
                    { <Hand/> }
                    { showPlayButton ? <PlayCardButton/> : null }
                    { showDiscardButton ? <DiscardButton/> : null }
                    { showCollectButton ? <CollectTrickButton /> : null }
                </div>
            : null}
        </div>
    )
}
