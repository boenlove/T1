<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Delete Tote</title>
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
        <link rel="stylesheet" type="text/css" href="struct.css">        
    </head>
    <body>
        <table border=0>
            <tr>
                <td>
                    <%
                        dim Cn,Rs,SQL,i
                        set Cn=Server.CreateObject("ADODB.connection")
                        
                        'Cn.CursorLocation = adUseClient
                        ' requires use of adovbs.inc; numeric value is 3
                        Cn.open "DSN=dsnFanta; UID=MESWEB; PWD=MESFULL"
                        
                        'Response.Write("Welcome " & Request.QueryString("disp"))
                        
                        SQL = "DELETE From MESDBA.PROCESS_CONTAINER P1 WHERE P1.PROCESS_CONTAINER_ID = '" & Request("tn") & "';"
                        'Response.Cookies("MESDBAsql") = SQL 
                         'Response.write SQL
                         On Error Resume Next
                         Cn.execute(SQL)
                         if cn.Errors.Item(0).Number = 0 Then
                             SQL = "COMMIT;"
                             Cn.execute(SQL)
                            
                             Response.Write "Tote " &  Request("tn") & " was deleted"
                         Else
                            Response.Write "Tote " &  Request("tn") & " was not deleted due to a child process.  Scrap the Tote."
                         End if
                        
                        Cn.close
                        set Cn = nothing
                    %>
                </td>
            </tr>
            <tr>
                <td><a href="default.html">Home</a>&nbsp;<a href="DeleteTote.html">Delete Tote</a></td>
            </tr>
            
        </table>
    </body>
</html>