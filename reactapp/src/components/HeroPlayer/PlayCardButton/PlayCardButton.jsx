import '../../../App.css'
import React from 'react';
import ConnectionService from '../../../services/connectionService';
import { useDispatch, useSelector } from 'react-redux';
import { selectCard } from '../../../slices/playerState/playerStateSlice';

export default function PlayCardButton() {
    const dispatch = useDispatch()

    const hand = useSelector((state) => state.playerState.hand)
    const selectedCardIds = useSelector((state) => state.playerState.selectedCardIds)

    const playCard = async () => {
        if (hand.length == 1) {
            await ConnectionService.getConnection().invoke("PlayCard", hand[0].id)
            dispatch(selectCard(-1))
        }
        else if (selectedCardIds.length != 1) {
            await ConnectionService.getConnection().invoke("ErrorMessage", "You must select only one card to play.")
        }
        else {
            await ConnectionService.getConnection().invoke("PlayCard", selectedCardIds[0])
            dispatch(selectCard(-1))
        }
    }

    return (
        <div className="vertical-div">
           <button className="button mt-3" onClick={() => playCard()}>
                Play Card
            </button>
        </div>
    )
}
