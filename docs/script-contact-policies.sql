CREATE TABLE [dbo].[SubscribersList](
	[IdSubscribersList] [int] IDENTITY(500000,1) NOT NULL,
	[IdUser] [int] NOT NULL,
	[IdLabel] [int] NULL,
	[SubscribersListStatus] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Active] [bit] NOT NULL,
	[UTCCreationDate] [datetime] NULL,
	[UTCLastUseDate] [datetime] NULL,
	[Ranking] [int] NULL,
	[APIKey] [varchar](100) NULL,
	[Visible] [bit] NOT NULL,
	[UTCDeleteDate] [datetime] NULL,
	[UTCLastSentDate] [datetime] NULL,
	[IsDuplicated] [bit] NOT NULL,
	[UpdatedAt] [datetime2](0) NOT NULL,
	[BlackListCount] [int] NOT NULL,
	[HasToValidate] [bit] NOT NULL,
	[IdLastImportRequest] [int] NULL,
 CONSTRAINT [PK_SuscriberList] PRIMARY KEY CLUSTERED ([IdSubscribersList] ASC)
)
GO

CREATE TABLE [dbo].[SubscribersListXShippingLimit](
	[IdUser] [int] NOT NULL,
	[IdSubscribersList] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_SubscribersListXShippingLimit] PRIMARY KEY CLUSTERED ([IdUser] ASC, [IdSubscribersList] ASC)
)
GO

CREATE TABLE [dbo].[User](
	[IdUser] [int] IDENTITY(50000,1) NOT NULL,
	[Email] [varchar](550) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([IdUser] ASC)
)
GO

CREATE TABLE [dbo].[UserShippingLimit](
	[IdUser] [int] NOT NULL,
	[Enabled] [bit] NULL,
	[Active] [bit] NOT NULL,
	[Amount] [int] NULL,
	[Interval] [int] NULL,
 CONSTRAINT [PK_UserShippingLimit] PRIMARY KEY CLUSTERED ([IdUser] ASC)
)
GO

SET IDENTITY_INSERT [dbo].[User] ON
INSERT [dbo].[User] ([IdUser], [Email]) VALUES (50001, N'prueba@makingsense.com')
SET IDENTITY_INSERT [dbo].[User] OFF

