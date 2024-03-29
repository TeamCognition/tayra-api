﻿/********************************************************************************************/
--Specify Start Date and End date here
--Value of Start Date Must be Less than Your End Date 

DECLARE @StartDate DATETIME = '01/01/2015' --Starting value of Date Range
DECLARE @EndDate DATETIME = '01/01/2040' --End Value of Date Range

--Temporary Variables To Hold the Values During Processing of Each Date of Year
DECLARE
    @DayOfWeekInMonth INT,
    @DayOfWeekInYear INT,
    @DayOfQuarter INT,
    @WeekOfMonth INT,
    @CurrentYear INT,
    @CurrentMonth INT,
    @CurrentQuarter INT

/*Table Data type to store the day of week count for the month and year*/
DECLARE @DayOfWeek TABLE (DOW INT, MonthCount INT, QuarterCount INT, YearCount INT)

INSERT INTO @DayOfWeek VALUES (1, 0, 0, 0)
INSERT INTO @DayOfWeek VALUES (2, 0, 0, 0)
INSERT INTO @DayOfWeek VALUES (3, 0, 0, 0)
INSERT INTO @DayOfWeek VALUES (4, 0, 0, 0)
INSERT INTO @DayOfWeek VALUES (5, 0, 0, 0)
INSERT INTO @DayOfWeek VALUES (6, 0, 0, 0)
INSERT INTO @DayOfWeek VALUES (7, 0, 0, 0)

--Extract and assign various parts of Values from Current Date to Variable

DECLARE @CurrentDate AS DATETIME = @StartDate
SET @CurrentMonth = DATEPART(MM, @CurrentDate)
SET @CurrentYear = DATEPART(YY, @CurrentDate)
SET @CurrentQuarter = DATEPART(QQ, @CurrentDate)

/********************************************************************************************/
--Proceed only if Start Date(Current date ) is less than End date you specified above

WHILE @CurrentDate < @EndDate
BEGIN
 
/*Begin day of week logic*/

         /*Check for Change in Month of the Current date if Month changed then 
          Change variable value*/
    IF @CurrentMonth != DATEPART(MM, @CurrentDate) 
    BEGIN
        UPDATE @DayOfWeek
        SET MonthCount = 0
        SET @CurrentMonth = DATEPART(MM, @CurrentDate)
    END

        /* Check for Change in Quarter of the Current date if Quarter changed then change 
         Variable value*/

    IF @CurrentQuarter != DATEPART(QQ, @CurrentDate)
    BEGIN
        UPDATE @DayOfWeek
        SET QuarterCount = 0
        SET @CurrentQuarter = DATEPART(QQ, @CurrentDate)
    END
       
        /* Check for Change in Year of the Current date if Year changed then change 
         Variable value*/
    

    IF @CurrentYear != DATEPART(YY, @CurrentDate)
    BEGIN
        UPDATE @DayOfWeek
        SET YearCount = 0
        SET @CurrentYear = DATEPART(YY, @CurrentDate)
    END
    
        -- Set values in table data type created above from variables 

    UPDATE @DayOfWeek
    SET 
        MonthCount = MonthCount + 1,
        QuarterCount = QuarterCount + 1,
        YearCount = YearCount + 1
    WHERE DOW = DATEPART(DW, @CurrentDate)

    SELECT
        @DayOfWeekInMonth = MonthCount,
        @DayOfQuarter = QuarterCount,
        @DayOfWeekInYear = YearCount
    FROM @DayOfWeek
    WHERE DOW = DATEPART(DW, @CurrentDate)
    
/*End day of week logic*/


