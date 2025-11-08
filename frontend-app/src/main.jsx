import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import { AuthProvider } from './context/AuthContext.jsx'
import APP_CONFIG from './config/app.js'
import './index.css'

// Set app title from config
document.title = APP_CONFIG.APP_NAME;

console.log(`🚀 ${APP_CONFIG.APP_NAME} v${APP_CONFIG.APP_VERSION}`);
console.log(`📍 Frontend URL: ${APP_CONFIG.FRONTEND_URL}`);
console.log(`🔗 API URL: ${APP_CONFIG.API_URL}`);

ReactDOM.createRoot(document.getElementById('root')).render(
  <AuthProvider>
    <App />
  </AuthProvider>,
)