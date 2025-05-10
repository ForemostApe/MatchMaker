import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from '../../context/AuthContext/AuthContext';

const ProtectedRoute = ({ children, isAllowed, allowedRoles, redirectPath = '/' }) => {
  const { user, isLoading } = useAuth();

  if (isLoading) {
    return <div>Loading authentication...</div>;
  }  

  const hasRequiredRole = allowedRoles.length === 0 || allowedRoles.includes(user?.role);

  if (!isAllowed || !user || !hasRequiredRole) {
    return <Navigate to={redirectPath} replace />;
  }

  return children || <Outlet />;
};

export default ProtectedRoute;