# Currency Manager Frontend

A modern web application for managing favorite currencies with user authentication, built with React and Vite.

## 🚀 Features

- **🔐 Authentication & Authorization**
  - User registration and login
  - JWT token-based authentication
  - Automatic token persistence
  - Protected routes

- **💱 Currency Management**
  - View all available currencies
  - Real-time exchange rates
  - Refresh currency data
  - Responsive currency table

- **⭐ Favorites System**
  - Add/remove currencies to favorites
  - Persistent favorite storage
  - Favorites summary display
  - One-click favorite toggling

## 🛠 Tech Stack

- **Frontend Framework**: React 18
- **Build Tool**: Vite
- **Routing**: React Router DOM
- **State Management**: React Context API + Hooks
- **HTTP Client**: Fetch API
- **Styling**: CSS3 with modern features
```

## 🚀 Getting Started

### Prerequisites

- Node.js 16+ 
- npm or yarn
- Backend API

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd frontend-app
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Environment Configuration**
   Create `.env` file:
   ```env
   VITE_API_BASE_URL=https://localhost:5092
   VITE_API_TIMEOUT=10000
   ```

4. **Start development server**
   ```bash
   npm run dev
   ```
   App will be available at `http://localhost:5173`

### Building for Production

```bash
npm run build
npm run preview
```

## 🔌 API Integration

The frontend integrates with three .NET microservices:

### Authentication Endpoints
- `POST /api/users` - User registration
- `POST /api/users/login` - User login (returns JWT in Authorization header)
- `POST /api/users/logout` - User logout
- `GET /api/users` - Get user profile

### Currency Endpoints
- `GET /api/currencies` - Get all currencies
- `POST /api/currencies/refresh` - Refresh currency rates

### Favorites Endpoints
- `GET /api/favorites` - Get user's favorite currencies
- `POST /api/favorites` - Add currencies to favorites
- `DELETE /api/favorites` - Remove currencies from favorites

## 🎨 UI/UX Features

- **Responsive Design**: Works on desktop and mobile devices
- **Dark/Light Theme Support**: Adaptive styling
- **Loading States**: Visual feedback for all operations
- **Error Handling**: User-friendly error messages
- **Form Validation**: Client-side validation with clear feedback

## 🔒 Security

- JWT tokens stored securely in localStorage
- Automatic token inclusion in API requests
- Protected routes for authenticated users only
- CORS properly configured for secure cross-origin requests

## 📱 Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+