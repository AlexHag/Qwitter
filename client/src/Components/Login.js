import { useState } from 'react';
import { Link } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import '../css/form.css';

function Login(props) {
    let navigate = useNavigate();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [wrong, setWrong] = useState("");
  
    const handleLogin = async () => {
        
        const requestOptions = {
            method: 'POST',
            headers: { 'Accept': 'application/json',
                      'Content-Type': 'application/json' },
            body: JSON.stringify({username: username, password: password})
          };
        
        const response = await fetch(`http://localhost:5295/Login`, requestOptions);
        if(response.status === 401)
        {
            console.log("WRONG PASSWORD FROM REACT");
            setWrong("Wrong username or password.");
        }


        if(response.status === 202)
        {
            console.log("RIGHT PASSWORD FROM REACT");
            const data = await response.json()
            console.log("DATA:");
            console.log(data)
            console.log(data[0].id);
            props.setIsLoggedIn(true);
            props.setUserInfo({Id: data[0].id, Username: username})
            navigate("/");
        }       
    }

    return (
        <div className="form">
            <h1>Login</h1>
                <input
                type="text"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                placeholder="Username"
                required
                />
                
                <input
                type="text"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Password"
                required
                />

                <button className="form-button" onClick={handleLogin} id="btnAddTodo">Add</button>
                <p style={{color: "red"}}>{wrong}</p>
                <p><Link to="/CreateAccount">No account? Create one</Link></p>
        </div>
    )
}

export default Login;