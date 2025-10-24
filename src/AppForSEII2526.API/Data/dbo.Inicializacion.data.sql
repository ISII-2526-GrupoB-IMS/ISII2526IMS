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