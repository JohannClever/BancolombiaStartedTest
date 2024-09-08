CREATE PROCEDURE [dbo].[Sp_Insert_Comment]
	@ProjectId INT,
	@Observations VARCHAR(250),
	@IdUser NVARCHAR(450),
	@Id BIGINT OUTPUT
AS
BEGIN
	INSERT INTO Report(CompanyServiceId, Observations,IdUser,CreationOn) 
	VALUES(@ProjectId,@Observations,@IdUser,GETDATE());
	SET @Id = SCOPE_IDENTITY();
END
