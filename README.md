# JohannBolivar_Test

Se crea .net backend el cual se lanza automaticamente solo cambiando la cadena de conexión del api
se debe de crear el procedimiento almacenado lo pueden encontrar en el proyecto Syspotec.ErrorReport.Db/Dbo/StoreProcedures/Sp_Insert_Report
En los datos semilla se crean todos los maestros y un usuario de tipo administrador

                    UserName: admin
                    Emai: admin@example.com
                    FullName: Amind Profile
                    Pass: @Admind1234
Y uno de tipo Employee

                    UserName: employee1
                    Email: employee1@example.com
                    FullName:employee Profile
                    Pass: @Employee1234

Para el front de angular se debe ejecutar el comando npm i  y ng s para ejecutar
maneja dos tipos de Roles
El admind puede ver una vista general de todos los resportes
y el employee discriminado por servicios.