/* Populate Your Dimension Table with values*/
    
    INSERT INTO [dbo].[Date]
    SELECT
        
        CONVERT (char(8),@CurrentDate,112) as Id,
        @CurrentDate AS Day,
        CONVERT (char(10),@CurrentDate,103) as FullDateUK,
        CONVERT (char(10),@CurrentDate,101) as FullDateUSA,
        DATEPART(DD, @CurrentDate) AS DayOfMonth,
        --Apply Suffix values like 1st, 2nd 3rd etc..
        CASE 
            WHEN DATEPART(DD,@CurrentDate) IN (11,12,13) _
            THEN CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'th'
            WHEN RIGHT(DATEPART(DD,@CurrentDate),1) = 1 _
            THEN CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'st'
            WHEN RIGHT(DATEPART(DD,@CurrentDate),1) = 2 _
            THEN CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'nd'
            WHEN RIGHT(DATEPART(DD,@CurrentDate),1) = 3 _
            THEN CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'rd'
            ELSE CAST(DATEPART(DD,@CurrentDate) AS VARCHAR) + 'th' 
            END AS DaySuffix,
        
        DATENAME(DW, @CurrentDate) AS DayName,
        DATEPART(DW, @CurrentDate) AS DayOfWeekUSA,

        -- check for day of week as Per US and change it as per UK format 
        CASE DATEPART(DW, @CurrentDate)
            WHEN 1 THEN 7
            WHEN 2 THEN 1
            WHEN 3 THEN 2
            WHEN 4 THEN 3
            WHEN 5 THEN 4
            WHEN 6 THEN 5
            WHEN 7 THEN 6
            END 
            AS DayOfWeekUK,
        
        @DayOfWeekInMonth AS DayOfWeekInMonth,
        @DayOfWeekInYear AS DayOfWeekInYear,
        @DayOfQuarter AS DayOfQuarter,
        DATEPART(DY, @CurrentDate) AS DayOfYear,
        DATEPART(WW, @CurrentDate) + 1 - DATEPART(WW, CONVERT(VARCHAR, _
        DATEPART(MM, @CurrentDate)) + '/1/' + CONVERT(VARCHAR, _
        DATEPART(YY, @CurrentDate))) AS WeekOfMonth,
        (DATEDIFF(DD, DATEADD(QQ, DATEDIFF(QQ, 0, @CurrentDate), 0), _
        @CurrentDate) / 7) + 1 AS WeekOfQuarter,
        DATEPART(WW, @CurrentDate) AS WeekOfYear,
        DATEPART(MM, @CurrentDate) AS Month,
        DATENAME(MM, @CurrentDate) AS MonthName,
        CASE
            WHEN DATEPART(MM, @CurrentDate) IN (1, 4, 7, 10) THEN 1
            WHEN DATEPART(MM, @CurrentDate) IN (2, 5, 8, 11) THEN 2
            WHEN DATEPART(MM, @CurrentDate) IN (3, 6, 9, 12) THEN 3
            END AS MonthOfQuarter,
        DATEPART(QQ, @CurrentDate) AS Quarter,
        CASE DATEPART(QQ, @CurrentDate)
            WHEN 1 THEN 'First'
            WHEN 2 THEN 'Second'
            WHEN 3 THEN 'Third'
            WHEN 4 THEN 'Fourth'
            END AS QuarterName,
        DATEPART(YEAR, @CurrentDate) AS Year,
        'CY ' + CONVERT(VARCHAR, DATEPART(YEAR, @CurrentDate)) AS YearName,
        LEFT(DATENAME(MM, @CurrentDate), 3) + '-' + CONVERT(VARCHAR, _
        DATEPART(YY, @CurrentDate)) AS MonthYear,
        RIGHT('0' + CONVERT(VARCHAR, DATEPART(MM, @CurrentDate)),2) + _
        CONVERT(VARCHAR, DATEPART(YY, @CurrentDate)) AS MMYYYY,
        CONVERT(DATETIME, CONVERT(DATE, DATEADD(DD, - (DATEPART(DD, _
        @CurrentDate) - 1), @CurrentDate))) AS FirstDayOfMonth,
        CONVERT(DATETIME, CONVERT(DATE, DATEADD(DD, - (DATEPART(DD, _
        (DATEADD(MM, 1, @CurrentDate)))), DATEADD(MM, 1, _
        @CurrentDate)))) AS LastDayOfMonth,
        DATEADD(QQ, DATEDIFF(QQ, 0, @CurrentDate), 0) AS FirstDayOfQuarter,
        DATEADD(QQ, DATEDIFF(QQ, -1, @CurrentDate), -1) AS LastDayOfQuarter,
        CONVERT(DATETIME, '01/01/' + CONVERT(VARCHAR, DATEPART(YY, _
        @CurrentDate))) AS FirstDayOfYear,
        CONVERT(DATETIME, '12/31/' + CONVERT(VARCHAR, DATEPART(YY, _
        @CurrentDate))) AS LastDayOfYear,
        NULL AS IsHolidayUSA,
        CASE DATEPART(DW, @CurrentDate)
            WHEN 1 THEN 0
            WHEN 2 THEN 1
            WHEN 3 THEN 1
            WHEN 4 THEN 1
            WHEN 5 THEN 1
            WHEN 6 THEN 1
            WHEN 7 THEN 0
            END AS IsWeekday,
        NULL AS HolidayUSA, Null, Null

    SET @CurrentDate = DATEADD(DD, 1, @CurrentDate)
