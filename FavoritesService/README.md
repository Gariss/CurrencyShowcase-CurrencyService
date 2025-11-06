# Favorites Service API

A RESTful API for managing user's favorite currencies.

## Features

- **Get Favorites** - Retrieve user's favorite currencies
- **Add to Favorites** - Add currencies to user's favorites
- **Remove from Favorites** - Remove currencies from user's favorites

## API Endpoints

### GET /api/favorites
Get user's favorite currencies.

**Response:** `200 OK` with list of Currency objects

### POST /api/favorites
Add currencies to user's favorites.

**Request Body:**
```json
["R01235", "R01239", "R01375"]
```

**Response:** `200 OK` on successful addition

### DELETE /api/favorites
Remove currencies from user's favorites.

**Request Body:**
```json
["R01235", "R01239"]
```

**Response:** `200 OK` on successful removal

## Error Handling

- `400 Bad Request` - Invalid input parameters or validation errors
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Internal server error

## Response Models

### Currency
```json
{
  "id": "string",
  "charCode": "string",
  "name": "string",
  "rate": "number"
}
```

## Technologies

- ASP.NET Core
- Entity Framework Core
- REST API conventions