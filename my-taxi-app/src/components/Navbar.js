import React from 'react';
import { Link } from 'react-router-dom';
import './styles/NavBar.css';

const NavBar = () => {
  return (
    <nav className="navbar">
      <div className="navbar-container">
        <Link to="/" className="navbar-logo">
          MyApp
        </Link>
        <ul className="navbar-menu">
          <li className="navbar-item">
            <Link to="/home" className="navbar-link">Home</Link>
          </li>
          <li className="navbar-item">
            <Link to="/about" className="navbar-link">Login</Link>
          </li>
          <li className="navbar-item">
            <Link to="/services" className="navbar-link">Register</Link>
          </li>
        </ul>
      </div>
    </nav>
  );
};

export default NavBar;
