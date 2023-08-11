import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect, useContext } from 'react';
import { authContext } from "../Auth/auth";

function CreateAccount() {
    const navigate = useNavigate();
    const { createAccount } = useContext(authContext);
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errors, setErrors] = useState('');

    const handleCreateAccount = async () => {
        if (!await createAccount(username, password)) {
            setErrors('Username already exists');
            return;
        }
        navigate('/');
    };

    return (
        <div>
            <Link to="/">
                <button>Home</button>
            </Link>
            <h1>Create Account</h1>
            <input placeholder="Username" onChange={(e) => setUsername(e.target.value)}></input>
            <br></br>
            <input placeholder="Password" onChange={(e) => setPassword(e.target.value)}></input>
            <br></br>
            {errors && <p>{errors}</p>}
            <br></br>
            <p><Link to="/Login">Already have an account? Login</Link></p>
            <button onClick={handleCreateAccount}>Create Account</button>
        </div>
    )
}

export default CreateAccount;