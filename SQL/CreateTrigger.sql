CREATE TRIGGER CW2.UpdateLastUpdated
ON [CW2].[Users]
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE U
    SET Last_Updated = GETDATE()
    FROM [CW2].[Users] U
    INNER JOIN (
        SELECT Email FROM inserted
        UNION
        SELECT Email FROM deleted
    ) I ON U.Email = I.Email;
END;
