<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Manual Inspect Buffer</title>
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
                        SQL = "SELECT DISTINCT(P2.PROCESS_CONTAINER_ID) AS ""TOTE ID"", M.PART_NBR AS ""PART NUMBER"", " & _
		                        "P1.QTY AS ""QUANTITY"", P3.MACHINE_GROUP_NAME AS ""LOCATION"", P3.SEQUENCE_NBR " & _
                                "FROM MESDBA.PROCESS_CONTAINER P1, MESDBA.PROCESS_HISTORY P2, MESDBA.PROCESS_CONFIG P3, MESDBA.PRODUCTION_RUN_TOOL PRT, MESDBA.MES_PART M " & _
                                "WHERE (P1.PROCESS_CONTAINER_ID = P2.PROCESS_CONTAINER_ID) " & _
                                "AND (P1.MES_PART_ID = M.MES_PART_ID) " & _
                                "AND (P1.MES_PART_ID = P3.MES_PART_ID) " & _
                                "AND P3.MACHINE_GROUP_NAME ='Tumbling' " & _
                                "AND (P1.PRODUCTION_RUN_ID = PRT.PRODUCTION_RUN_ID) " & _
                                "AND (P1.DISPOSITION_CODE = 'In-Process') " & _
                                "AND (P3.SEQUENCE_NBR >= " & _
                                    "(SELECT MAX(P6.SEQUENCE_NBR) " & _
                                     "FROM MESDBA.PROCESS_CONFIG P6 " & _
                                    "WHERE (P1.MES_PART_ID = P6.MES_PART_ID) " & _
                                    "AND (PRT.TOOL_ID = P6.TOOL_ID) " & _
                                    "AND P3.MACHINE_GROUP_NAME IN ('ManSort') " & _
                                    ")) " & _
                                "ORDER BY P2.PROCESS_CONTAINER_ID ASC;"
                                
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
                               j=j+1	'This is the code to Loop the code until all of the data that the sql statement asked for from the database
                               Rs.movenext
                            wend
                            
                            if j>1 then
                                Response.Write("<tr><td colspan=" & Rs.fields.count & " align=""center"" class=""head"">There were " & j & " records found</td></tr>")
                            else
                                Response.Write("<tr><td colspan=" & Rs.fields.count & " align=""center"" class=""head"">There was " & j & " record found</td></tr>")
                            end if	
                            'Response.Write("<tr><td colspan=" & Rs.fields.count & " align=""center"" class=""head"">" & SQL & "</td></tr>")
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