##### `DocumentAttachments`

Source: `WinApp/sqlData.sql:11884-11900`

```sql
CREATE TABLE [dbo].[DocumentAttachments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdDocument] [bigint] NULL,
	[IdAttachmentType] [int] NOT NULL,
	[AttachmentStatus] [nvarchar](50) NOT NULL,
	[AttachmentNotes] [ntext] NULL,
	[AttachmentPath] [nvarchar](550) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentAttachments_Active]  DEFAULT ((1)),
	[IdCustomer] [bigint] NULL,
	[IdVehicle] [bigint] NULL,
 CONSTRAINT [PK_DocumentAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

