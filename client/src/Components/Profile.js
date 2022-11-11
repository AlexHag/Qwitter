import { Link } from 'react-router-dom';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useState, useEffect } from 'react';

import PostCard from './PostCard';

function Profile(props) {

    const [profileData, setProfileData] = useState([]);

    useEffect(() => {
        fetch(`http://localhost:5295/GetProfile/${props.userInfo.Username}`)
        .then(p => p.json())
        .then(p => setProfileData(p));
    }, [])

    const renderProfile = () => {
        if(profileData.status === 404) return (<h1>Profile Not Found</h1>);
        if(profileData.posts === undefined ) return (<h1>Loading...</h1>);
        console.log(profileData);
        const renderPost = () => {
            if(profileData.posts === null || profileData.posts.length === 0) return (<h1>No posts...</h1>);
            return (
                <div className="post-container">
                    {profileData.posts.map(p => 
                    <PostCard key={p.postId} postId={p.postId} author={p.author} content={p.content} likes={p.likes} dislikes={p.dislikes} premium={p.isPremium}/>
                    )}
                </div>
            )
        }

        return (
            <div className="profile-container">
                <div className="profile-header">
                    <AccountCircleIcon style={{fontSize: "60px"}} />
                    <h1 style={{marginLeft: "20px", fontSize: "50px"}}>Hello {profileData.username}</h1>
                </div>
                <Link to='/' ><button className="form-button" style={{marginBottom: "30px"}} onClick={handleLogOut}>Log Out</button></Link>
                {!profileData.isPremium && <Link to='/BuyPremium' ><button className="form-button" style={{marginBottom: "30px", backgroundColor: "gold", color: "black"}}>Buy Premium</button></Link>}
                {renderPost()}
            </div>
        )
    }

    const handleLogOut = () => {
        props.setIsLoggedIn(false);
        props.setUserInfo([]);
    };

    return (
        <>
            {renderProfile()}
        </>
    )
}

export default Profile;