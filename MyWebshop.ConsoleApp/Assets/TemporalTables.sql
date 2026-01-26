USE master;
GO

DROP DATABASE IF EXISTS JessesDb;
GO

CREATE DATABASE JessesDb;
GO

USE JessesDb;
GO

CREATE TABLE dbo.Leveranciers
(
	id			 int IDENTITY(1,1) NOT NULL PRIMARY KEY,						-- eis 1: PK
    voornaam	 nvarchar(60),
    SysStartTijd datetime2 GENERATED ALWAYS AS ROW START HIDDEN
				 CONSTRAINT DF_Leveranciers_SysStartTijd 
				 DEFAULT SYSUTCDATETIME() NOT NULL,								-- eis 2: datetime2
	SysEindTijd	 datetime2 GENERATED ALWAYS AS ROW END HIDDEN
				 CONSTRAINT DF_Leveranciers_SysEindTijd 
  				 DEFAULT CONVERT(datetime2, '9999-12-31 23:59:59') NOT NULL,	-- eis 3: datetime2
    PERIOD FOR SYSTEM_TIME(SysStartTijd, SyseindTijd)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.LeveranciersHistory));
GO

-- eis 4: bij history table GEEN constraints
-- eis 5: zelf naam geven aan historie tabel vereist schema naam (dbo)

INSERT INTO dbo.Leveranciers
(voornaam)
VALUES
('Ad')
, ('Bo')
, ('Cas');
GO

SELECT * FROM dbo.Leveranciers

SELECT *, SysStartTijd, SysEindTijd FROM dbo.Leveranciers;

SELECT * FROM dbo.LeveranciersHistory;

UPDATE dbo.Leveranciers 
SET voornaam = 'Beau'
WHERE id = 2;

SELECT * FROM dbo.LeveranciersHistory;


-- Wow, 5 speciale keywords die je kunt gebruiken 
-- 1. ALL	
-- alle data van de current en historical tables zonder restricties

SELECT *, sysStartTijd, SysEindTijd 
FROM dbo.Leveranciers
FOR SYSTEM_TIME ALL
WHERE id = 2;

-- 2. AS OF <date_time>		
-- de status van de data voor het gegeven tijdmoment

SELECT *, sysStartTijd, SysEindTijd
FROM dbo.Leveranciers
FOR SYSTEM_TIME AS OF '2026-01-26 15:49:10.9819200'
WHERE id = 2;


-- 3. FROM <start_date_time> TO <end_date_time>	
-- alle current en historische rijen die actief waren in de tijdspanne
-- (behalve de rijen die inactief werden ten tijde van de TO clause)

-- 4. BETWEEN <start_date_time> AND <end_date_time>
-- zie FROM TO, maar dan + rijen die actief waren ten tijde van upper boundary

-- 5. CONTAINED IN (<start_date_time>, <end_date_time>)	
-- zie FROM TO, maar dan + rijen die inactief werden exact ten tijde van upper boundary 
-- (snelst, gebruikt enkel history table voor querying)
