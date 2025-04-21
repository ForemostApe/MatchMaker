import { createBrowserRouter, RouterProvider, Route } from 'react-router-dom'
import Layout from './layout/layout'
import HomePage from './pages/HomePage/HomePage';

import './App.css'
import LoginPage from './pages/LoginPage/LoginPage';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Layout />,
    children: [
      { index: true, element: <LoginPage /> },
      { path: 'home', element: <HomePage /> },
    ]
  }
]);

function App() {
  return <RouterProvider router={router} />
}

export default App
