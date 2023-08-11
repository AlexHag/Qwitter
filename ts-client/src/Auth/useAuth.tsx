import { useState, useEffect, useContext } from 'react'
import URLS from '../Statics/URLS.json'

interface UserType {
    Username: string,
    IsPremium: boolean
};

// TODO: Save authentication in sessionStorage
// TODO: Create logout
// TODO: Set signalR jwt
const useAuth = () => {
    const [authenticated, setAuthenticated] = useState(false);
    const [jwt, setJwt] = useState('');
    const [userData, setUserData] = useState<UserType | null>(null);

    const login = async (username: string, password: string) => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Accept': 'application/json',
                      'Content-Type': 'application/json' },
            body: JSON.stringify({username: username, password: password})
        };
        
        const response = await fetch(`${URLS.SERVER}/api/Login`, requestOptions);

        if (response.status !== 200) return false;

        const data = await response.json();
        setAuthenticated(true);
        setJwt(data.jwt)
        setUserData({ Username: data.username, IsPremium: data.isPremium });
        return true;
    }

    const createAccount = async (username: string, password: string) => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Accept': 'application/json',
                      'Content-Type': 'application/json' },
            body: JSON.stringify({username: username, password: password})
        };

        const response = await fetch(`${URLS.SERVER}/api/CreateAccount`, requestOptions);

        if (response.status !== 200) return false;

        const data = await response.json();
        setAuthenticated(true);
        setJwt(data.jwt)
        setUserData({ Username: data.username, IsPremium: data.isPremium });
        return true;
    }

    const refreshUser = async () => {
        if (authenticated && jwt) await fetch(`${URLS.SERVER}/api/Me`)
            .then(p => p.json())
            .then(p => setUserData({ Username: p.username, IsPremium: p.isPremium }));
    }

    return {
        authenticated,
        setAuthenticated,
        jwt,
        setJwt,
        userData,
        setUserData,
        login,
        createAccount,
        refreshUser
    };
}

export default useAuth;