USE [OrderManagement.OrderHistory]
GO

/****** Object:  Table [dbo].[EventStream]    Script Date: 02/03/2016 11:35:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EventStream](
	[OrderId] [varchar](50) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Occurred] [datetime] NOT NULL,
	[Data] [text] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


