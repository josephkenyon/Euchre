import '../../App.css'
import React from 'react';
import Game from '../Game/Game';
import Entry from '../Entry/Entry';
import { useSelector } from 'react-redux';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { TailSpin } from 'react-loader-spinner';

export default function App() {
    const connection = useSelector((state) => state.appState.connection)
    const hasState = useSelector((state) => state.playerState.hasState)

    const displayLoading = connection && !hasState

    return (
        <div className="vertical-div full-screen-div">
            <ToastContainer
                position="top-left"
                autoClose={5000}
                hideProgressBar
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable={false}
                pauseOnHover={false}
                theme="colored"
            />
            { displayLoading
                    ?   <div className="horizontal-div loading-div">
                            <TailSpin
                                height="50"
                                color="#ffffff"
                                ariaLabel="tail-spin-loading"
                                radius="1"
                                wrapperStyle={{}}
                                wrapperClass=""
                                visible={true}/>

                            <p className="loading-paragraph">loading</p>
                        </div> 
                    : connection
                        ? <Game/>
                        : <Entry/>
            }
        </div>
    )
}