`ENGLISH`
====================================================================================================================================================================================================================
# FerreteriaJuanitoApi
API with various functions in .NET using JWT and SQL Server. 
The goal is to develop a system that allows the management of product stock for a small business (SME).
This API includes the Serilog library for log tracking.

# Instructions for configuring the database and Serilog library.
The project comes with the Entity Framework (EF) library for managing migrations in SQL Server.
To perform the configuration:
-First: You must add the database connection string to the `appsettings.json`. This file also contains the configuration for creating a table to track logs, if desired.
-Second: Run the CLI in the IDE's terminal as follows: `dotnet ef database update Adjustes2`. Or, if you're experienced with this library, you can run `dotnet ef remove migrations` to start a fresh migration.

# Basic Entities
- User
- Customer
- Product
- StockItem
- Cart
- CartItems
- Purchase
- Login

# Features and Functionalities
All entities must have their corresponding CRUD (Create, Read, Update, Delete) implemented, unless it is implicit that one of these actions is not required to be supported.

# User
- User Creation
- Login

# Products
- Product Creation
- Edit Product
- Delete Product




# ESPAÑOL
====================================================================================================================================================================================================================
# FerreteriaJuanitoApi
API con diversas funciones en .NET usando JWT y SQL server.
El objetivo es desarrollar un sistema que permita la adminsitración del stock de productos a una PYME.
Esta API cuenta con la libreria Serilogs para el seguimento de logs.

# Intrucciones para configuracion de base datos y libreria Serilogs
El proyecto viene con la libreria Entity Framework(EF) para el gestionamiento de migraciones en SqlServer.
Para realizar la configuracion:
- Primero: Se debe agregar la cadena de conexion a base de datos en el `appsetting.json`, aca mismo esta para realizar la configuracion si se desea crear una tabla para llevar un control de logs.
- Segundo: Ejecutar CLI en la terminal del IDE que se este usando de la siguiente manera `dotnet ef database update Ajunstes2` o si eres experimentado con dicha libreria, puedes realizar un `dotnet ef remove migrations` para comenzar de cero tu propia migracion

# Entidades basicas
  - Usuario
  - Cliente
  - Procuto
  - StockItem
  - Carrito
  - CarritoItems
  - Compra
  - Login

  # Características y Funcionalidades
  Todas las entidades deben tener implementado su correspondiente ABM, a menos que sea implício el no tener que soportar alguna de estas acciones.

  # Usuario
  - Creacion de Usuario
  - Login

  # Productos
  - Creacion de Producto
  - Editar producto
  - Eliminar producto
  
