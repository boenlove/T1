<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Lot Trace</title>
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
                        val5 = Request("MachNo")
                        ProdRun = Request("ProdRun")
                        LotNo = Request("LotNbr")
                        Shift = Request("Shift")
                        Part = Request("PartNo")
                        
                        if val5 = "" then
                            val5 = "%"
                        End if 
                        
                        if ProdRun = "" then
                            ProdRun = "%"
                        End if
                        
                        if LotNo = "" then
                            LotNo = "%"
                        End if
                        
                        if Shift = "" then
                            Shift = "%"
                        End if
                        
                        if Part = "" then
                            Part = "%"
                        End if
                                                
                        'Response.Write("Welcome " & Request.QueryString("disp"))
                        
                        if Request("OmitDate") <> "1" then                      
                           SQL = "SELECT MH.PRODUCTION_RUN_ID AS ""PRODUCTION RUN"", M1.MACHINE_NAME AS ""MACHINE"", PART.PART_NBR AS ""PART NUMBER"", MH.LOT_NBR AS ""LOT NUMBER"", MH.SHIFT_DATE AS ""Date"", MH.SHIFT_ID as ""Shift""  " & _
                                "FROM MESDBA.MES_PART PART, MESDBA.MACHINE M1, MESDBA.MACHINE_HISTORY MH " & _
                                "WHERE (MH.MES_PART_ID = PART.MES_PART_ID) AND " & _                            
                                "(MH.MACHINE_ID = M1.MACHINE_ID) AND " & _
                                "(M1.MACHINE_NAME LIKE '" & val5 & "') AND " & _
                               "(TO_CHAR(MH.INSERT_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '" & val & " " & val3 & "' AND '"& val2 & " " & val4 & "') AND " & _
                               "(TO_CHAR(MH.INSERT_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '" & val & " " & val3 & "' AND '"& val2 & " " & val4 & "') AND " & _
                               "MH.PRODUCTION_RUN_ID LIKE '" & ProdRun & "' AND PART.PART_NBR LIKE '" & Part & "' AND MH.LOT_NBR LIKE '" & LotNo & "' AND MH.SHIFT_ID LIKE '" & Shift & "' "
                       else
                            SQL = "SELECT MH.PRODUCTION_RUN_ID AS ""PRODUCTION RUN"", M1.MACHINE_NAME AS ""MACHINE"", PART.PART_NBR AS ""PART NUMBER"", MH.LOT_NBR AS ""LOT NUMBER"", MH.SHIFT_DATE AS ""Date"", MH.SHIFT_ID as ""Shift""  " & _
                                "FROM MESDBA.MES_PART PART, MESDBA.MACHINE M1, MESDBA.MACHINE_HISTORY MH " & _
                                "WHERE (MH.MES_PART_ID = PART.MES_PART_ID) AND " & _                            
                                "(MH.MACHINE_ID = M1.MACHINE_ID) AND " & _
                                "(M1.MACHINE_NAME LIKE '" & val5 & "') AND " & _
                               "MH.PRODUCTION_RUN_ID LIKE '" & ProdRun & "' AND PART.PART_NBR LIKE '" & Part & "' AND MH.LOT_NBR LIKE '" & LotNo & "' AND MH.SHIFT_ID LIKE '" & Shift & "' "
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