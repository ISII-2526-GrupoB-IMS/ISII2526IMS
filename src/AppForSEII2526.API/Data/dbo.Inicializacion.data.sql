-- PASO 1: Insertar Modelos
SET IDENTITY_INSERT [dbo].[Modelo] ON;

INSERT INTO [dbo].[Modelo] ([Id], [NombreModelo]) VALUES
(1, 'iPhone 14 Pro'),
(2, 'iPhone 13'),
(3, 'Galaxy S23 Ultra'),
(4, 'Galaxy A54'),
(5, 'Xiaomi 13 Pro'),
(6, 'Pixel 7 Pro'),
(7, 'OnePlus 11'),
(8, 'Huawei P60 Pro'),
(9, 'Oppo Find X5'),
(10, 'Realme GT3');

SET IDENTITY_INSERT [dbo].[Modelo] OFF;
GO

-- PASO 2: Insertar Dispositivos (solo móviles)
SET IDENTITY_INSERT [dbo].[Dispositivo] ON;

INSERT INTO [dbo].[Dispositivo] 
([Id], [ModeloId], [Marca], [Color], [NombreDispositivo], [PrecioParaCompra], [PrecioParaAlquiler], [CantidadParaCompra], [CantidadParaAlquilar], [Año], [Calidad])
VALUES
-- iPhone
(1, 1, 'Apple', 'Negro', 'iPhone 14 Pro 256GB', 1199.99, 45.00, 5, 10, 2023, 0),
(2, 1, 'Apple', 'Plata', 'iPhone 14 Pro 512GB', 1399.99, 50.00, 3, 8, 2023, 0),
(3, 1, 'Apple', 'Morado', 'iPhone 14 Pro 128GB', 1099.99, 42.00, 7, 12, 2023, 0),
(4, 2, 'Apple', 'Azul', 'iPhone 13 256GB', 799.99, 35.00, 10, 15, 2022, 0),
(5, 2, 'Apple', 'Rosa', 'iPhone 13 128GB', 699.99, 30.00, 12, 20, 2022, 1),

-- Samsung Galaxy
(6, 3, 'Samsung', 'Verde', 'Galaxy S23 Ultra 512GB', 1299.99, 48.00, 4, 12, 2023, 0),
(7, 3, 'Samsung', 'Negro', 'Galaxy S23 Ultra 256GB', 1099.99, 42.00, 6, 15, 2023, 0),
(8, 4, 'Samsung', 'Blanco', 'Galaxy A54 5G 256GB', 449.99, 22.00, 15, 25, 2023, 0),
(9, 4, 'Samsung', 'Negro', 'Galaxy A54 5G 128GB', 399.99, 20.00, 18, 30, 2023, 1),

-- Xiaomi
(10, 5, 'Xiaomi', 'Negro', 'Xiaomi 13 Pro 256GB', 999.99, 40.00, 8, 18, 2023, 0),
(11, 5, 'Xiaomi', 'Blanco', 'Xiaomi 13 Pro 512GB', 1099.99, 45.00, 5, 12, 2023, 0),

-- Google Pixel
(12, 6, 'Google', 'Blanco', 'Pixel 7 Pro 256GB', 899.99, 38.00, 6, 14, 2022, 0),
(13, 6, 'Google', 'Negro', 'Pixel 7 Pro 128GB', 799.99, 35.00, 8, 16, 2022, 1),

-- OnePlus
(14, 7, 'OnePlus', 'Verde', 'OnePlus 11 5G 256GB', 849.99, 37.00, 7, 15, 2023, 0),

-- Huawei
(15, 8, 'Huawei', 'Dorado', 'Huawei P60 Pro 256GB', 949.99, 40.00, 5, 10, 2023, 0),

-- Oppo
(16, 9, 'Oppo', 'Azul', 'Oppo Find X5 Pro 256GB', 799.99, 35.00, 6, 12, 2022, 0),

-- Realme
(17, 10, 'Realme', 'Negro', 'Realme GT3 240W 256GB', 649.99, 28.00, 10, 20, 2023, 0),
(18, 10, 'Realme', 'Blanco', 'Realme GT3 240W 128GB', 599.99, 25.00, 12, 22, 2023, 1);

SET IDENTITY_INSERT [dbo].[Dispositivo] OFF;
GO

-- PASO 3: Insertar Usuarios
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
 'Carlos', 'Sánchez Ruiz', 'Plaza España 7, Valencia'),

