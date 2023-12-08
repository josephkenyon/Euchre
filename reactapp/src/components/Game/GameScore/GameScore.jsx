import '../../../App.css'
import React from 'react';
import { useSelector } from 'react-redux';

export default function GameScore() {
    const teamOneName = useSelector(state => state.playerState.teamOneName)
    const teamTwoName = useSelector(state => state.playerState.teamTwoName)
    const teamOneScoreLog = useSelector(state => state.playerState.teamOneScoreLog)
    const teamTwoScoreLog = useSelector(state => state.playerState.teamTwoScoreLog)

    let scoreSum1 = 0
    teamOneScoreLog.forEach(score => scoreSum1 += +score)

    let scoreSum2 = 0
    teamTwoScoreLog.forEach(score => scoreSum2 += +score)


    return (
        <div className="game-score-div horizontal-div">
            <div className='team-name-div blue-team-div vertical-div'>
                {`${teamOneName}:`}
            </div>

            <div className='score-sum-div me-5'>
                {`${scoreSum1}`}
            </div>

            <div className='ms-5 team-name-div green-team-div'>
                {`${teamTwoName}:`}
            </div>
            <div className='score-sum-div me-4'>
                {`${scoreSum2}`}
            </div>
        </div>
    )
}
