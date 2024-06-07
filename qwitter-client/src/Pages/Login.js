import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { useAuth } from "../Auth/AuthProvider";
import PageHeader from "../Components/PageHeader";
import "../Styles/Auth.css";

const Login = () => {
  const [usernameOrEmail, setUsernameOrEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const auth = useAuth();

  const handleSubmitEvent = async (e) => {
    e.preventDefault();

    if (usernameOrEmail !== "" && password !== "") {
      auth.loginAction(usernameOrEmail, password)
        .catch(() => alert("Invalid credentials"));
    }
    else {
      alert("please provide a valid input");
    }
  };

  return (
    <>
      <PageHeader />
      <button onClick={() => navigate("/")}>Home</button>

      <form onSubmit={handleSubmitEvent} className="auth-form">
        <h1>Login</h1>

        <div className="auth-form-input">
          <label htmlFor="username-or-email">Email</label>
          <input
            id="username-or-email"
            name="usernameOrEmail"
            placeholder="Username or email"
            onChange={(e) => setUsernameOrEmail(e.target.value)}
          />
        </div>

        <div className="auth-form-input">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            name="password"
            placeholder="Password"
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>

        <button className="auth-button-primary">Login</button>
        <button className="auth-button-secondary" onClick={() => navigate("/register")}>Register</button>
      </form>
    </>
  );
};

export default Login;
