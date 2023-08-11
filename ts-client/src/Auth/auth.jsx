import { createContext } from 'react';
import useAuth from './useAuth';

const authContext = createContext();
const { Provider } = authContext

const AuthProvider = ({ children }) => {
  const {
    authenticated,
    setAuthenticated,
    jwt,
    setJwt,
    userData,
    setUserData,
    login,
    createAccount,
    refreshUser
  } = useAuth();

  return (
    <Provider
      value={{
        authenticated,
        setAuthenticated,
        jwt,
        setJwt,
        userData,
        setUserData,
        login,
        createAccount,
        refreshUser
      }}>
        {children}
    </Provider>
  )
}

export { authContext, AuthProvider }