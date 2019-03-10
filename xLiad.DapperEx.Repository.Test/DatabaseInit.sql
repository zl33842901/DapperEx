-- Table  DictInfo
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DictInfo](
	[DictID] [int] IDENTITY(1,1) NOT NULL,
	[DictName] [varchar](100) NOT NULL,
	[DictType] [int] NULL,
	[Remark] [varchar](100) NULL,
	[CreateUserID] [int] NULL,
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF__DictInfo__Create__503BEA1C]  DEFAULT (getdate()),
	[OrderNum] [int] NULL,
	[Status] [int] NULL,
	[IsUserConfig] [int] NULL,
	[Deleted] [bit] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[DictInfo] ON 

INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (100018, N'创意长', 100, N'修改个备注', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (100019, N'技术副总裁', 100, N'修改个备注', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (100020, N'哈哈哈', 100, N'嘿嘿嘿', 0, CAST(N'2016-06-03 16:20:46.253' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (102098, N'哈哈哈', 102, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (102099, N'哈哈哈', 102, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (102100, N'哈哈哈', 102, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (102101, N'哈哈哈', 102, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (102102, N'哈哈哈', 102, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (102103, N'哈哈哈', 102, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (102104, N'哈哈哈', 102, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 1)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (104066, N'哈哈哈', 104, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (104067, N'哈哈哈', 104, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (104068, N'哈哈哈', 104, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (104069, N'哈哈哈', 104, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106069, N'哈哈哈', 106, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106070, N'哈哈哈', 106, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 1)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106071, N'哈哈哈', 106, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 2, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106072, N'哈哈哈', 106, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106073, N'哈哈哈', 106, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106074, N'哈哈哈', 106, N'嘿嘿嘿', 0, CAST(N'2017-05-02 00:00:00.000' AS DateTime), 1, 0, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106075, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-10-30 11:05:37.990' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106076, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-10-30 11:09:49.657' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106082, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-10-31 10:29:21.547' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106083, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-10-31 10:30:21.993' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106078, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-10-30 16:07:41.113' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106080, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-10-30 16:37:11.970' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106081, N'老总', 3, N'老总，请加薪', NULL, CAST(N'2018-12-06 16:23:29.693' AS DateTime), 2, NULL, NULL, 1)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106079, N'老总秘书', 2, N'老总秘书，请加薪', NULL, CAST(N'2018-12-06 16:23:29.693' AS DateTime), 1, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (1234, N'asdf', NULL, N'修改个备注', NULL, CAST(N'2018-10-31 10:31:07.623' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106084, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-10-31 13:16:09.083' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106086, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-11-27 22:52:29.607' AS DateTime), 0, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106087, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-11-28 22:29:29.770' AS DateTime), 0, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106088, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-11-28 22:35:28.633' AS DateTime), 0, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106089, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-11-29 14:05:51.390' AS DateTime), 0, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106090, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-11-29 14:05:52.133' AS DateTime), 0, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106091, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-11-29 14:05:52.133' AS DateTime), 0, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106092, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-11-29 14:06:36.230' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106093, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-11-29 14:06:36.780' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106094, N'哈哈哈', NULL, N'嘿嘿嘿', NULL, CAST(N'2018-11-29 14:06:36.780' AS DateTime), 2, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106095, N'康师傅1', NULL, NULL, NULL, CAST(N'2018-12-06 16:03:34.803' AS DateTime), 0, NULL, NULL, 0)
INSERT [dbo].[DictInfo] ([DictID], [DictName], [DictType], [Remark], [CreateUserID], [CreateTime], [OrderNum], [Status], [IsUserConfig], [Deleted]) VALUES (106096, N'康师傅2', NULL, NULL, NULL, CAST(N'2018-12-06 16:03:34.803' AS DateTime), 0, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[DictInfo] OFF
/****** Object:  Index [PK_DictInfo_1]    Script Date: 2018/12/8 20:01:21 ******/
ALTER TABLE [dbo].[DictInfo] ADD  CONSTRAINT [PK_DictInfo_1] PRIMARY KEY NONCLUSTERED 
(
	[DictID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
--------------------------------------------------------------------------------------------------
------------------------- Table TestStamp
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TestStamp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_TestStamp_CreateTime]  DEFAULT (getdate()),
	[ROWVERSION] [timestamp] NOT NULL,
 CONSTRAINT [PK_TestStamp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
-----------------------------------------------------------