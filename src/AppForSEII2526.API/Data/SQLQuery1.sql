INSERT INTO [dbo].[Modelo] ([NombreModelo]) VALUES
('Galaxy S24 Ultra'),
('iPhone 15 Pro Max'),
('Pixel 8 Pro'),
('ROG Phone 8'),
('Xperia 1 VI'),
('Redmi Note 13 Pro'),
('Surface Duo 2'),
('OnePlus 12'),
('Motorola Edge 50 Ultra'),
('Huawei P60 Pro');

INSERT INTO [dbo].[Dispositivo] 
([ModeloId], [Marca], [Color], [NombreDispositivo], [PrecioParaCompra], [PrecioParaAlquiler],
 [CantidadParaCompra], [CantidadParaAlquilar], [Año], [Calidad])
VALUES
(1, 'Samsung', 'Negro', 'Galaxy S24 Ultra 512GB', 1399.99, 59.99, 15, 5, 2024, 10),
(2, 'Apple', 'Blanco', 'iPhone 15 Pro Max 256GB', 1499.00, 69.99, 20, 8, 2023, 10),
(3, 'Google', 'Gris', 'Pixel 8 Pro 128GB', 1099.00, 49.99, 10, 6, 2024, 9),
(4, 'ASUS', 'Negro', 'ROG Phone 8 16GB', 1199.00, 54.99, 8, 4, 2024, 9),
(5, 'Sony', 'Morado', 'Xperia 1 VI 256GB', 1299.00, 52.99, 7, 3, 2024, 9),
(6, 'Xiaomi', 'Azul', 'Redmi Note 13 Pro 128GB', 349.99, 19.99, 30, 10, 2025, 8),
(7, 'Microsoft', 'Plata', 'Surface Duo 2 128GB', 999.00, 39.99, 5, 2, 2023, 8),
(8, 'OnePlus', 'Verde', 'OnePlus 12 256GB', 899.00, 44.99, 12, 6, 2025, 9),
(9, 'Motorola', 'Dorado', 'Motorola Edge 50 Ultra', 799.00, 34.99, 10, 5, 2024, 8),
(10, 'Huawei', 'Negro', 'Huawei P60 Pro 512GB', 1199.00, 49.99, 9, 4, 2023, 9);

INSERT INTO [dbo].[AspNetUsers] 
([Id],[NombreUsuario],[ApellidosUsuario],[DireccionDeEnvio], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], 
 [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp],
 [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd],
 [LockoutEnabled], [AccessFailedCount]
  )
VALUES
-- Usuario 1: admin@test.com (Password: Admin123!)
('user-admin-001','Administrador','Sistema',
 'Calle Principal 123, Madrid',   'admin@test.com', 'ADMIN@TEST.COM', 'admin@test.com', 'ADMIN@TEST.COM',
 1, 'AQAAAAIAAYagAAAAEDummyHashForTestingPurposesOnly=', NEWID(), NEWID(),
 NULL, 0, 0, NULL, 1, 0),

-- Usuario 2: cliente1@test.com
('user-cliente-001', 'Juan', 'García López', 'Avenida Libertad 45, Barcelona','cliente1@test.com', 'CLIENTE1@TEST.COM', 'cliente1@test.com', 'CLIENTE1@TEST.COM',
 1, 'AQAAAAIAAYagAAAAEDummyHashForTestingPurposesOnly=', NEWID(), NEWID(),
 '612345678', 1, 0, NULL, 1, 0
 ),

-- Usuario 3: cliente2@test.com
('user-cliente-002','María', 'Rodríguez Pérez', 'Plaza España 12, Valencia', 'cliente2@test.com', 'CLIENTE2@TEST.COM', 'cliente2@test.com', 'CLIENTE2@TEST.COM',
 1, 'AQAAAAIAAYagAAAAEDummyHashForTestingPurposesOnly=', NEWID(), NEWID(),
 '623456789', 1, 0, NULL, 1, 0
 );
 
 INSERT INTO [dbo].[Compra] 
    ([MetodoDePago], [FechaCompra],[PrecioTotal],[CantidadTotal] , [ApplicationUserId])
    VALUES
    (1, '2024-10-15', 1399.99, 1, 'user-cliente-001'),
    (2, '2024-10-20', 1399.99, 1, 'user-cliente-002'),
    (3, '2024-10-25', 1399.99, 1, 'user-cliente-001');

 INSERT INTO [dbo].[ItemCompra]
    ([IdDispositivo], [IdCompra], [Descripcion], [DispositivoId], [CompraId], [Precio], [Cantidad])
    VALUES
    (1, 1, 'Galaxy S24 Ultra 512GB', 1, 1, 1399.99, 1),
    (1, 2, 'Galaxy S24 Ultra 512GB', 1, 2, 1399.99, 1),
    (1, 3, 'Galaxy S24 Ultra 512GB', 1, 3, 1399.99, 1);
    