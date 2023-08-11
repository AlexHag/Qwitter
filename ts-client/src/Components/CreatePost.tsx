import { useState, useContext } from 'react';
import { authContext } from "../Auth/auth";

function CreatePost() {
  const [createPostContent, setCreatePostContent] = useState('');
  const {
    authenticated,
    setAuthenticated,
    jwt,
    setJwt,
    userData,
    setUserData,
    login,
    createAccount,
    refreshUser
  } = useContext(authContext);

  const handleUpload = () => {
    console.log(createPostContent);
  }

  return (
    <>
      <textarea placeholder={`What's on your mind ${userData.Username}?`} onChange={(e) => setCreatePostContent(e.target.value)}></textarea>
      <button onClick={handleUpload}>Upload</button>
    </>
  )
}

export default CreatePost;