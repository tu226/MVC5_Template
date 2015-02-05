CREATE TABLE [dbo].[Users]
(
	[UserId] INT NOT NULL PRIMARY KEY identity (1,1),
	username nvarchar(255) not null,
    passwordhash nvarchar(max) not null
);
GO
create Table [dbo].UsersRoles (
[Userid] int not null,
RoleName nvarchar(50) not null
);
GO
/*
Set an admin test account and a user account
password = Admin$2015
Don't push it to production.
*/
Insert into users (username,passwordhash) 
values ('Admin','AHRTMnZE4rVuxhk3wz8/HFpkixoFImxiy3bNCw7uCBmKEFN4zJeYHxhHHMlAi1BHzg==') 
;
GO
declare @id int;
SET @id=(select scope_identity());
Insert into usersRoles (userid,rolename)
values (@id,'admin');
GO
Insert into users (username,passwordhash) 
values ('user','AHRTMnZE4rVuxhk3wz8/HFpkixoFImxiy3bNCw7uCBmKEFN4zJeYHxhHHMlAi1BHzg==') 
;
GO
declare @id int;
SET @id=(select scope_identity());
Insert into usersRoles (userid,rolename)
values (@id,'user');
GO