CREATE PROCEDURE [CW2].[DeleteUser]
    @Email VARCHAR(320)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [CW2].[Users] WHERE Email = @Email)
    BEGIN
        DELETE FROM [CW2].[User-Activities] WHERE Email = @Email;
        DELETE FROM [CW2].[Users] WHERE Email = @Email;

        RETURN 1;
    END
    ELSE
    BEGIN
        RETURN 0;
    END
END;