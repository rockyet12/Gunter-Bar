# Gunter-Bar

Este proyecto contiene el **backend** y el **frontend** de **Gunter-Bar**, una aplicación diseñada para gestionar un bar.

A continuación, se detallan los pasos necesarios para clonar, configurar e iniciar el proyecto correctamente.

---

## ✅ Requisitos previos

Antes de comenzar, asegurate de tener instalados los siguientes programas:

1. **.NET SDK (v8.0 o superior)**  
    Descargalo desde: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

2. **Node.js y npm**  
    Se recomienda la última versión LTS. Descargala desde: [https://nodejs.org/](https://nodejs.org/)

3. **MySQL (v8.4 recomendado)**  
    Si usás otra versión, vas a tener que modificar el archivo `Program.cs` en el backend.

---

## 📁 Clonar y configurar el proyecto

### 1. Clonar el repositorio

Abrí una terminal y ejecutá:

```bash
git clone https://github.com/rockyet12/Gunter-Bar.git
cd Gunter-Bar
```

---

### 2. Configurar el backend

```bash
cd Backend-Bar
```

- Restaurá los paquetes necesarios:

```bash
dotnet restore
```

- Si usás una versión distinta de MySQL, abrí el archivo `Program.cs` en `Backend-Bar/BarGunter.API/Program.cs` y modificá la siguiente línea:

```csharp
new MySqlServerVersion(new Version(8, 4))
```

Cambiá `8, 4` por la versión que tengas instalada, por ejemplo `8, 0`.

- Además, tenés que configurar tu cadena de conexión en el archivo `appsettings.development.json` en `Backend-Bar/BarGunter.API/appsettings.development.json`. Buscá la clave `ConnectionStrings:DefaultConnection` y cambiá los valores según tu entorno (usuario, contraseña, nombre de base de datos, host, etc). Por ejemplo:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=GunterBarDB;user=root;password=tu_contraseña"
}
```

---

### 3. Configurar el frontend

```bash
cd ../Frontend-Bar
```

- Instalá las dependencias:

```bash
npm install
```

---

## 🚀 Iniciar el proyecto

### Iniciar el backend

```bash
cd ../Backend-Bar
dotnet run
```

### Iniciar el frontend

```bash
cd ../Frontend-Bar
npm run dev
```

Esto iniciará el servidor de desarrollo del frontend.

---

## 🔧 Notas adicionales

- Asegurate de tener creada la base de datos que indiques en la cadena de conexión.
- Podés usar herramientas como **MySQL Workbench** o **DBeaver** para gestionar la base de datos.
- Si el backend no puede conectarse, revisá bien la cadena de conexión y que el servidor de MySQL esté funcionando.
- Si tenés problemas, verificá que las versiones de las herramientas instaladas sean compatibles con los requisitos del proyecto.

---

¡Gracias por usar **Gunter-Bar**! 🍺
