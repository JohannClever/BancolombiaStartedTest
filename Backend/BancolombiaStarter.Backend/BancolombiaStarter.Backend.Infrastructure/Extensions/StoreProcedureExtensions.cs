using Microsoft.EntityFrameworkCore;
using BancolombiaStarter.Backend.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancolombiaStarter.Backend.Infrastructure.Extensions
{
    public static class StoreProcedureExtensions
    {
        public static void CheckSpHasBeenCreated(this PersistenceContext dbContext, string procedureName)
        {
            var connection = dbContext.Database.GetDbConnection();

            bool spExists = false;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = '{procedureName}' AND ROUTINE_TYPE = 'PROCEDURE'";
                connection.Open();
                var result = command.ExecuteScalar();
                connection.Close();
                spExists = (result != null);
            }

            if (!spExists)
            {
                string sqlFilePath;
                var sqlContent = string.Empty;
                if (IsRunningInDocker())
                {
                    // Ruta para Docker
                    sqlContent = "CREATE PROCEDURE [dbo].[Sp_Insert_Comment]\r\n\t@ProjectId INT,\r\n\t@Observations VARCHAR(250),\r\n\t@IdUser NVARCHAR(450),\r\n\t@Id BIGINT OUTPUT\r\nAS\r\nBEGIN\r\n\tINSERT INTO Report(ProjectId, Observations,IdUser,CreationOn) \r\n\tVALUES(@ProjectId,@Observations,@IdUser,GETDATE());\r\n\tSET @Id = SCOPE_IDENTITY();\r\nEND\r\n";
                }
                else
                {
                    // Ruta para Visual Studio
                    var solutionDirectory = PathExtension.FindSolutionBaseDirectory();
                    sqlFilePath = Path.Combine(solutionDirectory, "BancolombiaStarter.Backend.Db/bin/Debug/Dbo/StoreProcedures/Sp_Insert_Comment.sql");
                    sqlContent = File.ReadAllText(sqlFilePath);
                }

               
                dbContext.Database.ExecuteSqlRaw(sqlContent);
            }
        }

        private static bool IsRunningInDocker()
        {
            return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        }

    }
}
