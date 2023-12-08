import '../../../App.css'
import React from 'react';
import { useSelector } from 'react-redux';

const suits = ["♠︎", "♥︎", "♣︎", "♦︎"]

export default function ScoreLog() {
    const teamOneName = useSelector(state => state.playerState.teamOneName)
    const teamTwoName = useSelector(state => state.playerState.teamTwoName)
    const teamOneScoreLog = useSelector(state => state.playerState.teamOneScoreLog) //[0,1,2,3,4,5]
    const teamTwoScoreLog = useSelector(state => state.playerState.teamTwoScoreLog) //[0,1,2,3,4,5]
    const roundBidResults = useSelector(state => state.playerState.roundBidResults) //[{ trumpSuit: 2, teamIndex: 0, bid: 15}, { trumpSuit: 1, teamIndex: 1, bid: 16}, { trumpSuit: 0, teamIndex: 0, bid: 22}]

    let scoreSum1 = 0
    teamOneScoreLog.forEach(score => scoreSum1 += +score)

    let scoreSum2 = 0
    teamTwoScoreLog.forEach(score => scoreSum2 += +score)

    const columnOne = () => {
        let index = 1;

        return <div className='score-log-column-div'>
                    <div className='team-name-div blue-team-div'>
                        {`${teamOneName}`}
                    </div>
                    <hr className="solid"></hr>
                    {teamOneScoreLog.map(score => 
                        <div key={score} className='score-log-item-div'>
                            {`${index > 1 ? ((+score) >= 0 ? "+" : "") : ""}${score}`}
                            { index++ % 2 == 0 ? <hr className="solid"></hr> : null }
                        </div>
                    )}

                    {teamOneScoreLog.length > 1 ? <div className='score-log-item-div'>
                        { index % 2 == 0 ? <hr className="solid"></hr> : null }
                        {`${scoreSum1}`}
                    </div> : null}
                </div>
    }

    const columnTwo = () => {
        return <div className='score-bid-column-div'>
                    {roundBidResults.map(result =>
                        <div key={result} className={'score-bid-row-div ' + (result.teamIndex == 0 ? "blue-team-div" : "green-team-div")}>
                            {result.teamIndex == 0 ? <div className='number-bid-result-div'> {result.bid} </div> : null}
                            <div className={"trump-bid-result-div " + ((result.trumpSuit == 0 || result.trumpSuit == 2) ? 'card-black' : 'card-red')}>
                                {`${suits[result.trumpSuit]}`}
                            </div>
                            {result.teamIndex == 1 ? <div className='number-bid-result-div'> {result.bid} </div> : null}
                        </div>
                    )}
                </div>
    }
    
    const columnThree = () => {
        let index = 1;

        return <div className='score-log-column-div'>
                    <div className='team-name-div green-team-div'>
                        {`${teamTwoName}`}
                    </div>
                    <hr className="solid"></hr>
                    {teamTwoScoreLog.map(score => 
                        <div key={score} className='score-log-item-div'>
                            {`${index > 1 ? ((+score) >= 0 ? "+" : "") : ""}${score}`}
                            { index++ % 2 == 0 ? <hr className="solid"></hr> : null }
                        </div>
                    )}

                    {teamTwoScoreLog.length > 1 ? <div className='score-log-item-div'>
                        { index % 2 == 0 ? <hr className="solid"></hr> : null }
                        {`${scoreSum2}`}
                    </div> : null}
                </div>
    }

    return (
        <div className='game-log-div'>
           {columnOne()}
           {columnTwo()}
           {columnThree()}
        </div>
    )
}
