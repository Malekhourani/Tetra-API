CREATE DATABASE [Tetra Messenger]
GO

USE [Tetra Messenger]
GO
/****** Object:  Table [dbo].[AdminPermissions]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminPermissions](
	[AdminID] [int] NULL,
	[PermissionID] [int] NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admins](
	[AdminID] [int] IDENTITY(1,1) NOT NULL,
	[AdminType] [nvarchar](50) NULL,
 CONSTRAINT [Admin_PK] PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BlockedAccounts]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlockedAccounts](
	[BlockerID] [int] NULL,
	[BlockedID] [int] NULL,
	[BlockID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BlockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[ContactID] [int] NOT NULL,
	[UserID] [int] NULL,
	[FriendID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FriendID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupMessages]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupMessages](
	[MessageID] [int] NULL,
	[GroupID] [int] NULL,
	[SenderID] [int] NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[GroupID] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreateTime] [datetime] NULL,
	[GroupPicture] [int] NULL,
	[Owner] [int] NULL,
	[GroupType] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Media]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Media](
	[MediaID] [int] NOT NULL,
	[MediaName] [nvarchar](50) NULL,
	[Size] [nvarchar](50) NULL,
	[Path] [nvarchar](max) NULL,
 CONSTRAINT [Media_PK] PRIMARY KEY CLUSTERED 
(
	[MediaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[SentTime] [datetime] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[Attachment] [int] NULL,
	[ReplyMessageID] [int] NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotActivatedAccounts]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotActivatedAccounts](
	[ActivateCode] [nvarchar](50) NOT NULL,
	[UserID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ActivateCode] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Participants]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Participants](
	[UserID] [int] NULL,
	[GroupID] [int] NULL,
	[AdminID] [int] NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 9/8/2020 10:42:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[PermissionID] [int] IDENTITY(1,1) NOT NULL,
	[PermissionName] [nvarchar](50) NULL,
	[PermissionDescrption] [nvarchar](max) NULL,
 CONSTRAINT [Permission_PK] PRIMARY KEY CLUSTERED 
(
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[NickName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Bio] [nvarchar](50) NULL,
	[Email] [nvarchar](max) NOT NULL,
	[ProfilePicture] [int] NULL,
	[Activated] [varchar](3) NOT NULL,
	[LastSeen] [datetime] NULL,
	[Status] [varchar](10) NOT NULL,
 CONSTRAINT [User_PK] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Activated]  DEFAULT ('NO') FOR [Activated]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Status]  DEFAULT ('ONLINE') FOR [Status]
GO
ALTER TABLE [dbo].[AdminPermissions]  WITH CHECK ADD FOREIGN KEY([AdminID])
REFERENCES [dbo].[Admins] ([AdminID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AdminPermissions]  WITH CHECK ADD FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([PermissionID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BlockedAccounts]  WITH CHECK ADD FOREIGN KEY([BlockerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[BlockedAccounts]  WITH CHECK ADD FOREIGN KEY([BlockedID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Contacts]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Contacts]  WITH CHECK ADD  CONSTRAINT [FK_Friend_UserID] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Contacts] CHECK CONSTRAINT [FK_Friend_UserID]
GO
ALTER TABLE [dbo].[GroupMessages]  WITH CHECK ADD FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupMessages]  WITH CHECK ADD FOREIGN KEY([MessageID])
REFERENCES [dbo].[Messages] ([MessageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupMessages]  WITH CHECK ADD FOREIGN KEY([SenderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD FOREIGN KEY([GroupPicture])
REFERENCES [dbo].[Media] ([MediaID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD FOREIGN KEY([Owner])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD FOREIGN KEY([Attachment])
REFERENCES [dbo].[Media] ([MediaID])
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD FOREIGN KEY([ReplyMessageID])
REFERENCES [dbo].[Messages] ([MessageID])
GO
ALTER TABLE [dbo].[NotActivatedAccounts]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Participants]  WITH CHECK ADD FOREIGN KEY([AdminID])
REFERENCES [dbo].[Admins] ([AdminID])
GO
ALTER TABLE [dbo].[Participants]  WITH CHECK ADD FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Participants]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([ProfilePicture])
REFERENCES [dbo].[Media] ([MediaID])
GO
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD CHECK  (([GroupType]='CONVERSATION' OR [GroupType]='GROUP' OR [GroupType]='CHANNEL'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Activated]='NO' OR [Activated]='YES'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Status]='OFFLINE' OR [Status]='ONLINE'))
GO
/****** Object:  StoredProcedure [dbo].[ActivateMyAccount]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ActivateMyAccount]
                 @userID int,
				 @ActivateCode varchar(max)
AS
BEGIN

	declare @Code varchar(max) = (select ActivateCode from NotActivatedAccounts Where UserID = @userID)

	if @UserID is null or @Code is null 
	 throw 51111,'Invalid Information',1
	else 
	 begin
	 delete from NotActivatedAccounts
	 where UserID = @UserID

	 update Users
	 set Activated = 'YES'
	 Where UserID = @UserID
	 end
END
GO
/****** Object:  StoredProcedure [dbo].[AddAFriend]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddAFriend]
                 @UserID int,
                 @FriendID int
AS
BEGIN
	SET NOCOUNT ON;

	if @UserID is null or @FriendID is null
	 throw 50009,'Invalid Information',1;
	else 
	begin
	 declare @isFriends int = (select UserID From Contacts Where UserID = @UserID And ContactID = @FriendID)

	 if @isFriends is null
	  begin
	   Insert into Contacts(ContactID,UserID)
	   values(@FriendID,@UserID);
	   Insert into Contacts(ContactID,UserID)
	   values(@UserID,@FriendID)
	  end

	  else throw 50011,'They are Friends Already',1

	end
END
GO
/****** Object:  StoredProcedure [dbo].[AddAnAdmin]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddAnAdmin]
       @UserID int,
	   @OwnerID int,
	   @GroupID int
AS


BEGIN
	SET NOCOUNT ON;

	if @UserID is null OR @GroupID is null or @OwnerID is null 
	 throw 50009 , 'Invalid values' , 1

    else 
	 if Exists(Select userID from Participants Where UserID = @UserID And GroupID = @GroupID)
	    And 
	    Exists(select owner from groups where groupID = @GroupID And Owner = @OwnerID)
	     update Participants 
	     set AdminID = 2
	     where GroupID = @GroupID And UserID = @UserID
	else throw 50003,'The user is not a member this group or the requester does not the group owner',1
 
END
GO
/****** Object:  StoredProcedure [dbo].[AddAUser]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddAUser]
                 @GroupID int,
                 @UserID int,
				 @RequesterID int
				 
AS
BEGIN
	SET NOCOUNT ON;

	if @UserID is null OR @GroupID is null 
	 throw 50006 , 'Invalid values' , 2

	if exists(select userId from Participants where GroupID = @GroupID and UserID = @UserID)
	 throw 50005 ,'the user already exist' , 1
	
	if not exists(select userID 
	              from Participants 
			      where GroupID = @GroupID 
			      and UserID = @RequesterID 
			      and AdminID in (1,2))
     throw 50007,'missing privileges',1

	 
	 insert into Participants(UserID,GroupID)
	 Values(@UserID,@GroupID)

	 
END
GO
/****** Object:  StoredProcedure [dbo].[BlockAUser]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BlockAUser] 
                 @BlockerID int,
				 @BlockedID int
AS
BEGIN
	SET NOCOUNT ON;

	if @BlockedID is null or @BlockerID is null throw 50111,'Invalid Information',1

	else 
	begin

	if Not Exists(Select ContactID from Contacts Where ContactID = @BlockedID And UserID = @BlockerID )
	 begin
	  Insert into BlockedAccounts(BlockerID,BlockedID)
	  values(@BlockerID,@BlockedID)
	 end

	else 
	 begin
	  delete from Contacts
	  Where (UserID = @BlockerID And ContactID = @BlockedID)
	  OR (UserID = @BlockedID And ContactID = @BlockerID)

	  Insert into BlockedAccounts(BlockerID,BlockedID)
	  values(@BlockerID,@BlockedID)
	 end

	end
END
GO
/****** Object:  StoredProcedure [dbo].[CreateAConversation]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateAConversation]
                 @User1ID int,
				 @User2ID int
AS

BEGIN
	SET NOCOUNT ON;

	if Not Exists(Select * From Contacts Where UserID = @User1ID and ContactID = @User2ID)
	throw 50005,'These Users Are Not Friends',1

	else 
	begin
	if not Exists(
	             Select Groups.GroupID
	             From Groups
				 join Participants on Groups.GroupID = Participants.GroupID
				 Where Participants.UserID = @User1ID 
				 And Groups.GroupType = 'CONVERSATION'
				 Intersect
				 Select Groups.GroupID
	             From Groups
				 join Participants on Groups.GroupID = Participants.GroupID
				 Where Participants.UserID = @User2ID
				 And Groups.GroupType = 'CONVERSATION'
				 ) 
	begin
	declare @user1Nickname varchar(max) = (select NickName from users where UserID = @User1ID)
	declare @user2Nickname varchar(max) = (select NickName from users where UserID = @User2ID)

    Insert into Groups(GroupType,CreateTime,GroupName,Owner)
	values('CONVERSATION',GETDATE(),(@User1NickName+' '+@User2NickName),@User1ID); 

	Declare @GroupID int = (Select Max(GroupID) from Groups where GroupType = 'CONVERSATION' and owner = @User1ID);

	Insert into Participants(UserID,GroupID,AdminID)
	Values(@User1ID,@GroupID,1)
	Insert Into Participants(UserID,GroupID,AdminID)
	Values(@User2ID,@GroupID,2)
	end
	end
END
GO
/****** Object:  StoredProcedure [dbo].[CreateAGroup]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateAGroup]
	@GroupName nvarchar(max),
	@UserID int
AS
Declare @GroupID int;
Declare @IsUsed int;
BEGIN
	SET NOCOUNT ON;

	set @IsUsed = (Select Count(GroupName) from Groups where GroupName = @GroupName and GroupType = 'GROUP')

	if @IsUsed <> 0
	 THROW 50000,'Group Name USED BEFORE',1
	
    else 
	begin


	 Insert Into Groups(GroupName,GroupType,Owner,CreateTime)
	 Values(@GroupName,'GROUP',@UserID,GETDATE());

	 Set @GroupID = (Select GroupID
	                 From [Tetra Messenger].dbo.Groups
					 Where GroupName = @GroupName
					 and GroupType = 'GROUP');

	 Insert into Participants(UserID,GroupID,AdminID)
	 values(@UserID,@GroupID,1);
	end

END;
GO
/****** Object:  StoredProcedure [dbo].[CreateAUser]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateAUser]
	@FullName nvarchar(max) , 
	@NickName nvarchar(max) , 
	@Password nvarchar(max) , 
	@Bio nvarchar(max) , 
	@Email nvarchar(max) 
AS
Declare @AccountID int;
Declare @ActivateCode nvarchar(max);
Declare @IsUsed int;

BEGIN
	SET NOCOUNT ON;

	set @IsUsed = (Select Count(NickName) from Users where NickName = @NickName)

	if @IsUsed <> 0
	 THROW 50000,'UR NICK NAME USED BEFORE',1
	
    else 
	begin
	 Insert Into Users(FullName,NickName,Password,Bio,Email)
	 Values(@FullName,@NickName,@Password,@Bio,@Email)

	 Set @AccountID = (Select UserID
	                   From [Tetra Messenger].dbo.Users
					   Where NickName = @NickName)

     Set @ActivateCode = SUBSTRING(CONVERT(varchar(50), NEWID()),0,6)

	 Insert into NotActivatedAccounts
	 Values(@ActivateCode,@AccountID)
	 end
	
END;
GO

/****** Object:  StoredProcedure [dbo].[DeleteAConversation]    Script Date: 9/21/2020 8:34:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[DeleteAConversation]
                 @groupID int,
				 @requesterID int
AS
BEGIN
	SET NOCOUNT ON;

	  if @groupId is null or @requesterID is null 
	     throw 50008,'Invalid Information',3

	  if not Exists(select groupID from Participants where GroupID = @groupID and UserID = @requesterID)
	     throw 50012, 'User is not a member in this conversation', 1

	   delete from GroupMessages
	   where GroupID = @groupId

	   delete from Groups
	   where GroupID = @groupId

END
GO

/****** Object:  StoredProcedure [dbo].[DeleteAGroup]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAGroup]
                 @GroupID int,
				 @RequesterID int
AS
BEGIN
	SET NOCOUNT ON;

	if @Groupid is null or @RequesterID is null throw 50007,'Invalid Information',1

	if not exists(select groupID from Groups where GroupID = @GroupID and Owner = @RequesterID)
	   throw 50042,'missing privileges',1

	delete from Groups
	where GroupID = @Groupid

END
GO

/****** Object:  StoredProcedure [dbo].[DeleteFromContacts]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteFromContacts]
                 @UserID int,
                 @FriendID int
AS

BEGIN
	SET NOCOUNT ON;
	
	if @UserID is null or @FriendID is null
	 throw 50009,'Invalid Information',1;

	if not exists(select userID 
	              from Contacts 
				  where (userID = @UserID And ContactID = @FriendID)
				  or    (userID = @FriendID And ContactID = @UserID))
				  throw 50034,'users are not friends',1
	else 
	 Delete From Contacts
	 where (userID = @UserID And ContactID = @FriendID)
	 or    (userID = @FriendID And ContactID = @UserID)
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteMessage]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteMessage]
                 @RequseterID int,
				 @MessageID int
AS
BEGIN
	SET NOCOUNT ON;

	if @MessageID is null or @RequseterID is null 
	 throw 50013,'Invalid Information',1

	if not exists(select messageId from GroupMessages where SenderID = @RequseterID and MessageID = @MessageID)
	 throw 50014,'Missing Privileges',1
	
	delete from GroupMessages 
	where SenderID = @RequseterID 
	and MessageID = @MessageID

	delete from Messages
	where MessageID = @MessageID

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteUser]
       @UserID int
AS
BEGIN
	SET NOCOUNT ON;

	delete From Users
	where UserID = @UserID;
END
GO
/****** Object:  StoredProcedure [dbo].[EditGroupDescription]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditGroupDescription]
                 @GroupID int,
				 @NewDescription nvarchar(max)
AS
BEGIN

	SET NOCOUNT ON;

	if @GroupId is null throw 50065,'invalid Information',1

	update Groups
	set Description = @NewDescription
	where GroupID = @GroupId

END
GO
/****** Object:  StoredProcedure [dbo].[EditGroupName]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditGroupName]
                 @GroupID int,
                 @NewName varchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	update Groups
	set GroupName = @NewName
	where GroupID = @GroupID;

END
GO
/****** Object:  StoredProcedure [dbo].[EditGroupPicture]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditGroupPicture]
                 @GroupID int,
				 @ImageID int,
				 @ImagePath nvarchar(max),
				 @ImageName varchar(max)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if not exists(select groupId from groups where groupID = @groupID and groupType = 'GROUP') throw 50008,'Invalid Information',1
	
	insert into Media(MediaID,MediaName,Path)
	values(@ImageID, @ImageName ,@ImagePath)

	update Groups 
	set GroupPicture = @ImageID
	where GroupID = @GroupId
	
	
END
GO

/****** Object:  StoredProcedure [dbo].[EditUserBio]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserBio]
                 @userId int,
                 @NewBio varchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	update Users
	set Bio = @NewBio
	where userID = @userId;

END
GO
/****** Object:  StoredProcedure [dbo].[EditUserEmail]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserEmail]
                 @OldEmail varchar(max),
                 @NewEmail varchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	update Users
	set Email = @NewEmail
	where Email = @OldEmail;

END
GO
/****** Object:  StoredProcedure [dbo].[EditUserNickName]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserNickName]
                 @userID int,
                 @NewNickName varchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	update Users
	set NickName = @NewNickName
	where UserID = @userID;

END
GO
/****** Object:  StoredProcedure [dbo].[EditUserPassword]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserPassword]
                 @userID int,
                 @OldPassword varchar(max),
                 @NewPassword varchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	if @userId is null throw 50011,'Invalid Information',1

	else
	 begin 
	  update Users
	  set Password = @NewPassword
      where Password = @OldPassword
	  and UserID = @userID;
	 end
END
GO
/****** Object:  StoredProcedure [dbo].[GoOffline]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GoOffline] 
                 @userID int
AS
BEGIN
	SET NOCOUNT ON;

	update users
	set Status = 'OFFLINE',
	LastSeen = GetDate()
	where UserID = @userID

END
GO
/****** Object:  StoredProcedure [dbo].[GoOnline]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GoOnline] 
                 @UserID int
AS
BEGIN
	SET NOCOUNT ON;
	update users
	set Status = 'ONLINE',
	LastSeen = GetDate()
	where UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveAnAdmin]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[RemoveAnAdmin]
                 @GroupID int,
				 @RequesterID int,
				 @UserID int
As
Begin
	SET NOCOUNT ON;

	  if @userid is null or @GroupID is null or @RequesterID is null
	      throw 50031,'Invalid User Information',1

	  if  not exists(select owner 
	                 from Groups 
					 where groupid = @groupId
					 and Owner = @RequesterID)
	      throw 50055,'User does not the group owner',1

      if not Exists(select userid from Participants where UserID = @userid and GroupID = @groupid and AdminID in (2,null))
	      throw 50044,'User does not a member in this group or it is group owner or it is not an admin already',1

	   update Participants
	   set AdminID = null 
	   where GroupID = @groupid
	   and UserID = @userid

End
GO
/****** Object:  StoredProcedure [dbo].[RemoveAUser]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RemoveAUser]
                 @OwnerID int,
				 @UserID int,
				 @GroupID int
AS
BEGIN
	SET NOCOUNT ON;

	if not exists(select groupID from groups where GroupID = @GroupID) 
	 throw 50055,'Group Not Found',1

	if (Select Owner from Groups where GroupID = @GroupID) <> @OwnerID
	  throw 50054,'Missing Permission',1

	if not exists(select userId from Participants where GroupID = @GroupID and userID = @UserID)
	  throw 50056,'user Not a membre in this Group',1

	delete from Participants
    where UserID = @UserID
	and GroupID = @GroupID 

END
GO
/****** Object:  StoredProcedure [dbo].[RemoveBlock]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RemoveBlock] 
                 @BlockerID int,
				 @BlockedID int
AS
BEGIN
	SET NOCOUNT ON;

	if @BlockedID is null or @BlockerID is null throw 50111,'Invalid Information',1

	else  
	begin
	 if Exists(Select BlockerID , BlockedID from BlockedAccounts where BlockerID = @BlockerID and BlockedID = @BlockedID)
	  begin
	  delete from BlockedAccounts
	  where BlockerID = @BlockerID
	  And BlockedID = @BlockedID
	  end
	 else throw 50015,'Invalid Information',1
	end
END
GO
/****** Object:  StoredProcedure [dbo].[SaveMedia]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaveMedia]
                 @MediaID int,
                 @Name varchar(max),
				 @Path varchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	Insert into Media(MediaID,MediaName,Path)
	Values(@MediaID, @Name, @Path);
END
GO
/****** Object:  StoredProcedure [dbo].[SendMessage]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SendMessage]
                 @GroupID int,
				 @SenderID int,
				 @Content nvarchar(max),
				 @AttachmentPath nvarchar(max),
				 @AttachmentName varchar(max),
				 @MessageToReplay int,
				 @SentTime datetime
				 
AS
BEGIN
	SET NOCOUNT ON;

	if @groupID is null or @Content is null 
	 throw 50055,'Invalid Information',1

	if not exists(select UserID from Participants where GroupID = @groupID and UserID = @SenderID)
	 throw 50056,'User is Not a Member in this group',1

	Insert into Media(Path,MediaName)
	values(@AttachmentPath,@AttachmentName)

	Insert into Messages(Content,Attachment,SentTime)
	values(@Content,(select max(mediaId) from media where Path = @AttachmentPath),@SentTime)

	declare @messageId int = (select Messages.MessageID 
	                          from GroupMessages 
							  full join Messages on GroupMessages.MessageID = Messages.MessageID
							  where GroupID = @groupID
							  and SenderID = @SenderID
							  and SentTime = @SentTime)

	insert into GroupMessages(GroupID,MessageID,SenderID)
	values(@groupID,@messageId,@SenderID)

	if @MessageToReplay is not null 
	 update Messages
	 set ReplyMessageID = @MessageToReplay
	 where MessageID = @messageId
	 
END
GO
/****** Object:  StoredProcedure [dbo].[setAnewAdmin]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[setAnewAdmin]
                 @GroupID int,
				 @AdminID int,
				 @UserID int
AS
BEGIN
	SET NOCOUNT ON;

	if not exists(Select GroupID from Groups Where groupID = @groupid)
	 throw 50033,'Group Not Found',1
	else 
	 begin
	 
	 if not exists(select userID from users where userId = @UserID) 
	  throw 50031,'Invalid User Information',1
	 else 
	  begin
	   if not Exists(select userid from Participants where UserID = @userid and GroupID = @groupid)
	    throw 50044,'User does not a membre in this group',1
	   else 
	    begin
	                           

       if not exists (select users.UserID from users 
	                  full join Participants on users.UserID = Participants.UserID
					  where GroupID = @groupid
					  and Participants.UserID = @AdminID
					  and AdminID = 1)
	    throw 50035,'Missing Privilege',1
	   else
	    begin

		 update Participants
		 set AdminID = 2
		 where GroupID = @groupid
		 and userid = @userid

		end
	  end
	  end
	 end
END
GO
/****** Object:  StoredProcedure [dbo].[SetAnImage]    Script Date: 9/8/2020 10:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SetAnImage] 
                 @userID int,
				 @Path nvarchar(max),
				 @ImageName varchar(max),
				 @ImageID int
AS
BEGIN
	SET NOCOUNT ON;
	
	if Exists(select userID from Users where UserID = @userID)
	begin
	insert into Media(MediaName,Path,MediaID)
	values(@ImageName,@Path,@ImageID)


	update Users
	set ProfilePicture = @ImageID
	where UserID = @userID

	end
	else throw 50006,'User Not Found',1

END
GO

insert into Permissions(PermissionName,PermissionDescrption)
values('AddUsers','This Permission allows Admins to Add new Users To their Group'),
('RemoveUsers','This Permission allows Admins to Remove Users From their Group'),
('Mute','This Permission allows Admins to Mute users in their Group')
GO

insert into Admins(AdminType)
values('OWNER'),('ADMIN')
GO

insert into AdminPermissions(AdminID,PermissionID)
values(1,1),(1,2),(1,3),(2,1)
GO