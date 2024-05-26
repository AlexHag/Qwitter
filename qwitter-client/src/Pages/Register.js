import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { useAuth } from "../Auth/AuthProvider";
import "../Styles/Auth.css";

const Register = () => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const auth = useAuth();

  const handleSubmitEvent = async (e) => {
    e.preventDefault();

    if (username !== "" && password !== "" && email !== "") {
      auth.registerAction(username, email, password)
        .catch(() => alert("Username or email already exists"));
    }
    else {
      alert("please provide a valid input");
    }
  };

  return (
    <>
      <button onClick={() => navigate("/")}>Home</button>

      <form onSubmit={handleSubmitEvent} className="auth-form">
        <h1>Register</h1>

        <div className="auth-form-input">
          <label htmlFor="username">Username</label>
          <input
            id="username"
            name="username"
            placeholder="Username"
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>

        <div className="auth-form-input">
          <label htmlFor="email">Email</label>
          <input
            id="email"
            name="email"
            placeholder="Email"
            onChange={(e) => setEmail(e.target.value)}
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

        <button className="auth-button-primary">Register</button>
        <button className="auth-button-secondary" onClick={() => navigate("/login")}>Login</button>
      </form>
    </>
  );
};

export default Register;
