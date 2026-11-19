SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
GO

IF DB_ID(N'masar_db') IS NULL
    CREATE DATABASE masar_db;
GO
USE masar_db;
GO

CREATE TABLE dbo.Users (
    User_ID    INT IDENTITY(1,1) NOT NULL,
    Full_Name  NVARCHAR(100)     NOT NULL,
    Email      VARCHAR(150)      NOT NULL,
    Password   VARCHAR(255)      NOT NULL,
    Phone      VARCHAR(13)       NOT NULL,
    Role       VARCHAR(10)       NOT NULL,
    Status     VARCHAR(10)       NOT NULL CONSTRAINT DF_Users_Status DEFAULT ('Active'),
    City       NVARCHAR(50)      NULL,
    Photo_URL  NVARCHAR(500)     NULL,
    CONSTRAINT PK_Users PRIMARY KEY (User_ID),
    CONSTRAINT UQ_Users_Email UNIQUE (Email),
    CONSTRAINT UQ_Users_Phone UNIQUE (Phone),
    CONSTRAINT CK_Users_Phone CHECK (Phone LIKE '+9665[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT CK_Users_Role CHECK (Role IN ('Client', 'Expert', 'Admin')),
    CONSTRAINT CK_Users_Status CHECK (Status IN ('Active', 'Pending', 'Blocked'))
);
GO

CREATE TABLE dbo.Admin (
    Admin_ID    INT IDENTITY(1,1) NOT NULL,
    User_ID     INT               NOT NULL,
    Permission  VARCHAR(20)       NOT NULL,
    CONSTRAINT PK_Admin PRIMARY KEY (Admin_ID),
    CONSTRAINT UQ_Admin_User UNIQUE (User_ID),
    CONSTRAINT CK_Admin_Permission CHECK (Permission IN ('Super admin', 'Verification', 'Finance', 'Support')),
    CONSTRAINT FK_Admin_Users FOREIGN KEY (User_ID) REFERENCES dbo.Users (User_ID)
);
GO

CREATE TABLE dbo.Client (
    Client_ID  INT IDENTITY(1,1) NOT NULL,
    User_ID    INT               NOT NULL,
    Budget     VARCHAR(30)       NULL,
    Interests  NVARCHAR(200)     NULL,
    CONSTRAINT PK_Client PRIMARY KEY (Client_ID),
    CONSTRAINT UQ_Client_User UNIQUE (User_ID),
    CONSTRAINT FK_Client_Users FOREIGN KEY (User_ID) REFERENCES dbo.Users (User_ID)
);
GO

CREATE TABLE dbo.Expert (
    Expert_ID         INT IDENTITY(1,1) NOT NULL,
    User_ID           INT               NOT NULL,
    Specialization    VARCHAR(30)       NOT NULL,
    Experience_Years  INT               NOT NULL,
    Bio               NVARCHAR(1000)    NOT NULL,
    Rating            DECIMAL(2,1)      NULL,
    Session_Price     DECIMAL(8,2)      NOT NULL,
    Portfolio_Link    VARCHAR(200)      NULL,
    CONSTRAINT PK_Expert PRIMARY KEY (Expert_ID),
    CONSTRAINT UQ_Expert_User UNIQUE (User_ID),
    CONSTRAINT CK_Expert_Spec CHECK (Specialization IN ('Business Consulting', 'Financial Guidance', 'Legal Consulting',
                                                       'Branding', 'Packaging', 'Interior Design')),
    CONSTRAINT CK_Expert_Years CHECK (Experience_Years BETWEEN 0 AND 50),
    CONSTRAINT CK_Expert_Rating CHECK (Rating BETWEEN 1 AND 5),
    CONSTRAINT CK_Expert_Price CHECK (Session_Price BETWEEN 100 AND 20000),
    CONSTRAINT FK_Expert_Users FOREIGN KEY (User_ID) REFERENCES dbo.Users (User_ID)
);
GO

CREATE TABLE dbo.Verification_Document (
    Document_ID     INT IDENTITY(1,1) NOT NULL,
    Expert_ID       INT               NOT NULL,
    Admin_ID        INT               NULL,
    Document_Type   VARCHAR(30)       NOT NULL,
    Status          VARCHAR(15)       NOT NULL CONSTRAINT DF_VerDoc_Status DEFAULT ('Uploaded'),
    Submitted_Date  DATETIME2(0)      NOT NULL CONSTRAINT DF_VerDoc_Date DEFAULT (SYSDATETIME()),
    File_Path       NVARCHAR(300)     NOT NULL,
    CONSTRAINT PK_Verification_Document PRIMARY KEY (Document_ID),
    CONSTRAINT UQ_VerDoc_Type UNIQUE (Expert_ID, Document_Type),
    CONSTRAINT CK_VerDoc_Type CHECK (Document_Type IN ('Professional certificate', 'Commercial registration', 'Professional licence', 'Portfolio of previous work')),
    CONSTRAINT CK_VerDoc_Status CHECK (Status IN ('Uploaded', 'In review', 'Approved', 'Rejected')),
    CONSTRAINT CK_VerDoc_Admin CHECK (Status = 'Uploaded' OR Admin_ID IS NOT NULL),
    CONSTRAINT FK_VerDoc_Expert FOREIGN KEY (Expert_ID) REFERENCES dbo.Expert (Expert_ID),
    CONSTRAINT FK_VerDoc_Admin FOREIGN KEY (Admin_ID) REFERENCES dbo.Admin (Admin_ID)
);
GO

CREATE TABLE dbo.Portfolio (
    Portfolio_ID  INT IDENTITY(1,1) NOT NULL,
    Expert_ID     INT               NOT NULL,
    Title         NVARCHAR(100)     NOT NULL,
    description   NVARCHAR(500)     NULL,
    Image_URL     NVARCHAR(500)     NOT NULL,
    CONSTRAINT PK_Portfolio PRIMARY KEY (Portfolio_ID),
    CONSTRAINT FK_Portfolio_Expert FOREIGN KEY (Expert_ID) REFERENCES dbo.Expert (Expert_ID)
);
GO

CREATE TABLE dbo.Consultation_Type (
    Consultation_Type_ID  INT IDENTITY(1,1) NOT NULL,
    Type_Name             NVARCHAR(60)      NOT NULL,
    Description           NVARCHAR(300)     NULL,
    Base_Price            DECIMAL(8,2)      NOT NULL,
    CONSTRAINT PK_Consultation_Type PRIMARY KEY (Consultation_Type_ID),
    CONSTRAINT UQ_ConsType_Name UNIQUE (Type_Name),
    CONSTRAINT CK_ConsType_Price CHECK (Base_Price > 0)
);
GO

CREATE TABLE dbo.Consultation_Session (
    Consultation_Session_ID  INT IDENTITY(1,1) NOT NULL,
    Consultation_Type_ID     INT               NOT NULL,
    Expert_ID                INT               NOT NULL,
    Client_ID                INT               NULL,
    Price                    DECIMAL(8,2)      NOT NULL,
    Session_Date             DATE              NOT NULL,
    Start_Time               TIME(0)           NOT NULL,
    End_Time                 TIME(0)           NOT NULL,
    Session_Status           VARCHAR(10)       NOT NULL CONSTRAINT DF_Session_Status DEFAULT ('Available'),
    Session_Topic            NVARCHAR(200)     NULL,
    CONSTRAINT PK_Consultation_Session PRIMARY KEY (Consultation_Session_ID),
    CONSTRAINT CK_Session_Time CHECK (End_Time > Start_Time),
    CONSTRAINT CK_Session_Price CHECK (Price > 0),
    CONSTRAINT CK_Session_Status CHECK (Session_Status IN ('Available', 'Blocked', 'Held', 'Scheduled', 'Completed', 'Cancelled')),
    CONSTRAINT CK_Session_Client CHECK ((Session_Status IN ('Available', 'Blocked') AND Client_ID IS NULL AND Session_Topic IS NULL)
                                     OR (Session_Status NOT IN ('Available', 'Blocked') AND Client_ID IS NOT NULL AND Session_Topic IS NOT NULL)),
    CONSTRAINT FK_Session_Type FOREIGN KEY (Consultation_Type_ID) REFERENCES dbo.Consultation_Type (Consultation_Type_ID),
    CONSTRAINT FK_Session_Expert FOREIGN KEY (Expert_ID) REFERENCES dbo.Expert (Expert_ID),
    CONSTRAINT FK_Session_Client FOREIGN KEY (Client_ID) REFERENCES dbo.Client (Client_ID)
);

CREATE UNIQUE INDEX UX_Session_Slot ON dbo.Consultation_Session (Expert_ID, Session_Date, Start_Time) WHERE Session_Status <> 'Cancelled';
CREATE INDEX IX_Session_Client ON dbo.Consultation_Session (Client_ID, Session_Date) WHERE Client_ID IS NOT NULL;
GO

CREATE TABLE dbo.Service_Type (
    Service_Type_ID  INT IDENTITY(1,1) NOT NULL,
    Type_Name        NVARCHAR(60)      NOT NULL,
    Description      NVARCHAR(400)     NOT NULL,
    Min_Price        DECIMAL(9,2)      NOT NULL,
    Service_Packge   VARCHAR(15)       NOT NULL,
    CONSTRAINT PK_Service_Type PRIMARY KEY (Service_Type_ID),
    CONSTRAINT UQ_ServiceType UNIQUE (Type_Name, Service_Packge),
    CONSTRAINT CK_ServiceType_Pkg CHECK (Service_Packge IN ('Standard', 'Professional', 'Premium')),
    CONSTRAINT CK_ServiceType_Price CHECK (Min_Price > 0)
);
GO

CREATE TABLE dbo.Service_Request (
    Service_Request_ID   INT IDENTITY(1,1) NOT NULL,
    Client_ID            INT               NOT NULL,
    Service_Type_ID      INT               NOT NULL,
    Request_Description  NVARCHAR(2000)    NOT NULL,
    Budget_Range         VARCHAR(25)       NOT NULL,
    Deadline             DATE              NOT NULL,
    Status               VARCHAR(12)       NOT NULL CONSTRAINT DF_Request_Status DEFAULT ('Open'),
    Agreed_Price         DECIMAL(9,2)      NULL,
    Delivery_Date        DATE              NULL,
    Title                NVARCHAR(100)     NOT NULL,
    CONSTRAINT PK_Service_Request PRIMARY KEY (Service_Request_ID),
    CONSTRAINT CK_Request_Budget CHECK (Budget_Range LIKE '[0-9]%-[0-9]%'),
    CONSTRAINT CK_Request_Status CHECK (Status IN ('Open', 'Closed', 'In Progress', 'Completed', 'Cancelled')),
    CONSTRAINT CK_Request_Agreed CHECK ((Status IN ('In Progress', 'Completed') AND Agreed_Price IS NOT NULL AND Delivery_Date IS NOT NULL)
                                     OR (Status IN ('Open', 'Closed') AND Agreed_Price IS NULL AND Delivery_Date IS NULL)
                                     OR Status = 'Cancelled'),
    CONSTRAINT FK_Request_Client FOREIGN KEY (Client_ID) REFERENCES dbo.Client (Client_ID),
    CONSTRAINT FK_Request_Type FOREIGN KEY (Service_Type_ID) REFERENCES dbo.Service_Type (Service_Type_ID)
);
CREATE INDEX IX_Request_Open ON dbo.Service_Request (Status, Service_Type_ID);
GO

CREATE TABLE dbo.Proposal (
    Proposal_ID         INT IDENTITY(1,1) NOT NULL,
    Service_request_ID  INT               NOT NULL,
    Expert_ID           INT               NOT NULL,
    Proposal_Price      DECIMAL(9,2)      NOT NULL,
    Delivery_Time       INT               NOT NULL,
    Scope_OF_Work       NVARCHAR(1500)    NOT NULL,
    Proposal_Status     VARCHAR(10)       NOT NULL CONSTRAINT DF_Proposal_Status DEFAULT ('Waiting'),
    Expiration_Time     DATETIME2(0)      NOT NULL,
    CONSTRAINT PK_Proposal PRIMARY KEY (Proposal_ID),
    CONSTRAINT UQ_Proposal_Expert UNIQUE (Service_request_ID, Expert_ID),
    CONSTRAINT CK_Proposal_Price CHECK (Proposal_Price > 0),
    CONSTRAINT CK_Proposal_Days CHECK (Delivery_Time BETWEEN 1 AND 180),
    CONSTRAINT CK_Proposal_Status CHECK (Proposal_Status IN ('Waiting', 'Accepted', 'Rejected', 'Withdrawn')),
    CONSTRAINT FK_Proposal_Request FOREIGN KEY (Service_request_ID) REFERENCES dbo.Service_Request (Service_Request_ID),
    CONSTRAINT FK_Proposal_Expert FOREIGN KEY (Expert_ID) REFERENCES dbo.Expert (Expert_ID)
);

CREATE UNIQUE INDEX UX_Proposal_Accepted ON dbo.Proposal (Service_request_ID) WHERE Proposal_Status = 'Accepted';
GO

CREATE TABLE dbo.Business_Idea (
    Business_Idea_ID  INT IDENTITY(1,1) NOT NULL,
    Idea_Name         NVARCHAR(120)     NOT NULL,
    description       NVARCHAR(1500)    NULL,
    status            VARCHAR(12)       NOT NULL CONSTRAINT DF_Idea_Status DEFAULT ('Requested'),
    Expiration_Date   DATE              NULL,
    Client_ID         INT               NOT NULL,
    CONSTRAINT PK_Business_Idea PRIMARY KEY (Business_Idea_ID),
    CONSTRAINT CK_Idea_Status CHECK (status IN ('Requested', 'In Study', 'Delivered', 'Chosen', 'Expired')),
    CONSTRAINT CK_Idea_Expiry CHECK (status IN ('Requested', 'In Study') OR Expiration_Date IS NOT NULL),
    CONSTRAINT FK_Idea_Client FOREIGN KEY (Client_ID) REFERENCES dbo.Client (Client_ID)
);
GO

CREATE TABLE dbo.Business_Service (
    Business_Service_ID  INT IDENTITY(1,1) NOT NULL,
    Service_Type_ID      INT               NOT NULL,
    Business_Idea_ID     INT               NOT NULL,
    Description          NVARCHAR(400)     NOT NULL,
    Estimated_Cost       DECIMAL(10,2)     NOT NULL,
    CONSTRAINT PK_Business_Service PRIMARY KEY (Business_Service_ID),
    CONSTRAINT CK_BizService_Cost CHECK (Estimated_Cost > 0),
    CONSTRAINT FK_BizService_Type FOREIGN KEY (Service_Type_ID) REFERENCES dbo.Service_Type (Service_Type_ID),
    CONSTRAINT FK_BizService_Idea FOREIGN KEY (Business_Idea_ID) REFERENCES dbo.Business_Idea (Business_Idea_ID)
);
GO

CREATE TABLE dbo.Business_Consultation (
    Business_Consultation_ID  INT IDENTITY(1,1) NOT NULL,
    Consultation_Type_ID      INT               NOT NULL,
    Business_Idea_ID          INT               NOT NULL,
    Description               NVARCHAR(400)     NOT NULL,
    Estimated_Cost            DECIMAL(10,2)     NOT NULL,
    CONSTRAINT PK_Business_Consultation PRIMARY KEY (Business_Consultation_ID),
    CONSTRAINT CK_BizCons_Cost CHECK (Estimated_Cost > 0),
    CONSTRAINT FK_BizCons_Type FOREIGN KEY (Consultation_Type_ID) REFERENCES dbo.Consultation_Type (Consultation_Type_ID),
    CONSTRAINT FK_BizCons_Idea FOREIGN KEY (Business_Idea_ID) REFERENCES dbo.Business_Idea (Business_Idea_ID)
);
GO

CREATE TABLE dbo.Payment (
    Payment_ID               INT IDENTITY(1,1) NOT NULL,
    Service_Request_ID       INT               NULL,
    Consultation_Session_ID  INT               NULL,
    Business_Idea_ID         INT               NULL,
    Amount                   DECIMAL(10,2)     NOT NULL,
    Payment_Method           VARCHAR(15)       NOT NULL,
    Payment_Date             DATETIME2(0)      NOT NULL CONSTRAINT DF_Payment_Date DEFAULT (SYSDATETIME()),
    Payment_Status           VARCHAR(20)       NOT NULL CONSTRAINT DF_Payment_Status DEFAULT ('Pending'),
    CONSTRAINT PK_Payment PRIMARY KEY (Payment_ID),
    CONSTRAINT CK_Payment_Target CHECK ((CASE WHEN Service_Request_ID IS NULL THEN 0 ELSE 1 END
                                       + CASE WHEN Consultation_Session_ID IS NULL THEN 0 ELSE 1 END
                                       + CASE WHEN Business_Idea_ID IS NULL THEN 0 ELSE 1 END) = 1),
    CONSTRAINT CK_Payment_Amount CHECK (Amount > 0),
    CONSTRAINT CK_Payment_Method CHECK (Payment_Method IN ('mada', 'Apple Pay', 'Credit card')),
    CONSTRAINT CK_Payment_Status CHECK (Payment_Status IN ('Pending', 'Failed', 'Paid', 'Released', 'Refunded', 'Partially refunded')),
    CONSTRAINT FK_Payment_Request FOREIGN KEY (Service_Request_ID) REFERENCES dbo.Service_Request (Service_Request_ID),
    CONSTRAINT FK_Payment_Session FOREIGN KEY (Consultation_Session_ID) REFERENCES dbo.Consultation_Session (Consultation_Session_ID),
    CONSTRAINT FK_Payment_Idea FOREIGN KEY (Business_Idea_ID) REFERENCES dbo.Business_Idea (Business_Idea_ID)
);
CREATE INDEX IX_Payment_Status ON dbo.Payment (Payment_Status, Payment_Date);
GO

CREATE TABLE dbo.Review (
    Review_ID                INT IDENTITY(1,1) NOT NULL,
    User_ID                  INT               NOT NULL,
    Service_Request_ID       INT               NULL,
    Consultation_Session_ID  INT               NULL,
    Rating                   INT               NOT NULL,
    Comment                  NVARCHAR(1000)    NULL,
    Review_Date              DATETIME2(0)      NOT NULL CONSTRAINT DF_Review_Date DEFAULT (SYSDATETIME()),
    Is_Visible               BIT               NOT NULL CONSTRAINT DF_Review_Visible DEFAULT (1),
    CONSTRAINT PK_Review PRIMARY KEY (Review_ID),
    CONSTRAINT CK_Review_Target CHECK ((Service_Request_ID IS NULL AND Consultation_Session_ID IS NOT NULL)
                                    OR (Service_Request_ID IS NOT NULL AND Consultation_Session_ID IS NULL)),
    CONSTRAINT CK_Review_Rating CHECK (Rating BETWEEN 1 AND 5),
    CONSTRAINT FK_Review_Users FOREIGN KEY (User_ID) REFERENCES dbo.Users (User_ID),
    CONSTRAINT FK_Review_Request FOREIGN KEY (Service_Request_ID) REFERENCES dbo.Service_Request (Service_Request_ID),
    CONSTRAINT FK_Review_Session FOREIGN KEY (Consultation_Session_ID) REFERENCES dbo.Consultation_Session (Consultation_Session_ID)
);
CREATE UNIQUE INDEX UX_Review_Request ON dbo.Review (Service_Request_ID) WHERE Service_Request_ID IS NOT NULL;
CREATE UNIQUE INDEX UX_Review_Session ON dbo.Review (Consultation_Session_ID) WHERE Consultation_Session_ID IS NOT NULL;
GO

CREATE TABLE dbo.Message (
    Message_ID               INT IDENTITY(1,1) NOT NULL,
    Service_Request_ID       INT               NULL,
    Consultation_Session_ID  INT               NULL,
    Message_Text             NVARCHAR(2000)    NOT NULL,
    Sent_Date                DATETIME2(0)      NOT NULL CONSTRAINT DF_Message_Date DEFAULT (SYSDATETIME()),
    Sender_User_ID           INT               NOT NULL,
    CONSTRAINT PK_Message PRIMARY KEY (Message_ID),
    CONSTRAINT CK_Message_Target CHECK ((Service_Request_ID IS NULL AND Consultation_Session_ID IS NOT NULL)
                                     OR (Service_Request_ID IS NOT NULL AND Consultation_Session_ID IS NULL)),
    CONSTRAINT FK_Message_Request FOREIGN KEY (Service_Request_ID) REFERENCES dbo.Service_Request (Service_Request_ID),
    CONSTRAINT FK_Message_Session FOREIGN KEY (Consultation_Session_ID) REFERENCES dbo.Consultation_Session (Consultation_Session_ID),
    CONSTRAINT FK_Message_Sender FOREIGN KEY (Sender_User_ID) REFERENCES dbo.Users (User_ID)
);
CREATE INDEX IX_Message_Request ON dbo.Message (Service_Request_ID, Sent_Date) WHERE Service_Request_ID IS NOT NULL;
CREATE INDEX IX_Message_Session ON dbo.Message (Consultation_Session_ID, Sent_Date) WHERE Consultation_Session_ID IS NOT NULL;
GO

PRINT 'masar_db: 17 tables created.';
GO
