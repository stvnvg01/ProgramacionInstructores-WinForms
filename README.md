# Programación de Instructores – Aplicación de Escritorio (WinForms)

Aplicación de escritorio desarrollada en **C# WinForms (.NET Framework 4.5.2)** como proyecto de prácticas del **SENA**, orientada a apoyar procesos administrativos relacionados con la **gestión y programación de instructores**, fichas, ambientes y demás entidades asociadas a la formación.

El sistema fue concebido como una solución **local, autónoma y fácil de instalar**, pensada para entornos institucionales donde no siempre se dispone de infraestructura web o servidores dedicados.

---

## 📌 Contexto del proyecto

Este proyecto fue desarrollado durante la etapa práctica como **Tecnólogo en Desarrollo de Software (SENA)**, con el objetivo de:

- Aplicar conocimientos de **programación orientada a objetos**
- Implementar **persistencia de datos** usando SQL Server LocalDB
- Construir una aplicación **real de uso administrativo**
- Diseñar un flujo completo: desarrollo → compilación → instalación → despliegue

La aplicación simula un entorno administrativo institucional, utilizando una **base de datos demo (vacía)** para fines académicos y de demostración.

---

## 🧩 Funcionalidad general

La aplicación permite:

- Autenticación de usuarios por roles
- Gestión de entidades administrativas (instructores, fichas, ambientes, empresas, etc.)
- Formularios CRUD (crear, consultar, actualizar y eliminar información)
- Manejo de estados y validaciones
- Interfaz gráfica basada en WinForms
- Utilidades complementarias (librerías auxiliares incluidas en la solución)

> El alcance funcional está enfocado en demostrar estructura, lógica, persistencia y organización del código, más que en una operación productiva real.

---

## 🛠️ Tecnologías utilizadas

- **Lenguaje:** C#
- **Framework:** .NET Framework 4.5.2
- **Tipo de aplicación:** Windows Forms (WinForms)
- **Base de datos:** SQL Server LocalDB
- **Acceso a datos:** ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataAdapter`)
- **Instalador:** Inno Setup
- **Control de versiones:** Git + GitHub

---

## 🗂️ Arquitectura y estructura

La solución está organizada en varios proyectos y componentes:

- **Proyecto principal WinForms**
  - Formularios (`Frm*.cs`)
  - Clases de conexión y configuración
  - Variables globales y utilidades
- **Librerías auxiliares**
  - Componentes reutilizables
  - Soporte para exportación/manipulación de datos (por ejemplo Excel)

La conexión a base de datos se gestiona mediante una cadena definida en `App.config`, utilizando `LocalDB` con base de datos adjunta dinámicamente mediante `|DataDirectory|`.

---

## 🚀 Demo rápida (sin Visual Studio)

Para facilitar la evaluación y prueba del sistema, el proyecto incluye un **instalador ejecutable**.

### Pasos:
1. Ir a la sección **Releases** de este repositorio
2. Descargar el archivo:
3. Ejecutar el instalador
4. Abrir la aplicación desde el acceso directo creado

> El instalador **incluye SQL Server LocalDB** y configura automáticamente la base de datos demo.

### 🔑 Credenciales de demostración
- **Usuario:** Admin  
- **Contraseña:** 123  

> Estas credenciales corresponden a una base de datos **vacía y de prueba**, sin información real.

---

## 💻 Ejecución desde código (Visual Studio)

### Requisitos:
- Visual Studio 2017 o superior
- .NET Framework 4.5.2
- SQL Server Express LocalDB

### Pasos:
1. Clonar el repositorio
2. Abrir el archivo `.sln`
3. Restaurar dependencias si aplica
4. Compilar y ejecutar (F5)

> Para la demo desde código, la aplicación utiliza la configuración definida en `App.config`.

---

## 🧪 Base de datos

- Base de datos **LocalDB**
- Estructura preparada para uso administrativo
- **Sin datos reales**
- Usuario administrador creado únicamente para demostración

La base de datos se inicializa automáticamente al instalar la aplicación mediante el instalador.

---

## 📄 Estado del proyecto

- ✔ Proyecto funcional
- ✔ Compilación exitosa
- ✔ Instalador operativo
- ✔ Enfoque académico y demostrativo
- 🔄 Posibles mejoras futuras:
  - Separación por capas (DAL / BLL)
  - Manejo de hash de contraseñas
  - Migración a .NET moderno o aplicación web

---

## 👤 Autor

Proyecto desarrollado como parte de las **prácticas del SENA – Tecnólogo en Desarrollo de Software  - bajo el mando del subdirector del Complejo Minero Agroempresarial CTAME Fernando Cano Gomez-**, con fines académicos, demostrativos y de portafolio profesional.

---

## ⚠️ Nota final

Este repositorio tiene como finalidad **mostrar competencias técnicas**, estructura de proyecto y capacidad de llevar una aplicación desde el desarrollo hasta el despliegue, no representar un sistema productivo final.


