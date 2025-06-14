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
    <nav className="bg-gray-800 p-4 relative">
      <div className="container mx-auto flex justify-between items-center">
        <div className="hidden md:flex space-x-4">
          <Link to="/home" className="text-white">Hem</Link>

          {!isLoading && user?.userRole === 'Coach' && (
            <Link to="/game/create" className="text-white">Skapa match</Link>
          )}

          {!isLoading && user && (
            <Link to={profileLink} className="text-white">Profil</Link>
          )}

          {!isLoading && user?.userRole === 'Admin' && (
            <Link to="/admin" className="text-white">Admin</Link>
          )}
        </div>

        <div className="hidden md:block">
          {!isLoading && user && (
            <button className="text-white hover:cursor-pointer" onClick={handleLogoutClick}>
              Logga ut
            </button>
          )}
        </div>

        <div className="md:hidden ml-auto items-center">
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
  <div className="absolute left-0 w-full bg-gray-800 z-50 flex flex-col space-y-2 p-4 md:hidden items-end">
    <Link to="/home" className="text-white">Hem</Link>

    {!isLoading && user?.userRole === 'Coach' && (
      <Link to="/game/create" className="text-white">Skapa match</Link>
    )}

    {!isLoading && user && (
      <Link to={profileLink} className="text-white">Profil</Link>
    )}

    {!isLoading && user?.userRole === 'Admin' && (
      <Link to="/admin" className="text-white">Admin</Link>
    )}

    {!isLoading && user && (
      <button className="text-white text-left hover:cursor-pointer" onClick={handleLogoutClick}>
        Logga ut
      </button>
    )}
  </div>
)}
    </nav>
  );
};

export default Navbar;
