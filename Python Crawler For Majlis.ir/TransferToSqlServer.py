import sqlite3
import pyodbc


sectionList = []
RuleList = []
approvedList = []
DetailList = []

conn = sqlite3.connect('Majlis.db')
c = conn.cursor()
c.execute('select * from Rules')
RuleList = c.fetchall()
c.execute('select * from approved')
approvedList = c.fetchall()
c.execute('select RuleId,Text,ApprovId,AnnouncementNumber,Article from Details')
DetailList = c.fetchall()
conn.close()

conn = pyodbc.connect('Driver={SQL Server Native Client 11.0};'
                      'Server=.;'
                      'Database=LawDb;'
                      'Trusted_Connection=yes;')

cursor = conn.cursor()

cursor.executemany(
    "INSERT INTO Rules VALUES(?,?,?,?)", RuleList)
conn.commit()
cursor.executemany(
    "INSERT INTO Approveds VALUES(?,?)", approvedList)
conn.commit()
cursor.executemany(
    "INSERT INTO Details(RuleId,Text,ApprovedId,AnnouncementNumber,Article) VALUES(?,?,?,?,?)", DetailList)
conn.commit()
print('complete')
conn.close()
