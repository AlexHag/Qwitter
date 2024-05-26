import { useAuth } from "../Auth/AuthProvider";
import { useUserAPI } from "../Api/UserAPI";
import { useNavigate } from "react-router-dom";
import PageHeader from "../Components/PageHeader";

function Profile() {
  const userAPI = useUserAPI();
  const auth = useAuth();
  const navigate = useNavigate();

  const doStuff = () => {
    console.log(auth.user);
    console.log(auth.user.userState);
  }

  return (
    <div>
      <PageHeader />
      <h1>Profile</h1>
      <p>Email: {auth?.user?.email}</p>
      <p>Username: {auth?.user?.username}</p>
      <button onClick={() => auth.logOut()}>Logout</button>
      {auth?.user?.userState === "Created" &&
        <button onClick={() => {
          userAPI.verifyUser(auth.user.userId)
            .then(() => {
              auth.refreshUser();
              navigate("/profile");
            });
        }}>Verify account</button>
      }
      <button onClick={doStuff}>Dostuff</button>
    </div>
  )
}

export default Profile;