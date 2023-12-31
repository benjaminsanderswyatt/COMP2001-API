CREATE PROCEDURE CW2.UnArchiveUser
    @Email VARCHAR
AS
BEGIN
    UPDATE [CW2].[Users]
    SET [Is_Archived] = 0
    WHERE [Email] = @Email;
END;
GO
