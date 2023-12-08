import '../../../App.css'
import React from 'react';
import { useSelector } from 'react-redux';

export default function TricksTaken() {
    const teamOneTricksTaken = useSelector(state => state.playerState.teamOneTricksTaken)
    const teamTwoTricksTaken = useSelector(state => state.playerState.teamTwoTricksTaken)

    return (
        <div className="horizontal-div">
            <div className='team-name-div vertical-div'>
                {`Tricks won:`}
            </div>

            <div className='score-sum-div game-score-div blue-team-div me-2'>
                {`${teamOneTricksTaken}`}
            </div>

            <div className='score-sum-div game-score-div green-team-div'>
                {`${teamTwoTricksTaken}`}
            </div>
        </div>
    )
}
