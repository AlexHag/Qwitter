import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect, useContext } from 'react';
import { authContext } from "../Auth/auth";

function Login() {
    const navigate = useNavigate();
    const { login } = useContext(authContext);
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errors, setErrors] = useState('');

    const handleLogin = async () => {
        if (!await login(username, password)) {
            setErrors('Wrong username or password');
            return;
        }
        navigate('/');
    };

    return (
        <div>
            <Link to="/">
                <button>Home</button>
            </Link>
            <h1>Login</h1>
            <input placeholder="Username" onChange={(e) => setUsername(e.target.value)}></input>
            <br></br>
            <input placeholder="Password" onChange={(e) => setPassword(e.target.value)}></input>
            <br></br>
            {errors && <p>{errors}</p>}
            <br></br>
            <p><Link to="/CreateAccount">No account? Create one</Link></p>
            <button onClick={handleLogin}>Login</button>
        </div>
    )
}

export default Login;
