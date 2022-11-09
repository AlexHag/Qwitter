import { Link } from 'react-router-dom';

function Profile(props) {
    const handleLogOut = () => {
        props.setIsLoggedIn(false);
    };

    return (
        <>
            <h1>Profile</h1>
            <h2>{props.isLoggedIn.toString()}</h2>
            <h2>{props.userInfo.Username.toString()}</h2>
            <h2>{props.userInfo.Id.toString()}</h2>
            <Link to='/' ><button onClick={handleLogOut}>Log Out</button></Link>
        </>
    )
}

export default Profile;