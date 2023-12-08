import '../../../App.css'
import React from 'react';
import { useSelector } from 'react-redux';
import ConnectionService from '../../../services/connectionService';

export default function ReadyBox() {
    const isReady = useSelector((state) => state.isReady)

    const declareReady = async () => {
        await ConnectionService.getConnection().invoke("DeclareReady", !isReady)
    }

    return (
        <div className="horizontal-div hero-ready-box mb-2">
            Ready
            <input
                className="checkbox ms-3"
                type="checkbox"
                checked={isReady}
                onChange={() => declareReady()}/>
        </div>
    )
}
