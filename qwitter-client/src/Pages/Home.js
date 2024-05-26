import { useAuth } from "../Auth/AuthProvider";
import { useNavigate } from "react-router-dom";
import PageHeader from "../Components/PageHeader";

function Home() {
  const auth = useAuth();
  const navigate = useNavigate();

  return (
    <div>
      <PageHeader />
      <h1>Home</h1>
      <a href="/login">Login</a>
      <p>Not logged in</p>
      <button onClick={() => navigate("/login")}>Login</button>
      <button onClick={() => auth.logOut()}>Logout</button>
      {/* 
      {auth.token && 
      <>
        <h2>Hello you are logged in</h2>
        <p>Username: {auth.user?.username}</p>
      </>}

      <Feed /> */}
      

    </div>
  );
}

export default Home;