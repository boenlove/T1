CREATE OR REPLACE PROCEDURE usp_GetPartsFromSchedulePool()
BEGIN
	SELECT	PART_NBR, 
			ip.MES_PART_ID 
	FROM	MESDBA.ICS_SCHEDULE_POOL ip, 
			MESDBA.MES_PART mp 
	WHERE	ip.MES_PART_ID = mp.MES_PART_ID 
	GROUP BY mp.PART_NBR, ip.MES_PART_ID 
	ORDER BY PART_NBR ASC;
END;