('user-004', 'laura.martin@email.com', 'LAURA.MARTIN@EMAIL.COM', 'laura.martin@email.com', 'LAURA.MARTIN@EMAIL.COM', 1,
 'AQAAAAIAAYagAAAAEDummyHashForTesting', NEWID(), NEWID(), 0, 0, 1, 0,
 'Laura', 'Martín González', 'Calle Gran Vía 50, Sevilla'),

('user-005', 'david.gomez@email.com', 'DAVID.GOMEZ@EMAIL.COM', 'david.gomez@email.com', 'DAVID.GOMEZ@EMAIL.COM', 1,
 'AQAAAAIAAYagAAAAEDummyHashForTesting', NEWID(), NEWID(), 0, 0, 1, 0,
 'David', 'Gómez Fernández', 'Paseo de la Castellana 100, Madrid');
GO

-- PASO 4: Insertar Alquileres
SET IDENTITY_INSERT [dbo].[Alquiler] ON;

INSERT INTO [dbo].[Alquiler] 
([Id], [DireccionEntrega], [MetodoPago], [FechaAlquiler], [FechaAlquilerDesde], [FechaAlquilerHasta], [PrecioTotal], [ApplicationUserId])
VALUES
-- Alquiler 1: Usuario alquila iPhone 14 Pro por 7 días
(1, 'Calle Mayor 123, 28001 Madrid', 0, '2024-10-01', '2024-10-05', '2024-10-12', 315.00, 'user-001'),

-- Alquiler 2: Usuario alquila Galaxy S23 Ultra por 15 días
(2, 'Avenida Libertad 45, 08001 Barcelona', 1, '2024-10-03', '2024-10-10', '2024-10-25', 630.00, 'user-002'),

-- Alquiler 3: Usuario alquila varios móviles por 10 días
(3, 'Plaza España 7, 46001 Valencia', 2, '2024-10-05', '2024-10-15', '2024-10-25', 850.00, 'user-003'),

-- Alquiler 4: Usuario alquila Xiaomi por 5 días
(4, 'Calle Alcalá 200, 28028 Madrid', 0, '2024-10-10', '2024-10-20', '2024-10-25', 200.00, 'user-001'),

-- Alquiler 5: Usuario alquila Pixel 7 Pro por 14 días
(5, 'Rambla Catalunya 88, 08008 Barcelona', 0, '2024-10-12', '2024-10-25', '2024-11-08', 532.00, 'user-002'),

-- Alquiler 6: Usuario alquila varios móviles económicos por 30 días
(6, 'Calle Gran Vía 50, 41001 Sevilla', 1, '2024-10-15', '2024-10-20', '2024-11-20', 1410.00, 'user-004');

SET IDENTITY_INSERT [dbo].[Alquiler] OFF;
GO

-- PASO 5: Insertar Items de Alquiler
INSERT INTO [dbo].[AlquilarDispositivo] 
([IdDispositivo], [IdAlquiler], [Precio], [Cantidad])
VALUES
-- Alquiler 1: iPhone 14 Pro (7 días x 45€/día)
(1, 1, 45.00, 1),

-- Alquiler 2: Galaxy S23 Ultra (15 días x 42€/día)
(7, 2, 42.00, 1),

-- Alquiler 3: Varios móviles (10 días)
(10, 3, 40.00, 1),  -- Xiaomi 13 Pro
(14, 3, 37.00, 1),  -- OnePlus 11
(8, 3, 22.00, 1),   -- Galaxy A54

-- Alquiler 4: Xiaomi (5 días x 40€/día)
(10, 4, 40.00, 1),

-- Alquiler 5: Pixel 7 Pro (14 días x 38€/día)
(12, 5, 38.00, 1),

-- Alquiler 6: Varios móviles económicos (30 días)
(5, 6, 30.00, 1),   -- iPhone 13
(9, 6, 20.00, 1),   -- Galaxy A54
(17, 6, 28.00, 1);  -- Realme GT3
GO

-- PASO 6: Insertar Compras
SET IDENTITY_INSERT [dbo].[Compra] ON;

INSERT INTO [dbo].[Compra] 
([Id], [MetodoDePago], [FechaCompra], [PrecioTotal], [CantidadTotal], [ApplicationUserId])
VALUES
-- Compra 1: Juan compra un iPhone 14 Pro
(1, 0, '2024-09-15 10:30:00', 1199.99, 1, 'user-001'),

