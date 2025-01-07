# Sistema Web de Aprobación de Requisiciones

Este es un sistema web diseñado para gestionar las requisiciones de compra dentro de una organización, permitiendo la aprobación de solicitudes a través de varios niveles de autorización.

## Funcionalidades

### 1. Roles de Usuario
El sistema tiene tres roles principales, cada uno con permisos específicos:
- **Comprador**: Usuario que genera requisiciones de compra de artículos.
- **Aprobador jefe**: Primer nivel de aprobación. Es el jefe directo del comprador y se encarga de verificar la solicitud.
- **Aprobador financiero**: Usuarios que aprueban las compras dependiendo del monto de la solicitud. Los roles financieros son los siguientes:
  - Aprobador 1: Aprueba órdenes entre 1 y 100,000 colones.
  - Aprobador 2: Aprueba órdenes entre 100,000 y 1,000,000 colones.
  - Aprobador 3: Aprueba órdenes entre 1,000,000 y 100,000,000 colones.

### 2. Flujo de Aprobación
El flujo del sistema sigue las siguientes reglas:
1. El **Comprador** crea una requisición, incluyendo la solicitud de compra y el monto.
2. El sistema verifica quién es el jefe del comprador y envía la requisición para su aprobación.
3. Una vez aprobada por el jefe, el sistema determina qué **Aprobador financiero** se debe encargar de aprobar la solicitud, basado en el monto.
4. El **Aprobador financiero** revisa y aprueba/rechaza la requisición según el rango correspondiente.

### 3. Notificaciones
- El sistema enviará notificaciones por correo electrónico a los usuarios afectados en cada cambio de estado de una requisición (aprobación/rechazo).
- Los correos incluirán un enlace directo al formulario de la solicitud para su seguimiento.

### 4. Funcionalidades Adicionales
- **Administración de usuarios**: Los administradores pueden agregar, inactivar usuarios y asignar roles (Comprador, Aprobador jefe, Aprobador financiero).
- **Autenticación de usuarios**: Los usuarios deben iniciar sesión con un nombre de usuario y contraseña. Las contraseñas están almacenadas de manera encriptada para mayor seguridad.
- **Consulta de estatus**: Los usuarios pueden consultar el estatus de todas sus solicitudes, así como crear nuevas requisiciones desde el sistema.
