----------------------------------------------------
-- Szûrés térben HTM-mel

DECLARE @lon float = 19.040833
DECLARE @lat float = 47.498333
DECLARE @rad float = 1.83 * 5	-- roughly in km

SELECT TOP 1000 tweet_id, text
FROM Graywulf_Code.dbo.fHtmCoverCircleEq(@lon, @lat, @rad) htm
INNER LOOP JOIN tweet WITH (INDEX(IX_tweet_htm))
	ON tweet.htm_id BETWEEN htm.HtmIDStart AND htm.HtmIDEnd
WHERE run_id = 2004

-- A hintek kellenek, különben eltéved a szerver.

----------------------------------------------------
-- Szûrés idõben

DECLARE @tol bigint
DECLARE @ig bigint

SELECT @tol = tweet_id
FROM tweet_hour
WHERE run_id = 1004 AND time = '11/01/2012 0:00'

SELECT @ig = tweet_id FROM tweet_hour
WHERE run_id = 1004 AND time = '12/01/2012 0:00'

SELECT COUNT(*)
FROM tweet
WHERE run_id = 1004 AND tweet_id BETWEEN @tol AND @ig
       AND text LIKE '%gangnam%'

-- Ebben az a trükk, hogy a tweetek id szerint vannak sorba rakva, és nem dátum szerint,
-- így ha közvetlenül a dátumra szûrnél, akkor a szerver nem tudná kihasználni azt, hogy
-- sorban vannak, és végigolvasná az összeset. A tweet_hour táblából viszont ki lehet lesni,
-- hogy milyen ID-val kezdõdnek az egész órák (UTC).

----------------------------------------------------
-- Térbeli hisztogram számolása

DECLARE @binsize float = 0.01

SELECT FLOOR(lon / @binsize) * @binsize, FLOOR(lat / @binsize) * @binsize, COUNT(*)
FROM tweet
WHERE lon BETWEEN -75 AND -73 AND lat BETWEEN 40 AND 42
GROUP BY FLOOR(lon / @binsize) * @binsize, FLOOR(lat / @binsize) * @binsize
HAVING COUNT(*) > 1000


----------------------------------------------------
-- Idõbeli hisztogram

SELECT dateadd(day, datediff(day, 0, created_at),0), COUNT(*)
FROM tweet
WHERE run_id = 2004
GROUP BY dateadd(day, datediff(day, 0, created_at),0)
ORDER BY 1

----------------------------------------------------
-- Így néz ki egy query, ami megadja a love szó eloszlását a hét napjai szerint:

SELECT DATEPART(WEEKDAY, created_at), COUNT(*)
FROM tweet
WHERE run_id = 2004 AND CONTAINS(*, 'love')
GROUP BY DATEPART(WEEKDAY, created_at)
ORDER BY 1

----------------------------------------------------
-- Így lehet lekérni a legtöbbször elõforduló szavakat:

SELECT TOP 1000 * FROM sys.dm_fts_index_keywords(DB_ID('twitter_1'),OBJECT_ID('tweet'))
ORDER BY document_count DESC


----------------------------------------------------
-- Gráf fokszámeloszlása

WITH degree_dist AS
(
	SELECT user_id, COUNT(*) AS deg
	FROM tweet_user_mention WHERE run_id = 1004
	GROUP BY user_id
)
SELECT deg, COUNT(*)
FROM degree_dist
GROUP BY deg
ORDER BY deg
