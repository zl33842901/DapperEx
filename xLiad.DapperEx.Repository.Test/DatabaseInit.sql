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



/****** Object:  Table [dbo].[Articles]    Script Date: 2019/3/11 15:00:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NULL,
	[DictID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_Articles_CreateTime]  DEFAULT (getdate()),
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_Articles_Deleted]  DEFAULT ((0)),
 CONSTRAINT [PK_Articles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Articles] ON 

GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (3, N'个人资料类别', 100018, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (6, N'审批链单据人员替换类型', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (7, N'审批链单据人员替换类型', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (8, N'审批链适用单据分类', 100018, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (9, N'费用报销单据状态', 100018, CAST(N'2018-12-07 17:00:50.783' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (10, N'费用报销单据类型', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (11, N'OA工单流程状态', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (12, N'费用报销发票类型', 100018, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (13, N'费用报销事由', 100018, CAST(N'2018-12-07 17:00:50.783' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (14, N'费用报销借款费用项子类', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (15, N'费用报销相关配置', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (16, N'差旅申请单状态', 100018, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (17, N'差旅申请单', 100018, CAST(N'2018-12-07 17:00:50.783' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (18, N'身份证复印件', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (19, N'学历复印件', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (20, N'公章申请-个人相关', 100018, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (21, N'在途单据替换审批人', 100018, CAST(N'2018-12-07 17:00:50.783' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (22, N'公章申请-收入相关', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (23, N'公章申请-采购相关1', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (24, N'公章申请-采购相关2', 100018, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (25, N'公章申请-采购相关3', 100018, CAST(N'2018-12-07 17:00:50.783' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (26, N'公章申请-人事相关(人事组)', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (27, N'【增值税电子】普通发票', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (28, N'【增值税专用】发票', 100018, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (29, N'增值税普通发票及其他发票', 100018, CAST(N'2018-12-07 17:00:50.783' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (30, N'投标相关资料', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (31, N'结案相关资料', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (32, N'公司相关资质证明', 100018, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 1)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (33, N'付款发票相关证明资料', 100018, CAST(N'2018-12-07 17:00:50.783' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (34, N'合同相关资料', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (35, N'验收相关资料', 100019, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (36, N'其它相关资料', 100018, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (38, N'www.yiche.com', 100020, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 1)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (39, N'报销冲借款', 100020, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 0)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (40, N'现金还借款', 100020, CAST(N'2018-12-07 17:00:16.350' AS DateTime), 1)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (41, N'客户活动差旅', 100020, CAST(N'2018-12-07 17:00:50.783' AS DateTime), 1)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (42, N'客户活动其他', 100020, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 1)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (43, N'驳回驳回驳回驳回驳回', 1234, CAST(N'2018-12-07 17:01:08.973' AS DateTime), 1)
GO
INSERT [dbo].[Articles] ([Id], [Title], [DictID], [CreateTime], [Deleted]) VALUES (48, N'111111', 100018, CAST(N'2018-12-10 19:24:48.483' AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[Articles] OFF
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Author](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Author] ON 

GO
INSERT [dbo].[Author] ([Id], [AId], [Name]) VALUES (1, 3, N'aid为3的')
GO
INSERT [dbo].[Author] ([Id], [AId], [Name]) VALUES (2, 6, N'aid为6的')
GO
INSERT [dbo].[Author] ([Id], [AId], [Name]) VALUES (3, 39, N'aid为39的')
GO
INSERT [dbo].[Author] ([Id], [AId], [Name]) VALUES (4, 43, N'aid为43的')
GO
SET IDENTITY_INSERT [dbo].[Author] OFF
GO
