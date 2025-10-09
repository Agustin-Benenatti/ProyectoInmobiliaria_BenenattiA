# Sistema de Gestión Inmobiliaria 🏠

![.NET 8](https://img.shields.io/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![Bootstrap 5](https://img.shields.io/badge/Bootstrap-5-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)

Este proyecto es un sistema web integral para la gestión de una inmobiliaria, desarrollado en **ASP.NET Core MVC**. La plataforma permite administrar de forma centralizada las propiedades, propietarios, inquilinos, contratos de alquiler y los pagos asociados, todo protegido por un sistema de autenticación y autorización basado en roles.

***

## 📜 Tabla de Contenidos
1. [Características Principales](#-características-principales)
2. [Tecnologías Utilizadas](#-tecnologías-utilizadas)
3. [Guía de Instalación](#️-guía-de-instalación)
4. [Credenciales de Prueba](#-credenciales-de-prueba)

---

## ✨ Características Principales

#### 👤 Gestión de Usuarios
- **Sistema de Login:** Autenticación segura por cookies y contraseñas hasheadas con **Bcrypt**.
- **Roles de Usuario:** Dos niveles de acceso definidos:
  - `👑 Administrador`: Acceso total al sistema, incluyendo la eliminación de registros.
  - `👤 Empleado`: Acceso a la gestión diaria, con permisos restringidos.
- **Perfiles de Usuario:** Cada usuario puede editar su información personal y cambiar su foto de perfil (avatar).

#### 🏠 Gestión de Entidades (CRUD)
- Gestión completa (Crear, Leer, Actualizar, Eliminar) para:
  - **Propietarios**
  - **Inquilinos**
  - **Inmuebles** (con carga de imágenes de portada y galería, y asociación a un propietario).

#### 📄 Gestión de Contratos
- **Creación Inteligente:** Solo permite crear contratos sobre inmuebles con estado "Disponible".
- **Validación de Solapamiento:** Impide registrar un contrato si sus fechas se superponen con otro contrato activo para el mismo inmueble.
- **Ciclo de Vida Completo:**
  - **Renovación:** Opción para renovar contratos que han finalizado y cumplido con sus pagos.
  - **Finalización Anticipada:** Permite terminar un contrato antes de tiempo, con cálculo automático de multas.

#### 💳 Gestión de Pagos
- Registro, edición y anulación de pagos asociados a cada contrato.
- Visualización del historial de pagos por contrato.

#### 🔍 Búsquedas y Filtros Avanzados
- **Inmuebles Disponibles:** Búsqueda avanzada de inmuebles libres en un **rango de fechas** específico.
- **Filtros Combinados:** Filtro de inmuebles por estado, tipo y disponibilidad por fechas.
- **Contratos por Vencer:** Filtro para encontrar contratos que vencen en los próximos 30, 60 o 90 días.
- **Vistas Relacionadas:** Acceso rápido a todos los contratos de un inmueble específico desde la lista de inmuebles.

#### 🛡️ Auditoría
- **Seguimiento de Cambios:** El sistema registra las acciones importantes (Crear, Editar, Eliminar) en una tabla de auditoría, guardando qué usuario realizó la acción y cuándo.

---

## 🚀 Tecnologías Utilizadas

- **Backend:** C# con .NET 8 (ASP.NET Core MVC).
- **Base de Datos:** MySQL.
- **Acceso a Datos:** ADO.NET con el conector `MySql.Data`.
- **Frontend:** Vistas Razor con HTML, CSS y JavaScript, utilizando **Bootstrap 5** para el diseño responsivo.
- **Arquitectura:** Modelo-Vista-Controlador (MVC) y Patrón Repositorio.
- **Seguridad:** Autenticación por Cookies de ASP.NET Core y hasheo de contraseñas con la librería `BCrypt.Net`.

---

## 🛠️ Guía de Instalación

Sigue estos pasos para ejecutar el proyecto en tu entorno local.

#### 1. Prerrequisitos
Asegúrate de tener instalado:
- [.NET 8 SDK](https://dotnet.microsoft.com/download) o superior.
- Un servidor de **MySQL** (como XAMPP, WampServer o una instancia standalone).
- Un editor de código como **Visual Studio** o **Visual Studio Code**.
- Un cliente de Git como [Git SCM](https://git-scm.com/).

#### 2. Configuración del Proyecto

1.  **Clona el repositorio:**
    ```sh
    git clone [https://github.com/agustin-benenatti/proyectoinmobiliaria_benenattia.git](https://github.com/agustin-benenatti/proyectoinmobiliaria_benenattia.git)
    cd proyectoinmobiliaria_benenattia
    ```
2.  **Crea la Base de Datos:**
    - Abre tu cliente de MySQL (como MySQL Workbench o HeidiSQL).
    - Crea una nueva base de datos.
    - **Importante:** Ejecuta el script **`BDInmobiliariaActualizada.sql`** que se encuentra en la raíz del proyecto para crear todas las tablas y los usuarios de prueba.

3.  **Configura la Cadena de Conexión:**
    - Abre el archivo `appsettings.json`.
    - Modifica la cadena de conexión `ConnectionStrings` con tus credenciales de MySQL.

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=nombre_de_tu_db;User=tu_usuario;Password=tu_contraseña;"
      },
      // ...
    }
    ```

4.  **Ejecuta el Proyecto:**
    - Si usas la terminal, navega a la carpeta del proyecto y ejecuta:
    ```sh
    dotnet run
    ```
    - Si usas Visual Studio, simplemente presiona `F5` o el botón "Iniciar" (▶). La aplicación se abrirá en tu navegador por defecto.

---

## 🔑 Credenciales de Prueba

Puedes acceder al sistema con los siguientes usuarios para probar los diferentes roles:

| Rol             | Usuario (Email)      | Contraseña |
|:----------------|:---------------------|:-----------|
| 👑 Administrador | `admin@admin.com`    | `admin123`    |
| 👤 Empleado      | `empleado@empleado.com` | `empleado123`    |
