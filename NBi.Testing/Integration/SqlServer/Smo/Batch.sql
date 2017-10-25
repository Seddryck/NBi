create table TablexxxOne (Id int IDENTITY(1,1), Name varchar(255));
go
create table TablexxxTwo (Id int IDENTITY(1,1), Name varchar (255));
go
print 'Tables created'
insert into TablexxxOne values ('No name')
GO