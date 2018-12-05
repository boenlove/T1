
<%@ Language=VBScript %>
<%
   'Change HTML header to specify Excel's MIME content type
   Response.Buffer = TRUE
   Response.ContentType = "application/vnd.ms-excel"
   
%>

<html>
    <head>
        <style>   
            {border-style:none;}         
        </style>        
    </head>
    <table>
        <tr>
                <td>
                    <%
                        dim Cn,Rs,SQL,i
                        set Cn=Server.CreateObject("ADODB.connection")
                        set Rs=Server.CreateObject("ADODB.recordset")
                        
                        Cn.open "DSN=dsnFanta; UID=MESWEBRO; PWD=MESRO"
                        
                        'SQL = "Select "
                        'SQL = SQL & " PROCESS_CONTAINER_ID, "
                        'SQL = SQL & " PRODUCTION_RUN_ID, "
                        'SQL = SQL & " MES_PART_ID, "
                        'SQL = SQL & " INVENTORY_LOCATION_ID, "
                        'SQL = SQL & " DISPOSITION_CODE, "
                        'SQL = SQL & " QTY "
                        'SQL = SQL & " From MESDBA.PROCESS_CONTAINER"
                        SQL = Request.Cookies("MESDBAsql")

                        Rs.open sql,Cn,1,3
                   
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