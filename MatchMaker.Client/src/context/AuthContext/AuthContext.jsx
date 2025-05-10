import { createContext, useContext, useEffect, useState } from "react";
import authService from "../../services/authService";
import { setAccessToken, clearAccessToken } from "../../services/tokenStore";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const refresh = async () => {
    try {
      const response = await authService.refresh();
      const responseData = response?.data || response;

      if (!responseData?.accessToken) {
        console.warn("Refresh failed - no token in response", response);
        setUser(null);
        clearAccessToken();
        return;
      }

      setAccessToken(responseData.accessToken);
      setUser({
        ...(responseData.user || {}),
        accessToken: responseData.accessToken,
      });
      setError(null);
    } catch (err) {
      console.error("Refresh error:", err);
      setUser(null);
      clearAccessToken();
      setError(err.response?.data?.message || "Session refresh failed");
    }
  };

  const refreshUser = async () => {
    await refresh();
  };

  const login = async (credentials) => {
    try {
      setIsLoading(true);
      const response = await authService.login(credentials);
      const responseData = response?.data || response;

      if (responseData?.accessToken) {
        setAccessToken(responseData.accessToken);
        setUser({
          ...(responseData.user || {}),
          accessToken: responseData.accessToken,
        });
        setError(null);
      }
      return response;
    } catch (err) {
      setError(err.response?.data?.message || "Login failed");
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = async () => {
    try {
      setIsLoading(true);
      await authService.logout();
      setUser(null);
      clearAccessToken();
      setError(null);
    } catch (err) {
      setError(err.response?.data?.message || "Logout failed");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    let isMounted = true;

    const initializeAuth = async () => {
      try {
        setIsLoading(true);
        await refresh();
      } catch (e) {
        console.error("Initialization failed", e);
      } finally {
        if (isMounted) setIsLoading(false);
      }
    };

    initializeAuth();

    return () => {
      isMounted = false;
    };
  }, []);

  return (
    <AuthContext.Provider
      value={{ user, isLoading, error, login, logout, refresh, refreshUser }}
    >
      {isLoading ? <div>Loading auth...</div> : children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
