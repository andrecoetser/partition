declare @tmp varchar(max)
SET @tmp = ''
select @tmp = @tmp + '''' + CAST(datekey as varchar(10)) + ''',' from dbo.Dates

declare @sql varchar(max) = 'CREATE PARTITION FUNCTION [ByDatePF](datetime) AS RANGE RIGHT FOR VALUES(' + SUBSTRING(@tmp, 0, LEN(@tmp)) + ')'
exec (@sql)
  
CREATE PARTITION SCHEME [ByDateRange]   
   AS PARTITION [ByDatePF]   
   ALL TO ([PRIMARY]);  
GO


