
Create Database Project_Library;

--drop table Std_Login
--CREATE TABLE Std_Login (
--    UserID int identity(1,1) PRIMARY KEY,
--    FirstName varchar(255),
--	LastName varchar(255),
--    UserName varchar(255),
--    Password varchar(255)
--);
-------------------------------------------------------------------------------------------

CREATE TABLE tbl_Registration (
    UserID int identity(1,1) PRIMARY KEY,
    UserName varchar(255),
	Email varchar(255),
	Password varchar(255)

);


create table tbl_OTP
(
 OTPID int identity(1,1) PRIMARY KEY,
 UserID int ,
 OTP varchar(4),
 FOREIGN KEY (UserID) REFERENCES tbl_Registration(UserID)
)

create table tbl_CategoryMaster
(
 CategoryID int identity(1,1) PRIMARY KEY,
 CategoryName varchar(100)
)

insert into tbl_CategoryMaster values('Maths')
insert into tbl_CategoryMaster values('Science')
insert into tbl_CategoryMaster values('History')

create table tbl_CollegeMaster
(
 CollegeID int identity(1,1) PRIMARY KEY,
 CollegeName varchar(100)
)

insert into tbl_CollegeMaster values('Arts college, kowad')
--drop table tbl_SynopsisDetails
create table tbl_SynopsisDetails
(
 SynopsisID int identity(1,1) PRIMARY KEY,
 UserID int ,
 CategoryID int ,
 CollegeID int ,
 SynopsisHeader varchar(255),
 SynopsisDescription varchar(1000),
 FOREIGN KEY (UserID) REFERENCES tbl_Registration(UserID),
 FOREIGN KEY (CategoryID) REFERENCES tbl_CategoryMaster(CategoryID),
 FOREIGN KEY (CollegeID) REFERENCES tbl_CollegeMaster(CollegeID)
)

--drop table tbl_FilesDetails
create table tbl_FilesDetails
(
 FileID int identity(1,1) PRIMARY KEY,
 SynopsisID int ,
 FileName varchar(255),
 FilePath varchar(500),
 FOREIGN KEY (SynopsisID) REFERENCES tbl_SynopsisDetails(SynopsisID)
)


create table tbl_Comments
(
 CommentId int identity(1,1) PRIMARY KEY,
 UserID int ,
 CommentHeader varchar(255),
 CommentDescription varchar(1000),
 FOREIGN KEY (UserID) REFERENCES tbl_Registration(UserID)
)
