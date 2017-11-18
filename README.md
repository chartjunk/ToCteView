# ToCteView

1. Copy the following tsv:<br/>
```text
col1Name	col2Name	anotherCol
100	xxxx	NULL
200	yyy	98
300.5	zzz	qqqqqqqqqqqqqqqqq

```

2. Run `ToCteView.exe`
3. Paste the followinq query:

```sql
WITH mycte AS ( SELECT N'100' AS [col1Name], N'xxxx' AS [col2Name], NULL AS [anotherCol]
 UNION ALL SELECT N'200' AS [col1Name], N'yyy' AS [col2Name], N'98' AS [anotherCol]
 UNION ALL SELECT N'300.5' AS [col1Name], N'zzz' AS [col2Name], N'qqqqqqqqqqqqqqqqq' AS [anotherCol]) 
SELECT * FROM mycte
```

4. ???
5. Profit

## ToSelectValues

Similar to `ToCteView` but uses somewhat neater syntax:

```sql
SELECT * FROM (VALUES
  (N'100', N'xxxx', NULL)
, (N'200', N'yyy', N'98')
, (N'300.5', N'zzz', N'qqqqqqqqqqqqqqqqq') )
_([col1Name], [col2Name], [anotherCol])
```
