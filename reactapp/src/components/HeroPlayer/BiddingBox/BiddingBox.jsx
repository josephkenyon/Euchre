import '../../../App.css'
import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import ConnectionService from '../../../services/connectionService';
import { setAlone } from '../../../slices/playerState/playerStateSlice';

export default function BiddingBox() {
    const dispatch = useDispatch()

    const alone = useSelector((state) => state.playerState.alone)

    const bid = async (bid) => {
        let goAlone = bid == 1 ? alone : false
        
        if (bid != 1) {
            dispatch(setAlone(false))
        }

        await ConnectionService.getConnection().invoke("Bid", bid, goAlone)
    }

    return (
        <div className="vertical-div bidding-div">
            <div className='horizontal-div'>
                <button className="button" value="test" onClick={() => bid(-2)}>
                    Go Under
                </button>

                <button className="button" value="test" onClick={() => bid(-1)}>
                    Pass
                </button>

                <button className="button ms-5" value="test" onClick={() => bid(1)}>
                    Pick It Up
                </button>

                <input
                    className="checkbox ms-2"
                    type="checkbox"
                    checked={alone}
                    onChange={() => dispatch(setAlone(!alone))}/>
            </div>
        </div>
    )
}
