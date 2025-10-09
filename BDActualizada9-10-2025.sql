-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: inmobiliaria
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `auditorias`
--

DROP TABLE IF EXISTS `auditorias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `auditorias` (
  `IdAuditoria` int NOT NULL AUTO_INCREMENT,
  `Entidad` varchar(50) NOT NULL,
  `EntidadId` int NOT NULL,
  `UsuarioId` int NOT NULL,
  `Fecha` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Accion` varchar(50) NOT NULL,
  `Datos` text,
  `Detalle` text,
  PRIMARY KEY (`IdAuditoria`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `auditorias`
--

LOCK TABLES `auditorias` WRITE;
/*!40000 ALTER TABLE `auditorias` DISABLE KEYS */;
INSERT INTO `auditorias` VALUES (1,'Contrato',30,2,'2025-10-04 23:30:40','Edición',NULL,'Contrato editado por admin@admin.com'),(2,'Contrato',30,3,'2025-10-04 23:46:35','Edición',NULL,'Contrato editado por empleado@empleado.com'),(3,'Contrato',32,2,'2025-10-04 23:48:37','Eliminación',NULL,'Contrato eliminado por admin@admin.com'),(4,'Contrato',39,2,'2025-10-05 00:32:34','Creación',NULL,'Contrato creado por admin@admin.com'),(5,'Contrato',40,2,'2025-10-05 00:39:20','Creación',NULL,'Contrato creado por admin@admin.com'),(6,'Contrato',40,2,'2025-10-05 00:40:49','Finalización Anticipada',NULL,'Contrato terminado anticipadamente por admin@admin.com, multa: $ 68,00'),(7,'Pago',90,2,'2025-10-05 23:06:30','Creación',NULL,'Pago registrado por admin@admin.com'),(8,'Pago',90,2,'2025-10-05 23:06:50','Anulación',NULL,'Pago anulado por admin@admin.com'),(9,'Pago',86,2,'2025-10-05 23:08:23','Edición',NULL,'Pago editado por admin@admin.com'),(10,'Contrato',30,2,'2025-10-05 23:08:40','Finalización Anticipada',NULL,'Contrato terminado anticipadamente por admin@admin.com, multa: $ 420.000,00'),(11,'Contrato',41,2,'2025-10-07 15:58:19','Creación',NULL,'Contrato creado por admin@admin.com'),(12,'Contrato',41,2,'2025-10-07 16:03:44','Eliminación',NULL,'Contrato eliminado por admin@admin.com'),(13,'Contrato',42,2,'2025-10-07 16:04:03','Creación',NULL,'Contrato creado por admin@admin.com'),(14,'Contrato',42,2,'2025-10-07 16:11:40','Eliminación',NULL,'Contrato eliminado por admin@admin.com'),(15,'Contrato',43,2,'2025-10-07 16:12:11','Creación',NULL,'Contrato creado por admin@admin.com'),(16,'Pago',92,2,'2025-10-07 16:12:41','Creación',NULL,'Pago registrado por admin@admin.com'),(17,'Pago',93,2,'2025-10-07 16:32:05','Creación',NULL,'Pago registrado por admin@admin.com'),(18,'Pago',94,2,'2025-10-07 16:32:13','Creación',NULL,'Pago registrado por admin@admin.com'),(19,'Contrato',36,2,'2025-10-07 16:35:11','Eliminación',NULL,'Contrato eliminado por admin@admin.com'),(20,'Pago',95,2,'2025-10-07 16:35:18','Creación',NULL,'Pago registrado por admin@admin.com'),(21,'Pago',96,2,'2025-10-07 16:35:22','Creación',NULL,'Pago registrado por admin@admin.com'),(22,'Pago',96,2,'2025-10-07 16:35:32','Anulación',NULL,'Pago anulado por admin@admin.com'),(23,'Pago',97,2,'2025-10-07 16:35:38','Creación',NULL,'Pago registrado por admin@admin.com'),(24,'Pago',98,2,'2025-10-07 16:35:42','Creación',NULL,'Pago registrado por admin@admin.com'),(25,'Contrato',33,2,'2025-10-09 16:10:36','Eliminación',NULL,'Contrato eliminado por admin@admin.com'),(26,'Contrato',39,2,'2025-10-09 16:19:33','Eliminación',NULL,'Contrato eliminado por admin@admin.com'),(27,'Contrato',40,2,'2025-10-09 16:55:44','Eliminación',NULL,'Contrato eliminado por admin@admin.com'),(28,'Contrato',44,2,'2025-10-09 17:00:51','Creación',NULL,'Contrato creado por admin@admin.com'),(29,'Contrato',44,2,'2025-10-09 17:12:08','Eliminación',NULL,'Contrato eliminado por admin@admin.com'),(30,'Contrato',45,2,'2025-10-09 17:12:41','Creación',NULL,'Contrato creado por admin@admin.com'),(31,'Contrato',45,2,'2025-10-09 17:13:08','Eliminación',NULL,'Contrato eliminado por admin@admin.com');
/*!40000 ALTER TABLE `auditorias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contratos`
--

DROP TABLE IF EXISTS `contratos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contratos` (
  `IdContrato` int NOT NULL AUTO_INCREMENT,
  `FechaInicio` date NOT NULL,
  `FechaFin` date NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Estado` varchar(20) NOT NULL DEFAULT 'Activo',
  `InquilinoId` int NOT NULL,
  `IdInmueble` int NOT NULL,
  `FechaAnticipada` date DEFAULT NULL,
  `Multa` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`IdContrato`),
  KEY `FK_Contratos_Inquilinos` (`InquilinoId`),
  KEY `FK_Contratos_Inmuebles` (`IdInmueble`),
  CONSTRAINT `FK_Contratos_Inmuebles` FOREIGN KEY (`IdInmueble`) REFERENCES `inmuebles` (`IdInmueble`),
  CONSTRAINT `FK_Contratos_Inquilinos` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`InquilinoId`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contratos`
--

LOCK TABLES `contratos` WRITE;
/*!40000 ALTER TABLE `contratos` DISABLE KEYS */;
INSERT INTO `contratos` VALUES (30,'2025-09-30','2026-03-30',210000.00,'Finalizado',1,2,'2025-10-05',420000.00),(37,'2025-10-03','2025-12-02',60000.00,'Activo',2,3,NULL,NULL),(38,'2025-10-03','2026-01-01',70000.00,'Activo',6,4,NULL,NULL),(43,'2025-10-07','2026-04-07',430000.00,'Activo',7,13,NULL,NULL);
/*!40000 ALTER TABLE `contratos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `imagen`
--

DROP TABLE IF EXISTS `imagen`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `imagen` (
  `IdImagen` int NOT NULL AUTO_INCREMENT,
  `IdInmueble` int NOT NULL,
  `UrlImagen` varchar(500) NOT NULL,
  PRIMARY KEY (`IdImagen`),
  KEY `FK_Imagenes_Inmuebles` (`IdInmueble`),
  CONSTRAINT `FK_Imagenes_Inmuebles` FOREIGN KEY (`IdInmueble`) REFERENCES `inmuebles` (`IdInmueble`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `imagen`
--

LOCK TABLES `imagen` WRITE;
/*!40000 ALTER TABLE `imagen` DISABLE KEYS */;
INSERT INTO `imagen` VALUES (6,2,'/Uploads/Inmuebles/2/fbad2904-79ce-4253-b49c-f2761515c2f6.jpeg'),(7,2,'/Uploads/Inmuebles/2/0c643ac6-45f0-4203-85f8-5995a01b86cb.jpeg'),(8,2,'/Uploads/Inmuebles/2/374de8cf-7bce-471a-aabd-c78976b69a70.jpeg');
/*!40000 ALTER TABLE `imagen` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inmuebles`
--

DROP TABLE IF EXISTS `inmuebles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inmuebles` (
  `IdInmueble` int NOT NULL AUTO_INCREMENT,
  `Direccion` varchar(200) NOT NULL,
  `TipoInmueble` varchar(50) DEFAULT NULL,
  `Estado` varchar(50) DEFAULT NULL,
  `Ambientes` int DEFAULT NULL,
  `Superficie` int DEFAULT NULL,
  `Longitud` int DEFAULT NULL,
  `Latitud` int DEFAULT NULL,
  `Precio` decimal(18,2) DEFAULT NULL,
  `PropietarioId` int DEFAULT NULL,
  `PortadaUrl` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`IdInmueble`),
  KEY `PropietarioId` (`PropietarioId`),
  CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`PropietarioId`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmuebles`
--

LOCK TABLES `inmuebles` WRITE;
/*!40000 ALTER TABLE `inmuebles` DISABLE KEYS */;
INSERT INTO `inmuebles` VALUES (2,'Av SiempreViva 749','Casa','Disponible',5,1200,120,90,2000000.00,4,'/Uploads/Inmuebles/portada_2.jpg'),(3,'Barrio San Martin 982','Casa','No Disponible',4,123,123,90,500000.00,5,NULL),(4,'Barrio Frenos Americanos 456','Departamento','No Disponible',2,80,32,32,350000.00,5,NULL),(5,'Barrio Los Paraisos 1843','Casa','No Disponible',3,800,120,90,500000.00,4,NULL),(6,'Barrio Los Eucaliptos','Casa','Disponible',4,1100,120,90,750000.00,5,NULL),(9,'Julio a roca 756','Local','Disponible',2,800,120,90,200000.00,12,NULL),(10,'General Paz 978','Local','Disponible',1,540,120,90,600000.00,4,NULL),(11,'Calle Colon 874','Local','Disponible',1,280,120,90,180000.00,4,NULL),(13,'Barrio 500 viviendas','Casa','Disponible',3,420,120,90,800000.00,12,NULL);
/*!40000 ALTER TABLE `inmuebles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inquilinos`
--

DROP TABLE IF EXISTS `inquilinos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inquilinos` (
  `InquilinoId` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) DEFAULT NULL,
  `Apellido` varchar(100) DEFAULT NULL,
  `Dni` varchar(20) DEFAULT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`InquilinoId`),
  UNIQUE KEY `UQ_propietarios_Dni` (`Dni`),
  UNIQUE KEY `UQ_propietarios_Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilinos`
--

LOCK TABLES `inquilinos` WRITE;
/*!40000 ALTER TABLE `inquilinos` DISABLE KEYS */;
INSERT INTO `inquilinos` VALUES (1,'Santos','Johnson','33322211','2664432343','asd123@gmail.com'),(2,'Ezequiel ','Castro','4531234','6543523123','eze@gmail.com'),(6,'Walter','Santos','27127089','2664009876','Fabian@gmail.com'),(7,'Noa','Mayada','36789456','2664324354','noa@gmail.com');
/*!40000 ALTER TABLE `inquilinos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pagos`
--

DROP TABLE IF EXISTS `pagos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pagos` (
  `IdPago` int NOT NULL AUTO_INCREMENT,
  `NroPago` int NOT NULL,
  `FechaPago` date NOT NULL,
  `Monto` decimal(10,2) NOT NULL,
  `Detalle` varchar(255) DEFAULT NULL,
  `Anulado` tinyint(1) NOT NULL DEFAULT '0',
  `IdContrato` int NOT NULL,
  PRIMARY KEY (`IdPago`),
  KEY `FK_Pagos_Contrato` (`IdContrato`),
  CONSTRAINT `FK_Pagos_Contrato` FOREIGN KEY (`IdContrato`) REFERENCES `contratos` (`IdContrato`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=99 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pagos`
--

LOCK TABLES `pagos` WRITE;
/*!40000 ALTER TABLE `pagos` DISABLE KEYS */;
INSERT INTO `pagos` VALUES (86,1,'2025-10-04',20000.00,'Pago mes octubre (Pago en Efectivoo)',0,30),(87,1,'2025-10-04',20000.00,'asd',0,30),(90,1,'2025-10-05',210000.00,'prueba auditoria',1,30),(91,2,'2025-10-05',420000.00,'Multa por terminación anticipada. Fecha de finalización: 05/10/2025',0,30),(92,1,'2025-10-07',430000.00,'asd',0,43),(93,1,'2025-10-07',430000.00,'asdasdasd',0,43),(94,1,'2025-10-07',430000.00,'asdasdasdasd',0,43);
/*!40000 ALTER TABLE `pagos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `propietarios`
--

DROP TABLE IF EXISTS `propietarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propietarios` (
  `PropietarioId` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Dni` varchar(20) NOT NULL,
  `Telefono` varchar(50) DEFAULT NULL,
  `Email` varchar(100) NOT NULL,
  PRIMARY KEY (`PropietarioId`),
  UNIQUE KEY `Dni` (`Dni`),
  UNIQUE KEY `Email` (`Email`),
  UNIQUE KEY `UQ_propietarios_Dni` (`Dni`),
  UNIQUE KEY `UQ_propietarios_Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietarios`
--

LOCK TABLES `propietarios` WRITE;
/*!40000 ALTER TABLE `propietarios` DISABLE KEYS */;
INSERT INTO `propietarios` VALUES (4,'Augusto','Benenatti','42220800','2665-111283','agus.benenatti09@gmail.com'),(5,'Carloncho ','Menem','43754459','2664112233','Carlos09@gmail.com'),(12,'Laura','Maradona','27127089','2664897645','lau@gmail.com'),(13,'Cristina','Vera','43234543','2665896877','cristina@gmail.com'),(17,'Ezequiel ','Castro','44987789','2665009098','eze@gmail.com');
/*!40000 ALTER TABLE `propietarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `IdUsuario` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Rol` varchar(50) NOT NULL,
  `Avatar` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`IdUsuario`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (2,'Admin','Principal','admin@admin.com','$2a$11$sGBASn5hzPhfsbdQikMgmeDFJm10mFV/FUXusAaSzCmve7zbxCMW.','Administrador','/images/usuarios/e01d6e12-62d4-4706-b4b5-0da7928c59e9.png'),(3,'Empleado','Numero uno','empleado@empleado.com','$2a$11$rQRQx5EtgD9RFLNV7xDSxOZ6k4c2SkKo3gGXZvSinm9mHvPaheygG','Empleado','/images/default-avatar.png');
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-10-09 18:01:25
