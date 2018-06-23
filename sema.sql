/******** DMA Schema Migration Deployment Script      Script Date: 23.6.2018. 09.29.06 ********/

/****** Object:  Table [dbo].[Role]    Script Date: 23.6.2018. 09.29.03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Role]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Role](
	[Id] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
	[Name] [nvarchar](256) COLLATE Latin1_General_CI_AI NOT NULL,
 CONSTRAINT [PK_dbo.Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Role]') AND name = N'RoleNameIndex')
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[Role]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[User]    Script Date: 23.6.2018. 09.29.05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[Id] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
	[Email] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) COLLATE Latin1_General_CI_AI NULL,
	[SecurityStamp] [nvarchar](max) COLLATE Latin1_General_CI_AI NULL,
	[PhoneNumber] [nvarchar](max) COLLATE Latin1_General_CI_AI NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) COLLATE Latin1_General_CI_AI NOT NULL,
	[Name] [nvarchar](max) COLLATE Latin1_General_CI_AI NULL,
	[Surname] [nvarchar](max) COLLATE Latin1_General_CI_AI NULL,
	[NumberOfTokens] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND name = N'UserNameIndex')
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[User]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__User__NumberOfTo__46E78A0C]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF__User__NumberOfTo__46E78A0C]  DEFAULT ((0)) FOR [NumberOfTokens]
END

GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 23.6.2018. 09.29.05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRole]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserRole](
	[UserId] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
	[RoleId] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
 CONSTRAINT [PK_dbo.UserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UserRole]') AND name = N'IX_RoleId')
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[UserRole]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UserRole]') AND name = N'IX_UserId')
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[UserRole]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[Claim]    Script Date: 23.6.2018. 09.29.05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Claim]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Claim](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
	[ClaimType] [nvarchar](max) COLLATE Latin1_General_CI_AI NULL,
	[ClaimValue] [nvarchar](max) COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_dbo.Claim] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Claim]') AND name = N'IX_UserId')
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[Claim]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[Login]    Script Date: 23.6.2018. 09.29.05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Login]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Login](
	[LoginProvider] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
	[ProviderKey] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
	[UserId] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
 CONSTRAINT [PK_dbo.Login] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Login]') AND name = N'IX_UserId')
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[Login]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 23.6.2018. 09.29.05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__MigrationHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) COLLATE Latin1_General_CI_AI NOT NULL,
	[ContextKey] [nvarchar](300) COLLATE Latin1_General_CI_AI NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) COLLATE Latin1_General_CI_AI NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
/****** Object:  Table [dbo].[Auction]    Script Date: 23.6.2018. 09.29.05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Auction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Auction](
	[Name] [nvarchar](max) COLLATE Latin1_General_CI_AI NULL,
	[Image] [varbinary](max) NULL,
	[Duration] [bigint] NOT NULL,
	[StartPrice] [decimal](18, 2) NOT NULL,
	[CurrentPrice] [decimal](18, 2) NOT NULL,
	[Currency] [int] NOT NULL,
	[CreatingTime] [datetime] NOT NULL,
	[OpeningTime] [datetime] NULL,
	[ClosingTime] [datetime] NULL,
	[User_Id] [nvarchar](128) COLLATE Latin1_General_CI_AI NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Auction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Auction]') AND name = N'IX_User_Id')
CREATE NONCLUSTERED INDEX [IX_User_Id] ON [dbo].[Auction]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Auction__Id__71D1E811]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Auction] ADD  CONSTRAINT [DF__Auction__Id__71D1E811]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [Id]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Auction__Status__05D8E0BE]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Auction] ADD  CONSTRAINT [DF__Auction__Status__05D8E0BE]  DEFAULT ((0)) FOR [Status]
END

GO
/****** Object:  Table [dbo].[Order]    Script Date: 23.6.2018. 09.29.05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Order](
	[Id] [uniqueidentifier] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Currency] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[User_Id] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
	[NumberOfTokens] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND name = N'IX_User_Id')
CREATE NONCLUSTERED INDEX [IX_User_Id] ON [dbo].[Order]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Order__NumberOfT__68487DD7]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF__Order__NumberOfT__68487DD7]  DEFAULT ((0)) FOR [NumberOfTokens]
END

GO
/****** Object:  Table [dbo].[Bid]    Script Date: 23.6.2018. 09.29.05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bid]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Bid](
	[Id] [uniqueidentifier] NOT NULL,
	[PlacingTime] [datetime] NOT NULL,
	[NumberOfTokens] [bigint] NOT NULL,
	[User_Id] [nvarchar](128) COLLATE Latin1_General_CI_AI NOT NULL,
	[Auction_Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Bid] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Bid]') AND name = N'IX_Auction_Id')
CREATE NONCLUSTERED INDEX [IX_Auction_Id] ON [dbo].[Bid]
(
	[Auction_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Bid]') AND name = N'IX_User_Id')
CREATE NONCLUSTERED INDEX [IX_User_Id] ON [dbo].[Bid]
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Table [dbo].[DefaultValues]    Script Date: 23.6.2018. 09.29.06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DefaultValues]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DefaultValues](
	[Id] [uniqueidentifier] NOT NULL,
	[NumberOfAuctionsPerPage] [bigint] NOT NULL,
	[AuctionDuration] [bigint] NOT NULL,
	[SilverTokenNumber] [bigint] NOT NULL,
	[GoldTokenNumber] [bigint] NOT NULL,
	[PlatinuTokenNumber] [bigint] NOT NULL,
	[TokenValue] [decimal](18, 2) NOT NULL,
	[Currency] [int] NOT NULL,
 CONSTRAINT [PK_dbo.DefaultValues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRole]'))
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRole]'))
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRole]'))
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRole]'))
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Claim]'))
ALTER TABLE [dbo].[Claim]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Claim]'))
ALTER TABLE [dbo].[Claim] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Login]'))
ALTER TABLE [dbo].[Login]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Login]'))
ALTER TABLE [dbo].[Login] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Auction_dbo.User_User_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[Auction]'))
ALTER TABLE [dbo].[Auction]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Auction_dbo.User_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[User] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Auction_dbo.User_User_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[Auction]'))
ALTER TABLE [dbo].[Auction] CHECK CONSTRAINT [FK_dbo.Auction_dbo.User_User_Id]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Order_dbo.User_User_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[Order]'))
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Order_dbo.User_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Order_dbo.User_User_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[Order]'))
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_dbo.Order_dbo.User_User_Id]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Bid_dbo.Auction_Auction_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bid]'))
ALTER TABLE [dbo].[Bid]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Bid_dbo.Auction_Auction_Id] FOREIGN KEY([Auction_Id])
REFERENCES [dbo].[Auction] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Bid_dbo.Auction_Auction_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bid]'))
ALTER TABLE [dbo].[Bid] CHECK CONSTRAINT [FK_dbo.Bid_dbo.Auction_Auction_Id]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Bid_dbo.User_User_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bid]'))
ALTER TABLE [dbo].[Bid]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Bid_dbo.User_User_Id] FOREIGN KEY([User_Id])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.Bid_dbo.User_User_Id]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bid]'))
ALTER TABLE [dbo].[Bid] CHECK CONSTRAINT [FK_dbo.Bid_dbo.User_User_Id]
GO

