import React, { createContext, useState, useContext, useCallback, useEffect } from 'react';
import { authService } from '../services/authService';

const AuthContext = createContext();

export const useAuthContext = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuthContext must be used within an AuthProvider');
  }
  return context;
};

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(localStorage.getItem('authToken'));
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [isCheckingAuth, setIsCheckingAuth] = useState(true);

  useEffect(() => {
    const loadInitialUser = async () => {
      const savedToken = localStorage.getItem('authToken');
      if (savedToken) {
        try {
          const userProfile = await authService.getProfile(savedToken);
          setUser(userProfile);
          setToken(savedToken);
        } catch (err) {
          console.error('Failed to load user profile on init:', err);
          localStorage.removeItem('authToken');
          setToken(null);
          setUser(null);
        }
      }
      setIsCheckingAuth(false);
    };

    loadInitialUser();
  }, []);

  const loadUserProfile = useCallback(async (profileToken = token) => {
    try {
      setIsCheckingAuth(true);
      if (!profileToken) {
        throw new Error('No token provided for profile loading');
      }
      
      const userProfile = await authService.getProfile(profileToken);
      setUser(userProfile);
      setError(null);
      return userProfile;
    } catch (err) {
      console.error('Failed to load user profile:', err);
      setError('Failed to load user profile');
      logout();
      throw err;
    } finally {
      setIsCheckingAuth(false);
    }
  }, [token]);

  const login = async (credentials) => {
    try {
      setLoading(true);
      setError(null);
      
      const { token: newToken } = await authService.login(credentials);
      
      localStorage.setItem('authToken', newToken);
      setToken(newToken);
      
      const userProfile = await authService.getProfile(newToken);
      setUser(userProfile);
      
      return true;
    } catch (err) {
      setError(err.message || 'Login failed');
      return false;
    } finally {
      setLoading(false);
    }
  };

  const register = async (userData) => {
    try {
      setLoading(true);
      setError(null);
      
      await authService.register(userData);
      return true;
    } catch (err) {
      setError(err.message || 'Registration failed');
      return false;
    } finally {
      setLoading(false);
    }
  };

  const logout = useCallback(async () => {
    try {
      if (token) {
        await authService.logout(token);
      }
    } catch (err) {
      console.error('Logout error:', err);
    } finally {
      localStorage.removeItem('authToken');
      setToken(null);
      setUser(null);
      setError(null);
    }
  }, [token]);

  const value = {
    token,
    user,
    loading,
    error,
    isCheckingAuth,
    login,
    register,
    logout,
    isAuthenticated: !!token && !!user
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};