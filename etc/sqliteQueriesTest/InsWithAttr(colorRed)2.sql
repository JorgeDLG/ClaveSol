-- SQLite
SELECT Type, Value
FROM `Attribut` as A
INNER JOIN Attribut_Ins as a_i
ON 
A.Id = a_i.AttributId
INNER JOIN Instrument as I
ON
I.Id = a_i.InstrumentId
WHERE I.Id = 1