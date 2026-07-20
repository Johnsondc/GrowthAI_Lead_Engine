create table Tenant (
  Id uniqueidentifier primary key,
  Name nvarchar(100) not null,
  Industry nvarchar(50) null,
  City nvarchar(50) null,
  ContactName nvarchar(50) null,
  ContactPhone nvarchar(30) null,
  CreatedAt datetime2 not null default sysutcdatetime(),
  UpdatedAt datetime2 not null default sysutcdatetime()
);

go

create table AppUser (
  Id uniqueidentifier primary key,
  TenantId uniqueidentifier not null,
  Name nvarchar(50) not null,
  Phone nvarchar(30) not null,
  Role nvarchar(30) not null,
  IsActive bit not null default 1,
  CreatedAt datetime2 not null default sysutcdatetime(),
  UpdatedAt datetime2 not null default sysutcdatetime(),
  constraint FK_AppUser_Tenant foreign key (TenantId) references Tenant(Id)
);

go

create table LeadSource (
  Id uniqueidentifier primary key,
  TenantId uniqueidentifier not null,
  Platform nvarchar(50) not null,
  AccountName nvarchar(100) null,
  SourceCode nvarchar(80) not null,
  LandingUrl nvarchar(500) null,
  CreatedAt datetime2 not null default sysutcdatetime(),
  UpdatedAt datetime2 not null default sysutcdatetime(),
  constraint FK_LeadSource_Tenant foreign key (TenantId) references Tenant(Id)
);

go

create table LeadCustomer (
  Id uniqueidentifier primary key,
  TenantId uniqueidentifier not null,
  SourceId uniqueidentifier null,
  OwnerUserId uniqueidentifier null,
  Name nvarchar(50) not null,
  Phone nvarchar(30) null,
  Wechat nvarchar(80) null,
  City nvarchar(50) null,
  SourcePlatform nvarchar(50) not null,
  SourceAccount nvarchar(100) null,
  ConsultationContent nvarchar(1000) null,
  InterestedProduct nvarchar(100) null,
  Status nvarchar(30) not null,
  Remark nvarchar(1000) null,
  CreatedAt datetime2 not null default sysutcdatetime(),
  UpdatedAt datetime2 not null default sysutcdatetime(),
  constraint FK_LeadCustomer_Tenant foreign key (TenantId) references Tenant(Id),
  constraint FK_LeadCustomer_Source foreign key (SourceId) references LeadSource(Id),
  constraint FK_LeadCustomer_Owner foreign key (OwnerUserId) references AppUser(Id)
);

go

create table AiTask (
  Id uniqueidentifier primary key,
  TenantId uniqueidentifier not null,
  Provider nvarchar(50) not null,
  TaskType nvarchar(50) not null,
  InputJson nvarchar(max) not null,
  Status nvarchar(30) not null,
  ErrorMessage nvarchar(1000) null,
  CreatedAt datetime2 not null default sysutcdatetime(),
  UpdatedAt datetime2 not null default sysutcdatetime(),
  constraint FK_AiTask_Tenant foreign key (TenantId) references Tenant(Id)
);

go

create table AiContent (
  Id uniqueidentifier primary key,
  TenantId uniqueidentifier not null,
  AiTaskId uniqueidentifier null,
  Platform nvarchar(50) not null,
  Industry nvarchar(50) null,
  ProductName nvarchar(100) null,
  City nvarchar(50) null,
  TargetAudience nvarchar(200) null,
  Title nvarchar(200) null,
  Body nvarchar(max) null,
  Script nvarchar(max) null,
  Tags nvarchar(500) null,
  CallToAction nvarchar(500) null,
  CreatedAt datetime2 not null default sysutcdatetime(),
  UpdatedAt datetime2 not null default sysutcdatetime(),
  constraint FK_AiContent_Tenant foreign key (TenantId) references Tenant(Id),
  constraint FK_AiContent_AiTask foreign key (AiTaskId) references AiTask(Id)
);

go

create index IX_LeadCustomer_Tenant_Status_CreatedAt on LeadCustomer(TenantId, Status, CreatedAt desc);
create index IX_LeadCustomer_Tenant_Platform_CreatedAt on LeadCustomer(TenantId, SourcePlatform, CreatedAt desc);
create index IX_LeadSource_Tenant_Platform on LeadSource(TenantId, Platform);
