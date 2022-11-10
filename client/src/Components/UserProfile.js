import { useParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from 'react';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import PostCard from "./PostCard";

import '../css/profile.css';

function UserProfile(props) {

    let { username } = useParams();
    const [profileData, setProfileData] = useState([]);

    useEffect(() => {
        fetch(`http://localhost:5295/GetProfile/${username}`)
        .then(p => p.json())
        .then(p => setProfileData(p));
    }, [username])

    const renderProfile = () => {
        if(profileData.status === 404) return (<h1>Profile Not Found</h1>);
        if(profileData.posts === undefined ) return (<h1>Loading...</h1>);

        const renderPost = () => {
            if(profileData.posts.length === 0) return (<h1>No posts...</h1>);
            return (
                <div className="post-container">
                    {profileData.posts.map(p => 
                    <PostCard key={p.postId} postId={p.postId} author={p.author} content={p.content} likes={p.likes} dislikes={p.dislikes}/>
                    )}
                </div>
            )
        }

        return (
            <div className="profile-container">
                <div className="profile-header">
                    <AccountCircleIcon style={{fontSize: "60px"}} />
                    <h1 style={{marginLeft: "20px", fontSize: "50px"}}>{profileData.username}</h1>
                </div>
                {renderPost()}
            </div>
        )
    }

    return (
        <>
            {renderProfile()}
        </>
    )
}

export default UserProfile;