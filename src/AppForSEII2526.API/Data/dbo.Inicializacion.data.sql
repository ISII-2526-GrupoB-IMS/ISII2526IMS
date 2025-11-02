-- PASO 1: Insertar Modelos
SET IDENTITY_INSERT [dbo].[Modelo] ON;

INSERT INTO [dbo].[Modelo] ([Id], [NombreModelo]) VALUES
(1, 'iPhone 14 Pro'),
(2, 'Galaxy S23 Ultra'),
(3, 'MacBook Pro M2'),
(4, 'iPad Air'),
(5, 'Surface Pro 9'),
(6, 'PlayStation 5'),
(7, 'Xbox Series X'),
(8, 'Apple Watch Series 8'),
(9, 'AirPods Pro'),
(10, 'Canon EOS R6');

SET IDENTITY_INSERT [dbo].[Modelo] OFF;
GO

-- PASO 2: Insertar Dispositivos
SET IDENTITY_INSERT [dbo].[Dispositivo] ON;

INSERT INTO [dbo].[Dispositivo] 
([Id], [ModeloId], [Marca], [Color], [NombreDispositivo], [PrecioParaCompra], [PrecioParaAlquiler], [CantidadParaCompra], [CantidadParaAlquilar], [Año], [Calidad])
VALUES
-- Smartphones
(1, 1, 'Apple', 'Negro', 'iPhone 14 Pro 256GB', 1199.99, 45.00, 5, 10, 2023, 0),
(2, 1, 'Apple', 'Plata', 'iPhone 14 Pro 512GB', 1399.99, 50.00, 3, 8, 2023, 0),
(3, 2, 'Samsung', 'Verde', 'Galaxy S23 Ultra 512GB', 1299.99, 48.00, 4, 12, 2023, 0),
(4, 2, 'Samsung', 'Negro', 'Galaxy S23 Ultra 256GB', 1099.99, 42.00, 6, 15, 2023, 0),

-- Laptops
(5, 3, 'Apple', 'Gris Espacial', 'MacBook Pro 14" M2 16GB', 2199.99, 85.00, 3, 6, 2023, 0),
(6, 3, 'Apple', 'Plata', 'MacBook Pro 16" M2 32GB', 2999.99, 120.00, 2, 4, 2023, 0),
(7, 5, 'Microsoft', 'Platino', 'Surface Pro 9 i7 16GB', 1599.99, 65.00, 4, 8, 2023, 0),

-- Tablets
(8, 4, 'Apple', 'Azul', 'iPad Air 5 256GB', 749.99, 35.00, 8, 20, 2022, 0),
(9, 4, 'Apple', 'Rosa', 'iPad Air 5 64GB', 599.99, 28.00, 10, 25, 2022, 1),

-- Consolas
(10, 6, 'Sony', 'Blanco', 'PlayStation 5 Disc Edition', 549.99, 25.00, 5, 12, 2023, 0),
(11, 6, 'Sony', 'Blanco', 'PlayStation 5 Digital', 449.99, 22.00, 6, 15, 2023, 0),
(12, 7, 'Microsoft', 'Negro', 'Xbox Series X 1TB', 499.99, 23.00, 7, 14, 2023, 0),

-- Accesorios
(13, 8, 'Apple', 'Medianoche', 'Apple Watch Series 8 45mm', 449.99, 18.00, 10, 18, 2023, 0),
(14, 9, 'Apple', 'Blanco', 'AirPods Pro 2da Gen', 249.99, 12.00, 15, 30, 2023, 0),

-- Cámaras
(15, 10, 'Canon', 'Negro', 'Canon EOS R6 Body', 2499.99, 95.00, 2, 5, 2022, 0);

SET IDENTITY_INSERT [dbo].[Dispositivo] OFF;
GO

-- PASO 3: Insertar Usuarios (requiere que AspNetUsers exista)
-- Nota: Estos INSERT asumen que ya tienes usuarios creados.
-- Si no los tienes, debes crearlos a través del sistema de Identity de ASP.NET

-- Ejemplo de cómo insertar un usuario básico (solo para testing - en producción usa Identity):

