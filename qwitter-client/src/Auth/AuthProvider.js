import { useContext, createContext, useState, useEffect } from 'react';
import { useNavigate } from "react-router-dom";
import { useUserAPI } from '../Api/UserAPI';
import Loading from '../Components/Loading/Loading';

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const userAPI = useUserAPI();
  const [user, setUser] = useState(() => JSON.parse(localStorage.getItem("user")));
  const [token, setToken] = useState(() => localStorage.getItem("token"));
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    refreshUser();
  }, [])

  const refreshUser = () => {
    if (!token) {
      setIsLoading(false);
      return;
    }

    setIsLoading(true);
    userAPI.getMe(token)
      .then(user => {
        setUser(user)
        setIsLoading(false);
      })
      .catch(err => {
        console.log(err);
        setIsLoading(false);
        throw err;
      });
  }

  const registerAction = (username, email, password) => 
    userAPI.register(username, email, password)
      .then(response => {
        console.log(response);
        setToken(response.token);
        setUser(response.user);
        localStorage.setItem("token", response.token);
        navigate("/");
        return;
      });

  const loginAction = (usernameOrEmail, password) =>
      userAPI.login(usernameOrEmail, password)
        .then(response => {
          console.log(response);
          setToken(response.token);
          setUser(response.user);
          localStorage.setItem("token", response.token);
          navigate("/");
          return;
        });

  const logOut = () => {
    setUser(null);
    setToken("");
    localStorage.removeItem("token");
    navigate("/");
    return;
  };

  return (
    <AuthContext.Provider value={{ token, user, loginAction, registerAction, refreshUser, logOut }}>
      {isLoading ? <Loading /> : children}
    </AuthContext.Provider>
  );
}

export default AuthProvider;

export const useAuth = () => {
  return useContext(AuthContext);
};