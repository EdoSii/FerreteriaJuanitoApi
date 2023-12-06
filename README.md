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
  
