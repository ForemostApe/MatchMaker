import './App.css';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext/AuthContext';
import Layout from './layout/layout';
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
    element: <Layout />,
    children: [
      { index: true, element: <LoginPage /> },
      { path: 'register', element: <RegistrationPage /> },
      { path: '/verify-email', element: <VerifyEmailPage /> },
      { path: 'game/:gameId', element: <GamePage /> },
      {
        path: 'home',
        element: (
          <ProtectedRouteWrapper>
            <HomePage />
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
