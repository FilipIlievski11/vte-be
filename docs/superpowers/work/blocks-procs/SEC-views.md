##### Views (SEC database)

All 3 views found in sqlData.sql relate to the SEC module (privilege evaluation).

##### View `View_1`

Source: `WinApp/sqlData.sql:824-832`

```sql
CREATE VIEW [dbo].[View_1]
AS
SELECT     dbo.Users.ID AS IdUser, dbo.Users.IdRole, dbo.Users.IdDataBase, dbo.Users.UserFullName, dbo.Users.UserName, dbo.Users.UserPass, 
                      dbo.Roles.RoleName, dbo.DataBases.EndUserName, dbo.DataBases.DatabaseName, dbo.DataBases.ConnetionString
FROM         dbo.Users INNER JOIN
                      dbo.Roles ON dbo.Users.IdRole = dbo.Roles.Id INNER JOIN
                      dbo.DataBases ON dbo.Users.IdDataBase = dbo.DataBases.Id
WHERE     (dbo.Users.Active = 1)
GO
```

##### View `FieldPrivilegesList`

Source: `WinApp/sqlData.sql:1216-1224`

```sql
CREATE VIEW [dbo].[FieldPrivilegesList]
AS
SELECT     dbo.Roles.RoleName, dbo.CSLAObjects.CSLAObjectName, dbo.CSLAObjects.CSLAObjectType, dbo.FieldsPrivileges.CSLAObjectPropertyName, 
                      dbo.FieldsPrivileges.Id, dbo.Roles.Id AS IdRole, dbo.CSLAObjects.Id AS IdObject
FROM         dbo.Roles INNER JOIN
                      dbo.FieldsPrivileges ON dbo.Roles.Id = dbo.FieldsPrivileges.IdRole INNER JOIN
                      dbo.CSLAObjects ON dbo.FieldsPrivileges.IdCSLAObject = dbo.CSLAObjects.Id
WHERE     (dbo.FieldsPrivileges.Active = 1)
GO
```

##### View `ObjectPrivilegesList`

Source: `WinApp/sqlData.sql:1455-1464`

```sql
CREATE VIEW [dbo].[ObjectPrivilegesList]
AS
SELECT     dbo.ObjectPrivileges.Id, dbo.ObjectPrivileges.IdRole, dbo.Roles.RoleName, dbo.ObjectPrivileges.IdCSLAObject, dbo.CSLAObjects.CSLAObjectName, 
                      dbo.ObjectPrivileges.CanAddObject, dbo.ObjectPrivileges.CanGetObject, dbo.ObjectPrivileges.CanDeleteObject, 
                      dbo.ObjectPrivileges.CanEditObject
FROM         dbo.ObjectPrivileges INNER JOIN
                      dbo.CSLAObjects ON dbo.ObjectPrivileges.IdCSLAObject = dbo.CSLAObjects.Id INNER JOIN
                      dbo.Roles ON dbo.ObjectPrivileges.IdRole = dbo.Roles.Id
WHERE     (dbo.ObjectPrivileges.Active = 1)
GO
```

