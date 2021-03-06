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
    conn.Open "DSN=dsnFanta;UID=MESWEBRO;PWD=MESRO;"
    
    Dim rstData
    Set rstData = Server.CreateObject("ADODB.Recordset")
        
    k=0
   
    If Request("sn") <> "" Then
        ' val = Request("mo") & "-" & Request("yr")        
        'Get the information out of the database with this sql
        sel = "SELECT S1.SERIAL_NBR, M1.PART_NBR, S1.QTY, S1.MOD_TMSTM "
        frm = "FROM MESDBA.STANDARD_PACK S1, MESDBA.MES_PART M1 "
        whr = "WHERE M1.MES_PART_ID = S1.MES_PART_ID AND S1.SERIAL_NBR =  '" & Request("sn") & "' "
        ord = ""
        sql = sel & frm & whr & ord & ";"                     
        rstData.Open sql, conn, 1, 3   
  ElseIf Request("pn") <> ""  Then        
         ' Date format from symbol scanner year, month, day 20030908
       ' val = Request("mo") & "-" & Request("dy") &"-" & Request("yr")         
        'Get the information out of the database with this sql
        sel = "SELECT S1.SERIAL_NBR, M1.PART_NBR, S1.QTY, S1.MOD_TMSTM "
        frm = "FROM MESDBA.STANDARD_PACK S1, MESDBA.MES_PART M1 "
        whr = "WHERE M1.MES_PART_ID = S1.MES_PART_ID AND M1.PART_NBR =  '" & Request("pn") & "' "
        ord = "ORDER BY S1.SERIAL_NBR ASC"
        sql = sel & frm & whr & ord & ";"
        rstData.Open sql, conn, 1, 3   
    End if
    if Request("sn") <> "" OR Request("pn") <> "" THEN
    
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
                Response.Write("<tr><td colspan=" & rstdata.fields.count - 1 & " align=""left""><a href=""SearchBySerial.html"">New Search</a></td></tr>")	
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
	
	rstData.Close	'Closes the recordset
    end if
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