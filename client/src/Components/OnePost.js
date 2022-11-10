import ThumbDownIcon from '@mui/icons-material/ThumbDown';
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import PersonIcon from '@mui/icons-material/Person';
import TextField from '@mui/material/TextField';
import AccountCircle from '@mui/icons-material/AccountCircle';
import Box from '@mui/material/Box';

import { useParams } from "react-router-dom";
import { useEffect, useState } from 'react';

import PostCard from './PostCard';

function OnePost(props) {

    let { id } = useParams();
    const [post, setPosts] = useState([]);

    useEffect(() => {
        fetch(`http://localhost:5295/GetOnePost/${id}`)
        .then(p => p.json())
        .then(p => {setPosts(p); setLikes(p.likes); setDislikes(p.dislikes)});
    }, [])

    const [likes, setLikes] = useState(0);
    const [dislikes, setDislikes] = useState(0);

    const like = async (e) => {
        e.stopPropagation();
        await fetch(`http://localhost:5295/LikePost/${post.postId}`, {method: 'PUT'});
        setLikes(likes + 1);
    }
    const dislike = async (e) => {
        e.stopPropagation();
        await fetch(`http://localhost:5295/DislikePost/${post.postId}`, {method: 'PUT'});
        setDislikes(dislikes + 1);
    }

    const dosum = () => {
        console.log("do");
        console.log(post);
    }

    return (
        <>
            <div className="one-post">
                <PersonIcon />
                <b className="author">{post.author}</b>
                <p className="content">{post.content}</p>
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
            <div className="reply-box">
                <AccountCircle sx={{ color: 'action.active', mr: 1, my: 0.5 }} />
                <textarea placeholder={`Reply to ${post.author}`}/>
            </div>
        </>
    )
}

export default OnePost;



// import * as React from 'react';
// import Box from '@mui/material/Box';
// import Input from '@mui/material/Input';
// import InputLabel from '@mui/material/InputLabel';
// import InputAdornment from '@mui/material/InputAdornment';
// import FormControl from '@mui/material/FormControl';
// import TextField from '@mui/material/TextField';
// import AccountCircle from '@mui/icons-material/AccountCircle';
