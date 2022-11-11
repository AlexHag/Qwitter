import ThumbUpAltIcon from '@mui/icons-material/ThumbUpAlt';
import { useState, useEffect } from 'react';

function Testing(props) {

    const [address, setAddress] = useState("");

    const dosum = () => {
        setAddress("EXIST");
        console.log(address);
    }

    useEffect(() => {
        const interval = setInterval(() => {
            console.log("address dont exist");
            console.log(address);
            let t = cb();
            console.log("T: ");
            console.log(t);
            if(t=="EXIST")
            {
                console.log("address exist");
                console(t);
                clearInterval(interval);
            }
        }, 2000);
        return () => clearInterval(interval);
    }, []);

    const cb = () => {
        return address;
    }

    return (
        <>
            <h1>Testing</h1>
            <ThumbUpAltIcon onClick={dosum}></ThumbUpAltIcon>
        </>
    )
}

export default Testing;