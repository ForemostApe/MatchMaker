import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from '../../context/AuthContext/AuthContext';

const ProtectedRoute = ({ children, isAllowed, redirectPath = '/' }) => {
  const { user, isLoading } = useAuth();
  
  if (isLoading) {
    return <div>Loading authentication...</div>;
  }

  if (!isAllowed) {
    return <Navigate to={redirectPath} replace />;
  }

  return children || <Outlet />;
};

export default ProtectedRoute;