
SELECT COUNT(*) FROM Gadm..Region WHERE GeoLevel = 0	-- csak nagy országok
-- 88

SELECT COUNT(*) FROM Gadm..Region WHERE GeoLevel = 1	-- államok v kis országok
-- 2000

SELECT COUNT(*) FROM Gadm..Region WHERE GeoLevel = 2	-- megyék
-- 35075

SELECT COUNT(*) FROM Gadm..Region WHERE GeoLevel = 3
-- 100907

SELECT COUNT(*) FROM Gadm..Region WHERE GeoLevel = 4
-- 55965

SELECT COUNT(*) FROM Gadm..Region WHERE GeoLevel = 5
-- 55759

SELECT COUNT(*) FROM Gadm..Region WHERE GeoLevel = 6
-- 0


SELECT TOP 100 Name FROM Gadm..Region WHERE GeoLevel = 0

SELECT TOP 100 Name FROM Gadm..Region WHERE GeoLevel = 1

SELECT TOP 100 Name FROM Gadm..Region WHERE GeoLevel = 2

--------

TRUNCATE TABLE user_region

-------- 

DECLARE @geo_level tinyint = 2	-- megyék
DECLARE @region_id int
DECLARE @geom geography

DECLARE regions CURSOR FOR
	SELECT --TOP 100
		ID, geom FROM Gadm..Region
	WHERE GeoLevel = @geo_level

OPEN regions

FETCH NEXT FROM regions 
INTO @region_id, @geom

WHILE @@FETCH_STATUS = 0
BEGIN

	/*SELECT COUNT(*)
	FROM user_location
	WHERE @geom.Filter(point) = 1*/

	INSERT user_region
	SELECT user_id, cluster_id, @geo_level, @region_id
	FROM user_location
	WHERE @geom.Filter(point) = 1

	FETCH NEXT FROM regions 
	INTO @region_id, @geom
END 
CLOSE regions;
DEALLOCATE regions;


