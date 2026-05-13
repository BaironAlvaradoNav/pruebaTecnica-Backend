PRUEBA TECNICA BACKEND - API DE CONTACTOS

Este proyecto es una API simple para guardar contactos en memoria.
No usa base de datos.

se necesita NECESITAS

- Tener instalado .NET SDK 10.0 o superior

COMO EJECUTAR

1. Abrir una terminal en la carpeta del proyecto.
2. Ejecutar:
   dotnet restore
3. Ejecutar:
   dotnet run --project PruebaBackend.Api
4. Copiar la URL que aparece en consola.
5. Abrir en navegador:
   http://localhost:PUERTO/swagger

EJEMPLOS DE ENDPOINTS

1. Listar contactos
   GET /api/contactos

2. Buscar contacto por id
   GET /api/contactos/{id}

3. Crear contacto
   POST /api/contactos
   Body JSON:
   {
     "nombre": "Juan Perez",
     "telefono": "123456789"
   }

RESPUESTAS ESPERADAS

- 200: consulta correcta
- 201: contacto creado
- 400: faltan datos
- 404: contacto no existe
- 409: telefono repetido

PROBAR en POSTMAN

insertar datos
POST http://localhost:5207/api/contactos  (corregir puerto si es necesario)
json: 
{
  "nombre": "prueba contacto",
  "telefono": "1231231"
}

-------------------------------

obtener contacto
GET http://localhost:5207/api/contactos (corregir puerto si es necesario)

-------------------------------

obtener contacto por id
GET http://localhost:5207/api/contactos/1 (corregir puerto si es necesario)


COMO EJECUTAR TESTS

1. Si la API esta corriendo, detenla primero.
2. Ejecutar:
   dotnet test -c Release
