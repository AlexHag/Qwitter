import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import '../css/form.css';

function CreateAccount() {

    let navigate = useNavigate();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [wrong, setWrong] = useState("");

    const handleCreateAccount = async () => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Accept': 'application/json',
                      'Content-Type': 'application/json' },
            body: JSON.stringify({username: username, password: password})
          };
        
        const response = await fetch(`http://localhost:5295/CreateAccount`, requestOptions);
        console.log(response);
        if(response.status === 409)
        {
            console.log("Username Already exist");
            console.log(response);
            setWrong("Username already exist");
        }


        if(response.status === 201)
        {
            navigate("/Login");
        } 
    }

    return (
        <div className="form">
            <h1>Create Account</h1>
                <input
                type="text"
                className="input-type-text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="Username"
                required
                />
                
                <input
                type="text"
                className="input-type-text"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Password"
                required
                />

                <button className="form-button" onClick={handleCreateAccount} id="btnAddTodo">Add</button>
                <p style={{color: "red"}}>{wrong}</p>
                <p><Link to="/Login">Already have an account? Log in</Link></p>
        </div>
    )
}

export default CreateAccount;