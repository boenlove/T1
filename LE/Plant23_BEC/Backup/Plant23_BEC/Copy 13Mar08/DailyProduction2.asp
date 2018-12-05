<%@ Language=VBScript %>
<html>
<head>
    <title>LSR Reports</title>
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
    
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '//Totes Packed OUT    
    k=0
    ' Date format from symbol scanner year, month, day 20030908
    'val = Request("mo") & "-" & Request("dy") & "-" & Request("yr")  
    val = "02-21-2005"
    sel = "SELECT S1.SERIAL_NBR, M1.PART_NBR, S1.QTY, S1.MOD_TMSTM "
    frm = "FROM MESDBA.STANDARD_PACK S1, MESDBA.MES_PART M1 "
    whr = "WHERE M1.MES_PART_ID = S1.MES_PART_ID AND TO_CHAR(S1.MOD_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '" & val & " 07:00:00' AND '" & val & " 19:00:00'"
    ord = "ORDER BY S1.SERIAL_NBR ASC"
    sql = sel & frm & whr & ord & ";"
    'rstData.Open sql, "PremoICS", 3
    Response.Write("<tr><td colspan=""4"" align=""left"">Shift 1 Totes Packed Out</td></tr>")
    Set rstData = conn.Execute(sql)
   
    If Not rstData.BOF and not rstData.EOF then	
		'Table Data	
         Response.Write("<tr><td colspan=""4"" align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
         Response.Write("<tr><td>Serial Number</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
		Do While not rstData.EOF			
			Response.Write("<tr ALIGN='center'>")
			For i = 0 to rstdata.fields.count - 1
				Response.Write("<td align='center' bgcolor='f7efde' class=""a"">")
				
						if rstdata(i).value = "" or isnull(rstdata(i).value) then
							Response.Write("&nbsp;")
						else
							Response.Write(rstData(i).Value) 
						end if			
				Response.Write("</td>")
			next
			Response.Write("</tr>")
			rstData.MoveNext
            k=k+1
            if k = 25 then
                Response.Write("<tr><td colspan=" & rstdata.fields.count  & " align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
                Response.Write("<tr><td>Serial Number</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
                k=0
            End if
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
        
        
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    'Scrap
    k=0
    j=0
    ' Date format from symbol scanner year, month, day 20030908
    'val = Request("mo") & "-" & Request("dy") & "-" & Request("yr")  
    val = "02-21-2005"
    sel = "Select P1.PROCESS_CONTAINER_ID, M1.PART_NBR, P1.QTY, P1.DISPOSITION_CODE " 
    frm = "FROM MESDBA.PROCESS_CONTAINER P1, MESDBA.MES_PART M1 "
    whr = "WHERE M1.MES_PART_ID = P1.MES_PART_ID AND TO_CHAR(P1.MOD_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '" & val & " 07:00:00' AND '" & val & " 19:00:00' AND P1.DISPOSITION_CODE = 'Scrap' "
    ord = "ORDER BY P1.PROCESS_CONTAINER_ID ASC"
    sql = sel & frm & whr & ord & ";"
    
    Response.Write("<tr><td colspan=""4"" align=""left"">Shift 1 Totes Scraped</td></tr>")
    Set rstData = conn.Execute(sql)
   
    If Not rstData.BOF and not rstData.EOF then	
		'Table Data	
         Response.Write("<tr><td colspan=""4"" align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
         Response.Write("<tr><td>Tote</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
		Do While not rstData.EOF			
			Response.Write("<tr ALIGN='center'>")
			For i = 0 to rstdata.fields.count - 1
				Response.Write("<td align='center' bgcolor='f7efde' class=""a"">")
				
						if rstdata(i).value = "" or isnull(rstdata(i).value) then
							Response.Write("&nbsp;")
						else
							Response.Write(rstData(i).Value) 
						end if			
				Response.Write("</td>")
			next
			Response.Write("</tr>")
			rstData.MoveNext
            k=k+1
            if k = 25 then
                Response.Write("<tr><td colspan=" & rstdata.fields.count  & " align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
                Response.Write("<tr><td>Serial Number</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
                k=0
            End if
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
        
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    'Tumble 1
     k=0
    j=0
    ' Date format from symbol scanner year, month, day 20030908
    'val = Request("mo") & "-" & Request("dy") & "-" & Request("yr")  
    val = "02-21-2005"
    'sel = "Select P1.PROCESS_CONTAINER_ID, M1.PART_NBR, P1.QTY, P1.DISPOSITION_CODE " 
    'frm = "FROM MESDBA.PROCESS_CONTAINER P1, MESDBA.MES_PART M1 "
    'whr = "WHERE M1.MES_PART_ID = P1.MES_PART_ID AND TO_CHAR(P1.MOD_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '" & val & " 07:00:00' AND '" & val & " 19:00:00' AND P1.DISPOSITION_CODE = 'Scrap' "
    'ord = "ORDER BY P1.PROCESS_CONTAINER_ID ASC"
    'sql = sel & frm & whr & ord & ";"
    SQL = " Select "
    SQL = SQL & "   P1.PROCESS_CONTAINER_ID, P1.MES_PART_ID, P1.QTY, P2.INSERT_TMSTM "
    SQL = SQL & " From MESDBA.PROCESS_CONTAINER P1, MESDBA.PROCESS_HISTORY P2, MESDBA.PROCESS_CONFIG P3 "
    SQL = SQL & " Where "
    SQL = SQL & "   (P1.DISPOSITION_CODE = 'In-Process') AND "
    SQL = SQL & "   (P1.PROCESS_CONTAINER_ID = P2.PROCESS_CONTAINER_ID) AND "
    SQL = SQL & "   (P2.PROCESS_CONFIG_ID = P3.PROCESS_CONFIG_ID) AND "
    SQL = SQL & "   (P3.MACHINE_GROUP_NAME = 'Tumbling') AND "
    SQL = SQL & "   (P3.SEQUENCE_NBR = '2') AND "
    SQL = SQL & "   (TO_CHAR(P2.INSERT_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '02-21-2005 07:00:00' AND '02-21-2005 19:00:00') "
    
    Response.Write("<tr><td colspan=""4"" align=""left"">Shift 1 Tumbling</td></tr>")
    Set rstData = conn.Execute(sql)
   
    If Not rstData.BOF and not rstData.EOF then	
		'Table Data	
         Response.Write("<tr><td colspan=""4"" align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
         Response.Write("<tr><td>Tote</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
		Do While not rstData.EOF			
			Response.Write("<tr ALIGN='center'>")
			For i = 0 to rstdata.fields.count - 1
				Response.Write("<td align='center' bgcolor='f7efde' class=""a"">")
				
						if rstdata(i).value = "" or isnull(rstdata(i).value) then
							Response.Write("&nbsp;")
						else
							Response.Write(rstData(i).Value) 
						end if			
				Response.Write("</td>")
			next
			Response.Write("</tr>")
			rstData.MoveNext
            k=k+1
            if k = 25 then
                Response.Write("<tr><td colspan=" & rstdata.fields.count  & " align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
                Response.Write("<tr><td>Serial Number</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
                k=0
            End if
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
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    'Molded Parts
    k=0
    j=0
    ' Date format from symbol scanner year, month, day 20030908
    'val = Request("mo") & "-" & Request("dy") & "-" & Request("yr")  
    val = "02-21-2005"
    sql = "Select P.PROCESS_CONTAINER_ID, M.PART_NBR, P.QTY, P.MOD_TMSTM " & _
          "From MESDBA.PROCESS_CONTAINER P, MESDBA.MES_PART M " & _
        "WHERE (M.MES_PART_ID = P.MES_PART_ID) AND P.DISPOSITION_CODE = 'In-Process' AND P.PROCESS_CONTAINER_ID NOT IN " & _
        "(SELECT P2.Process_Container_Id FROM MESDBA.Process_History P2) AND TO_CHAR(P.MOD_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '" & val & " 07:00:00' AND '" & val & " 19:00:00' " & _
        "ORDER BY P.PROCESS_CONTAINER_ID ASC;"
    
    Response.Write("<tr><td colspan=""4"" align=""left"">Shift 1 Totes Molded</td></tr>")
    Set rstData = conn.Execute(sql)
   
    If Not rstData.BOF and not rstData.EOF then	
		'Table Data	
         Response.Write("<tr><td colspan=""4"" align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
         Response.Write("<tr><td>Tote</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
		Do While not rstData.EOF			
			Response.Write("<tr ALIGN='center'>")
			For i = 0 to rstdata.fields.count - 1
				Response.Write("<td align='center' bgcolor='f7efde' class=""a"">")
				
						if rstdata(i).value = "" or isnull(rstdata(i).value) then
							Response.Write("&nbsp;")
						else
							Response.Write(rstData(i).Value) 
						end if			
				Response.Write("</td>")
			next
			Response.Write("</tr>")
			rstData.MoveNext
            k=k+1
            if k = 25 then
                Response.Write("<tr><td colspan=" & rstdata.fields.count  & " align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
                Response.Write("<tr><td>Serial Number</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
                k=0
            End if
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
        
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    'Tumble 2
     k=0
    j=0
    ' Date format from symbol scanner year, month, day 20030908
    'val = Request("mo") & "-" & Request("dy") & "-" & Request("yr")  
    val = "02-21-2005"
    'sel = "Select P1.PROCESS_CONTAINER_ID, M1.PART_NBR, P1.QTY, P1.DISPOSITION_CODE " 
    'frm = "FROM MESDBA.PROCESS_CONTAINER P1, MESDBA.MES_PART M1 "
    'whr = "WHERE M1.MES_PART_ID = P1.MES_PART_ID AND TO_CHAR(P1.MOD_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '" & val & " 07:00:00' AND '" & val & " 19:00:00' AND P1.DISPOSITION_CODE = 'Scrap' "
    'ord = "ORDER BY P1.PROCESS_CONTAINER_ID ASC"
    'sql = sel & frm & whr & ord & ";"
    SQL = " Select "
    SQL = SQL & "   P1.PROCESS_CONTAINER_ID, P1.MES_PART_ID, P1.QTY, P2.INSERT_TMSTM "
    SQL = SQL & " From MESDBA.PROCESS_CONTAINER P1, MESDBA.PROCESS_HISTORY P2, MESDBA.PROCESS_CONFIG P3 "
    SQL = SQL & " Where "
    SQL = SQL & "   (P1.DISPOSITION_CODE = 'In-Process') AND "
    SQL = SQL & "   (P1.PROCESS_CONTAINER_ID = P2.PROCESS_CONTAINER_ID) AND "
    SQL = SQL & "   (P2.PROCESS_CONFIG_ID = P3.PROCESS_CONFIG_ID) AND "
    SQL = SQL & "   (P3.MACHINE_GROUP_NAME = 'Tumbling') AND "
    SQL = SQL & "   (P3.SEQUENCE_NBR = '3') AND "
    SQL = SQL & "   (TO_CHAR(P2.INSERT_TMSTM, 'MM-DD-YYYY HH24:MI:SS') BETWEEN '02-21-2005 07:00:00' AND '02-21-2005 19:00:00') "
    
    Response.Write("<tr><td colspan=""4"" align=""left"">Shift 1 Tumbling 2</td></tr>")
    Set rstData = conn.Execute(sql)
   
    If Not rstData.BOF and not rstData.EOF then	
		'Table Data	
         Response.Write("<tr><td colspan=""4"" align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
         Response.Write("<tr><td>Tote</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
		Do While not rstData.EOF			
			Response.Write("<tr ALIGN='center'>")
			For i = 0 to rstdata.fields.count - 1
				Response.Write("<td align='center' bgcolor='f7efde' class=""a"">")
				
						if rstdata(i).value = "" or isnull(rstdata(i).value) then
							Response.Write("&nbsp;")
						else
							Response.Write(rstData(i).Value) 
						end if			
				Response.Write("</td>")
			next
			Response.Write("</tr>")
			rstData.MoveNext
            k=k+1
            if k = 25 then
                Response.Write("<tr><td colspan=" & rstdata.fields.count  & " align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
                Response.Write("<tr><td>Serial Number</td><td>Part Number</td><td>Qty</td><td>Date</td></tr>")
                k=0
            End if
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
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////
    '///////////////////////////////////////////////////////////////////////    
    
	
	rstData.Close	'Closes the recordset
    Set rstData = nothing
    conn.Close 'Close the Connection
    Set conn = nothing
    
    
%>
    <tr>
        <td colspan="4" align="left" class="bottom"><a href="SearchBySerial.html">New Search</a><a href="default.html">Home</a></td>        
    </tr>
</table>
</body>
</html>