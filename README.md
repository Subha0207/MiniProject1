# FLIGHT MANAGEMENT SYSTEM API Documentation

## Overview

This Flight Management API provides functionalities for role-based login, user registration, account activation, flight and route management, subroute handling, refund requests, payment processing, and flight cancellation management.

## Authentication Endpoints

### Login

**Description:** Used for role-based login for users and admins. Accounts must be activated before access is granted.

**Endpoint:** `POST /api/auth/login`

**Parameters:**

- `userLoginDTO` (LoginDTO): Contains login details.

**Returns:** Task\<LoginReturnDTO>

---

### Register

**Description:** Used for user and admin registration.

**Endpoint:** `POST /api/auth/register`

**Parameters:**

- `userRegisterDTO` (RegisterDTO): Contains registration details.

**Returns:** Task\<RegisterReturnDTO>

---

### User Activation

**Description:** User activation by admin after registration.

**Endpoint:** `POST /api/auth/activate`

**Parameters:**

- `userId` (int): ID of the user to be activated.

**Returns:** Task\<int>

---

### Generate Token

**Description:** Generates a token for JWT authentication.

**Endpoint:** `POST /api/auth/token`

**Parameters:**

- `user` (User): User details for token generation.

**Returns:** string (JWT token)

---

## Flight Management Endpoints

### Add Flight

**Description:** Adds a flight by admin.

**Endpoint:** `POST /api/flights`

**Parameters:**

- `flightDTO` (FlightDTO): Flight details.

**Returns:** Task\<FlightReturnDTO>

---

### Update Flight

**Description:** Updates flight details by admin.

**Endpoint:** `PUT /api/flights`

**Parameters:**

- `FlightReturnDTO` (FlightReturnDTO): Updated flight details.

**Returns:** Task\<FlightReturnDTO>

---

### Delete Flight

**Description:** Deletes a flight by its ID.

**Endpoint:** `DELETE /api/flights/{flightId}`

**Parameters:**

- `flightId` (int): ID of the flight to be deleted.

**Returns:** Task\<FlightReturnDTO>

---

### Get Flight

**Description:** Retrieves flight details by flight ID.

**Endpoint:** `GET /api/flights/{flightId}`

**Parameters:**

- `flightId` (int): ID of the flight.

**Returns:** Task\<FlightReturnDTO>

---

### Get All Flights

**Description:** Retrieves all flights.

**Endpoint:** `GET /api/flights`

**Returns:** Task\<List\<FlightReturnDTO>>

---

## Route Management Endpoints

### Add Route

**Description:** Adds a route for a flight by admin.

**Endpoint:** `POST /api/routes`

**Parameters:**

- `routeDTO` (RouteDTO): Route details.

**Returns:** Task\<RouteReturnDTO>

---

### Delete Route

**Description:** Deletes a route by its ID.

**Endpoint:** `DELETE /api/routes/{routeId}`

**Parameters:**

- `routeId` (int): ID of the route to be deleted.

**Returns:** Task\<RouteReturnDTO>

---

### Get All Routes

**Description:** Retrieves all routes.

**Endpoint:** `GET /api/routes`

**Returns:** Task\<List\<RouteReturnDTO>>

---

### Get Route

**Description:** Retrieves route details by route ID.

**Endpoint:** `GET /api/routes/{routeId}`

**Parameters:**

- `routeId` (int): ID of the route.

**Returns:** Task\<RouteReturnDTO>

---

### Update Route

**Description:** Updates route details by admin.

**Endpoint:** `PUT /api/routes`

**Parameters:**

- `routeReturnDTO` (RouteReturnDTO): Updated route details.

**Returns:** Task\<RouteReturnDTO>

---

## SubRoute Management Endpoints

### Add SubRoutes

**Description:** Adds subroutes for flights with more than one stop.

**Endpoint:** `POST /api/subroutes`

**Parameters:**

- `subrouteDTO` (SubRouteDTO[]): Array of subroute details.

**Returns:** Task\<SubRouteReturnDTO[]>

---

### Delete SubRoute

**Description:** Deletes a subroute by its ID.

**Endpoint:** `DELETE /api/subroutes/{subrouteId}`

**Parameters:**

- `subrouteId` (int): ID of the subroute to be deleted.

**Returns:** Task\<SubRouteReturnDTO>

---

### Get All SubRoutes

**Description:** Retrieves all subroutes for a given route.

**Endpoint:** `GET /api/subroutes`

**Returns:** Task\<List\<SubRouteReturnDTO>>

---

### Get SubRoute

**Description:** Retrieves subroute details by subroute ID.

**Endpoint:** `GET /api/subroutes/{subrouteId}`

**Parameters:**

- `subrouteId` (int): ID of the subroute.

