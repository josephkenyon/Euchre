import '../../../App.css'
import React from 'react';
import ConnectionService from '../../../services/connectionService';

export default function CollectTrickButton() {
    const collectTrick = async () => {
        await ConnectionService.getConnection().invoke("CollectTrick", true)
    }

    return (
        <div className="vertical-div">
           <button className="button mt-3" onClick={() => collectTrick()}>
                Collect Trick
            </button>
        </div>
    )
}
