import { useAuth } from "../Auth/AuthProvider";
import { Link } from 'react-router-dom';
import "../Styles/PageHeader.css";

function PageHeader() {
  const auth = useAuth();

  function loginHeader() {
    if(auth.token) return <h1><Link to="/profile">Profile</Link></h1>
    return <h1><Link to="/login">Login</Link></h1>
  }

  return (
    <div className="page-header">
      <h1><Link to="/">Home</Link></h1>
      {loginHeader()}
    </div>
  )
}

export default PageHeader;