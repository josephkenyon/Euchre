import '../../../App.css'
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { selectCard } from '../../../slices/playerState/playerStateSlice';
import Card from '../../Card/Card';
import ConnectionService from '../../../services/connectionService';

export default function Hand() {
    const dispatch = useDispatch()

    const hand = useSelector((state) => state.playerState.hand)
    const displayedCards = useSelector((state) => state.playerState.displayedCards)

    const playCard = async (id) => {
        await ConnectionService.getConnection().invoke("PlayCard", id)
    }

    const onSelectCard = async (id) => {
        if (displayedCards.length == 0) {
            if (hand.length == 1) {
                playCard(-1)
            } else {
                dispatch(selectCard(id))
            }
        }
    }

    let index = 0;
    return (
        <div className="horizontal-div hero-hand-div">
            {hand.map(card => {
                return <Card key={index} suitIndex={card.suit} rankIndex={card.rank} zIndex={index++}
                    small={false} selected={displayedCards.map(card => card.id).includes(card.id) || (hand.length > 1 && card.selected)} onClick={() => onSelectCard(card.id)} onDoubleClick={() => playCard(card.id)}/>
            })}
        </div>
    )
}
