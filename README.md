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
