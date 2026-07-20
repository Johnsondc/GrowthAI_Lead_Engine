create table Tenant (
  Id char(36) primary key,
  Name varchar(100) not null,
  Industry varchar(50) null,
  City varchar(50) null,
  ContactName varchar(50) null,
  ContactPhone varchar(30) null,
  CreatedAt datetime(6) not null default current_timestamp(6),
  UpdatedAt datetime(6) not null default current_timestamp(6) on update current_timestamp(6)
) engine=InnoDB default charset=utf8mb4 collate=utf8mb4_unicode_ci;

create table AppUser (
  Id char(36) primary key,
  TenantId char(36) not null,
  Name varchar(50) not null,
  Phone varchar(30) not null,
  Role varchar(30) not null,
  IsActive boolean not null default true,
  CreatedAt datetime(6) not null default current_timestamp(6),
  UpdatedAt datetime(6) not null default current_timestamp(6) on update current_timestamp(6),
  constraint FK_AppUser_Tenant foreign key (TenantId) references Tenant(Id)
) engine=InnoDB default charset=utf8mb4 collate=utf8mb4_unicode_ci;

create table LeadSource (
  Id char(36) primary key,
  TenantId char(36) not null,
  Platform varchar(50) not null,
  AccountName varchar(100) null,
  SourceCode varchar(80) not null,
  LandingUrl varchar(500) null,
  CreatedAt datetime(6) not null default current_timestamp(6),
  UpdatedAt datetime(6) not null default current_timestamp(6) on update current_timestamp(6),
  constraint FK_LeadSource_Tenant foreign key (TenantId) references Tenant(Id),
  unique key UX_LeadSource_Tenant_SourceCode (TenantId, SourceCode)
) engine=InnoDB default charset=utf8mb4 collate=utf8mb4_unicode_ci;

create table LeadCustomer (
  Id char(36) primary key,
  TenantId char(36) not null,
  SourceId char(36) null,
  OwnerUserId char(36) null,
  Name varchar(50) not null,
  Phone varchar(30) null,
  Wechat varchar(80) null,
  City varchar(50) null,
  SourcePlatform varchar(50) not null,
  SourceAccount varchar(100) null,
  ConsultationContent varchar(1000) null,
  InterestedProduct varchar(100) null,
  Status varchar(30) not null,
  Remark varchar(1000) null,
  CreatedAt datetime(6) not null default current_timestamp(6),
  UpdatedAt datetime(6) not null default current_timestamp(6) on update current_timestamp(6),
  constraint FK_LeadCustomer_Tenant foreign key (TenantId) references Tenant(Id),
  constraint FK_LeadCustomer_Source foreign key (SourceId) references LeadSource(Id),
  constraint FK_LeadCustomer_Owner foreign key (OwnerUserId) references AppUser(Id)
) engine=InnoDB default charset=utf8mb4 collate=utf8mb4_unicode_ci;

create table AiTask (
  Id char(36) primary key,
  TenantId char(36) not null,
  Provider varchar(50) not null,
  TaskType varchar(50) not null,
  InputJson json not null,
  Status varchar(30) not null,
  ErrorMessage varchar(1000) null,
  CreatedAt datetime(6) not null default current_timestamp(6),
  UpdatedAt datetime(6) not null default current_timestamp(6) on update current_timestamp(6),
  constraint FK_AiTask_Tenant foreign key (TenantId) references Tenant(Id)
) engine=InnoDB default charset=utf8mb4 collate=utf8mb4_unicode_ci;

create table AiContent (
  Id char(36) primary key,
  TenantId char(36) not null,
  AiTaskId char(36) null,
  Platform varchar(50) not null,
  Industry varchar(50) null,
  ProductName varchar(100) null,
  City varchar(50) null,
  TargetAudience varchar(200) null,
  Title varchar(200) null,
  Body text null,
  Script text null,
  Tags varchar(500) null,
  CallToAction varchar(500) null,
  CreatedAt datetime(6) not null default current_timestamp(6),
  UpdatedAt datetime(6) not null default current_timestamp(6) on update current_timestamp(6),
  constraint FK_AiContent_Tenant foreign key (TenantId) references Tenant(Id),
  constraint FK_AiContent_AiTask foreign key (AiTaskId) references AiTask(Id)
) engine=InnoDB default charset=utf8mb4 collate=utf8mb4_unicode_ci;

create index IX_LeadCustomer_Tenant_Status_CreatedAt on LeadCustomer(TenantId, Status, CreatedAt);
create index IX_LeadCustomer_Tenant_Platform_CreatedAt on LeadCustomer(TenantId, SourcePlatform, CreatedAt);
create index IX_LeadSource_Tenant_Platform on LeadSource(TenantId, Platform);
