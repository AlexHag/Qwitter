import { useAuth } from "../Auth/AuthProvider";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import CreatePost from "../Components/CreatePost";
import PostCard from "./PostCard";

function Feed() {
  const auth = useAuth();
  const navigate = useNavigate();
  const [page, setPage] = useState(0);
  const [posts, setPosts] = useState([])

  const fetchPosts = async () => {
    const response = await fetch("https://localhost:7003/posts/latest", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${auth.token}`,
      },
      body: JSON.stringify({ take: 10, offset: page * 10})
    })

    if (!response.ok) {
      console.log(response);
      alert("Failed to fetch posts");
      return;
    }

    const data = await response.json();
    console.log(data);
    setPosts(data);
  }

  return (
    <div>
      <CreatePost />
      <button onClick={fetchPosts}>Get posts</button>
      {posts.map((post) => <PostCard key={post.id} {...post} />)}
      <button>More</button>
    </div>
  )
}

export default Feed;