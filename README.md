## Prueba técnica de Nextlane

Este repositorío es la parte de código para la solución de la prueba técnica de Nextlane.
El repositorio esta ordenado de la siguiente manera
```
Nextlane
|_ 1. General Concepts
    |_ S
    |_ O
    |_ L
    |_ I
    |_ D

|_ 2. C# Knowledge
    |_ Solutions
       |_ A
       |_ B
       |_ C
       |_ D

|_ 3. Software development and testing
    |_ EF
       |_ B

|_ ProductManagement
```

## Requisitos 

- .NET 8 SDK o superior
- Un editor de código como Visual Studio o Visual Studio Code
- Acceso a NuGet para instalación de paquetes
- SQL Server
- Entity Framework Core Tools

## Pasos para ejecutar el código
- Restaurar los paquetes de las soluciones, desde Visual estudio seleccionar una solución y en el menu de click derecho selecionar "Restaurar NuGetPackages".
- Para la solución ProductManagement se deben aplicar las migraciones, esto creara la base de datos. 
Desde la consola del Package Manager ejecuta el siguiente comando:
```bash
dotnet ef database update --project Infrastructure --startup-project ProductAPI
```
### Ejecución del código
- Los archivos dentro de la carpeta "1. General Concepts" sólo son para ilustrar conceptos, no necesitan ser ejecutados.
- Los archivos dentro de la carpeta "2. C# Knowledge/Solutions" y "3. Software development and testing/EF", sólo basta con ejecutarlos despues de restaurar los paquetes de la solución.
- Para la aplicación "ProductManagement" despues de restaurar los paquetes de la solución y ejecutar las migraciones, bastaría con ejecutarla, en caso de tener algún problema, prueba seleccionar "ProductAPI" como "Startup project".
- Para la ejecución de los "tests" de la aplicación "ProductManagement" puedes tanto utilizar el "TestExplorer" de Visual Studio, o ejecuar el siguiente comando en la consola del Package Manager:
```
dotnet test
```

