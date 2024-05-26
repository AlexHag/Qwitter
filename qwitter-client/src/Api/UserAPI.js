import { useMemo } from "react";
import { backend } from "./backend";

export const authHeader = (token) => ({
  Authorization: `Bearer ${token}`
});

export class UserAPI {
  constructor(apiUrl) {
    this.apiUrl = apiUrl;
  }

  getMe = (token) =>
    backend.GET({
      url: `${this.apiUrl}/user/me`,
      headers: authHeader(token)
    });

  login = (usernameOrEmail, password) =>
    backend.POST({
      url: `${this.apiUrl}/auth/login`,
      body: { usernameOrEmail, password }
    })

  register = (username, email, password) =>
    backend.POST({
      url: `${this.apiUrl}/auth/register`,
      body: { username, email, password }
    })
  
  verifyUser = (userId) =>
    backend.PUT({
      url: `${this.apiUrl}/user/${userId}/verify`,
    })
}

export const useUserAPI = () => 
  useMemo(() => new UserAPI("https://localhost:7001"), []);