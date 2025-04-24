import { createBrowserRouter, RouterProvider, Route } from 'react-router-dom'
import Layout from './layout/layout'
import HomePage from './pages/HomePage/HomePage';
import './App.css'
import LoginPage from './pages/LoginPage/LoginPage';
import ProtectedRoute from './components/ProtectedRoute/ProtectedRoute';
import { AuthProvider, useAuth } from './context/AuthContext/AuthContext';
import RegistrationPage from './pages/RegistrationPage/RegistrationPage';
import VerifyEmailPage from './pages/VerifyEmailPage/VerifyEmailPage';

const ProtectedRouteWrapper = ({ children }) => {
  const { user } = useAuth();
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
        { path: "/verify-email", element: <VerifyEmailPage />},
        { path: 'home', element: 
          <ProtectedRouteWrapper>
            <HomePage />
          </ProtectedRouteWrapper> 
        },
    ]
  }
]);

function App() {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  )
}

export default App
