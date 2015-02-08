CREATE TABLE [dbo].[Users]
(
	[UserId] INT NOT NULL PRIMARY KEY identity (1,1),
	username nvarchar(255) not null,
    passwordhash nvarchar(max) not null
);
GO
create Table [dbo].UsersRoles (
[Userid] int not null,
RoleId int not null
);
GO
create table [dbo].ROLES
(
RoleId int not null primary key identity(1,1),
RoleName nvarchar(50) not null,
RoleDescription nvarchar(255) null
);
GO
/*
Set an admin test account and a user account
password = Admin$2015
Don't push it to production.
*/
Insert into users (username,passwordhash) 
values ('Admin','AHRTMnZE4rVuxhk3wz8/HFpkixoFImxiy3bNCw7uCBmKEFN4zJeYHxhHHMlAi1BHzg==');
declare @id int;
SET @id=(select scope_identity());
Insert into Roles (rolename,roledescription)
values ('admin','compte administrateur');
declare @roleid int;
set @roleid=(select scope_identity());
Insert into usersroles (userid,roleid) values (@id,@roleid);
GO

/*Create user account Password=Admin$2015*/
Insert into users (username,passwordhash) 
values ('user','AHRTMnZE4rVuxhk3wz8/HFpkixoFImxiy3bNCw7uCBmKEFN4zJeYHxhHHMlAi1BHzg==');
declare @id int;
SET @id=(select scope_identity());
Insert into Roles (rolename,roledescription)
values ('user','compte utilisateur');
declare @roleid int;
set @roleid=(select scope_identity());
Insert into usersroles (userid,roleid) values (@id,@roleid);
GO