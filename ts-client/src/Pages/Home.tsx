import { Link } from "react-router-dom";
import { useContext } from "react";
import { authContext } from "../Auth/auth";
import CreatePost from "../Components/CreatePost";

function Home() {
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

  const loginHeader = () => {
    if (!authenticated) 
      return <>
        <Link to="Login">
          <button>Login</button>
        </Link>
        <br></br>
        <Link to="CreateAccount">
          <button>Create Account</button>
        </Link>
      </>
    return <>
      <Link to="Profile">
        <button>Profile</button>
      </Link>
      <br></br>
      <Link to="Logout">
        <button>Logout</button>
      </Link>
    </>
  }

  return (
    <div>
      <h1>Home</h1>
      {loginHeader()}
      {authenticated &&
        <>
          <p>Hello {userData.Username}</p>
          <CreatePost />
        </>}
    </div>
  )
}

export default Home;