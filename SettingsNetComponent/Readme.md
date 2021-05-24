Компонент, позволяющий централизованно хранить настройки различных приложений


// **************************************************************

Для работы данного компонента требуется создать БД SettingsDb
В созданной БД создать таблицу AppSettings 

// **************************************************************
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AppSettings](
	[Id] [uniqueidentifier] NOT NULL,
	[JsonData] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


// **************************************************************

Запустить приложение SettingsStorage.exe и нажать кнопку "Start"

// **************************************************************