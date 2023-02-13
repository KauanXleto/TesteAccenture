IF DB_ID('AccentureDb') IS NULL
begin
	Create dataBase AccentureDb

	print('Database foi criado')
end
go

use AccentureDb;
go
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'LogType'))
begin

	Create table LogType(
		Id int identity primary key not null,
		Name varchar(500)
	);

	Insert into LogType(Name)
					Values	('unknown'),
							('Received disconnect'),
							('Invalid user'),
							('Reverse mapping checking'),
							('Many authentication failures'),
							('Connection reset'),
							('Connection closed'),
							('Did not receive identification string'),
							('Session closed'),
							('Session opened'),
							('Does not map back to the address'),
							('Corrupted MAC on input'),
							('Could not write ident string'),
							('Bad protocol version identification')

	print('Tabelas foram criadas')
end;

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'LogInfo'))

begin
	truncate table LogInfo
	DBCC CHECKIDENT('LogInfo', RESEED, 1)

	print('Tabelas foram limpas')
end 
else
begin

	Create table LogInfo(
		Id int identity primary key not null,
		LogIp varchar(500),
		LogDate varchar(500),
		LogIdentification varchar(500),
		LogDescription varchar(500),
		LogTypeId int,

		Foreign key(LogTypeId) references LogType(Id) 
	)
	print('Tabelas foram criadas')
end