import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from '../../context/AuthContext/AuthContext';

const ProtectedRoute = ({ children, isAllowed, redirectPath = '/' }) => {
  const { user, isLoading } = useAuth();
  
  console.log('ProtectedRoute - isLoading:', isLoading);
  console.log('ProtectedRoute - user:', user);
  console.log('ProtectedRoute - isAllowed:', isAllowed);

  if (isLoading) {
    console.log('Showing loading state');
    return <div>Loading authentication...</div>;
  }

  if (!isAllowed) {
    console.log('Redirecting to', redirectPath);
    return <Navigate to={redirectPath} replace />;
  }

  console.log('Rendering protected content');
  return children || <Outlet />;
};

export default ProtectedRoute;