import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { useAuth } from "../Auth/AuthProvider";

function CreatePost() {
  const auth = useAuth();
  const navigate = useNavigate();

  const [content, setContent] = useState("");

  const handlePost = async () => {
    if (!auth.token) {
      navigate("/login");
      return;
    }

    if (content === "") {
      alert("Please provide a valid input");
      return;
    }

    const response = await fetch("https://localhost:7003/posts", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${auth.token}`
      },
      body: JSON.stringify({ content })
    })

    if (!response.ok) {
      console.log(response);
      alert("Failed to create post");
      return;
    }
  }

  return (
    <div>
      <textarea 
        style={{width: "600px", height: "150px", border: "2px solid #ccc", borderRadius: "4px", resize: "none", fontSize: "20px", padding: "10px"}}
        placeholder={`Write something? ${auth.user?.username ?? ""}`} 
        value={content}
        onChange={(e) => setContent(e.target.value)} />
      <br></br>
      <br></br>
      <button
        style={{width: "600px", height: "25px"}}
        onClick={handlePost}>
        Send
      </button>
    </div>
  )
}

export default CreatePost
