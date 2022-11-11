import ThumbUpAltIcon from '@mui/icons-material/ThumbUpAlt';

import { useState, useEffect } from 'react';
import { QRCode } from 'react-qrcode'
import { useNavigate } from "react-router-dom";

import '../css/premium.css';

function BuyPremium(props) {
    let navigate = useNavigate();
    const [address, setAddress] = useState("");
    const [addressInfo, setAddressInfo] = useState([]);

    useEffect(() => {
        fetch(`http://localhost:5295/RequestWallet/${props.userInfo.Id}`)
        .then(p => p.text())
        .then(a => {
            setAddress(a);
            const interval = setInterval(() => {
                console.log(a);
                fetch(`https://node.algoexplorerapi.io/v2/accounts/${a}`)
                .then(p => p.json())
                .then(p => {
                    console.log(p);
                    if(p.amount > 899999)
                    {
                        clearInterval(interval);
                        sendBuyRequest(p.address);
                    }
                });
            }, 2000);
            return () => clearInterval(interval);
        });
    }, [])

    // useEffect(() => {
    //     const interval = setInterval(() => {
    //         fetch(`https://node.algoexplorerapi.io/v2/accounts/${address}`)
    //         .then(p => p.json())
    //         .then(p => setAddressInfo(p));

    //         if(addressInfo.amount > 999999)
    //         {
    //             sendBuyRequest();
    //             clearInterval(interval);
    //         }
    //     }, 1000);
    //     return () => clearInterval(interval);
    // }, []);

    // const checkPayment = () => {
    //     const interval = setInterval(() => {
    //         console.log(address);
    //         fetch(`https://node.algoexplorerapi.io/v2/accounts/${address}`)
    //         .then(p => p.json())
    //         .then(p => setAddressInfo(p));
    //         console.log(addressInfo);
    //         if(addressInfo.amount > 999999)
    //         {
    //             sendBuyRequest();
    //             clearInterval(interval);
    //         }
    //     }, 1000);
    //     return () => clearInterval(interval);
    // };

    

    const sendBuyRequest = (p) => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Accept': 'application/json',
                      'Content-Type': 'application/json' },
            body: JSON.stringify({userId: props.userInfo.Id, address: p.toString()})
        };
        console.log("ID: " + props.userInfo.Id);
        console.log("Address p: " + p);
        console.log("Address p: string " + p.toString());
        console.log("Address adress: " + address);

        fetch('http://localhost:5295/HaveBought', requestOptions)
        .then(p => {
            if(p.status==200) {
                props.setUserInfo({Id: props.userInfo.Id, Username: props.userInfo.Username, isPremium: true});
                navigate("/PaymentSuccess");
            }
            else {
                navigate("/ErrorOccured");
            }
        });
    };


    return (
        <>
            <h1>Buy Premium</h1>
            <h2 style={{textAlign: "center"}}>Price: 5 Algorand</h2>
            {/* <p>{props.userInfo.Id}</p> */}
            <div className="qr-div">
                <QRCode value={address} className="qr-code" scale={10}/>
            </div>
            <p className="address-p">{address}</p>
            {/* <ThumbUpAltIcon onClick={dosum}></ThumbUpAltIcon> */}
        </>
    )
}

export default BuyPremium;