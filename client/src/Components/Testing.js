import ThumbUpAltIcon from '@mui/icons-material/ThumbUpAlt';

function Testing(props) {

    const dosum = () => {
        console.log("do");
    }

    return (
        <>
            <h1>Testing</h1>
            <ThumbUpAltIcon onClick={dosum}></ThumbUpAltIcon>
        </>
    )
}

export default Testing;