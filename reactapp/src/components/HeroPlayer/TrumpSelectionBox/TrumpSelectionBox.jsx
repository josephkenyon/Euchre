import '../../../App.css'
import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import ConnectionService from '../../../services/connectionService';
import { Dropdown, DropdownButton } from 'react-bootstrap';
import { setAlone } from '../../../slices/playerState/playerStateSlice';

export default function TrumpSelectionBox() {
    const dispatch = useDispatch()
    
    const alone = useSelector((state) => state.playerState.alone)

    const declareTrumpSuit = async (trumpSuit) => {
        await ConnectionService.getConnection().invoke("DeclareTrump", trumpSuit, alone)
    }

    return (
        <div className="vertical-div bidding-div">
            <button className="button" value="test" onClick={() => declareTrumpSuit(-1)}>
                Pass
            </button>

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

            <input
                className="checkbox ms-2"
                type="checkbox"
                checked={alone}
                onChange={() => dispatch(setAlone(!alone))}/>
        </div>
    )
}
