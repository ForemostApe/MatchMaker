import './App.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext/AuthContext';
import AuthLayout from './layout/AuthLayout';
import PublicLayout from './layout/PublicLayout';
import HomePage from './pages/HomePage/HomePage';
import LoginPage from './pages/LoginPage/LoginPage';
import RegistrationPage from './pages/RegistrationPage/RegistrationPage';
import VerifyEmailPage from './pages/VerifyEmailPage/VerifyEmailPage';
import GamePage from './pages/GamePage/GamePage';
import CreateGame from './pages/CreateGamePage/CreateGamePage';
import ProfilePage from './pages/ProfilePage/ProfilePage';
import ProtectedRoute from './components/ProtectedRoute/ProtectedRoute';
import AdminPage from './pages/AdminPage/AdminPage';

const ProtectedRouteWrapper = ({ children, allowedRoles = [] }) => {
  const { user, isLoading } = useAuth();
  if (isLoading) return <div>Loading...</div>;

  return (
    <ProtectedRoute isAllowed={!!user} allowedRoles={allowedRoles} redirectPath="/">
      {children}
    </ProtectedRoute>
  );
};

const router = createBrowserRouter([
  {
    path: '/',
    element: <PublicLayout />,
    children: [
      { index: true, element: <LoginPage /> },
      { path: 'register', element: <RegistrationPage /> },
      { path: 'verify-email', element: <VerifyEmailPage /> },
    ],
  },
  {
    path: '/',
    element: <AuthLayout />,
    children: [
      {
        path: 'home',
        element: (
          <ProtectedRouteWrapper>
            <HomePage />
          </ProtectedRouteWrapper>
        ),
      },
      {
        path: 'game/:gameId',
        element: (
          <ProtectedRouteWrapper>
            <GamePage />
          </ProtectedRouteWrapper>
        ),
      },
      {
        path: 'profile/:userId',
        element: (
          <ProtectedRouteWrapper>
            <ProfilePage />
          </ProtectedRouteWrapper>
        ),
      },
      {
        path: 'game/create',
        element: (
          <ProtectedRouteWrapper roles={['Coach']}>
            <CreateGame />
          </ProtectedRouteWrapper>
        ),
      },
      {
        path: 'admin',
        element: (
          <ProtectedRouteWrapper roles={['Admin']}>
            <AdminPage />
          </ProtectedRouteWrapper>
        )
      }
    ],
  },
]);

function App() {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  );
}

export default App;
