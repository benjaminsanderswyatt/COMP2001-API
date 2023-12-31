CREATE VIEW [CW2].[ActiveView]
AS
SELECT *
FROM [CW2].[Users]
WHERE [Is_Archived] = 0;
GO

CREATE VIEW [CW2].[ArchivedView]
AS
SELECT *
FROM [CW2].[Users]
WHERE [Is_Archived] = 1;
GO

CREATE VIEW [CW2].[AdminView]
AS
SELECT *
FROM [CW2].[Users]
WHERE [Is_Admin] = 1;
GO

CREATE VIEW [CW2].[UserActivitiesView]
AS
SELECT
    U.Email,
    U.Username,
    A.Activity_Name
FROM
    [CW2].[Users] U
JOIN
    [CW2].[User-Activities] UA ON U.Email = UA.Email
JOIN
    [CW2].[Activities] A ON UA.ActivityID = A.ActivityID;
GO
