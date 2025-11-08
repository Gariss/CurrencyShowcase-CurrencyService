import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { useAuthContext } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import CurrencyPage from './pages/CurrencyPage';
import './App.css';
import APP_CONFIG from './config/app.js';

const ProtectedRoute = ({ children }) => {
  const { isAuthenticated, isCheckingAuth, user, token } = useAuthContext();
  
  if (isCheckingAuth) {
    return (
      <div style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '200px' 
      }}>
        <div>Loading...</div>
      </div>
    );
  }
  
  return isAuthenticated ? children : <Navigate to="/login" replace />;
};

const PublicRoute = ({ children }) => {
  const { isAuthenticated, isCheckingAuth } = useAuthContext();
  
  if (isCheckingAuth) {
    return (
      <div style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '200px' 
      }}>
        <div>Loading...</div>
      </div>
    );
  }
  
  return !isAuthenticated ? children : <Navigate to="/" replace />;
};

function App() {
  const { logout, user, isAuthenticated, isCheckingAuth, token } = useAuthContext();

  if (isCheckingAuth) {
    return (
      <div style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '100vh' 
      }}>
        <div>Checking authentication...</div>
      </div>
    );
  }

  return (
    <Router>
      <div className="App">
        <header className="app-header">
        <h1>{APP_CONFIG.APP_NAME}</h1>
          {user && (
            <div className="user-info">
              <span>Welcome, {user.name || user.login}!</span>
              <button onClick={logout} className="logout-btn">Logout</button>
            </div>
          )}
        </header>

        <main className="app-main">
          <Routes>
            <Route path="/login" element={<PublicRoute><LoginPage /></PublicRoute>} />
            <Route path="/" element={<ProtectedRoute><CurrencyPage /></ProtectedRoute>} />
            <Route path="*" element={<Navigate to="/" />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;