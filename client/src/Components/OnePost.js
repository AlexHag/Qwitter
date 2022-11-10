import ThumbDownIcon from '@mui/icons-material/ThumbDown';
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import PersonIcon from '@mui/icons-material/Person';
import AccountCircle from '@mui/icons-material/AccountCircle';
import SendIcon from '@mui/icons-material/Send';
import Button from '@mui/material/Button';

import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from 'react';

import '../css/comment.css';

function OnePost(props) {

    let { id } = useParams();
    const [post, setPosts] = useState([]);
    const [reply, setReply] = useState("");
    let navigate = useNavigate();

    useEffect(() => {
        fetch(`http://localhost:5295/GetOnePost/${id}`)
        .then(p => p.json())
        .then(p => {setPosts(p); setLikes(p.likes); setDislikes(p.dislikes)});
        
    }, [])

    const [likes, setLikes] = useState(0);
    const [dislikes, setDislikes] = useState(0);
    const [comments, setComments] = useState([]);

    useEffect(() => {
        fetch(`http://localhost:5295/GetCommentsOnPost/${id}`)
        .then(p => p.json())
        .then(p => setComments(p));
    }, [])

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


    const reunderComments = () => {
        if(comments.status === 404) return (<h2 className="no-comments-h1">No replys</h2>)
        else
        {
            return (
                <div className="comment-container">
                    {comments.map(p => 
                        <div className="comment-card">
                            <h2>{p.author}</h2>
                            <p>{p.content}</p>
                        </div>
                    )}
                </div>
            )
        }
    }

    const sendComment = async () => {
        if(props.userInfo.Id === undefined) {navigate("/Login"); return;}
        const requestOptions = {
            method: 'POST',
            headers: { 'Accept': 'application/json',
                      'Content-Type': 'application/json' },
            body: JSON.stringify({relatedPostId: id, userId: props.userInfo.Id, content: reply})
          };
        
        await fetch(`http://localhost:5295/CommentOnPost`, requestOptions);
        window.location.reload(false);
    };

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
                <textarea 
                    placeholder={`Reply to ${post.author}`} 
                    value={reply}
                    onChange={(e) => setReply(e.target.value)}
                />
                <Button variant="contained" endIcon={<SendIcon />} onClick={sendComment}>Send</Button>
            </div>
            <div className="comments-container">
                {reunderComments()}
            </div>
        </>
    )
}

export default OnePost;
