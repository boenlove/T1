<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Scrap Tote</title>
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
        <link rel="stylesheet" type="text/css" href="struct.css">        
    </head>
    <body>
        <table border=0>
            <tr>
                <td>
                    <%
                        Dim conn
                        Set conn = Server.CreateObject("ADODB.Connection")
                        conn.ConnectionTimeout=50   
                        conn.Open "DSN=dsnFanta; UID=MESWEBRO; PWD=MESRO"
                        
                        Dim rs
                        Set rs = Server.CreateObject("ADODB.Recordset")
                        
                        Dim SQL
                        'SQL = "DELETE From MESDBA.PROCESS_CONTAINER P1 WHERE P1.PROCESS_CONTAINER_ID = '" & Request("tn") & "';"
                        SQL = "Select SCRAP_CODE, DESCR From MESDBA.SCRAP_CODE;"
                        'response.write (SQL)
                        rs.Open SQL, conn, 1, 3   
                        
                        Response.Write("<select name=""scrap"">")
                        
                        Do While Not rs.EOF			
                            response.write("<option value=" & rs.fields("SCRAP_CODE").value & ">" & rs.fields("DESCR").value & "</option>")
                            'Response.Write(rs.fields(0).value)
                            rs.MoveNext    
                        Loop
                        Response.Write("</select>")
                        ' On Error Resume Next
                        ' Cn.execute(SQL)
                        ' if cn.Errors.Item(0).Number = 0 Then
                         '    SQL = "COMMIT;"
                         '    Cn.execute(SQL)
                            
                          '   Response.Write "Tote " &  Request("tn") & " was deleted"
                        ' Else
                         '   Response.Write "Tote " &  Request("tn") & " was not deleted due to a child process.  Scrap the Tote."
                         'End if
                        rs.close
                        conn.close
                        set conn = nothing
                        set rs = nothing
                    %>
                </td>
            </tr>
            <tr>
                <td><a href="default.html">Home</a>&nbsp;<a href="DeleteTote.html">Delete Tote</a></td>
            </tr>
            
        </table>
    </body>
</html>