SET IDENTITY_INSERT [dbo].[SubscribersList] ON
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (500000, 50001, NULL, 1, N'Test', 1, CAST(N'2015-07-02T17:12:31.187' AS DateTime), CAST(N'2015-08-03T18:42:08.773' AS DateTime), NULL, NULL, 1, NULL, CAST(N'2015-08-03T18:42:33.423' AS DateTime), 0, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (545067, 50001, NULL, 1, N'AUTOMATION_SEGMENT_959b738b-9ae1-49de-82fe-c223c0e54e9a', 0, CAST(N'2016-09-08T03:03:08.787' AS DateTime), CAST(N'2016-05-17T19:50:20.800' AS DateTime), NULL, NULL, 1, NULL, CAST(N'2016-09-08T03:04:18.747' AS DateTime), 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (575593, 50001, NULL, 1, N'easla nueva', 1, CAST(N'2016-08-24T18:56:13.297' AS DateTime), CAST(N'2018-01-15T12:04:23.807' AS DateTime), NULL, NULL, 1, NULL, CAST(N'2018-07-25T17:13:11.800' AS DateTime), 0, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (583041, 50001, NULL, 1, N'INTERNAL 3673 A', 1, CAST(N'2016-09-08T18:41:49.953' AS DateTime), NULL, NULL, NULL, 0, NULL, CAST(N'2016-09-08T18:41:59.857' AS DateTime), 0, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (583042, 50001, NULL, 1, N'INTERNAL 3673 B', 1, CAST(N'2016-09-08T18:41:50.407' AS DateTime), NULL, NULL, NULL, 0, NULL, CAST(N'2016-09-08T18:42:12.673' AS DateTime), 0, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (583043, 50001, NULL, 1, N'INTERNAL 3673 Result', 1, CAST(N'2016-09-08T18:41:50.467' AS DateTime), NULL, NULL, NULL, 0, NULL, NULL, 0, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (623034, 50001, NULL, 1, N'AUTOMATION_SEGMENT_da4ddda5-9b7f-48f5-9b87-3cb8819e33ab', 0, CAST(N'2016-11-04T12:19:05.333' AS DateTime), CAST(N'2016-11-04T12:21:23.700' AS DateTime), NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (643620, 50001, NULL, 1, N'AUTOMATION_SEGMENT_09a9319e-f606-49f8-a8d2-505810eadd90', 0, CAST(N'2016-12-02T18:16:55.060' AS DateTime), CAST(N'2016-11-22T17:26:40.507' AS DateTime), NULL, NULL, 1, NULL, CAST(N'2016-12-02T18:17:25.957' AS DateTime), 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (712260, 50001, NULL, 1, N'7865678tyg', 1, CAST(N'2017-01-30T11:11:42.387' AS DateTime), CAST(N'2017-04-06T13:28:19.920' AS DateTime), NULL, NULL, 1, NULL, CAST(N'2017-04-06T13:29:23.580' AS DateTime), 0, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (712262, 50001, NULL, 1, N'asd', 1, CAST(N'2017-01-30T11:15:27.487' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 0, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (718978, 50001, NULL, 1, N'Tienda Nube', 1, CAST(N'2017-02-06T16:55:57.370' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 0, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (821228, 50001, NULL, 1, N'AUTOMATION_SEGMENT_sssss', 0, CAST(N'2017-07-07T17:20:11.137' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (821229, 50001, NULL, 1, N'AUTOMATION_SEGMENT_sasdasd', 0, CAST(N'2017-07-07T17:20:11.177' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (821230, 50001, NULL, 1, N'AUTOMATION_SEGMENT_asdasdasd', 0, CAST(N'2017-07-09T19:17:27.017' AS DateTime), NULL, NULL, NULL, 1, NULL, CAST(N'2017-07-09T19:18:02.770' AS DateTime), 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (821231, 50001, NULL, 1, N'AUTOMATION_SEGMENT_a', 0, CAST(N'2017-07-07T17:20:11.320' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (823111, 50001, NULL, 1, N'AUTOMATION_SEGMENT_dd6cbdac-e06c-4d21-92ad-2af857ae2c3b', 0, CAST(N'2017-07-10T12:01:50.470' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (823115, 50001, NULL, 1, N'AUTOMATION_SEGMENT_16cd0021-3362-46f4-a476-9f7119c1346e', 0, CAST(N'2017-07-10T15:32:12.587' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (823178, 50001, NULL, 1, N'AUTOMATION_SEGMENT_9c6c60d6-f214-48c8-83cd-7f2e0cf8041b', 0, CAST(N'2017-07-11T18:44:07.337' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (823179, 50001, NULL, 1, N'AUTOMATION_SEGMENT_9a244cb2-563d-47d7-b813-94901ccbc4a1', 0, CAST(N'2017-07-11T18:52:11.333' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (824471, 50001, NULL, 1, N'AUTOMATION_SEGMENT_4ae32b5e-51bd-4a7e-a4f7-91dd638d41cb', 0, CAST(N'2017-07-18T11:51:22.183' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (830408, 50001, NULL, 1, N'Testa', 1, CAST(N'2017-08-25T10:32:40.310' AS DateTime), CAST(N'2020-08-24T19:11:50.250' AS DateTime), NULL, NULL, 1, NULL, CAST(N'2020-08-24T19:32:06.180' AS DateTime), 0, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[SubscribersList] ([IdSubscribersList], [IdUser], [IdLabel], [SubscribersListStatus], [Name], [Active], [UTCCreationDate], [UTCLastUseDate], [Ranking], [APIKey], [Visible], [UTCDeleteDate], [UTCLastSentDate], [IsDuplicated], [UpdatedAt], [BlackListCount], [HasToValidate], [IdLastImportRequest]) VALUES (831465, 50001, NULL, 1, N'AUTOMATION_SEGMENT_6842849d-e9fd-4d6f-8359-61c2d2fdf1c1', 0, CAST(N'2017-08-30T19:13:52.847' AS DateTime), NULL, NULL, NULL, 1, NULL, NULL, 1, CAST(N'2020-11-15T23:14:32.0000000' AS DateTime2), 0, 0, NULL)
SET IDENTITY_INSERT [dbo].[SubscribersList] OFF

INSERT [dbo].[UserShippingLimit] ([IdUser], [Enabled], [Active], [Amount], [Interval]) VALUES (50001, 1, 1, 1, 1)

INSERT [dbo].[SubscribersListXShippingLimit] ([IdUser], [IdSubscribersList], [Active]) VALUES (50001, 500000, 1)


CREATE NONCLUSTERED INDEX [IX_SubscribersList_HasToValidate] ON [dbo].[SubscribersList]
(
	[HasToValidate] ASC
)
WHERE ([HasToValidate]=(1))
GO

CREATE NONCLUSTERED INDEX [IX_SubscribersList_IdLastImportRequest] ON [dbo].[SubscribersList]
(
	[IdLastImportRequest] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_SubscribersList_IdUser] ON [dbo].[SubscribersList]
(
	[IdUser] ASC,
	[Active] ASC,
	[Visible] ASC
)
INCLUDE([IdLabel])
GO

CREATE NONCLUSTERED INDEX [IX_SubscribersList_IdUser_UpdateAt] ON [dbo].[SubscribersList]
(
	[IdUser] ASC,
	[UpdatedAt] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_SubscribersListXShippingLimit_IdSubscribersList] ON [dbo].[SubscribersListXShippingLimit]
(
	[IdSubscribersList] ASC
)
INCLUDE([Active])
GO


CREATE NONCLUSTERED INDEX [IX_UserAccountCancellationRequest_IdUser] ON [dbo].[User]
(
	[IdUser] ASC
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [PK_Email] ON [dbo].[User]
(
	[Email] ASC
)
GO

ALTER TABLE [dbo].[SubscribersList] ADD  CONSTRAINT [DF_SubscribersList_SubscribersListStatus]  DEFAULT ('1') FOR [SubscribersListStatus]
GO
ALTER TABLE [dbo].[SubscribersList] ADD  CONSTRAINT [DF_SubscribersList_Visible]  DEFAULT ((1)) FOR [Visible]
GO
ALTER TABLE [dbo].[SubscribersList] ADD  DEFAULT ((0)) FOR [IsDuplicated]
GO
ALTER TABLE [dbo].[SubscribersList] ADD  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[SubscribersList] ADD  CONSTRAINT [DF_SubscribersList_BlackListCount]  DEFAULT ((0)) FOR [BlackListCount]
GO
ALTER TABLE [dbo].[SubscribersList] ADD  CONSTRAINT [DF_SubscribersList_HasToValidate]  DEFAULT ((0)) FOR [HasToValidate]
GO
ALTER TABLE [dbo].[UserShippingLimit] ADD  CONSTRAINT [DF_UserShippingLimit_Active]  DEFAULT ((0)) FOR [Active]
GO

ALTER TABLE [dbo].[SubscribersListXShippingLimit]  WITH CHECK ADD  CONSTRAINT [FK_SubscribersListXShippingLimit_ShippingLimit] FOREIGN KEY([IdUser])
REFERENCES [dbo].[UserShippingLimit] ([IdUser])
GO
ALTER TABLE [dbo].[SubscribersListXShippingLimit] CHECK CONSTRAINT [FK_SubscribersListXShippingLimit_ShippingLimit]
GO
ALTER TABLE [dbo].[UserShippingLimit]  WITH CHECK ADD  CONSTRAINT [FK_UserShippingLimit_User] FOREIGN KEY([IdUser])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[UserShippingLimit] CHECK CONSTRAINT [FK_UserShippingLimit_User]
GO
ALTER TABLE [dbo].[SubscribersList]  WITH CHECK ADD  CONSTRAINT [CK_SubscribersList_BlackListCount_EqualGreaterThanZero] CHECK  (([BlackListCount]>=(0)))
GO
ALTER TABLE [dbo].[SubscribersList] CHECK CONSTRAINT [CK_SubscribersList_BlackListCount_EqualGreaterThanZero]
GO

ALTER TABLE [dbo].[SubscribersList]  WITH CHECK ADD  CONSTRAINT [FK_SubscribersList_User] FOREIGN KEY([IdUser])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[SubscribersList] CHECK CONSTRAINT [FK_SubscribersList_User]
GO
ALTER TABLE [dbo].[SubscribersListXShippingLimit]  WITH CHECK ADD  CONSTRAINT [FK_SubscribersListXShippingLimit_SubscribersList] FOREIGN KEY([IdSubscribersList])
REFERENCES [dbo].[SubscribersList] ([IdSubscribersList])
GO
ALTER TABLE [dbo].[SubscribersListXShippingLimit] CHECK CONSTRAINT [FK_SubscribersListXShippingLimit_SubscribersList]
GO
ALTER TABLE [dbo].[SubscribersListXShippingLimit]  WITH CHECK ADD  CONSTRAINT [FK_SubscribersListXShippingLimit_User] FOREIGN KEY([IdUser])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[SubscribersListXShippingLimit] CHECK CONSTRAINT [FK_SubscribersListXShippingLimit_User]
GO
