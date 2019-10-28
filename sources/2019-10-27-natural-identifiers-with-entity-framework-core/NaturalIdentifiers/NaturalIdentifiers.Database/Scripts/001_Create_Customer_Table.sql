IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'dbo' AND TABLE_NAME = N'Customer')
BEGIN
	CREATE TABLE [dbo].[Customer] (
		[Id] [uniqueidentifier] NOT NULL,
		[FirstName] [nvarchar](max) NOT NULL,
		[LastName] [nvarchar](max) NOT NULL,
		[Address_Street] [nvarchar](max) NULL,
		[Address_PostalCode] [nvarchar](max) NULL,
		[Address_City] [nvarchar](max) NULL,
	 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END