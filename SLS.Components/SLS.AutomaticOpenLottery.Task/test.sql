

SELECT temp3.ParentName AS Name,
       COUNT(temp3.UserID) AS cnt,
       SUM(temp3.pay) AS pay,
       SUM(temp3.dis) AS dis,
       SUM(temp3.win) AS win
FROM
(
    SELECT temp2.*,
           ISNULL(tb3.name, '') AS ParentName
    FROM
    (
        SELECT temp.UserID,
               MAX(temp.Name) AS Name,
               SUM(temp.pay) AS pay,
               SUM(temp.dis) AS dis,
               SUM(temp.win) AS win
        FROM
        (
            SELECT u.SiteID,
                   u.ID AS UserID,
                   u.Name,
                   0.00 AS pay,
                   0.00 AS dis,
                   0.00 AS win
            FROM V_Users u
            UNION ALL
            SELECT p.SiteID,
                   p.UserID,
                   p.Name,
                   p.Money AS pay,
                   0.00 AS dis,
                   0.00 AS win
            FROM V_UserPayDetails p
            WHERE p.Result = 1
                  AND p.DateTime BETWEEN '2017-06-01' AND '2017-08-01'
            UNION ALL
            SELECT d.SiteID,
                   d.UserID,
                   d.Name,
                   0.00 AS pay,
                   d.Money AS dis,
                   0.00 AS win
            FROM V_UserDistills d
            WHERE d.Result = 1
                  AND d.DateTime BETWEEN '2017-06-01' AND '2017-08-01'
            UNION ALL
            SELECT w.SiteID,
                   w.UserID,
                   w.Name,
                   0.00 AS pay,
                   0.00 AS dis,
                   w.Money AS win
            FROM V_UserDetails w
            WHERE w.DateTime BETWEEN '2017-06-01' AND '2017-08-01'
        ) AS temp
        WHERE temp.SiteID = 1
        GROUP BY temp.UserID
    ) AS temp2
    LEFT JOIN
    (
        SELECT ID,
               UserID,
               [DateTime],
               PristineUserID,
               NowUserID,
               OperatorID,
               [Type],
               ChangeType
        FROM T_CpsUserChange AS a
        WHERE ID IN
        (
            SELECT TOP 1 ID
            FROM T_CpsUserChange
            WHERE UserID = a.UserID
            ORDER BY DateTime DESC
        )
    ) tb2 ON temp2.UserID = tb2.UserID
    LEFT JOIN dbo.T_Users tb3 ON tb2.NowUserID = tb3.ID
    WHERE 1 = 1
) AS temp3
GROUP BY temp3.ParentName;