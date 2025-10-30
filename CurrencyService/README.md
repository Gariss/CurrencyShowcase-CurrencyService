# Currency Service

- Background Service for refreshing of all currency rates from the data source
- A simple RESTful API for managing and retrieving currency exchange rates.

## Features

- **Get All Currencies** - Retrieve a complete list of all available currencies
- **Get Currency by Code** - Fetch specific currency details by its character code (e.g., USD, EUR)
- **Refresh Rates** - Update currency rates with latest exchange data

## API Endpoints

### `GET /api/currencies`
Returns a list of all available currencies.

**Response:** `200 OK` with array of `Currency` objects

### `GET /api/currencies/{charCode}`
Returns details for a specific currency by its character code.

**Parameters:**
- `charCode` (path) - Currency character code (e.g., USD, EUR, GBP)

**Response:** `200 OK` with `Currency` object  
**Errors:** `404 Not Found` if currency doesn't exist

### `POST /api/currencies/refresh`
Forces a refresh of all currency rates from the data source.

**Response:** `200 OK` on success  
**Errors:** `500 Internal Server Error` if refresh fails

## Error Handling

- `400 Bad Request` - Invalid input parameters
- `404 Not Found` - Requested resource not found
- `500 Internal Server Error` - Internal server error

## Technologies

- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- REST API conventions
- Refit http client