import { createContext, useContext, useEffect, useState } from "react";
import authService from "../../services/authService";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  const login = async (credentials) => {
    try {
      setIsLoading(true);
      const response = await authService.login(credentials);
      
      if (response?.data?.accessToken && response.data.user) {
        setUser({
          ...response.data.user,
          accessToken: response.data.accessToken
        });
      }
      return response;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = async () => {
    await authService.logout();
    setUser(null);
  };

  const refresh = async () => {
    try {
      setIsLoading(true);
      const response = await authService.refresh();
      
      if (response?.data?.accessToken && response.data.user) {
        setUser({
          ...response.data.user,
          accessToken: response.data.accessToken
        });
      } else {
        setUser(null);
      }
    } catch (error) {
      setUser(null);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
