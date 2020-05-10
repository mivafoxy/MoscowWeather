USE [*БД*]
GO

/****** Object:  Table [dbo].[weather]    Script Date: 10.05.2020 22:33:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[weather](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NOT NULL,
	[temperature] [numeric](8, 2) NULL,
	[humidity] [numeric](8, 2) NULL,
	[td] [numeric](8, 2) NULL,
	[atmospheric_pressure] [numeric](8, 2) NULL,
	[wind_direction] [nchar](150) NULL,
	[wind_speed] [numeric](8, 2) NULL,
	[cloudiness] [numeric](8, 2) NULL,
	[h] [numeric](8, 2) NULL,
	[vv] [nchar](150) NULL,
	[weather_condition] [nchar](150) NULL,
	[time] [time](7) NOT NULL,
 CONSTRAINT [PK_weather] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


