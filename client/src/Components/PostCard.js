import '../css/feed.css';
import { useState } from 'react';
import { useNavigate } from "react-router-dom";

import ThumbDownIcon from '@mui/icons-material/ThumbDown';
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import PersonIcon from '@mui/icons-material/Person';

function PostCard(props) {
    const navigate = useNavigate();
    const [likes, setLikes] = useState(props.likes);
    const [dislikes, setDislikes] = useState(props.dislikes);

    const like = async (e) => {
        e.stopPropagation();
        await fetch(`http://localhost:5295/LikePost/${props.postId}`, {method: 'PUT'});
        setLikes(likes + 1);
    }
    const dislike = async (e) => {
        e.stopPropagation();
        console.log(props.premium);
        await fetch(`http://localhost:5295/DislikePost/${props.postId}`, {method: 'PUT'});
        setDislikes(dislikes + 1);
    }

    const gotoPost =  () => {
        console.log(props.postId);
        navigate(`/Post/${props.postId}`);
    }

    return (
        <div className={`post-card premium-${props.premium}`} onClick={gotoPost}>
            <PersonIcon />
            <b className="author">{props.author}</b>
            <p className="content">{props.content}</p>
            <div className="liking">
                <div>
                <ThumbDownIcon onClick={dislike}></ThumbDownIcon>
                <label>{dislikes}</label>
                </div>
                <div>
                <ThumbUpIcon onClick={like}></ThumbUpIcon>
                <label>{likes}</label>
                </div>
            </div>
        </div>
    )
}

export default PostCard;