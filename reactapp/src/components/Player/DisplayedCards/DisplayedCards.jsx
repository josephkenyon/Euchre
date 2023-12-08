import '../../../App.css'
import React from 'react';
import { useSelector } from 'react-redux';
import Card from '../../Card/Card';

export default function DisplayedCards({ playerStateName }) {
    const displayedCards = useSelector((state) => state.playerState[playerStateName].displayedCards)

    let index = 0;
    return (
        <div className="horizontal-div hand-div">
            { displayedCards.map(card => {
                return <Card key={index} suitIndex={card.suit} rankIndex={card.rank} zIndex={index++} small={true}/>
            })}
        </div>
    )
}