INSERT INTO [dbo].[AspNetUsers] 
([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], 
 [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount],
 [NombreUsuario], [ApellidosUsuario], [DireccionDeEnvio])
VALUES
('user-001', 'juan.perez@email.com', 'JUAN.PEREZ@EMAIL.COM', 'juan.perez@email.com', 'JUAN.PEREZ@EMAIL.COM', 1,
 'AQAAAAIAAYagAAAAEDummyHashForTesting', NEWID(), NEWID(), 0, 0, 1, 0,
 'Juan', 'Pérez García', 'Calle Mayor 123, Madrid'),
 
('user-002', 'maria.lopez@email.com', 'MARIA.LOPEZ@EMAIL.COM', 'maria.lopez@email.com', 'MARIA.LOPEZ@EMAIL.COM', 1,
 'AQAAAAIAAYagAAAAEDummyHashForTesting', NEWID(), NEWID(), 0, 0, 1, 0,
 'María', 'López Martínez', 'Avenida Libertad 45, Barcelona'),
 
('user-003', 'carlos.sanchez@email.com', 'CARLOS.SANCHEZ@EMAIL.COM', 'carlos.sanchez@email.com', 'CARLOS.SANCHEZ@EMAIL.COM', 1,
 'AQAAAAIAAYagAAAAEDummyHashForTesting', NEWID(), NEWID(), 0, 0, 1, 0,
 'Carlos', 'Sánchez Ruiz', 'Plaza España 7, Valencia');


-- PASO 4: Insertar Alquileres
-- IMPORTANTE: Reemplaza 'user-001', 'user-002', 'user-003' con IDs reales de tu tabla AspNetUsers
SET IDENTITY_INSERT [dbo].[Alquiler] ON;

INSERT INTO [dbo].[Alquiler] 
([Id], [DireccionEntrega], [MetodoPago], [FechaAlquiler], [FechaAlquilerDesde], [FechaAlquilerHasta], [PrecioTotal], [ApplicationUserId])
VALUES
-- Alquiler 1: Usuario alquila iPhone por 7 días
(1, 'Calle Mayor 123, 28001 Madrid', 0, '2024-10-01', '2024-10-05', '2024-10-12', 315.00, 'user-001'),

-- Alquiler 2: Usuario alquila MacBook por 15 días
(2, 'Avenida Libertad 45, 08001 Barcelona', 1, '2024-10-03', '2024-10-10', '2024-10-25', 1275.00, 'user-002'),

-- Alquiler 3: Usuario alquila PlayStation y varios accesorios por 30 días
(3, 'Plaza España 7, 46001 Valencia', 2, '2024-10-05', '2024-10-15', '2024-11-15', 750.00, 'user-003'),

-- Alquiler 4: Usuario alquila iPad por 10 días
(4, 'Calle Alcalá 200, 28028 Madrid', 0, '2024-10-10', '2024-10-20', '2024-10-30', 350.00, 'user-001'),

-- Alquiler 5: Usuario alquila cámara Canon por 5 días (evento/boda)
(5, 'Rambla Catalunya 88, 08008 Barcelona', 0, '2024-10-12', '2024-10-25', '2024-10-30', 475.00, 'user-002');

SET IDENTITY_INSERT [dbo].[Alquiler] OFF;
GO

-- PASO 5: Insertar Items de Alquiler (tabla intermedia AlquilarDispositivo)
INSERT INTO [dbo].[AlquilarDispositivo] 
([IdDispositivo], [IdAlquiler], [Precio], [Cantidad])
VALUES
-- Alquiler 1: iPhone 14 Pro (7 días x 45€/día)
(1, 1, 45.00, 1),

-- Alquiler 2: MacBook Pro (15 días x 85€/día)
(5, 2, 85.00, 1),

-- Alquiler 3: PlayStation + accesorios (30 días)
(10, 3, 25.00, 1),  -- PlayStation 5
(14, 3, 12.00, 2),  -- 2x AirPods (por ejemplo, para jugar con amigos)

-- Alquiler 4: iPad Air (10 días x 35€/día)
(8, 4, 35.00, 1),

-- Alquiler 5: Canon EOS R6 (5 días x 95€/día)
(15, 5, 95.00, 1);
GO