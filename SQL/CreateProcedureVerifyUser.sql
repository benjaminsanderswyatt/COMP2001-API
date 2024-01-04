CREATE PROCEDURE [CW2].[VerifyUser]
    @Email VARCHAR(320),
    @Password VARCHAR(30),
    @Verified NVARCHAR(10) OUTPUT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [CW2].[Users] WHERE [Email] = @Email AND [Password] = @Password)
    BEGIN
        SET @Verified = 'True';
    END
    ELSE
    BEGIN
        SET @Verified = 'False';
    END
END;