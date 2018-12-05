<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Tumble 1 Process</title>
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
                        ', P.MOD_USERID AS ""OPERATOR""
                        SQL = "Select P.PROCESS_CONTAINER_ID AS ""TOTE ID"", M.PART_NBR AS ""PART NUMBER"", " & _
                              "P.QTY AS ""QUANTITY"", P.MOD_TMSTM AS ""TIME"", P.DISPOSITION_CODE AS ""LOCATION"" From MESDBA.PROCESS_CONTAINER P, MESDBA.MES_PART M " & _
                              "WHERE (M.MES_PART_ID = P.MES_PART_ID) AND P.DISPOSITION_CODE = 'In-Process' AND P.PROCESS_CONTAINER_ID NOT IN " & _
                              "(SELECT P2.Process_Container_Id FROM MESDBA.Process_History P2)" & _
                              "ORDER BY P.PROCESS_CONTAINER_ID ASC;"
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