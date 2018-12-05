<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Tumble 2 Buffer</title>
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
                        dim Cn,Rs,SQL,i,j
                        set Cn=Server.CreateObject("ADODB.connection")
                        set Rs=Server.CreateObject("ADODB.recordset")
                        
                        Cn.open "DSN=dsnFanta; UID=MESWEBRO; PWD=MESRO"
                        
                        'Response.Write("Welcome " & Request.QueryString("disp"))
                        '///////////////////////////////////////////////////////
                        '///////////////////////////////////////////////////////
                        '///////////////////////////////////////////////////////
                        '///////////////////////////////////////////////////////
                        ', P2.MOD_USERID AS ""OPERATOR""                       
                        SQL = ""
                        SQL = SQL & " SELECT P1.PROCESS_CONTAINER_ID AS ""TOTE ID"", PART.PART_NBR AS ""PART NUMBER"", P1.QTY AS ""QUANTITY"", P2.MACHINE_GROUP_NAME AS ""LOCATION"", P2.SEQUENCE_NBR "
                        SQL = SQL & " FROM MESDBA.PROCESS_CONTAINER P1, MESDBA.PRODUCTION_RUN_TOOL PRT, MESDBA.PROCESS_HISTORY PH, MESDBA.PROCESS_CONFIG P2, MESDBA.MES_PART PART "
                        SQL = SQL & " WHERE (P1.PROCESS_CONTAINER_ID = PH.PROCESS_CONTAINER_ID) AND "
                        SQL = SQL & "       (PH.PROCESS_CONFIG_ID = P2.PROCESS_CONFIG_ID) AND "
                        SQL = SQL & "       (PRT.PRODUCTION_RUN_ID = P1.PRODUCTION_RUN_ID) AND "
                        SQL = SQL & "       (PRT.TOOL_ID = P2.TOOL_ID) AND "
                        SQL = SQL & "       (P1.MES_PART_ID = P2.MES_PART_ID) AND "
                        SQL = SQL & "       (P1.MES_PART_ID = PART.MES_PART_ID) AND  "
                        SQL = SQL & "       (P1.DISPOSITION_CODE = 'In-Process') AND "
                        SQL = SQL & "       (P2.SEQUENCE_NBR < (SELECT MAX(P6.SEQUENCE_NBR) FROM MESDBA.PROCESS_CONFIG P6 WHERE P6.MACHINE_GROUP_NAME ='Tumbling'  "
                        SQL = SQL & " AND (P1.MES_PART_ID = P6.MES_PART_ID) AND (PRT.TOOL_ID = P6.TOOL_ID) ))  "
                        SQL = SQL & " ORDER BY P1.PROCESS_CONTAINER_ID ASC "
                        
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
                                    if Rs(i).value = "" or isnull(Rs(i).value) then
                                        Response.Write("<td>&nbsp;</td>")
                                    else
                                        response.write "<td>" & Rs.fields(i) & "</td>"
                                    end if
                               next
                               response.write "</tr>"
                               j = j + 1
                               Rs.movenext
                            wend
                            if j>1 then
                                Response.Write("<tr><td colspan=" & Rs.fields.count & " align=""center"" class=""head"">There were " & j & " records found</td></tr>")
                            else
                                Response.Write("<tr><td colspan=" & Rs.fields.count & " align=""center"" class=""head"">There was " & j & " record found</td></tr>")
                            end if	
                            Response.Write("<tr><td colspan=" & Rs.fields.count & " align=""center"" class=""head"">" & SQL & "</td></tr>")
                            response.write "</table>"
                        end if
                        '///////////////////////////////////////////////////////
                        '///////////////////////////////////////////////////////
                        '///////////////////////////////////////////////////////
                        '///////////////////////////////////////////////////////
                        rs.close
                        set rs = nothing
                        Cn.close
                        set Cn = nothing
                    %>
                </td>
            </tr>
        </table>
    </body>
</html>