END

/********************************************************************************************/
 
Step 3.
Update Values of Holiday as per UK Government Declaration for National Holiday.

/*Update HOLIDAY fields of UK as per Govt. Declaration of National Holiday*/
    
-- Good Friday  April 18 
    UPDATE [dbo].[Date]
        SET HolidayUK = 'Good Friday'
    WHERE [Month] = 4 AND [DayOfMonth]  = 18

-- Easter Monday  April 21 
    UPDATE [dbo].[Date]
        SET HolidayUK = 'Easter Monday'
    WHERE [Month] = 4 AND [DayOfMonth]  = 21

-- Early May Bank Holiday   May 5 
   UPDATE [dbo].[Date]
        SET HolidayUK = 'Early May Bank Holiday'
    WHERE [Month] = 5 AND [DayOfMonth]  = 5

-- Spring Bank Holiday  May 26 
    UPDATE [dbo].[Date]
        SET HolidayUK = 'Spring Bank Holiday'
    WHERE [Month] = 5 AND [DayOfMonth]  = 26

-- Summer Bank Holiday  August 25 
    UPDATE [dbo].[Date]
        SET HolidayUK = 'Summer Bank Holiday'
    WHERE [Month] = 8 AND [DayOfMonth]  = 25

-- Boxing Day  December 26      
    UPDATE [dbo].[Date]
        SET HolidayUK = 'Boxing Day'
    WHERE [Month] = 12 AND [DayOfMonth]  = 26   

--CHRISTMAS
    UPDATE [dbo].[Date]
        SET HolidayUK = 'Christmas Day'
    WHERE [Month] = 12 AND [DayOfMonth]  = 25

--New Years Day
    UPDATE [dbo].[Date]
        SET HolidayUK  = 'New Year''s Day'
    WHERE [Month] = 1 AND [DayOfMonth] = 1

--Update flag for UK Holidays 1= Holiday, 0=No Holiday
    
    UPDATE [dbo].[Date]
        SET IsHolidayUK  = CASE WHEN HolidayUK   IS NULL _
        THEN 0 WHEN HolidayUK   IS NOT NULL THEN 1 END
        
 
Step 4.
Update Values of Holiday as per USA Govt. Declaration for National Holiday.

