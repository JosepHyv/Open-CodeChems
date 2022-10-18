Create table [dbo].[User] (
	[username] [nvarchar] (40) not null,
	[password] [nvarchar] (32) not null,
	[email] [nvarchar] (max) not null,
	[name] [nvarchar] (50) not null,
	CONSTRAINT [PK_dbo.User] PRIMARY KEY ([username])
)
Create table [dbo].[Profile] (
	[nickname] [nvarchar] (40) not null,
	[victories] [int] ,
	[imageProfile] [varbinary] (max),
	[avatar] [varbinary] (max),
	[defaults] [int],
	[username] [nvarchar] (40) not null,
	CONSTRAINT [PK_dbo.Profile] PRIMARY KEY ([nickname])
)
    CREATE INDEX [IX_username] ON [dbo].[Profile]([username])

    ALTER TABLE [dbo].[Profile] ADD CONSTRAINT [FK_dbo.Profile_dbo.User_username] FOREIGN KEY ([username]) REFERENCES [dbo].[User] ([username]) ON DELETE CASCADE