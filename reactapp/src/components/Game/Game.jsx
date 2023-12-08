import '../../App.css'
import React from 'react';
import Player from '../Player/Player';
import HeroPlayer from '../HeroPlayer/HeroPlayer';
import ScoreLog from './GameLog/GameLog';
import Trick from '../HeroPlayer/Trick/Trick';
import ReactSlidingPane from 'react-sliding-pane';
import { useDispatch, useSelector } from 'react-redux';
import { setShowLog } from '../../slices/appState/appStateSlice';
import "react-sliding-pane/dist/react-sliding-pane.css";
import GameScoreHeader from './GameScoreHeader/GameScoreHeader';

export default function Game() {
    const dispatch = useDispatch()
    const showGameLog = useSelector(state => state.appState.showLog)

    return (
        <div className='horizontal-game-div'>
            <ReactSlidingPane
                className='score-log-pane' overlayClassName="score-log-overlay"
                from="left" width='fit-content' hideHeader={true} isOpen={showGameLog}
                onRequestClose={() => {
                    dispatch(setShowLog(false))
                }}>

                <div className="close-log-button-div">
                    <button onClick={() => dispatch(setShowLog(false))}>
                        {"<<"} Hide Score Sheet
                    </button>
                </div>
                                    
                <ScoreLog/>
            </ReactSlidingPane>
            <div className='game-div'>
                <GameScoreHeader/>
                
                <div className='players-div'>
                    <div className='ally-div'>
                        <Player playerStateName={'allyState'} ally={true} />
                    </div>
                    <div className="opponents-div hundred-percent-div">
                        <div className='opponent-div hundred-percent-div'>
                            <Player playerStateName={'leftOpponentState'} ally={false} />
                        </div>

                        <Trick/>

                        <div className='opponent-div'>
                            <Player playerStateName={'rightOpponentState'} ally={false} />
                        </div>
                    </div>
                    <div className='hero-player-div'>
                        <HeroPlayer/>
                    </div>
                </div>
            </div>
        </div>
    )
}
