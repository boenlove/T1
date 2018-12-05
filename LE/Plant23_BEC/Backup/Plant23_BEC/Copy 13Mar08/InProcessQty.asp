<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>In Process Quantity</title>
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
                        
                        'SQL = "Select SUM(QTY) AS ""TOTAL PARTS"", DISPOSITION_CODE AS ""LOCATION"" From MESDBA.PROCESS_CONTAINER WHERE DISPOSITION_CODE NOT IN('ContainerPacked') " & _
                        SQL = "Select SUM(QTY) AS ""TOTAL PARTS"", DISPOSITION_CODE AS ""LOCATION"" From MESDBA.PROCESS_CONTAINER WHERE DISPOSITION_CODE IN('In-Process') " & _
                              "GROUP BY DISPOSITION_CODE ORDER BY DISPOSITION_CODE;"
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
                                        if rs.fields(i).name = "LOCATION" then
                                            response.write "<td><a href=""InProcessContainer.asp?disp=" & Rs.Fields("LOCATION").value & """>" & Rs.fields(i) & "</a></td>"
                                        else
                                            response.write "<td>" & Rs.fields(i) & "</td>"
                                        end if
                                    end if
                               next
                               response.write "</tr>"
                               
                               Rs.movenext
                            wend
                            'Response.Write("<tr><td>" & SQL & "</td></tr>")
                            response.write "</table>"
                        end if
                        
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