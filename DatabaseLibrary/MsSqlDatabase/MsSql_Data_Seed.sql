BEGIN TRY

	BEGIN TRANSACTION

    INSERT INTO [dbo].[Blog](
	     [Url]
        ,[CreatedBy]
        ,[CreatedDate]
	)
    SELECT 'BlogA', N'dbo', GETDATE()
	UNION ALL
	SELECT 'BlogB', N'dbo', GETDATE()
	UNION ALL
	SELECT 'BlogC', N'dbo', GETDATE()

	INSERT INTO [dbo].[Post](
		 [Title]
        ,[Content]
        ,[BlogId]
        ,[CreatedBy]
        ,[CreatedDate]
	)
    SELECT 'PostA_A', 'TESTA', 1, N'dbo', GETDATE()
	UNION ALL
    SELECT 'PostB_A', 'TESTB', 1, N'dbo', GETDATE()
	UNION ALL
    SELECT 'PostC_A', 'TESTC', 1, N'dbo', GETDATE()
	UNION ALL
    SELECT 'PostA_B', 'TESTA', 2, N'dbo', GETDATE()
	UNION ALL
    SELECT 'PostB_B', 'TESTB', 2, N'dbo', GETDATE()
	UNION ALL
    SELECT 'PostC_B', 'TESTC', 2, N'dbo', GETDATE()
	UNION ALL
    SELECT 'PostA_C', 'TESTA', 3, N'dbo', GETDATE()
	UNION ALL
    SELECT 'PostB_C', 'TESTB', 3, N'dbo', GETDATE()
	UNION ALL
    SELECT 'PostC_C', 'TESTC', 3, N'dbo', GETDATE()

    
    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_STATE() AS ErrorState,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage;

    -- Rollback any active or uncommittable transactions before
    -- inserting information in the ErrorLog
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END

    --DECLARE @errorLogId INT;

	--EXECUTE [dbo].[uspLogError] @errorLogId OUTPUT;
	
	--SELECT @errorLogId AS ErrorLogID ;

END CATCH

