create table loan_collectors(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[loan_id] [decimal](18,0) NULL,
	[assigned_on] [datetime] NULL,
	[collector] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]