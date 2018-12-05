<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Daily Production</title>
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
                        val = Request("StartMo") & "-" & Request("StartDy") & "-" & Request("StartYr")
                        val2 = Request("EndMo") & "-" & Request("EndDy") & "-" & Request("EndYr")
                        val3 = Request("StartHr") & ":" & Request("StartMin") & ":00" 
                        val4 = Request("EndHr") & ":" & Request("EndMin") & ":00" 
                        'Response.Write("Welcome " & Request.QueryString("disp"))
                        
                        'SQL = "Select P.PROCESS_CONTAINER_ID AS ""TOTE ID"", M.PART_NBR AS ""PART NUMBER"", P.PRODUCTION_RUN_ID AS ""PRODUCTION RUN"", " & _
                        '      "P.QTY AS ""QUANTITY"", P.MOD_TMSTM AS ""TIME"", P.DISPOSITION_CODE AS ""LOCATION"", P.MOD_USERID AS ""OPERATOR"" From MESDBA.PROCESS_CONTAINER P, MESDBA.MES_PART M " & _
                        '      "WHERE (M.MES_PART_ID = P.MES_PART_ID) AND P.PROCESS_CONTAINER_ID = '"& Request("tn") & "';"
                       SQL = "" 
                       SQL = "SELECT P1.PRODUCTION_RUN_ID AS ""PRODUCTION RUN"", count(P1.PROCESS_CONTAINER_ID) AS ""NUMBER TOTES"", M1.MACHINE_NAME AS ""MACHINE"", PART.PART_NBR AS ""PART NUMBER"", SUM(P1.QTY) AS ""QUANTITY"", P1.DISPOSITION_CODE " & _
                            "FROM MESDBA.PROCESS_CONTAINER P1,  MESDBA.MES_PART PART, MESDBA.MACHINE M1, MESDBA.PRODUCTION_RUN PR " & _
                            "WHERE  (P1.MES_PART_ID = PART.MES_PART_ID) AND " & _
                            "(P1.PRODUCTION_RUN_ID = PR.PRODUCTION_RUN_ID) AND " & _
                            "(M1.MACHINE_ID = PR.MACHINE_ID) AND " & _
                           "(TO_CHAR(P1.MOD_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '" & val & " " & val3 & "' AND '"& val2 & " " & val4 & "') " & _
                            "GROUP BY P1.PRODUCTION_RUN_ID, PART.PART_NBR, P1.DISPOSITION_CODE, M1.MACHINE_NAME " & _
                            "ORDER BY P1.PRODUCTION_RUN_ID;"
                             'response.write(SQL)
                        Response.Cookies("MESDBAsql") = SQL
                        Rs.open SQL,Cn,1,3
                   
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
                                    ElseIf Rs.fields(i).name = "PRODUCTION RUN" then
                                        Response.Write ("<td><a href=""ProductionRunInfo.asp?ProdRun=" & Rs.fields(i).value & """>" & Rs.fields(i) & "</a></td>")
                                    ElseIf Rs.fields(i).name = "NUMBER TOTES" then
                                        Response.Write ("<td><a href=""ProductionRunTote.asp?ProdRun=" & Rs.fields("PRODUCTION RUN").value & """>" & Rs.fields(i) & "</a></td>")
                                    Else
                                        response.write ("<td>" & Rs.fields(i).value & "</td>")
                                    End If
                               next
                               response.write "</tr>"
                               
                               Rs.movenext
                            wend
                            response.write "</table>"
                        end if
                        
                        set rs=nothing
                        Cn.close
                    %>
                </td>
            </tr>
        </table>
    </body>
</html>