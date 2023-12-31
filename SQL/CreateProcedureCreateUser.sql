CREATE PROCEDURE [CW2].[CreateProfile]
	@Email varchar(320),
    @Username varchar(100),
    @Password varchar(30),
    @About_Me varchar(500) = NULL,
    @Location varchar(160) = NULL,
    @Birthday date = NULL,
    @Height_cm DECIMAL(5,2) = NULL,
    @Weight_kg DECIMAL(5,2) = NULL,
    @Pref_Units_Is_Metric BIT = 1,
    @Activ_Time_Pref_Is_Speed BIT = 1,
    @Marketing_Language varchar(30),
    @Is_Archived BIT = 0,
    @Last_Updated smalldatetime = NULL
AS
BEGIN
    INSERT INTO [CW2].[Users] 
    (
        Username,
        Email,
        [Password],
        About_Me,
        [Location],
        Birthday,
        Height_cm,
        Weight_kg,
        Pref_Units_Is_Metric,
        Activ_Time_Pref_Is_Speed,
        Marketing_Language,
        Is_Archived,
        Last_Updated
    )
    VALUES
    (
        @Username,
        @Email,
        @Password,
        @About_Me,
        @Location,
        @Birthday,
        @Height_cm,
        @Weight_kg,
        @Pref_Units_Is_Metric,
        @Activ_Time_Pref_Is_Speed,
        @Marketing_Language,
        @Is_Archived,
        @Last_Updated
    );
END;
