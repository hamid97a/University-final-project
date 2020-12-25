import sqlite3

# ---------------------InitializeValue--------------------------

conn = sqlite3.connect("Temporary.db")
c = conn.cursor()
c.execute('''CREATE TABLE "Rules" (
	"Id"	INTEGER NOT NULL,
	"Title"	TEXT,
	"approvalDate"	date,
	"announcementDate"	date,
	PRIMARY KEY("Id")
)''')
conn.commit()
conn.close()

conn = sqlite3.connect("Majlis.db")
c = conn.cursor()
c.execute('''CREATE TABLE "Rules" (
	"Id"	INTEGER NOT NULL,
	"Title"	TEXT,
	"approvalDate"	date,
	"announcementDate"	date,
	PRIMARY KEY("Id")
)''')
c.execute('''CREATE TABLE "Approveds" (
	"Id"	INTEGER NOT NULL,
	"ApprovedName"	NVARCHAR(500) NOT NULL,
	PRIMARY KEY("Id")
)''')
c.execute('''CREATE TABLE "Details" (
	"Id"	INTEGER NOT NULL,
	"Text"	TEXT,
	"RuleId"	INTEGER NOT NULL,
	"ApprovId"	INTEGER,
	"AnnouncementNumber"	NVARCHAR(100),
	"Article"	NVARCHAR(10),
	PRIMARY KEY("Id")
)''')
conn.commit()
conn.close()
