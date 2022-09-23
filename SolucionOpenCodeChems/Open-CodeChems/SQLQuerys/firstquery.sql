Create table [dbo].[User] (
	[username] [nvarchar] (40) not null,
	[password] [nvarchar] (32) not null,
	[email] [nvarchar] (max) not null,
	[name] [nvarchar] (50) not null,
	[nickname] [nvarchar] (40) not null,
	CONSTRAINT [PK_dbo.User] PRIMARY KEY ([username])
)
Create table [dbo].[Profile] (
	[nickname] [nvarchar] (40) not null,
	[victories] [int]  not null,
	[imageProfile] [varbinary] (max),
	[avatar] [varbinary] (max)
	CONSTRAINT [PK_dbo.Profile] PRIMARY KEY ([nickname])
)
    CREATE INDEX [IX_nickname] ON [dbo].[User]([nickname])

    ALTER TABLE [dbo].[User] ADD CONSTRAINT [FK_dbo.User_dbo.Profile_nickname] FOREIGN KEY ([nickname]) REFERENCES [dbo].[Profile] ([nickname]) ON DELETE CASCADE