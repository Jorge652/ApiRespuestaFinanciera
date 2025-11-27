CREATE DATABASE ApiRespuesta;
USE ApiRespuesta;
GO
CREATE TABLE Asesor (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre VARCHAR(100) NOT NULL,
        Activo BIT DEFAULT 1
    );
CREATE TABLE Financiera (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre VARCHAR(100) NOT NULL,
        Activo BIT DEFAULT 1
    );

CREATE TABLE ParametroFinancieraTiempoReconsulta (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        IdFinanciera INT NOT NULL,
        MinutosReconsulta INT NOT NULL DEFAULT 1440,
        Activo BIT DEFAULT 1,
        CONSTRAINT FK_Parametro_Financiera FOREIGN KEY (IdFinanciera) REFERENCES Financiera(Id)
    );
    PRINT 'Tabla ParametroFinancieraTiempoReconsulta creada';


CREATE TABLE SolicitudCredito (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NumeroSolicitud VARCHAR(50) NOT NULL UNIQUE,
        IdAsesor INT NOT NULL,
        IdFinanciera INT NOT NULL,
        FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
        
        CONSTRAINT FK_Solicitud_Asesor FOREIGN KEY (IdAsesor) REFERENCES Asesor(Id),
        CONSTRAINT FK_Solicitud_Financiera FOREIGN KEY (IdFinanciera) REFERENCES Financiera(Id)
    );
    
    CREATE INDEX IX_Solicitud_Numero ON SolicitudCredito(NumeroSolicitud);

CREATE TABLE RespuestaCreditoFinanciera (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        IdSolicitud INT NOT NULL,
        NumeroSolicitud VARCHAR(50) NOT NULL,
        Estado VARCHAR(30) NOT NULL,
        MontoAprobado DECIMAL(18,2) NULL,
        Tasa DECIMAL(5,2) NULL,
		Plazo INT NULL,
        Observaciones NVARCHAR(1000) NULL,
        FechaRespuesta DATETIME NOT NULL ,
        JsonCompleto NVARCHAR(MAX) NOT NULL,
        FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
        
        CONSTRAINT FK_Respuesta_Solicitud FOREIGN KEY (IdSolicitud) REFERENCES SolicitudCredito(Id)
    );
    
    CREATE INDEX IX_Respuesta_Numero ON RespuestaCreditoFinanciera(NumeroSolicitud);

CREATE TABLE NotificacionAsesor (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdAsesor INT NOT NULL,
    IdSolicitud INT NOT NULL,
    Mensaje NVARCHAR(500) NOT NULL,
    Fecha DATETIME DEFAULT GETUTCDATE(),
    Leido BIT DEFAULT 0
);


INSERT INTO Asesor (Nombre, Activo) VALUES ('Jorge Nasimba',1);
INSERT INTO Asesor (Nombre, Activo) VALUES ('Andres Nasimba',1);

INSERT INTO Financiera (Nombre, Activo) VALUES ('Banco Pichincha',1);
INSERT INTO Financiera (Nombre, Activo) VALUES ('Banco Pordubanco',1);

INSERT INTO ParametroFinancieraTiempoReconsulta (IdFinanciera, MinutosReconsulta,Activo) 
VALUES (1, 30,1); --respuesta de 30 min

INSERT INTO SolicitudCredito (NumeroSolicitud, IdAsesor, IdFinanciera, FechaCreacion)
VALUES ('SOL-2025-001245', 2, 1, GETDATE());