-- Compra 2: María compra Galaxy S23 Ultra
(2, 1, '2024-09-18 14:20:00', 1099.99, 1, 'user-002'),

-- Compra 3: Carlos compra 2 móviles
(3, 0, '2024-09-22 16:45:00', 1799.98, 2, 'user-003'),

-- Compra 4: Laura compra iPhone 13 y Galaxy A54
(4, 2, '2024-09-25 11:00:00', 1249.98, 2, 'user-004'),

-- Compra 5: David compra Xiaomi 13 Pro
(5, 1, '2024-09-28 09:15:00', 999.99, 1, 'user-005'),

-- Compra 6: Juan hace segunda compra (3 móviles económicos)
(6, 0, '2024-10-02 15:30:00', 1649.97, 3, 'user-001'),

-- Compra 7: María compra 2 Pixel 7 Pro
(7, 1, '2024-10-05 12:00:00', 1799.98, 2, 'user-002'),

-- Compra 8: Carlos compra OnePlus y Huawei
(8, 0, '2024-10-08 10:45:00', 1799.98, 2, 'user-003'),

-- Compra 9: Laura compra 4 Galaxy A54 (para empresa)
(9, 2, '2024-10-12 14:30:00', 1799.96, 4, 'user-004'),

-- Compra 10: David compra múltiples móviles premium
(10, 1, '2024-10-15 16:20:00', 3299.97, 3, 'user-005');

SET IDENTITY_INSERT [dbo].[Compra] OFF;
GO

-- PASO 7: Insertar Items de Compra
INSERT INTO [dbo].[ItemCompra] 
([DispositivoId], [CompraId], [Descripcion], [Precio], [Cantidad])
VALUES
-- Compra 1: iPhone 14 Pro
(1, 1, 'iPhone 14 Pro 256GB color negro, pantalla 6.1 pulgadas', 1199.99, 1),

-- Compra 2: Galaxy S23 Ultra
(7, 2, 'Samsung Galaxy S23 Ultra 256GB con S Pen integrado', 1099.99, 1),

-- Compra 3: 2 móviles (iPhone + Xiaomi)
(4, 3, 'iPhone 13 256GB azul, uso personal',  799.99, 1),
(10, 3, 'Xiaomi 13 Pro negro, cámara profesional',  999.99, 1),

-- Compra 4: iPhone 13 + Galaxy A54
(5, 4, 'iPhone 13 128GB rosa, gama media', 699.99, 1),
(8, 4, 'Galaxy A54 5G 256GB blanco, batería larga duración', 449.99, 1),

-- Compra 5: Xiaomi 13 Pro
(11, 5, 'Xiaomi 13 Pro 512GB blanco, versión premium',  999.99, 1),

-- Compra 6: 3 móviles económicos
(9, 6, 'Galaxy A54 128GB negro, relación calidad-precio',  399.99, 1),
(17, 6, 'Realme GT3 256GB negro, carga rápida 240W',  649.99, 1),
(5, 6, 'iPhone 13 128GB rosa, segunda unidad',  699.99, 1),

-- Compra 7: 2 Pixel 7 Pro
(12, 7, 'Google Pixel 7 Pro 256GB blanco, Android puro',  899.99, 2),

-- Compra 8: OnePlus + Huawei
(14, 8, 'OnePlus 11 5G verde, pantalla AMOLED 120Hz',  849.99, 1),
(15, 8, 'Huawei P60 Pro dorado, cámara Leica',  949.99, 1),

-- Compra 9: 4 Galaxy A54 (compra empresarial)
(8, 9, 'Pack 4 Galaxy A54 5G 256GB para empresa',  449.99, 4),

-- Compra 10: Múltiples premium
(2, 10, 'iPhone 14 Pro 512GB plata, máximo almacenamiento', 1399.99, 1),
(6, 10, 'Galaxy S23 Ultra 512GB verde, máxima potencia',  1299.99, 1),
(16, 10, 'Oppo Find X5 Pro azul, diseño premium', 799.99, 1);
GO

-- PASO 8: Insertar Reviews
SET IDENTITY_INSERT [dbo].[Review] ON;

INSERT INTO [dbo].[Review] 
([Id], [Titulo], [Pais], [FechaReview], [CalificaciónGeneral] , [ApplicationUserId])
VALUES
-- Review 1
(1, 'Excelente experiencia con el iPhone', 'España', '2024-09-16', 5, 'user-001'),

