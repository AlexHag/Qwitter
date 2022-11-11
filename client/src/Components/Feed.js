import PostCard from "./PostCard";
import '../css/feed.css';
import { useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";

function Feed(props) {
    const [posts, setPosts] = useState([]);
    const [content, setContent] = useState("");
    let navigate = useNavigate();

    useEffect(() => {
        fetch('http://localhost:5295/GetAllPosts')
        .then(p => p.json())
        .then(p => setPosts(p));
    }, [])

    const handlePost = async () => {
        //console.log(props.userInfo.Id);

        if(props.userInfo.Id === undefined) {navigate("/Login"); return;}

        const requestOptions = {
            method: 'POST',
            headers: { 'Accept': 'application/json',
                      'Content-Type': 'application/json' },
            body: JSON.stringify({userId: props.userInfo.Id, content: content})
          };
        
        await fetch(`http://localhost:5295/CreatePost`, requestOptions);
        window.location.reload(false);
    };

    return (
        <div className="feed-container">
            <h1>Feed</h1>
            <textarea className="post-text-area" placeholder={`What's on your mind? ${props.userInfo.Username == null ? "" : props.userInfo.Username.toString()}`} 
            value={content}
            onChange={(e) => setContent(e.target.value)}></textarea>
            <button className="post-button" type="submit" onClick={handlePost}>Post</button>
            <div className="post-container">
                {posts.map(p => <PostCard key={p.postId} postId={p.postId} author={p.author} content={p.content} likes={p.likes} dislikes={p.dislikes} premium={p.isPremium}/>)}
            </div>
        </div>
    )
}

export default Feed;