**Returns:** Task\<SubRouteReturnDTO>

---

### Update SubRoute

**Description:** Updates subroute details.

**Endpoint:** `PUT /api/subroutes`

**Parameters:**

- `subrouteReturnDTO` (SubRouteReturnDTO): Updated subroute details.

**Returns:** Task\<SubRouteReturnDTO>

---

## Refund Management Endpoints

### Add Refund

**Description:** Adds a refund request by user.

**Endpoint:** `POST /api/refunds`

**Parameters:**

- `refundDTO` (RefundDTO): Refund request details.

**Returns:** Task\<ReturnRefundDTO>

---

### Get Refund

**Description:** Retrieves refund status by refund ID.

**Endpoint:** `GET /api/refunds/{refundId}`

**Parameters:**

- `refundId` (int): ID of the refund.

**Returns:** Task\<ReturnRefundDTO>

---

### Update Refund

**Description:** Updates refund status by admin (approve/reject).

**Endpoint:** `PUT /api/refunds`

**Parameters:**

- `updateRefundDTO` (UpdateRefundDTO): Updated refund status details.

**Returns:** Task\<ReturnRefundDTO>

---

### Get All Pending Refunds

**Description:** Retrieves all pending refund requests by admin.

**Endpoint:** `GET /api/refunds/pending`

**Returns:** Task\<List\<ReturnRefundDTO>>

---

### Delete Refund

**Description:** Deletes a refund by its ID.

**Endpoint:** `DELETE /api/refunds/{refundId}`

**Parameters:**

- `refundId` (int): ID of the refund to be deleted.

**Returns:** Task\<ReturnRefundDTO>

---

## Payment Management Endpoints

### Add Payment

**Description:** Adds a payment by user after booking.

**Endpoint:** `POST /api/payments`

**Parameters:**

- `paymentDTO` (PaymentDTO): Payment details.

**Returns:** Task\<ReturnPaymentDTO>

---

### Get Payment

**Description:** Retrieves payment details by payment ID.

**Endpoint:** `GET /api/payments/{paymentId}`

**Parameters:**

- `PaymentId` (int): ID of the payment.

**Returns:** Task\<PaymentDetailsDTO>

---

### Get All Payments

**Description:** Retrieves all payments.

**Endpoint:** `GET /api/payments`

**Returns:** Task\<List\<PaymentDetailsDTO>>

---

### Delete Payment

**Description:** Deletes a payment by its ID.

**Endpoint:** `DELETE /api/payments/{paymentId}`

**Parameters:**

- `paymentId` (int): ID of the payment to be deleted.

**Returns:** Task\<ReturnPaymentDTO>

---

## Flight Route and Subroute Management Endpoints

### Get All Flights Routes and Subroutes

**Description:** Retrieves all routes and subroutes for all flights.

**Endpoint:** `GET /api/flights/routesandsubroutes`

**Returns:** Task\<Dictionary\<int, Dictionary\<int, List\<SubRouteDisplayDTO>>>>

---

### Get All Direct Flights

**Description:** Retrieves all direct flights.

**Endpoint:** `GET /api/flights/direct`

**Returns:** Task\<Dictionary\<int, List\<RouteDTO>>>

---

## Cancellation Management Endpoints

### Add Cancellation

**Description:** Adds a cancellation request by user.

**Endpoint:** `POST /api/cancellations`

**Parameters:**

- `cancellationDTO` (CancellationDTO): Cancellation request details.

**Returns:** Task\<ReturnCancellationDTO>

---

### Get All Cancellations

**Description:** Retrieves all cancellations by admin.

**Endpoint:** `GET /api/cancellations`

**Returns:** Task\<List\<ReturnCancellationDTO>>

---

### Get Cancellation By ID

**Description:** Retrieves cancellation details by cancellation ID.

**Endpoint:** `GET /api/cancellations/{cancellationId}`

**Parameters:**

- `cancellationId` (int): ID of the cancellation.

**Returns:** Task\<ReturnCancellationDTO>

---

### Delete Cancellation

**Description:** Deletes a cancellation by its ID.

**Endpoint:** `DELETE /api/cancellations/{cancellationId}`

**Parameters:**

- `cancellationId` (int): ID of the cancellation to be deleted.

**Returns:** Task\<ReturnCancellationDTO>

---

## Error Handling

All endpoints return standard HTTP status codes to indicate success or failure. Common status codes include:

- `200 OK`: The request was successful.
- `400 Bad Request`: The request was invalid or cannot be otherwise served.
- `401 Unauthorized`: Authentication failed or user does not have permissions for the requested operation.
- `404 Not Found`: The requested resource could not be found.
- `500 Internal Server Error`: An error occurred on the server.
