/****** Object:  Table [dbo].[ShortLink]    Script Date: 2020/10/31 1:07:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ShortLink](
	[ID] [int] IDENTITY(1,1) NOT NULL, --Key
	[Title] [nvarchar](500) NULL, -- Custom title for short link
	[Token] [nvarchar](50) NOT NULL, -- Short link name e.g. AbCdE will be peresent in site as http://mydomain.com/AbCdE
	[IsPublish] [bit] NOT NULL, -- If is false it won't be work and will be redirected to 404 error
	[OriginLink] [nvarchar](max) NOT NULL, -- Original link 
	[EditAdminID] [int] NULL, -- last editor of the file ? <for your management if you want>
	[EditAdminDate] [smalldatetime] NULL, --last modifiy date of the file ? <for your management if you want>
	[CreateAdminID] [int] NOT NULL, -- who created the file ? <for your management if you want>
	[CreateAdminDate] [smalldatetime] NOT NULL,--created date of the file ? <for your management if you want>
 CONSTRAINT [PK_ShortLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ShortLink] UNIQUE NONCLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [LogData] TEXTIMAGE_ON [LogData]
GO

ALTER TABLE [dbo].[ShortLink] ADD  CONSTRAINT [DF_ShortLink_IsPublish]  DEFAULT ((0)) FOR [IsPublish]
GO

ALTER TABLE [dbo].[ShortLink] ADD  CONSTRAINT [DF_ShortLink_CreateAdminDate]  DEFAULT (getdate()) FOR [CreateAdminDate]
GO

/****** Object:  Table [dbo].[ShortLinkDetail]    Script Date: 2020/11/01 9:25:36 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ShortLinkDetail](
	[ID] [bigint] IDENTITY(1,1) NOT NULL, -- Key
	[ShortLinkID] [int] NULL, -- foreign key
	[VisitDate] [datetime] NOT NULL,-- click date
	[Country] [nvarchar](50) NULL, -- country of clicker
	[OS] [nvarchar](50) NULL, -- Operating system of clicker
	[Browser] [nvarchar](50) NULL, -- browser name of clicker
 CONSTRAINT [PK_ShortLinkDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShortLinkDetail] ADD  CONSTRAINT [DF_ShortLinkDetail_VisitDate]  DEFAULT (getdate()) FOR [VisitDate]
GO



