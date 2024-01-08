import '../../../App.css'
import React from 'react';
import ConnectionService from '../../../services/connectionService';
import { useDispatch, useSelector } from 'react-redux';
import { selectCard } from '../../../slices/playerState/playerStateSlice';

export default function DiscardButton() {
    const dispatch = useDispatch()
    const selectedCardIds = useSelector((state) => state.playerState.selectedCardIds)

    const discardCard = async () => {
        if (selectedCardIds.length != 1) {
            await ConnectionService.getConnection().invoke("ErrorMessage", "You must select only one card to discard.")
        }
        else {
            await ConnectionService.getConnection().invoke("DiscardCard", selectedCardIds[0])
            dispatch(selectCard(-1))
        }
    }

    return (
        <div className="vertical-div">
           <button className="button mt-3" onClick={() => discardCard()}>
                Discard
            </button>
        </div>
    )
}
