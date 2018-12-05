<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Production Run Info</title>
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
                        
                        Cn.open "DSN=dsnFanta;UID=MESWEBRO;PWD=MESRO;"
                        
                        'Response.Write("Welcome " & Request.QueryString("disp"))
                        '///////////////////////////////////////////////////////
                        '///////////////////////////////////////////////////////
                        '///////////////////////////////////////////////////////
                        '///////////////////////////////////////////////////////
                        ', P.MOD_USERID AS ""OPERATOR""
                        SQL = "Select P1.PRODUCTION_RUN_ID AS ""PRODUCTION RUN"", V1.TOOL_DESIGN_NBR || '-' || V1.TOOL_DESIGN_LEVEL || '-' || V1.SERIES_NBR || '-' || V1.REVISION_LEVEL AS ""TOOL NUMBER"", " & _
                              "P3.PART_NBR AS ""PART NUMBER"", " & _
                              "M1.MACHINE_NAME AS ""MACHINE"", " & _
                              "P1.INSERT_TMSTM AS ""START TIME"", " & _
                              "P1.MOD_TMSTM AS ""STOP TIME"" " & _
                            "From MESDBA.PRODUCTION_RUN P1, MESDBA.PRODUCTION_RUN_TOOL P2, MESDBA.VW_TOOL V1, MESDBA.MES_PART P3, MESDBA.MACHINE M1 " & _
                            "Where " & _
                            "(P1.PRODUCTION_RUN_ID = '" & Request.QueryString("ProdRun") & "') And " & _
                           "(P1.PRODUCTION_RUN_ID = P2.PRODUCTION_RUN_ID) AND " & _
                           "(P2.TOOL_ID = V1.TOOL_ID) AND " & _
                           "(P1.MES_PART_ID = P3.MES_PART_ID) And " & _
                           "(P1.MACHINE_ID = M1.MACHINE_ID);"
                        Response.Cookies("MESDBAsql") = SQL  
                       'response.write(SQL)
                                               
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