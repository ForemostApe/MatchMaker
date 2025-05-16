import { useEffect } from "react";
import { useAuth } from "../../context/AuthContext/AuthContext";
import api, { setApiAuthState } from "../../services/axiosConfig";

const AuthSync = () => {
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    setApiAuthState(isAuthenticated);
  }, [isAuthenticated]);

  return null;
}

export default AuthSync;