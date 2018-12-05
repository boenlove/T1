<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Tote Scrap</title>
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
                        
                        if Request("OmitDate") <> "1" then
                            SQL = ""
                            SQL = "Select P2.Machine_Group_Name AS ""Scrap Process"", PN.PART_NBR AS ""PART NUMBER"", Count(P1.PROCESS_CONTAINER_ID) AS ""NBR Totes"", SC.DESCR AS ""Scrap Description"", TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY') AS ""DATE"" " & _
                                  "From MESDBA.PROCESS_SCRAP_HISTORY P1, MESDBA.Process_Config P2, MESDBA.SCRAP_CODE SC, MESDBA.MES_PART PN " & _
                                  "WHERE (P1.Process_Config_ID = P2.Process_Config_ID) AND (P1.SCRAP_CODE = SC.scrap_code) AND (P2.MES_PART_ID = PN.MES_PART_ID) AND " & _
                                  "(TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '" & val & " " & val3 & "' AND '"& val2 & " " & val4 & "') " & _
                                  "GROUP BY P2.Machine_Group_Name, PN.PART_NBR, SC.DESCR, P1.INSERT_TMSTM"                           
                       else
                            SQL = ""
                            SQL = "Select P2.Machine_Group_Name AS ""Scrap Process"", PN.PART_NBR AS ""PART NUMBER"", Count(P1.PROCESS_CONTAINER_ID) AS ""NBR Totes"", SC.DESCR AS ""Scrap Description"", TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY') AS ""DATE"" " & _
                                  "From MESDBA.PROCESS_SCRAP_HISTORY P1, MESDBA.Process_Config P2, MESDBA.SCRAP_CODE SC, MESDBA.MES_PART PN " & _
                                  "WHERE (P1.Process_Config_ID = P2.Process_Config_ID) AND (P1.SCRAP_CODE = SC.scrap_code) AND (P2.MES_PART_ID = PN.MES_PART_ID) " & _
                                  "AND (TO_CHAR(P1.INSERT_TMSTM, 'MM-DD-YYYY') LIKE '%" & val & "%') " & _
                                  "GROUP BY P2.Machine_Group_Name, PN.PART_NBR, SC.DESCR, P1.INSERT_TMSTM "
                       end if
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