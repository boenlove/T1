<%@ Language=VBScript %>
<html>
<head>
    <title>Machine Status</title>
    <style>
        a:link{color:black;font-size:10px;font-weight:bold;font-family:"Verdana","MS Sans Serif", "Arial";}
        a:hover{background-color:red;color:white;font-size: 10px;font-weight:bold;font-family:"Verdana","MS Sans Serif", "Arial";}
        a:active{color:maroon;font-size:10px;font-weight:bold;font-family:"Verdana","MS Sans Serif", "Arial";}
        a{color:black;font-size: 10px;font-weight:bold;font-family:"Verdana","MS Sans Serif", "Arial";}  
        td.top{border-top:none;border-left:none;border-right:none;}
        td.bottom{border-bottom:none;border-left:none;border-right:none;}
        td.head{background:#FFCC66;}
    </style>
</head>
<body>
<table align="center" cellpadding="1" border="1" width="470">  
<tr>
    <td colspan = "5">
        <a href="export_to_excel.asp">Export To Excel</a>
    </td>
</tr>
<%
'Option Explicit
'This opens the MESDBA Database on Fanta
    j=0
    
    Dim conn
    Set conn = Server.CreateObject("ADODB.Connection")
    conn.ConnectionTimeout=50   
    conn.Open "DSN=dsnFanta; UID=MESWEBRO; PWD=MESRO"
    
    Dim rstData
    Set rstData = Server.CreateObject("ADODB.Recordset")
   
    'Get the information out of the database with this sql
    'sel = "SELECT MACHINE_ID, MES_PART_ID, PRODUCTION_RUN_ID, RUNTIME, IDLETIME, DOWNTIME, FAULT_COUNT, SCRAP_COUNT, CURRENT_QTY  "
    sel = "SELECT M1.MACHINE_NAME AS ""MACHINE"", MCS.LOT_NBR AS ""LOT NUMBER"", MCS.PRODUCTION_RUN_ID AS ""PRODUCTION RUN"", P3.PART_NBR AS ""PART NUMBER"", MCS.MOD_TMSTM "
    frm = "FROM MESDBA.MACHINE_CURRENT_STATUS MCS, MESDBA.MACHINE M1, MESDBA.MES_PART P3 "
    whr = "WHERE (M1.MACHINE_ID = MCS.MACHINE_ID) AND (P3.MES_PART_ID = MCS.MES_PART_ID) "
    ord = ""
    sql = sel & frm & whr & ord & ";"                     
    Response.Cookies("MESDBAsql") = SQL
    
    rstData.Open sql, conn, 1, 3   

    If Not rstData.BOF and not rstData.EOF then	
		'Table Data	
         Response.Write("<tr><td colspan=" & rstdata.fields.count & " align=""left""></td></tr>")	
         Response.Write("<tr>")
         for i = 0 to rstData.fields.count - 1
            Response.Write "<td>" & rstData.fields(i).name & "</td>"
        next
        Response.Write("</tr>")
        
		Do While not rstData.EOF			
			Response.Write("<tr ALIGN='center'>")
			For i = 0 to rstdata.fields.count - 1
				Response.Write("<td align='center' bgcolor='f7efde' class=""a"">")
				
						if rstdata(i).value = "" or isnull(rstdata(i).value) then
							Response.Write("&nbsp;")
                         ElseIf rstdata.fields(i).name = "PRODUCTION RUN" then
                                        Response.Write ("<a href=""ProductionRunInfo.asp?ProdRun=" & rstdata.fields(i).value & """>" & rstdata.fields(i).value & "</a>")
						else
							Response.Write(rstData(i).Value) 
						end if			
				Response.Write("</td>")
			next
			Response.Write("</tr>")
			rstData.MoveNext
           
			j=j+1	'This is the code to Loop the code until all of the data that the sql statement asked for from the database
					'is on the web page
		Loop
		Response.Write("</tr>")
    End If 
    'Outside if      
        if j>1 then
            Response.Write("<tr><td colspan=" & rstdata.fields.count & " align=""center"" class=""head"">There were " & j & " records found</td></tr>")
        else
            Response.Write("<tr><td colspan=" & rstdata.fields.count & " align=""center"" class=""head"">There was " & j & " record found</td></tr>")
        end if	
	
	rstData.Close	'Closes the recordset
    Set rstData = nothing
    conn.Close 'Close the Connection
    Set conn = nothing
    
    
%>
    <tr>
        <td colspan="9" align="left" class="bottom"><a href="default.html">Home</a></td>        
    </tr>
</table>
</body>
</html>