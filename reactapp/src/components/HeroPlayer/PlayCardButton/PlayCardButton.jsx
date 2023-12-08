import '../../../App.css'
import React from 'react';
import ConnectionService from '../../../services/connectionService';
import { useDispatch, useSelector } from 'react-redux';
import { selectCard } from '../../../slices/playerState/playerStateSlice';

export default function PlayCardButton() {
    const dispatch = useDispatch()
    const selectedCardId = useSelector((state) => state.playerState.selectedCardId)

    const playCard = async () => {
        await ConnectionService.getConnection().invoke("PlayCard", selectedCardId)
        dispatch(selectCard(-1))
    }

    return (
        <div className="vertical-div">
           <button className="button mt-3" onClick={() => playCard()}>
                Play Card
            </button>
        </div>
    )
}
