<%@ Language=VBScript %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
    <head>
        <title>Tote Status</title>
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
                        
                        'Response.Write("Welcome " & Request.QueryString("disp"))
                        
                        SQL = "Select P.PROCESS_CONTAINER_ID AS ""TOTE ID"", M.PART_NBR AS ""PART NUMBER"", P.PRODUCTION_RUN_ID AS ""PRODUCTION RUN"", " & _
                              "P.QTY AS ""QUANTITY"", P.MOD_TMSTM AS ""TIME"", P.DISPOSITION_CODE AS ""LOCATION"", P.MOD_USERID AS ""OPERATOR"" From MESDBA.PROCESS_CONTAINER P, MESDBA.MES_PART M " & _
                              "WHERE (M.MES_PART_ID = P.MES_PART_ID) AND P.PROCESS_CONTAINER_ID = '"& Request("tn") & "';"
                        'Response.Cookies("MESDBAsql") = SQL  
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