/*Update HOLIDAY Field of USA In dimension*/
    
    /*THANKSGIVING - Fourth THURSDAY in November*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Thanksgiving Day'
    WHERE
        [Month] = 11 
        AND [DayOfWeekUSA] = 'Thursday' 
        AND DayOfWeekInMonth = 4

    /*CHRISTMAS*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Christmas Day'
        
    WHERE [Month] = 12 AND [DayOfMonth]  = 25

    /*4th of July*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Independance Day'
    WHERE [Month] = 7 AND [DayOfMonth] = 4

    /*New Years Day*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'New Year''s Day'
    WHERE [Month] = 1 AND [DayOfMonth] = 1

    /*Memorial Day - Last Monday in May*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Memorial Day'
    FROM [dbo].[Date]
    WHERE Id IN 
        (
        SELECT
            MAX(Id)
        FROM [dbo].[Date]
        WHERE
            [MonthName] = 'May'
            AND [DayOfWeekUSA]  = 'Monday'
        GROUP BY
            [Year],
            [Month]
        )

    /*Labor Day - First Monday in September*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Labor Day'
    FROM [dbo].[Date]
    WHERE Id IN 
        (
        SELECT
            MIN(Id)
        FROM [dbo].[Date]
        WHERE
            [MonthName] = 'September'
            AND [DayOfWeekUSA] = 'Monday'
        GROUP BY
            [Year],
            [Month]
        )

    /*Valentine's Day*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Valentine''s Day'
    WHERE
        [Month] = 2 
        AND [DayOfMonth] = 14

    /*Saint Patrick's Day*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Saint Patrick''s Day'
    WHERE
        [Month] = 3
        AND [DayOfMonth] = 17

    /*Martin Luthor King Day - Third Monday in January starting in 1983*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Martin Luthor King Jr Day'
    WHERE
        [Month] = 1
        AND [DayOfWeekUSA]  = 'Monday'
        AND [Year] >= 1983
        AND DayOfWeekInMonth = 3

    /*President's Day - Third Monday in February*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'President''s Day'
    WHERE
        [Month] = 2
        AND [DayOfWeekUSA] = 'Monday'
        AND DayOfWeekInMonth = 3

    /*Mother's Day - Second Sunday of May*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Mother''s Day'
    WHERE
        [Month] = 5
        AND [DayOfWeekUSA] = 'Sunday'
        AND DayOfWeekInMonth = 2

    /*Father's Day - Third Sunday of June*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Father''s Day'
    WHERE
        [Month] = 6
        AND [DayOfWeekUSA] = 'Sunday'
        AND DayOfWeekInMonth = 3

    /*Halloween 10/31*/
    UPDATE [dbo].[Date]
        SET HolidayUSA = 'Halloween'
    WHERE
        [Month] = 10
        AND [DayOfMonth] = 31

    /*Election Day - The first Tuesday after the first Monday in November*/
    BEGIN
    DECLARE @Holidays TABLE (ID INT IDENTITY(1,1), _
    DateID int, Week TINYINT, YEAR CHAR(4), DAY CHAR(2))

        INSERT INTO @Holidays(DateID, [Year],[Day])
        SELECT
            Id,
            [Year],
            [DayOfMonth] 
        FROM [dbo].[Date]
        WHERE
            [Month] = 11
            AND [DayOfWeekUSA] = 'Monday'
        ORDER BY
            YEAR,
            DayOfMonth 

        DECLARE @CNTR INT, @POS INT, @STARTYEAR INT, @ENDYEAR INT, @MINDAY INT

        SELECT
            @CURRENTYEAR = MIN([Year])
            , @STARTYEAR = MIN([Year])
            , @ENDYEAR = MAX([Year])
        FROM @Holidays

        WHILE @CURRENTYEAR <= @ENDYEAR
        BEGIN
            SELECT @CNTR = COUNT([Year])
            FROM @Holidays
            WHERE [Year] = @CURRENTYEAR

            SET @POS = 1

            WHILE @POS <= @CNTR
            BEGIN
                SELECT @MINDAY = MIN(DAY)
                FROM @Holidays
                WHERE
                    [Year] = @CURRENTYEAR
                    AND [Week] IS NULL

                UPDATE @Holidays
                    SET [Week] = @POS
                WHERE
                    [Year] = @CURRENTYEAR
                    AND [Day] = @MINDAY

                SELECT @POS = @POS + 1
            END

            SELECT @CURRENTYEAR = @CURRENTYEAR + 1
        END

        UPDATE [dbo].[Date]
            SET HolidayUSA  = 'Election Day'                
        FROM [dbo].[Date] DT
            JOIN @Holidays HL ON (HL.DateID + 1) = DT.Id
        WHERE
            [Week] = 1
    END
    --set flag for USA holidays in Dimension
    UPDATE [dbo].[Date]
SET IsHolidayUSA = CASE WHEN HolidayUSA  IS NULL THEN 0 WHEN HolidayUSA  IS NOT NULL THEN 1 END
/*****************************************************************************************/