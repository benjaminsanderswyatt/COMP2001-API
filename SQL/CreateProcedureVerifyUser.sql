CREATE PROCEDURE [CW2].[VerifyUser]
    @Email VARCHAR(320),
    @Password VARCHAR(30),
    @IsArchived BIT OUTPUT,
    @Verified BIT OUTPUT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [CW2].[Users] WHERE [Email] = @Email AND [Password] = @Password)
    BEGIN
        SELECT @IsArchived = [Is_Archived]
        FROM [CW2].[Users]
        WHERE [Email] = @Email AND [Password] = @Password;

        SET @Verified = 1;
    END
    ELSE
    BEGIN
        SET @IsArchived = NULL;
        SET @Verified = 0;
    END
END;