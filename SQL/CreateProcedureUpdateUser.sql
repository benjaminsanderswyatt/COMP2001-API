CREATE PROCEDURE CW2.UpdateUserAndActivities
(
    @Email VARCHAR(320),
    @Username VARCHAR(100),
    @Password VARCHAR(30) = NULL,
    @About_Me VARCHAR(500),
    @Location VARCHAR(160),
    @Birthday DATE,
    @Height_cm DECIMAL(5,2),
    @Weight_kg DECIMAL(5,2),
    @Pref_Units_Is_Metric BIT,
    @Activ_Time_Pref_Is_Speed BIT,
    @Marketing_Language VARCHAR(30),
    @ActivityList CW2.TempActivityList READONLY
)
AS
BEGIN
    UPDATE [CW2].[Users]
    SET
        [Username] = @Username,
        [Password] = ISNULL(@Password,[Password]),
        [About_Me] = @About_Me,
        [Location] = @Location,
        [Birthday] = @Birthday,
        [Height_cm] = @Height_cm,
        [Weight_kg] = @Weight_kg,
        [Pref_Units_Is_Metric] = @Pref_Units_Is_Metric,
        [Activ_Time_Pref_Is_Speed] = @Activ_Time_Pref_Is_Speed,
        [Marketing_Language] = @Marketing_Language
    WHERE
        [Email] = @Email;
    DELETE FROM [CW2].[User-Activities]
    WHERE
        [Email] = @Email;
    INSERT INTO [CW2].[User-Activities] ([Email], [ActivityID])
    SELECT @Email, ActivityID
    FROM @ActivityList;
END;
