import '../../../App.css'
import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import ConnectionService from '../../../services/connectionService';
import { setAlone } from '../../../slices/playerState/playerStateSlice';

export default function BiddingBox() {
    const dispatch = useDispatch()

    const alone = useSelector((state) => state.playerState.alone)
    const showPickItUp = useSelector((state) => state.playerState.showPickItUp)
    const showGoingUnder = useSelector((state) => state.playerState.showGoingUnder)
    const selectedCardIds = useSelector((state) => state.playerState.selectedCardIds)

    const bid = async (bid) => {
        let goAlone = bid == 1 ? alone : false
        
        if (bid == -2 ) {
            let validGoingUnderIds = true;

            if (selectedCardIds.length != 3) {
                validGoingUnderIds = false
            }

            selectedCardIds.forEach(id => {
                let suit = Math.floor(id / 6);
                let rank = id - (suit * 6)

                if (rank != 4 && rank != 5) {
                    validGoingUnderIds = false
                }
            });

            if (selectedCardIds.length != 3) {
                await ConnectionService.getConnection().invoke("ErrorMessage", "You must select 3 cards to go under.")
            }
            else if (!validGoingUnderIds) {
                await ConnectionService.getConnection().invoke("ErrorMessage", "You must select 3 nines and/or tens to go under.")
            }
            else {
                await ConnectionService.getConnection().invoke("Bid", bid, goAlone, `${selectedCardIds[0]};${selectedCardIds[1]};${selectedCardIds[2]}`)
            }
        } else {
            await ConnectionService.getConnection().invoke("Bid", bid, goAlone, "")
        }

        dispatch(setAlone(false))
    }

    return (
        <div className="vertical-div bidding-div">
            <div className='horizontal-div'>
                { !(selectedCardIds.length > 0) ? <button className="button" value="test" onClick={() => bid(-1)}>
                    Pass
                </button> : null}

                { showGoingUnder ? <button className="button ms-2" value="test" onClick={() => bid(-2)}>
                    Go Under
                </button> : null}

                { showPickItUp ? <button className="button ms-2" value="test" onClick={() => bid(1)}>
                    Pick It Up
                </button> : null}

                <label className="loner-label">
                    Go Alone:
                </label>

                <input
                    className="player-checkbox ms-2"
                    type="checkbox"
                    checked={alone}
                    onChange={() => dispatch(setAlone(!alone))}/>
            </div>
        </div>
    )
}