-- Review 2
(2, 'Galaxy S23 Ultra impresionante', 'México', '2024-09-19', 4, 'user-002'),

-- Review 3
(3, 'Buena compra de varios móviles', 'Argentina', '2024-09-23', 4, 'user-003'),

-- Review 4
(4, 'iPhone y Galaxy A54 combinados', 'Chile', '2024-09-26', 5, 'user-004'),

-- Review 5
(5, 'Xiaomi 13 Pro muy recomendable', 'Perú', '2024-09-29', 5, 'user-005'),

-- Review 6
(6, 'Compré 3 móviles económicos, contento', 'España', '2024-10-03', 4, 'user-001'),

-- Review 7
(7, 'Pixel 7 Pro, buen rendimiento', 'México', '2024-10-06', 4, 'user-002'),

-- Review 8
(8, 'OnePlus y Huawei cumplen expectativas', 'Argentina', '2024-10-09', 4, 'user-003'),

-- Review 9
(9, 'Galaxy A54 para empresa, excelente', 'Chile', '2024-10-13', 5, 'user-004'),
-- Review 10
(10, 'Móviles premium, muy satisfecho', 'Perú', '2024-10-16', 5, 'user-005');

SET IDENTITY_INSERT [dbo].[Review] OFF;
GO


-- PASO 9: Insertar Items de Review
INSERT INTO [dbo].[ItemReview] 
([IdDispositivo], [IdReview], [Comentario], [Puntuacion])
VALUES
-- Review 1: Opinión sobre iPhone 14 Pro
(1, 1, 'El iPhone 14 Pro tiene un rendimiento excepcional y una cámara impresionante. La batería podría durar un poco más.', 5),

-- Review 2: Experiencia con Galaxy S23 Ultra
(7, 2, 'El Galaxy S23 Ultra es increíble para fotografía, aunque el tamaño lo hace algo incómodo de usar con una mano.', 4),

-- Review 3: Comparativa iPhone 13 vs Xiaomi 13 Pro
(4, 3, 'El iPhone 13 es muy equilibrado, pero el Xiaomi 13 Pro ofrece mejor carga y cámara por el precio.', 4),
(10, 3, 'Excelente pantalla y autonomía. Android muy fluido.', 5),

-- Review 4: Gama media equilibrada
(5, 4, 'iPhone 13 128GB rosa: buena calidad y cámara, aunque algo caro.', 4),
(8, 4, 'Galaxy A54: excelente autonomía y pantalla, ideal para uso diario.', 5),

-- Review 5: Xiaomi 13 Pro Premium
(11, 5, 'Xiaomi 13 Pro versión premium, materiales de lujo y cámara brutal.', 5),

-- Review 6: Móviles económicos
(9, 6, 'Galaxy A54 ofrece mucho por su precio, buen rendimiento.', 4),
(17, 6, 'Realme GT3 destaca por su carga ultrarrápida, aunque se calienta.', 3),


-- Review 7: Google Pixel 7 Pro
(12, 7, 'Android puro y excelente cámara, aunque la batería no destaca.', 5),

-- Review 8: OnePlus 11 y Huawei P60 Pro
(14, 8, 'OnePlus 11 es una bestia en fluidez y pantalla.', 5),
(15, 8, 'Huawei P60 Pro tiene una cámara insuperable, pero limita por falta de servicios Google.', 4),

-- Review 9: Compra empresarial Galaxy A54
(8, 9, 'Perfectos para empleados: buena autonomía, rendimiento decente y diseño moderno.', 4),

-- Review 10: Dispositivos premium 2025
(2, 10, 'iPhone 14 Pro 512GB: espectacular pero demasiado caro.', 4),
(6, 10, 'Galaxy S23 Ultra 512GB: potencia y cámara sobresalientes.', 5);
GO



PRINT 'Datos de inicialización insertados correctamente';
PRINT 'Total Modelos: 10';
PRINT 'Total Dispositivos móviles: 18';
PRINT 'Total Usuarios: 5';
PRINT 'Total Alquileres: 6';
PRINT 'Total Items Alquiler: 9';
PRINT 'Total Compras: 10';
PRINT 'Total Items Compra: 17';
PRINT 'Total Reviews: 10';
PRINT 'Total Items Reviews: 15';