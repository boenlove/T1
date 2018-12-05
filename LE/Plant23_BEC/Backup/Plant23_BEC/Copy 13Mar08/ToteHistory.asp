<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Tote Status</title>
        <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
        <link rel="stylesheet" type="text/css" href="struct.css">        
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
                        
                        'Cn.CursorLocation = adUseClient
                        ' requires use of adovbs.inc; numeric value is 3
                        Cn.open "DSN=dsnFanta; UID=MESWEBRO; PWD=MESRO"
                        
                        'Response.Write("Welcome " & Request.QueryString("disp"))
                        response.write("<h5>Completed Processes</h5>")
                        
                        SQL = "Select P.PROCESS_CONTAINER_ID AS ""TOTE ID"", P1.MACHINE_GROUP_NAME AS ""PROCESS"", P.NON_STANDARD_TOOL_NBR AS ""TOOL/LOCATION"", " & _
                              "P.MOD_TMSTM AS ""TIME"",  O1.LAST_NAME || ', ' || O1.FIRST_NAME AS ""OPERATOR"" From MESDBA.PROCESS_CONFIG P1, MESDBA.PROCESS_HISTORY P, MESDBA.VW_PEOPLE O1 " & _
                              "WHERE P.PROCESS_CONTAINER_ID = "& Request("tn") & " AND (P1.PROCESS_CONFIG_ID=P.PROCESS_CONFIG_ID) AND (O1.USER_ID = P.MOD_USERID);"
                        'Response.Cookies("MESDBAsql") = SQL 
                         'Response.write SQL

                        'Rs.open SQL,Cn,1,3
                        Rs.Source = SQL
                        RS.ActiveConnection = Cn
                        Rs.open
                   
                        if Rs.eof <> true then
                            response.write "<table border=1>"
                            for i = 0 to Rs.fields.count - 1
                                Response.Write "<td>" & Rs.fields(i).name & "</td>"
                            next
                            while not Rs.eof
                                response.write "<tr>"
                                for i = 0 to rs.fields.count - 1
                                    if Rs(i).value = "" or isnull(Rs(i).value) then
                                        Response.Write("<td>&nbsp;</td>")
                                    else
                                        response.write "<td>" & Rs.fields(i) & "</td>"
                                    end if
                               next
                               response.write "</tr>"
                               
                               Rs.movenext
                            wend
                            response.write "</table>"
                        end if
                        
                        Rs.close
                        response.write("<h5>Remaining Processes</h5>")
                       '/////////////////////////////////////////////////////////////////// 
                       '///////////////////////////////////////////////////////////////////
                       '///////////////////////////////////////////////////////////////////
                       '///////////////////////////////////////////////////////////////////
                       '///////////////////////////////////////////////////////////////////
                       ' SQL = "Select P.PROCESS_CONTAINER_ID AS ""TOTE ID"", P1.MACHINE_GROUP_NAME AS ""PROCESS"", P.NON_STANDARD_TOOL_NBR AS ""TOOL/LOCATION"", " & _
                       '       "P.MOD_TMSTM AS ""TIME"",  P.MOD_USERID AS ""OPERATOR"" From MESDBA.PROCESS_CONFIG P1, MESDBA.PROCESS_HISTORY P " & _
                       '       "WHERE P.PROCESS_CONTAINER_ID = " & Request("tn") & " AND (P1.PROCESS_CONFIG_ID=P.PROCESS_CONFIG_ID);"
                       SQL = "Select P1.PROCESS_CONTAINER_ID AS ""TOTE ID"", PC.PROCESS_CONFIG_ID, PC.MACHINE_GROUP_NAME AS ""PROCESS"" " & _
                            "From MESDBA.PROCESS_CONTAINER P1, MESDBA.PRODUCTION_RUN_TOOL PRT, MESDBA.PROCESS_CONFIG PC, MESDBA.PRODUCTION_RUN PR " & _
                            "Where (P1.PROCESS_CONTAINER_ID = '" & Request("tn") & "') AND PRT.PRODUCTION_RUN_ID = P1.PRODUCTION_RUN_ID AND PC.TOOL_ID = PRT.TOOL_ID " & _
                            "AND P1.PRODUCTION_RUN_ID = PR.PRODUCTION_RUN_ID   AND PR.MES_PART_ID = PC.MES_PART_ID AND PC.PROCESS_CONFIG_ID NOT IN " & _
                            "(SELECT PH.PROCESS_CONFIG_ID FROM MESDBA.PROCESS_HISTORY PH WHERE PH.PROCESS_CONTAINER_ID = '" & Request("tn") & "') AND PC.SEQUENCE_NBR > 1"
                        'Response.Cookies("MESDBAsql") = SQL 
                         'Response.write SQL

                        'Rs.open SQL,Cn,1,3
                        Rs.Source = SQL
                        RS.ActiveConnection = Cn
                        Rs.open
                   
                        if Rs.eof <> true then
                            response.write "<table border=1>"
                            for i = 0 to Rs.fields.count - 1
                                Response.Write "<td>" & Rs.fields(i).name & "</td>"
                            next
                            while not Rs.eof
                                response.write "<tr>"
                                for i = 0 to rs.fields.count - 1
                                    if Rs(i).value = "" or isnull(Rs(i).value) then
                                        Response.Write("<td>&nbsp;</td>")
                                    else
                                        response.write "<td>" & Rs.fields(i) & "</td>"
                                    end if
                               next
                               response.write "</tr>"
                               
                               Rs.movenext
                            wend
                            response.write "</table>"
                        end if
                        
                        Rs.close
                        response.write("<h5>Incorrect Processes</h5>")
                       '/////////////////////////////////////////////////////////////////// 
                       '///////////////////////////////////////////////////////////////////
                       '///////////////////////////////////////////////////////////////////
                       '///////////////////////////////////////////////////////////////////
                       '///////////////////////////////////////////////////////////////////                                         
                        SQL = "Select P2.PROCESS_CONTAINER_ID, P2.PROCESS_CONFIG_ID, PC2.MACHINE_GROUP_NAME, P2.PROCESS_TMSTM, O1.LAST_NAME || ', ' || O1.FIRST_NAME AS ""OPERATOR"", P2.NON_STANDARD_TOOL_NBR " & _
                               "From MESDBA.PROCESS_HISTORY P2, MESDBA.PROCESS_CONFIG PC2, MESDBA.VW_PEOPLE O1 " & _
                        "Where " & _
                          "(P2.PROCESS_CONTAINER_ID = '" & Request("tn") & "') AND PC2.PROCESS_CONFIG_ID = P2.PROCESS_CONFIG_ID AND (O1.USER_ID = P2.MOD_USERID) AND " & _
                           "P2.PROCESS_CONFIG_ID NOT IN (SELECT  " & _
                        "PC.PROCESS_CONFIG_ID " & _
                                                   " From MESDBA.PROCESS_CONTAINER P1, MESDBA.PRODUCTION_RUN_TOOL PRT, MESDBA.PROCESS_CONFIG PC, MESDBA.PRODUCTION_RUN PR " & _
                                                    "Where (P1.PROCESS_CONTAINER_ID = '" & Request("tn") & "') AND PRT.PRODUCTION_RUN_ID = P1.PRODUCTION_RUN_ID AND PC.TOOL_ID = PRT.TOOL_ID " & _
                                                    "AND P1.PRODUCTION_RUN_ID = PR.PRODUCTION_RUN_ID   AND PR.MES_PART_ID = PC.MES_PART_ID )"
                        'Response.Cookies("MESDBAsql") = SQL 
                         'Response.write SQL

                        'Rs.open SQL,Cn,1,3
                        Rs.Source = SQL
                        RS.ActiveConnection = Cn
                        Rs.open
                   
                        if Rs.eof <> true then
                            response.write "<table border=1>"
                            for i = 0 to Rs.fields.count - 1
                                Response.Write "<td>" & Rs.fields(i).name & "</td>"
                            next
                            while not Rs.eof
                                response.write "<tr>"
                                for i = 0 to rs.fields.count - 1
                                    if Rs(i).value = "" or isnull(Rs(i).value) then
                                        Response.Write("<td>&nbsp;</td>")
                                    else
                                        response.write "<td>" & Rs.fields(i) & "</td>"
                                    end if
                               next
                               response.write "</tr>"
                               
                               Rs.movenext
                            wend
                            response.write "</table>"
                        end if
                        
                        Rs.close
                        
                        
                        
                        set rs = nothing
                        Cn.close
                        set Cn = nothing
                    %>
                </td>
            </tr>
        </table>
    </body>
</html>