import '../../../App.css'
import React from 'react';
import { useSelector } from 'react-redux';

const suits = ["♠︎", "♥︎", "♣︎", "♦︎"]

export default function TrumpIndicator() {
    const roundBidResults = useSelector(state => state.playerState.roundBidResults) //[{ trumpSuit: 2, teamIndex: 0, bid: 15}, { trumpSuit: 1, teamIndex: 1, bid: 16}, { trumpSuit: 0, teamIndex: 0, bid: 22}]
    const lastRoundBidResult = roundBidResults[roundBidResults.length - 1]

    return  <div className={'trump-indicator-div ' + (lastRoundBidResult.teamIndex == 0 ? "blue-team-div" : "green-team-div")}>
                <div className='trump-indicator-value-div'> Winning bid: {lastRoundBidResult.bid} </div>

                <div className={"trump-indicator-suit-div ms-1 " + ((lastRoundBidResult.trumpSuit == 0 || lastRoundBidResult.trumpSuit == 2) ? 'card-black' : 'card-red')}>
                    {`${suits[lastRoundBidResult.trumpSuit]}`}
                </div>

            </div>
}
