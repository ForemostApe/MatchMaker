import { useState } from 'react';
import { Link } from 'react-router-dom';

const Navbar = ({ user }) => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const toggleMenu = () => setIsMenuOpen(!isMenuOpen);

  return (
    <nav className="bg-gray-800 p-4">
      <div className="container mx-auto flex justify-between items-center">
        <div className="flex space-x-4">
          <Link to="/" className="text-white">Home</Link>
          <Link to="/profile" className="text-white">Profile</Link>
          {user === 'Admin' && (
            <Link to="/admin" className="text-white">Admin</Link>
          )}
        </div>

        <div className="hidden md:block">
          <button className="text-white" onClick={() => console.log('Logout')}>Logout</button>
        </div>

        <div className="md:hidden flex items-center">
          <button onClick={toggleMenu} className="text-white">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" className="w-6 h-6">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16"></path>
            </svg>
          </button>
        </div>
      </div>

      {isMenuOpen && (
        <div className="md:hidden bg-gray-800 p-4">
          <div className="flex flex-col space-y-2">
            <Link to="/" className="text-white">Home</Link>
            <Link to="/profile" className="text-white">Profile</Link>
            {user === 'Admin' && (
              <Link to="/admin" className="text-white">Admin</Link>
            )}
            <button className="text-white" onClick={() => console.log('Logout')}>Logout</button>
          </div>
        </div>
      )}
    </nav>
  );
};

export default Navbar;
