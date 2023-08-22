# Microservice Based Ecommerce project in C# developed by Milan Pantelić

## Technical Specification:

## 1. Connection between microservices, reverse proxy and RabbitMQ Message Broker: 
  ### 1.1 Reverse proxy:
  The project consists of 4 microservices and 1 API gateway serving as a reverse proxy for accessing API routes of all 4 microservices. 
  
  ### 1.2 Communication between microservices: 
Communication between microservices is accomplished through the publish-subscribe approach, with each microservice listening to the necessary queues in Rabbit       MQ based on the type of message they expect to receive. Each microservice has its own separate database, and data synchronization is facilitated through the         publish-subscribe mechanism as well. 
  
  ### 1.3 Authentication/Authorization:
Authentication and authorization are handled via JWT tokens, as they enable microservices 
to verify user identity without having to request validation from an authentication service each time. 
Instead, they utilize a secret key, unique to each microservice requiring user identification.
  
  ### 1.4 CORS Policy:
Each microservice has a CORS policy that allows access only through the API Gateway. 

## 2. The internal architecture of each microservice:
  ### 2.1 CQRS Pattern: 
In terms of project architecture, a separate CQRS (Command Query Responsibility Segregation) approach is employed. The business logic is built around commands       and queries, with each use case having its corresponding command or query, depending on the need. 
  
  ### 2.2 Repository and UnitOfWork Pattern:
Within these commands and queries, the necessary repositories are injected, as the data access layer is abstracted using the Repository Pattern. To potentially      combine multiple changes to the database context into a single transaction, the Unit of Work Pattern is used. This pattern separates the SaveChangesAsync()          method from Repository Pattern, allowing you to call SaveChangesAsync() at the end of a transaction within a use case.
  
  ### 2.3 MSSQL, Entity Framework Core 
For each microservice where a database is required, MSSQL is used as the database engine, and Entity Framework serves as the Object-Relational Mapping (ORM)         tool. 
  
  ### 2.4 MediatR library:
CQRS is facilitated by the MediatR library, which aids in implementing the Mediator design pattern for handling commands and queries.
  
  ### 2.5 Mass Transit and Shared Library for RabbitMQ Credentials and Events 
To handle publish and subscribe operations on RabbitMQ queues, the MassTransit library is used. It greatly abstracts and simplifies working with message             brokers. All events sent or received through RabbitMQ, as well as RabbitMQ credentials, are encapsulated in a shared library that all microservices can use.         This library, named MessagingHelper, helps avoid code duplication.
  
  ### 2.6 Shared Jwt Auth Library for extracting payload data from JWT:
Additionally, there is a shared JwtAuthLibrary that contains fundamental methods for extracting payload data from JWT tokens using access keys. However, methods     related to login, registration, and email verification are specifically implemented within the authentication service.
  
## 3. Core functionality:
  ### 3.1 Login, register and password encryption:  
Regarding the login and registration mechanism, it is located in the Auth microservice. Registration uses bcrypt to encrypt the password, and upon successful        registration, a record is created in the database. 
    
  ### 3.2 Mail verification with Notification Service, Auth Service and caching in Auth Service: 
However, there is a field called "MailVerified" which is set to false by default. To change this field to true, a request must be sent to a route in the Auth 
service (with a JWT token in the header). This route generates a verification code within the Auth service,places it in a cache memory for 60 seconds, and 
forwards the user's email (extracted from the JWT payload) and that code to a queue listened to by the Notification Service, responsible for sending an email to 
the user.
    
The Auth service also has a route for checking the verification code, which involves checking the cache memory for the code.

  ### 3.3 Features of this Distributed System and Role base authorization:
Ordinary users, upon registration, are assigned the "User" role, which does not grant access to certain administrator-level methods. Within this distributed 
system, there are methods for CRUD operations on categories and subcategories, CRUD operations on products, adding and removing products from the shopping cart, 
and creating orders. An order is created by taking all the products from the shopping cart.
  
  ### 3.4 Data Synchronization between databases: 
Synchronization of all this data between separate databases is achieved using a publish-subscribe approach, where copies of the necessary data are made from one 
database to another. This enables data synchronization so that a microservice never directly requests data from another but indirectly, when the microservice 
needs certain data, it retrieves it from the synchronized database with other microservices.
