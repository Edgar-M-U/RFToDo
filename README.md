Para utilizar el programa sera necesario ejecutar el siguiente script en la base de datos:
1.
/**************************************************************************************\
CREATE DATABASE ToDoDB
GO

USE ToDoDB
GO

CREATE LOGIN TodoAdmin WITH PASSWORD = 'Admin123*'
,DEFAULT_DATABASE = ToDoDB
GO

USE ToDoDB;
CREATE USER TodoAdmin FOR LOGIN TodoAdmin;
EXEC sp_addrolemember 'db_datareader', 'TodoAdmin'
EXEC sp_addrolemember 'db_datawriter', 'TodoAdmin'
EXEC sp_addrolemember 'db_owner', 'TodoAdmin'
GO

CREATE TABLE Metas(
	Id_Meta INT NOT NULL IDENTITY(1,1) PRIMARY KEY
	,Nombre VARCHAR(80) NOT NULL
	,Fecha_Creacion DATETIME NOT NULL
)

CREATE TABLE Tareas(
	Id_Tarea INT NOT NULL IDENTITY(1,1) PRIMARY KEY
	,Importante BIT NOT NULL
	,Nombre VARCHAR(80) NOT NULL
	,Fecha_Creacion DATETIME NOT NULL
	,Estado VARCHAR(20) NOT NULL
	,Id_Meta int NOT NULL
	,CONSTRAINT FK_Metas_Tareas FOREIGN KEY (Id_Meta) REFERENCES Metas(Id_Meta)
)
\**************************************************************************************/

2. Posteriormente en el Projecto: RfToDoAPI -> appsettings.json
Cambiar la ruta del servidor y su instancia (encerrados en las llaves {})

Actualmente esta: 
  "ConnectionStrings": {
    "DbConnection": "Server={SERVIDOR}\{INSTANCIA};Database=ToDoDB;User ID=TodoAdmin;Password=Admin123*;Trusted_Connection=False;Trust Server Certificate=true"
  }

El programa estara listo para su uso.
