import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthContext } from '../context/AuthContext';
import APP_CONFIG from '../config/app.js';

const LoginPage = () => {
  const { login, register, loading, error, isAuthenticated, user } = useAuthContext();
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState('login');
  const [formData, setFormData] = useState({
    login: '',
    password: '',
    name: '',
    email: ''
  });

  useEffect(() => {    
    if (isAuthenticated && user) {
      setTimeout(() => {
        navigate('/', { replace: true });
      }, 100);
    }
  }, [isAuthenticated, user, navigate]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    const success = await login({
      login: formData.login,
      password: formData.password
    });
    
    if (success) {
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    const success = await register(formData);
    
    if (success) {
      setActiveTab('login');
      setFormData(prev => ({ ...prev, password: '' }));
      alert('Registration successful! Please login.');
    }
  };

  const isLoginFormValid = formData.login && formData.password;
  const isRegisterFormValid = formData.login && formData.password && formData.name && formData.email;

  return (
    <div className="form-container">
      <div className="tab-container">
        <button 
          className={`tab ${activeTab === 'login' ? 'active' : ''}`}
          onClick={() => setActiveTab('login')}
        >
          Login
        </button>
        <button 
          className={`tab ${activeTab === 'register' ? 'active' : ''}`}
          onClick={() => setActiveTab('register')}
        >
          Register
        </button>
      </div>

      {error && (
        <div className="error-message">
          {error}
        </div>
      )}

      {activeTab === 'login' && (
        <form onSubmit={handleLogin}>
          <div className="form-group">
            <label htmlFor="login-username">Username:</label>
            <input
              id="login-username"
              type="text"
              name="login"
              value={formData.login}
              onChange={handleInputChange}
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="login-password">Password:</label>
            <input
              id="login-password"
              type="password"
              name="password"
              value={formData.password}
              onChange={handleInputChange}
              required
            />
          </div>

          <button 
            type="submit" 
            className="btn" 
            disabled={!isLoginFormValid || loading}
          >
            {loading ? 'Logging in...' : 'Login'}
          </button>
        </form>
      )}

      {activeTab === 'register' && (
        <form onSubmit={handleRegister}>
          <div className="form-group">
            <label htmlFor="register-name">Full Name:</label>
            <input
              id="register-name"
              type="text"
              name="name"
              value={formData.name}
              onChange={handleInputChange}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="register-email">Email:</label>
            <input
              id="register-email"
              type="email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="register-username">Username:</label>
            <input
              id="register-username"
              type="text"
              name="login"
              value={formData.login}
              onChange={handleInputChange}
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="register-password">Password:</label>
            <input
              id="register-password"
              type="password"
              name="password"
              value={formData.password}
              onChange={handleInputChange}
              required
            />
          </div>

          <button 
            type="submit" 
            className="btn" 
            disabled={!isRegisterFormValid || loading}
          >
            {loading ? 'Registering...' : 'Register'}
          </button>
        </form>
      )}
    </div>
  );
};

export default LoginPage;