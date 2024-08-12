import React, { useState } from 'react';

const HomePage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = (e) => {
    e.preventDefault();
    // Add your login logic here
    console.log('Login attempt with:', email, password);
  };

  const handleRegister = () => {
    // Add your navigation logic to the registration page here
    console.log('Navigating to registration page');
    navigate('/register');
  };

  return (
    <div className="home-page">
      <h1>Welcome</h1>
      <form onSubmit={handleLogin}>
        <div>
          <label htmlFor="email">Email:</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Login</button>
      </form>
      <p>
        Don't have an account?{' '}
        <button onClick={handleRegister}>Register</button>
      </p>
    </div>
  );
};

export default HomePage;