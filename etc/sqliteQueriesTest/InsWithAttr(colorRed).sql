--I want all the instruments that has red color Atribute. (MANY TO MANY)

SELECT *
FROM Instrument AS ins
    INNER JOIN Attribut_Ins AS atrIns ON --PK's that relates Instrument with Atribute
        ins.Id = atrIns.InstrumentId
    INNER JOIN Attribut AS atr ON
        atr.Id = atrIns.AttributId
WHERE
    --atrIns.AttributId = 1 --Red Color attribute
    atr.Id = 1 