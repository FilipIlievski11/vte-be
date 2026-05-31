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

##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 40 procs.

| Procedure | Source line |
|---|---|
| `GetChildFieldsPrivileges` | sqlData.sql:1195 |
| `GetChildObjectPrivileges` | sqlData.sql:1166 |
| `GetPrivileges` | sqlData.sql:1147 |
| `Login` | sqlData.sql:1013 |
| `addCSLAObject` | sqlData.sql:1426 |
| `addDataBase` | sqlData.sql:1042 |
| `addEmploye` | sqlData.sql:2079 |
| `addFieldsPrivilege` | sqlData.sql:1722 |
| `addObjectPrivilege` | sqlData.sql:1753 |
| `addRole` | sqlData.sql:2265 |
| `addUser` | sqlData.sql:2003 |
| `deleteCSLAObject` | sqlData.sql:1411 |
| `deleteDataBase` | sqlData.sql:1073 |
| `deleteEmploye` | sqlData.sql:2064 |
| `deleteFieldsPrivilege` | sqlData.sql:1707 |
| `deleteObjectPrivilege` | sqlData.sql:1868 |
| `deleteRole` | sqlData.sql:2217 |
| `deleteUser` | sqlData.sql:1883 |
| `getCSLAObjectById` | sqlData.sql:1392 |
| `getCSLAObjects` | sqlData.sql:1374 |
| `getConnectionString` | sqlData.sql:1128 |
| `getDataBaseById` | sqlData.sql:1106 |
| `getDataBases` | sqlData.sql:1088 |
| `getEmployeById` | sqlData.sql:2152 |
| `getEmployes` | sqlData.sql:2128 |
| `getFieldsPrivilegeById` | sqlData.sql:1688 |
| `getFieldsPrivileges` | sqlData.sql:1670 |
| `getObjectPrivilegeById` | sqlData.sql:1812 |
| `getObjectPrivileges` | sqlData.sql:1791 |
| `getRoleById` | sqlData.sql:2248 |
| `getRoles` | sqlData.sql:2232 |
| `getUserById` | sqlData.sql:1926 |
| `getUsers` | sqlData.sql:1898 |
| `updateCSLAObject` | sqlData.sql:1614 |
| `updateDataBase` | sqlData.sql:982 |
| `updateEmploye` | sqlData.sql:2177 |
| `updateFieldsPrivilege` | sqlData.sql:1642 |
| `updateObjectPrivilege` | sqlData.sql:1834 |
| `updateRole` | sqlData.sql:2290 |
| `updateUser` | sqlData.sql:1955 |

