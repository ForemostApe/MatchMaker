import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext/AuthContext';

const Navbar = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const { user, logout, isLoading } = useAuth();
  const navigate = useNavigate();

if (isLoading) {
    return <div>Loading...</div>;
  }

  const toggleMenu = () => setIsMenuOpen(!isMenuOpen);

  const handleLogoutClick = async () => {
    await logout();
    navigate("/");
  };

  return (
    <nav className="bg-gray-800 p-4">
      <div className="container mx-auto flex justify-between items-center">
        <div className="flex space-x-4">
          <Link to="/" className="text-white">Home</Link>
          {user && <Link to="/profile/:userId" className="text-white">Profile</Link>}
          {user?.role === 'Admin' && (
            <Link to="/admin" className="text-white">Admin</Link>
          )}
        </div>

        <div className="hidden md:block">
          {user && (
            <button className="text-white" onClick={handleLogoutClick}>Logout</button>
          )}
        </div>

        <div className="md:hidden flex items-center">
          <button onClick={toggleMenu} className="text-white">
            <svg xmlns="http://www.w3.org/2000/svg" className="w-6 h-6" fill="none"
                 viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round"
                    strokeWidth="2" d="M4 6h16M4 12h16M4 18h16"/>
            </svg>
          </button>
        </div>
      </div>

      {isMenuOpen && (
        <div className="md:hidden bg-gray-800 p-4">
          <div className="flex flex-col space-y-2">
            <Link to="/" className="text-white">Home</Link>
            {user && <Link to="/profile" className="text-white">Profile</Link>}
            {user?.role === 'Admin' && (
              <Link to="/admin" className="text-white">Admin</Link>
            )}
            {user && (
              <button className="text-white" onClick={handleLogoutClick}>Logout</button>
            )}
          </div>
        </div>
      )}
    </nav>
  );
};

export default Navbar;
