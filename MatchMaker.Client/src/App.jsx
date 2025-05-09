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
import ProtectedRoute from './components/ProtectedRoute/ProtectedRoute';

const ProtectedRouteWrapper = ({ children }) => {
  const { user, isLoading } = useAuth();
  if (isLoading) return <div>Loading...</div>;

  return (
    <ProtectedRoute isAllowed={!!user} redirectPath="/">
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
