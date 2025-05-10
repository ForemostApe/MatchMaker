import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext/AuthContext';

const Navbar = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const { user, logout, isLoading } = useAuth();
  const navigate = useNavigate();

  const toggleMenu = () => setIsMenuOpen(!isMenuOpen);

  const handleLogoutClick = async () => {
    await logout();
    navigate('/');
  };

  const profileLink = user?.id ? `/profile/${user.id}` : '/profile';

  return (
    <nav className="bg-gray-800 p-4">
      <div className="container mx-auto flex justify-between items-center">
        <div className="flex space-x-4">
          <Link to="/home" className="text-white">Home</Link>

          {!isLoading && user?.userRole === 'Coach' && (
            <Link to="/game/create" className="text-white">Create Game</Link>
          )}

          {!isLoading && user && (
            <Link to={profileLink} className="text-white">Profile</Link>
          )}

          {!isLoading && user?.userRole === 'Admin' && (
            <Link to="/admin" className="text-white">Admin</Link>
          )}
        </div>

        <div className="hidden md:block">
          {!isLoading && user && (
            <button className="text-white" onClick={handleLogoutClick}>
              Logout
            </button>
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
        <div className="flex flex-col space-y-2">
          <Link to="/home" className="text-white">Home</Link>

          {!isLoading && user?.role === 'Coach' && (
            <Link to="/game/create" className="text-white">Create Game</Link>
          )}

          {!isLoading && user && (
            <Link to={profileLink} className="text-white">Profile</Link>
          )}

          {!isLoading && user?.role === 'Admin' && (
            <Link to="/admin" className="text-white">Admin</Link>
          )}

          {!isLoading && user && (
            <button className="text-white" onClick={handleLogoutClick}>
              Logout
            </button>
          )}
        </div>
      )}
    </nav>
  );
};

export default Navbar;
