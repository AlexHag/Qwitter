import '../Css/PostCard.css';

function PostCard(post) {
  const clickOnPost = () => {
    console.log("Clicked on post");
    console.log(post.id)
  }


  return (
    <div onClick={clickOnPost} className="post-card">
      <p className="post-card-username">{post.username}</p>
      <p className="post-card-content">{post.content}</p>
      <div className="post-card-likes">
        <div>
          <button>Like</button>
          <p>{post.likes}</p>
        </div>
        <div>
          <button>Dislike</button>
          <p>{post.dislikes}</p>
        </div>
      </div>
    </div>
  )
}

export default PostCard;