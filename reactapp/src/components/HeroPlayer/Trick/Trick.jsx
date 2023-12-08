import '../../../App.css'
import React from 'react';
import { useSelector } from 'react-redux';
import Card from '../../Card/Card';

export default function Trick() {
    const allyCard = useSelector((state) => state.playerState.trickState.allyCard)
    const leftOpponentCard = useSelector((state) => state.playerState.trickState.leftOpponentCard)
    const rightOpponentCard = useSelector((state) => state.playerState.trickState.rightOpponentCard)
    const myCard = useSelector((state) => state.playerState.trickState.myCard)

    return (
        <div className="vertical-div hundred-percent-div">
            <div className="top-trick-card-div">
                <div className="trick-card-div">
                    {allyCard ? <Card suitIndex={allyCard.suit} rankIndex={allyCard.rank} small={true}/> : null}
                </div>
            </div>

            <div className="horizontal-div middle-trick-card-div">
                <div className="trick-card-div">
                    {leftOpponentCard ? <Card suitIndex={leftOpponentCard.suit} rankIndex={leftOpponentCard.rank} small={true}/> : null}
                </div>
                <div className="trick-spacer-div"/>
                <div className="trick-card-div">
                    {rightOpponentCard ? <Card suitIndex={rightOpponentCard.suit} rankIndex={rightOpponentCard.rank} small={true}/> : null}
                </div>
            </div>

            <div className="bott-trick-card-div">
                <div className="trick-card-div">
                    {myCard ? <Card suitIndex={myCard.suit} rankIndex={myCard.rank} small={true}/> : null}
                </div>
            </div>
        </div>
    )
}
