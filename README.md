# Bancolombia STARTER

Propuesta para Utilizar Azure

Dada la decisión de utilizar Azure como la plataforma en la nube para la aplicación de red social para financiar proyectos, actualizaré la propuesta para reflejar un enfoque centrado en Azure y ajustaré el diseño backend para un único microservicio inicialmente, debido a un Spike realizado. A continuación se detalla la arquitectura tentativa y los ajustes necesarios:

Arquitectura en Azure

Componentes Utilizados:

Azure Kubernetes Service (AKS): Orquestación de contenedores para la ejecución de un único microservicio inicialmente, con la posibilidad de expandirse a múltiples microservicios en el futuro.
Azure SQL Database: Base de datos relacional para almacenar datos de usuarios y proyectos.

Azure Blob Storage:

Almacenamiento de archivos multimedia como imágenes y videos.
Azure Active Directory B2C: Autenticación y manejo de usuarios.
Azure Notification Hubs: Notificaciones y alertas.
Azure Front Door o Azure CDN: Distribución del contenido del frontend para mejorar la velocidad de carga y la experiencia del usuario globalmente.

Ventajas:

Integración y Gestión Simplificada: Azure proporciona una integración nativa entre sus servicios, facilitando la configuración y gestión de la autenticación, bases de datos, y almacenamiento de archivos.
Escalabilidad y Flexibilidad: AKS permite una escalabilidad fácil y efectiva, permitiendo la expansión a múltiples microservicios si es necesario en el futuro.

Backend

Desarrollo de Microservicios: Aunque inicialmente se empezará con un único microservicio debido a las limitaciones del Spike, la arquitectura se diseñará para ser fácilmente escalable a una arquitectura de microservicios completa utilizando AKS.
Base de Datos: Utilizar Azure SQL Database para almacenar información relacional, aprovechando características como grupos elásticos de bases de datos si es necesario para escalabilidad.
Frontend

Tecnología: 
Se utilizará Angular o Vue.js para desarrollar el frontend, según la preferencia del equipo de desarrollo o los requisitos del proyecto.

Distribución de Contenido: Uso de Azure Front Door o Azure CDN para optimizar la entrega de contenido estático y dinámico a usuarios en diferentes ubicaciones geográficas.
Diagrama de Arquitectura
Se actualizará el diagrama de arquitectura existente para reflejar esta nueva configuración en Azure, incluyendo todos los componentes mencionados y su interacción. El diagrama también ilustrará cómo el único microservicio interactúa con la base de datos SQL Server y otros servicios de Azure.

Implementación y Entrega

Control de Versiones y CI/CD: 

Se utilizarán herramientas de Azure como Azure Repos para el control de versiones y Azure Pipelines para la integración continua y la entrega continua (CI/CD).

Monitoreo y Diagnóstico: Implementación de Azure Monitor y Application Insights para monitorear el rendimiento de la aplicación y diagnosticar problemas rápidamente.
Conclusión

Esta propuesta actualizada para la arquitectura de Azure proporciona un marco robusto y escalable para la aplicación de red social destinada a financiar proyectos. Aunque comienza con un único microservicio, la infraestructura y el diseño permiten una expansión fácil y eficiente, asegurando que la plataforma pueda crecer y adaptarse a las necesidades futuras de sus usuarios.
Se crea .net backend el cual se lanza automaticamente solo cambiando la cadena de conexión del api
se debe de crear el procedimiento almacenado lo pueden encontrar en el proyecto Backend\BancolombiaStarter.Backend\BancolombiaStarter.Backend.Db\Dbo\StoreProcedures\Sp_Insert_Comment.sql


En los datos semilla se crean todos los maestros y un usuario de tipo administrador

                    UserName: admin
                    Emai: admin@example.com
                    FullName: Amind Profile
                    Pass: @Admind1234
Y dos de tipo User

                    UserName: User1
                    Email: user1@example.com
                    FullName:user1 Profile
                    Pass: @User1234

                     UserName: User2
                    Email: user2@example.com
                    FullName:user2 Profile
                    Pass: @User21234

Para el front de angular se debe ejecutar el comando npm i  y ng s para ejecutar


Actualmente puedes crear proyectos , editar los tuyos y pedir sugerencias al Open IA


Aqui dejo el recurso figma con que se diseño las pantallas
https://www.figma.com/design/f3eeLrfhvP06Ij9HGKHQYV/Bancolombia-starter?node-id=0-1&t=6yY3G6bUhykA2JKj-1
