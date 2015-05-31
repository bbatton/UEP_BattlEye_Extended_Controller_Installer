Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Net
Imports Shell32

Module Module1
    Sub Main()
        'Declaring Needed Variables
        Dim Restart As String, BEC As String, BEServer As String, BEConfig As String, BEConfigFile As String, Schedule As String, BECexe As String, RInstance As String, ServerName As String, Var2 As Boolean, SchTimer As Boolean, Advanced As Boolean
        Dim Password As String, Ping As String, Wan As String, Port As String, MSG1 As String, MSG2 As String, Cancel As String, Error1 As String, Instance As String, C As String, Error2 As String, Error3 As String, Error4 As String, Error6 As String, Error7 As String
        Dim Server = Directory.GetCurrentDirectory()
        Dim Var As Boolean = False

        'Setting Warning & Error Messages
        Cancel = "WARNING: UEP BEC Installer Terminated!" & vbCrLf & "You need to enter all information to complete set up!"
        Error1 = "WARNING: UEP BEC Installer Terminated!" & vbCrLf & "Installer could not find your BEC download!"
        Error2 = "WARNING: UEP BEC Installer Terminated!" & vbCrLf & "You did not select your Bec.zip file location!"
        Error3 = "Please Enter Only The Number That Corresponds Your Server Map!"
        Error4 = "Please Only Enter A Map Number!" & vbCrLf & "Options 1-19"
        Error6 = "ERROR:" & vbCrLf & "Port must be a numeric value!" & vbCrLf & "Default port: 2302"
        Error7 = "ERROR:" & vbCrLf & "Ping must be a numeric value!" & vbCrLf & "The higher number, the higher the desync"
        Server = Directory.GetCurrentDirectory()


        'User Input & Null String Check
        Wan = InputBox("Step 1 of 7" & vbCrLf & "Enter the Wide Area Network IP for your server:", _
"Unified Epoch Project BEC Installer")
        If Wan = "" Or IsNothing(Wan) Then
            MsgBox(Cancel)
            System.Environment.Exit(1)
        End If

        Do
            Port = InputBox("Step 2 of 7" & vbCrLf & "Enter the port number that your sever listens on:", _
    "Unified Epoch Project BEC Installer")
            If Port = "" Or IsNothing(Port) Then
                MsgBox(Cancel)
                System.Environment.Exit(1)
            ElseIf Not IsNumeric(Port) Then
                MsgBox(Error6)
                Var = False
            Else
                Var = True
            End If
        Loop Until Var = True

        Password = InputBox("Step 3 of 7" & vbCr & vbLf & "Enter your Rcon password that is set in your hive.ext: (If password is Blank put Blank)", _
    "Unified Epoch Project BEC Installer")
        If Password = "" Or IsNothing(Password) Then
            MsgBox(Cancel)
            System.Environment.Exit(1)
        ElseIf Password = "Blank" Then
            Password = ""
        End If


        Do
            Ping = InputBox("Step 4 of 7" & vbCrLf & "Enter the max ping you want allowed on your server:", _
    "Unified Epoch Project BEC Installer")
            If Ping = "" Or IsNothing(Ping) Then
                MsgBox(Cancel)
                System.Environment.Exit(1)
            ElseIf Not IsNumeric(Ping) Then
                MsgBox(Error7)
                Var = False
            Else
                Var = True
            End If
        Loop Until Var = True


        MSG1 = InputBox("Step 5 of 7" & vbCrLf & "Enter a message that you want to broadcast over global chat:", _
    "Unified Epoch Project BEC Installer")
        If MSG1 = "" Then
            MsgBox(Cancel)
            System.Environment.Exit(1)
        End If


        MSG2 = InputBox("Step 6 of 7" & vbCrLf & "Enter another message that you want to broadcast over global chat:", _
    "Unified Epoch Project BEC Installer")
        If MSG2 = "" Then
            MsgBox(Cancel)
            System.Environment.Exit(1)
        End If


        'Gets the map version the server runs
        Do
            Instance = InputBox("Step 7 of 7" & vbCrLf & "Input the number that corresponds to the map your server runs." & vbCrLf & "" & vbCrLf & "1: Chernarus" & vbCrLf & "2: Namalsk" & vbCrLf & "3: Panthera" & vbCrLf & "4: Takistan" & vbCrLf & "5: Napf" & vbCrLf & "6: Proving Grounds" & vbCrLf & "7: Sauerland" & vbCrLf & "8: utes" & vbCrLf & "9: Duala" & vbCrLf & "10: Tavi" & vbCrLf & "11: Sahrani" & vbCrLf & "12: Poda" & vbCrLf & "13: Fapovo" & vbCrLf & "14: Caribou" & vbCrLf & "15: SMDsahrani" & vbCrLf & "16: Shapur_Baf" & vbCrLf & "17: Zargabad" & vbCrLf & "18: Dingor" & vbCrLf & "19: Lingor" & vbCr & vbLf, _
    "Unified Epoch Project BEC Installer")
            If Instance = "" Or IsNothing(Instance) Then
                MsgBox(Cancel)
                System.Environment.Exit(1)
            ElseIf Not IsNumeric(Instance) Then
                MsgBox(Error3)
                Var = False
            ElseIf Not Instance <= 19 Or Not Instance > 0 Then
                MsgBox(Error4)
                Var = False
            Else
                Var = True
            End If
        Loop Until Var = True

        C = Instance
        Select Case C
            Case 1
                Instance = "instance_11_Chernarus"
            Case 2
                Instance = "instance_15_namalsk"
            Case 3
                Instance = "instance_16_panthera"
            Case 4
                Instance = "instance_1_takistan"
            Case 5
                Instance = "instance_24_Napf"
            Case 6
                Instance = "instance_8_ProvingGrounds_PMC"
            Case 7
                Instance = "instance_25_sauerland"
            Case 8
                Instance = "instance_2_utes"
            Case 9
                Instance = "instance_12_duala"
            Case 10
                Instance = "instance_13_Tavi"
            Case 11
                Instance = "instance_18_sahrani"
            Case 12
                Instance = "instance_19_poda"
            Case 13
                Instance = "instance_20_fapovo"
            Case 14
                Instance = "instance_21_caribou"
            Case 15
                Instance = "instance_22_SMDsahrani"
            Case 16
                Instance = "instance_3_shapur_baf"
            Case 17
                Instance = "instance_4_zargabad"
            Case 18
                Instance = "instance_6_Dingor"
            Case 19
                Instance = "instance_7_Lingor"
        End Select

        'Checking for server batch file name changes
        ServerName = ""
        Dim CurFile = Server & "\" & "DayZ_Epoch_" & Instance & ".bat"
        If System.IO.File.Exists(CurFile) Then
            ServerName = "null"
            Var = False
        ElseIf Not System.IO.File.Exists(CurFile) Then
            Var = True
            ServerName = InputBox("It appears you have renamed your server batch file, please type its name minus the file extension")
        End If

        If ServerName = "" Then
            MsgBox(Cancel)
            System.Environment.Exit(1)
        End If

        'Setting File Paths
        Restart = Server & "\restart.bat"
        RInstance = Server & "\" & Instance & "\"
        BEC = RInstance & "BattlEye\"
        BEServer = RInstance & "BattlEye\BEServer.cfg"
        BEConfig = RInstance & "BattlEye\Bec\Config\"
        BEConfigFile = RInstance & "BattlEye\Bec\Config\Config.cfg"
        Schedule = RInstance & "BattlEye\Bec\Config\Scheduler.xml"
        BECexe = RInstance & "BattlEye\Bec\"


        'Download BEC 
        MsgBox("Downloading BattlEye Extended Controller")
        Down(BECexe)

        'Unzip BEC File
        unzip(BECexe)

        'Cleans up extracted files
        MsgBox("Configuring Settings")

        'Pre-Formatting Variables
        MSG1 = "	<cmd>say -1 " & MSG1 & "</cmd>"
        MSG2 = "	<cmd>say -1 " & MSG2 & "</cmd>"
        Dim SchedulePath As String = String.Format("	<cmd>{0}</cmd>", Restart)

        'BEServer Writer
        Do
            Directory.SetCurrentDirectory(BEC)
            WriteBEServer(BEC, Password, Ping)
            If System.IO.File.Exists(BEServer) Then
                Var2 = True
            Else
                Var2 = False
                System.IO.File.Delete("*.cfg")
            End If
        Loop Until Var2 = True

        'Ban check
        BanCheck(BEC)

        'Creating BEC Config
        CreatingBEConfig(BEConfig, Wan, Port, BEC)

        'Creating Scheduler
        If SchTimer = True Then
            NoneStaticScheduler(BEConfig, MSG1, MSG2, SchedulePath)
        Else
            Scheduler(BEConfig, MSG1, MSG2, SchedulePath)
        End If

        'Creating Restart
        CreateRestart(Server, Instance, RInstance, Var, ServerName)

        'Creating Auto Start Script
        StartAll(Server, Instance, RInstance, Var, ServerName)

        'Completion Notification
        MsgBox("INSTALL COMPLETE!" & vbCrLf & "To Start BEC and your server double click the start script in your server directory.", 0, "UEP BEC INSTALL COMPLETED")
        System.Environment.Exit(1)

    End Sub

    Private Sub BanCheck(ByVal BEC As String)
        Directory.SetCurrentDirectory(BEC)
        If Not System.IO.File.Exists(BEC & "Bans.txt") Then
            File.Create(BEC & "Bans.txt").Dispose()
        End If
    End Sub

    Private Sub unzip(ByVal BECexe As String)
        Dim sc As New Shell32.Shell()
        Dim output As Shell32.Folder = sc.NameSpace(BECexe)
        Dim input As Shell32.Folder = sc.NameSpace(BECexe & "Bec.zip")
        output.CopyHere(input.Items, 4)
        System.IO.File.Delete(BECexe & "Bec.zip")
    End Sub

    Private Sub StartAll(ByVal Server As String, ByVal Instance As String, ByVal RInstance As String, ByVal Var As String, ByVal ServerName As String)
        File.Create(Server & "\Start.bat").Dispose()
        Using objWriter As New System.IO.StreamWriter(Server & "\Start.bat")
            objWriter.WriteLine("@ echo off")
            objWriter.WriteLine("pushd %~dp0")
            objWriter.WriteLine("cd /d %CD%")
            If Var = False Then
                Dim fString As String = String.Format("start /b " & """Dayz Epoch Server""" & " /min " & """DayZ_Epoch_{0}.bat""", Instance)
                objWriter.WriteLine(fString)
            Else
                Dim fString As String = String.Format("start /b " & """Dayz Epoch Server""" & " /min " & """{0}.bat""", ServerName)
                objWriter.WriteLine(fString)
            End If
            objWriter.WriteLine("timeout 15")
            Dim fString2 As String = String.Format("cd " & """{0}BattlEye\Bec""", RInstance)
            objWriter.WriteLine(fString2)
            objWriter.WriteLine("start  " & """UEP BattlEye Extended Control""" & " /min " & """BEC.exe""")
            objWriter.WriteLine("taskkill /f /im cmd.exe")
            objWriter.WriteLine("cls")
            objWriter.WriteLine("@exit")
            objWriter.Dispose()
        End Using
    End Sub

    Private Sub WriteBEServer(ByVal BEC As String, ByVal Password As String, ByVal Ping As String)
        File.Create(BEC & "BEServer.cfg").Dispose()
        Using objWriter As New System.IO.StreamWriter(BEC & "BEServer.cfg")
            objWriter.WriteLine("RConPassword " & Password)
            objWriter.WriteLine("MaxPing " & Ping)
            objWriter.Dispose()
        End Using
    End Sub
    Private Sub Down(ByVal BECexe As String)
        If (Not System.IO.Directory.Exists(BECexe)) Then
            System.IO.Directory.CreateDirectory(BECexe)
        End If
        If (Not System.IO.Directory.Exists(BECexe & "Bec.zip")) Then
            Dim wc As New WebClient
            Dim remoteUri As Uri = New Uri("https://www.dropbox.com/s/00e4xj1wnd4v05g/Bec.zip?dl=0")
            wc.DownloadFile(remoteUri, BECexe & "Bec.zip")
        End If
    End Sub

    Private Sub CreateRestart(ByVal Server As String, ByVal Instance As String, ByVal RInstance As String, ByVal Var As String, ByVal ServerName As String)
        File.Create(Server & "\Restart.bat").Dispose()
        Using objWriter As New System.IO.StreamWriter(Server & "\Restart.bat")
            objWriter.WriteLine("@ echo off")
            objWriter.WriteLine("pushd %~dp0")
            objWriter.WriteLine("cd /d %CD%")
            objWriter.WriteLine("timeout 7")
            objWriter.WriteLine("taskkill /f /im bec.exe")
            objWriter.WriteLine("timeout 15")
            If Var = False Then
                Dim fString As String = String.Format("start /b " & """Dayz Epoch Server""" & " /min " & """DayZ_Epoch_{0}.bat""", Instance)
                objWriter.WriteLine(fString)
            Else
                Dim fString As String = String.Format("start /b " & """Dayz Epoch Server""" & " /min " & """{0}.bat""", ServerName)
                objWriter.WriteLine(fString)
            End If
            objWriter.WriteLine("timeout 15")
            Dim fString2 As String = String.Format("cd " & """{0}BattlEye\Bec""", RInstance)
            objWriter.WriteLine(fString2)
            objWriter.WriteLine("start  " & """UEP BattlEye Extended Control""" & " /min " & """BEC.exe""")
            objWriter.WriteLine("taskkill /f /im cmd.exe")
            objWriter.WriteLine("cls")
            objWriter.WriteLine("@exit")
            objWriter.Dispose()
        End Using
    End Sub

    Private Sub CreatingBEConfig(ByVal BEConfig As String, ByVal Wan As String, ByVal Port As String, ByVal BEC As String)
        File.Create(BEConfig & "Config.cfg").Dispose()
        Using objWriter As New System.IO.StreamWriter(BEConfig & "Config.cfg")
            objWriter.WriteLine("[Bec]")
            objWriter.WriteLine("IP = " & Wan)
            objWriter.WriteLine("Port = " & Port)
            objWriter.WriteLine("BePath = " & BEC)
            objWriter.WriteLine("Admins = A2_Admins.xml")
            objWriter.WriteLine("Commands = Commands.xml")
            objWriter.WriteLine("[Misc]")
            objWriter.WriteLine("Ban = 3")
            objWriter.WriteLine("ConsoleHeight	= 30")
            objWriter.WriteLine("ConsoleWidth	= 60")
            objWriter.WriteLine("Scheduler = Scheduler.xml")
            objWriter.WriteLine("KickLobbyIdlers	= 400")
            objWriter.WriteLine("ChatChannelFiles = True")
            objWriter.WriteLine("Timeout = 60")
            objWriter.WriteLine("[Reporter]")
            objWriter.WriteLine("#User = alfred")
            objWriter.WriteLine("#Password = 123456")
            objWriter.Dispose()
        End Using
    End Sub

    Private Sub Scheduler(ByVal BEConfig As String, ByVal MSG1 As String, ByVal MSG2 As String, ByVal SchedulePath As String)
        File.Create(BEConfig & "Scheduler.xml").Dispose()
        Using ObjWriter As New System.IO.StreamWriter(BEConfig & "Scheduler.xml")
            ObjWriter.WriteLine("<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>")
            ObjWriter.WriteLine("<Scheduler>")
            ObjWriter.WriteLine("<job id='0'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>000600</start>")
            ObjWriter.WriteLine("	<runtime>000600</runtime>")
            ObjWriter.WriteLine("	<loop>-1</loop>")
            ObjWriter.WriteLine(MSG1)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='1'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>000600</start>")
            ObjWriter.WriteLine("	<runtime>000600</runtime>")
            ObjWriter.WriteLine("	<loop>-1</loop>")
            ObjWriter.WriteLine(MSG2)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<!-- 06:00 AM restart -->")
            ObjWriter.WriteLine("<job id='2'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>05:45:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 15 minutes</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='3'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>05:55:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 5 minute, log out now to prevent item loss!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='4'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>05:59:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 1 minute, Log out now!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='5'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>06:00:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>#shutdown</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='6'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>06:00:10</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine(SchedulePath)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("<!-- End 06:00 AM restart -->")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<!-- 09:00 AM restart -->")
            ObjWriter.WriteLine("<job id='7'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>08:45:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 15 minutes</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='8'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>08:55:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 5 minute, log out now to prevent item loss!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='9'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>08:59:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 1 minute, Log out now!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='10'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>09:00:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>#shutdown</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='11'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>09:00:10</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine(SchedulePath)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("<!-- End 09:00 AM restart -->")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<!-- 12:00 PM restart -->")
            ObjWriter.WriteLine("<job id='12'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>11:45:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 15 minutes</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='13'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>11:55:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 5 minute, log out now to prevent item loss!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='14'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>11:59:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 1 minute, Log out now!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='15'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>12:00:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>#shutdown</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='16'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>12:00:10</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine(SchedulePath)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("<!-- End 12:00 PM restart -->")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<!-- 03:00 PM restart -->")
            ObjWriter.WriteLine("<job id='17'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>14:45:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 15 minutes</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='18'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>14:55:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 5 minute, log out now to prevent item loss!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='19'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>14:59:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 1 minute, Log out now!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='20'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>15:00:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>#shutdown</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='21'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>15:00:10</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine(SchedulePath)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("<!-- End 03:00 PM restart -->")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<!-- 06:00 PM restart -->")
            ObjWriter.WriteLine("<job id='22'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>17:45:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 15 minutes</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='23'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>17:55:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 5 minute, log out now to prevent item loss!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='24'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>17:59:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 1 minute, Log out now!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='25'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>18:00:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>#shutdown</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='26'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>18:00:10</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine(SchedulePath)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("<!-- End 06:00 PM restart -->")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<!-- 09:00 PM restart -->")
            ObjWriter.WriteLine("<job id='27'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>20:45:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 15 minutes</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='28'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>20:55:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 5 minute, log out now to prevent item loss!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='29'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>20:59:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 1 minute, Log out now!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='30'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>21:00:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>#shutdown</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='31'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>21:00:10</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine(SchedulePath)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("<!-- End 09:00 PM restart -->")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<!-- 12:00 AM restart -->")
            ObjWriter.WriteLine("<job id='32'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>23:45:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 15 minutes</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='33'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>23:55:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 5 minute, log out now to prevent item loss!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='34'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>23:59:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 1 minute, Log out now!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='35'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>00:00:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>#shutdown</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='36'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>00:00:10</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine(SchedulePath)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("<!-- End 12:00 AM restart -->")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<!-- 03:00 AM restart -->")
            ObjWriter.WriteLine("<job id='37'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>02:45:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 15 minutes</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='38'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>02:55:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 5 minute, log out now to prevent item loss!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='39'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>02:59:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 1 minute, Log out now!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='40'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>03:00:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>#shutdown</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='41'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>03:00:10</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine(SchedulePath)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("<!-- End 03:00 AM restart -->")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("</Scheduler>")
            ObjWriter.Dispose()
        End Using
    End Sub

    Private Sub NoneStaticScheduler(ByVal BEConfig As String, ByVal MSG1 As String, ByVal MSG2 As String, ByVal SchedulePath As String)
        File.Create(BEConfig & "Scheduler.xml").Dispose()
        Using ObjWriter As New System.IO.StreamWriter(BEConfig & "Scheduler.xml")
            ObjWriter.WriteLine("<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>")
            ObjWriter.WriteLine("<Scheduler>")
            ObjWriter.WriteLine("<job id='0'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>000600</start>")
            ObjWriter.WriteLine("	<runtime>000600</runtime>")
            ObjWriter.WriteLine("	<loop>-1</loop>")
            ObjWriter.WriteLine(MSG1)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='1'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>000600</start>")
            ObjWriter.WriteLine("	<runtime>000600</runtime>")
            ObjWriter.WriteLine("	<loop>-1</loop>")
            ObjWriter.WriteLine(MSG2)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<!-- Begining timer restart -->")
            ObjWriter.WriteLine("<job id='2'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>05:45:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 15 minutes</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='3'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>05:55:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 5 minute, log out now to prevent item loss!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='4'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>05:59:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>say -1 This server will restart in 1 minute, Log out now!</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='5'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>06:00:00</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine("	<cmd>#shutdown</cmd>")
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("<job id='6'>")
            ObjWriter.WriteLine("	<day>1,2,3,4,5,6,7</day>")
            ObjWriter.WriteLine("	<start>06:00:10</start>")
            ObjWriter.WriteLine("	<runtime>000000</runtime>")
            ObjWriter.WriteLine("	<loop>0</loop>")
            ObjWriter.WriteLine(SchedulePath)
            ObjWriter.WriteLine("</job>")
            ObjWriter.WriteLine("")
            ObjWriter.WriteLine("</Scheduler>")
            ObjWriter.Dispose()
        End Using
    End Sub
End Module
