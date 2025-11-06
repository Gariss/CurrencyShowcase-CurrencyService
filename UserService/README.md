# User Service API

A RESTful API for user management, authentication, and profile management.

## Features

- **User Registration** - Create new user accounts
- **User Authentication** - Login with credentials  
- **Profile Management** - Retrieve user profile information

## API Endpoints

### POST /api/users
Register a new user account.

**Request Body:**
```json
{
  "login": "string",
  "email": "string", 
  "password": "string",
  "name": "string"
}
```

**Response:** `200 OK` on successful registration

### POST /api/users/login
Authenticate user and return access token.

**Request Body:**
```json
{
  "login": "string",
  "password": "string"
}
```

**Response:** `200 OK` with authentication token

### GET /api/users
Get user profile information.

**Query Parameters:**
- `login` (required) - User login identifier

**Response:** `200 OK` with UserResponse object

## Error Handling

- `400 Bad Request** - Invalid input parameters or validation errors
- `404 Not Found** - User not found
- `500 Internal Server Error** - Internal server error

## Response Models

### UserResponse
```json
{
  "id": "uuid",
  "login": "string",
  "email": "string",
  "name": "string",
  "createdAt": "datetime"
}
```

## Technologies

- ASP.NET Core
- Entity Framework Core
- JWT Authentication