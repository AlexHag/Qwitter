import React, { useEffect } from 'react';
import './css/app.css';
import './css/header.css';

import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
import { useState } from 'react';

import Login from './Components/Login';
import CreateAccount from './Components/CreateAccount';
import Profile from './Components/Profile';
import Feed from './Components/Feed';
import Testing from './Components/Testing';
import OnePost from './Components/OnePost';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(() => 
    JSON.parse(localStorage.getItem("isLoggedIn")) ?? false
  );

  const [userInfo, setUserInfo] = useState(() => 
  {
    const temp = localStorage.getItem("userInfo");
    if(temp === null || temp === "")
    {
      return [];
    }
    return JSON.parse(temp);
  });
  
  function loginHeader() {
    if(isLoggedIn) return <h1><Link to="/Profile">Profile</Link></h1>
    return <h1><Link to="/Login">Login</Link></h1>
  }

  useEffect(() => {
    localStorage.setItem("isLoggedIn", isLoggedIn);
  }, [isLoggedIn])
  
  useEffect(() => {
    localStorage.setItem("userInfo", JSON.stringify(userInfo));
  }, [userInfo])

  return (
    <div className="App">
      <BrowserRouter> 
        <div className="top-header">
            <h1><Link to="/">SRC</Link></h1>
            <h1><Link to="/Testing">Testing</Link></h1>
            {loginHeader()}
        </div>
        <Routes> 
          <Route path="/" element={<Feed userInfo={userInfo}/>}></Route>
          <Route path="/Testing" element={<Testing />}></Route>
          <Route path="/Login" element={<Login setIsLoggedIn={setIsLoggedIn} setUserInfo={setUserInfo}/>}></Route>
          <Route path="/Profile" element={<Profile isLoggedIn={isLoggedIn} setIsLoggedIn={setIsLoggedIn} userInfo={userInfo} setUserInfo={setUserInfo}/>}></Route>
          <Route path="/CreateAccount" element={<CreateAccount />}></Route>
          <Route path="Post/:id" element={<OnePost />}></Route>
        </Routes>  
      </BrowserRouter>
    </div>
  );
}

export default App;
