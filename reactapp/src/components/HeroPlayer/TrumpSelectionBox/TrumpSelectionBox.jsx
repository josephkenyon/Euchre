import '../../../App.css'
import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import ConnectionService from '../../../services/connectionService';
import { Dropdown, DropdownButton } from 'react-bootstrap';
import { setAlone } from '../../../slices/playerState/playerStateSlice';

export default function TrumpSelectionBox() {
    const dispatch = useDispatch()
    
    const alone = useSelector((state) => state.playerState.alone)
    const showPassButton = useSelector((state) => state.playerState.showPassButton)

    const declareTrumpSuit = async (trumpSuit) => {
        let goAlone = trumpSuit != -1 ? alone : false
        
        dispatch(setAlone(false))

        await ConnectionService.getConnection().invoke("DeclareTrump", trumpSuit, goAlone)
    }

    return (
        <div className="horizontal-div bidding-div">
            { showPassButton ? <button className="button me-2" value="test" onClick={() => declareTrumpSuit(-1)}>
                Pass
            </button> : null }

            <DropdownButton
                style={{fontSize:"10px"}}
                className="trump-dropdown"
                variant="secondary"
                onSelect={(eventKey, _) => declareTrumpSuit(+eventKey)}
                title="Declare Trump">

                <Dropdown.Item as="button" eventKey="0">Spades</Dropdown.Item>
                <Dropdown.Item as="button" eventKey="1">Hearts</Dropdown.Item>
                <Dropdown.Item as="button" eventKey="2">Clubs</Dropdown.Item>
                <Dropdown.Item as="button" eventKey="3">Diamonds</Dropdown.Item>
            </DropdownButton>

            <label className="loner-label">
                Go Alone:
            </label>

            <input
                className="player-checkbox ms-2"
                type="checkbox"
                checked={alone}
                onChange={() => dispatch(setAlone(!alone))}/>
        </div>
    )
}
