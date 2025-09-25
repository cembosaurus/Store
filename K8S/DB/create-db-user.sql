USE master;
CREATE LOGIN testUser WITH PASSWORD='pa55w0rd!', DEFAULT_DATABASE=master, DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'testUser')
BEGIN
CREATE USER testUser FOR LOGIN testUser
ALTER ROLE db_datareader ADD MEMBER testUser
ALTER ROLE db_datawriter ADD MEMBER testUser
GRANT EXECUTE TO testUser
END;