CREATE PROCEDURE CW2.ArchiveUser
    @Email VARCHAR
AS
BEGIN
    UPDATE [CW2].[Users]
    SET [Is_Archived] = 1
    WHERE [Email] = @Email;
END;
GO
