# sql-tunnel.ps1 — secure tunnel for SSMS access to PRODUCTION SQL Server.
#
#   .\deploy\sql-tunnel.ps1
#
# While this window stays open, SSMS can connect with:
#   Server name:     127.0.0.1,14333
#   Authentication:  SQL Server Authentication
#   Login:           sa
#   Password:        SA_PASSWORD from deploy\prod.secrets.local
#   (check "Trust server certificate" on the Connection Properties tab)
#
# The database container only listens on the server's loopback — this SSH
# tunnel is the ONLY way in from outside. Close the window (Ctrl+C) when done.

ssh -i "$env:USERPROFILE\.ssh\vte_deploy" -N -L 127.0.0.1:14333:localhost:1433 root@116.202.8.155
