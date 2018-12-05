<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Employee Process History</title>
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
        <link rel=stylesheet type="text/css" href="struct.css">        
    </head>
    <body>
        <table border=0>
            <tr>
                <td>
                    <a href="export_to_excel.asp">Export To Excel</a><a href="default.html">Home</a>
                </td>
            </tr>
            <tr>
                <td>
                    <%
                        dim Cn,Rs,SQL,i
                        set Cn=Server.CreateObject("ADODB.connection")
                        set Rs=Server.CreateObject("ADODB.recordset")
                        
                        Cn.open "DSN=dsnFanta; UID=MESWEBRO; PWD=MESRO"
                        
                        
                        If Request("EmpID") <> "" Then
                        SQL = "SELECT " & _
                              "P1.MOD_USERID AS ""EMPLOYEE"", " & _
                              "COUNT(P1.PROCESS_CONTAINER_ID) AS ""Totes Processed"", " & _
                              "TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY') AS ""DATE"" " & _
                              "From MESDBA.PROCESS_HISTORY P1 " & _
                              "Where P1.MOD_USERID = '" & request("EmpID") & "' AND " & _
                              "(TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY') BETWEEN '" & request("StartDate") & "' AND '" & request("EndDate") & "') " & _
                               "GROUP BY P1.MOD_USERID, TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY');" 
                        ElseIF Request("EmpID") = "" then
                            '"P1.MOD_USERID AS ""EMPLOYEE"", " & _
                            SQL = "SELECT " & _                              
                              "O1.LAST_NAME || ', ' || O1.FIRST_NAME AS ""OPERATOR"", " & _
                              "COUNT(P1.PROCESS_CONTAINER_ID) AS ""Totes Processed"", " & _
                              "TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY') AS ""DATE"" " & _
                              "FROM MESDBA.PROCESS_HISTORY P1, MESDBA.VW_PEOPLE O1 " & _
                              "WHERE (TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY') BETWEEN '" & request("StartDate") & "' AND '" & request("EndDate") & "') " & _
                              "AND (O1.USER_ID = P1.MOD_USERID) " & _
                               "GROUP BY O1.LAST_NAME || ', ' || O1.FIRST_NAME, TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY');" 
                        end if
'response.write SQL                       
                       
                        Response.Cookies("MESDBAsql") = SQL
                        Rs.open SQL,Cn,1,3
                        if Cn.Errors.Item(0).Number = 0 Then
                            if Rs.eof <> true then
                                response.write "<table border=1>"
                                for i = 0 to Rs.fields.count - 1
                                    Response.Write "<td>" & Rs.fields(i).name & "</td>"
                                next
                                while not Rs.eof
                                    response.write "<tr>"
                                    for i = 0 to rs.fields.count - 1
                                        If Rs.fields(i).value = "" or isnull(Rs.fields(i).value) then
                                            Response.Write("<td>&nbsp;</td>")
                                        Else
                                            response.write ("<td>" & Rs.fields(i).value & "</td>")
                                        End If
                                   next
                                   response.write "</tr>"
                                   
                                   Rs.movenext
                                wend
                                response.write "</table>"
                                rs.close
                            end if
                        End if //rs error check
                        set rs=nothing
                        Cn.close
                    %>
                </td>
            </tr>
        </table>
    </body>
</html>