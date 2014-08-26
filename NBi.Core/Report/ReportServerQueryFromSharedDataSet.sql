WITH cte
AS (
	SELECT 
		[path]
		, [name] AS SharedDataSetName,
		CONVERT(XML, CONVERT(VARBINARY(MAX), content)) AS rdl
	FROM
		dbo.catalog
	WHERE
		type=8
),
SharedDataSet AS (
SELECT
	LEFT([Path], LEN([path]) - CHARINDEX('/',REVERSE([Path])) + 1) AS ReportPath
	, SharedDataSetName AS SharedDataSetName
	, ISNULL(T1.N.value('(*:CommandType/text())[1]', 'nvarchar(128)'), 'T-SQL') AS CommandType
	, T1.N.value('(*:CommandText/text())[1]', 'nvarchar(max)') AS CommandText
FROM
	cte AS T
CROSS APPLY
	T.rdl.nodes('/*:SharedDataSet/*:DataSet/*:Query') AS T1 (N)
)
SELECT
	*
FROM
	SharedDataSet
WHERE
	SharedDataSetName=@SharedDataSetName