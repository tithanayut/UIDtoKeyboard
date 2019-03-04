Imports PCSC
Imports PCSC.Iso7816
Imports PCSC.Monitoring

Public Class frmMain

    Private Shared ReadOnly _contextFactory As IContextFactory = ContextFactory.Instance
    Private _hContext As ISCardContext
    Dim readerName As String
    Dim readingMode As String
    Dim isstart As Boolean = False

    Function loadReaderList()
        Dim readerList As String()
        Try
            cbxReaderList.DataSource = Nothing

            _hContext = _contextFactory.Establish(SCardScope.System)
            readerList = _hContext.GetReaders()
            _hContext.Release()

            If readerList.Length > 0 Then
                cbxReaderList.DataSource = readerList
            Else
                MessageBox.Show("No card reader detected!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            Return True
        Catch ex As Exceptions.PCSCException
            MessageBox.Show("Error: getReaderList() : " & ex.Message & " (" & ex.SCardError.ToString() & ")")
            Return False
        End Try
    End Function

    Dim monitor

    Private Sub startMonitor()
        Dim monitorFactory As MonitorFactory = MonitorFactory.Instance
        monitor = monitorFactory.Create(SCardScope.System)
        AttachToAllEvents(monitor)
        monitor.Start(cbxReaderList.Text)

        readerName = cbxReaderList.Text
        readingMode = txtReadingMode.Text
    End Sub

    Private Sub AttachToAllEvents(monitor As ISCardMonitor)
        AddHandler monitor.CardInserted, AddressOf cardInit
    End Sub

    Sub cardInit(eventName As SCardMonitor, unknown As CardStatusEventArgs)
        SendUID()
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadReaderList()
    End Sub

    Private Sub btnRefreshReader_Click(sender As Object, e As EventArgs) Handles btnRefreshReader.Click
        loadReaderList()
    End Sub

    Private Sub btnStartMonitor_Click(sender As Object, e As EventArgs) Handles btnStartMonitor.Click
        If txtReadingMode.Text <> 1 AndAlso txtReadingMode.Text <> 2 Then
            MessageBox.Show("Error: Reading mode not macth the preset.")
        Else
            If isstart = True Then
                monitor.Cancel()
            End If
            startMonitor()
            isstart = True
        End If
    End Sub

    Private Sub btnStopMonitor_Click(sender As Object, e As EventArgs) Handles btnStopMonitor.Click
        If isstart = True Then
            monitor.Cancel()
        End If
    End Sub

    Function SendUID()
        Try
            Using context = _contextFactory.Establish(SCardScope.System)
                Using rfidReader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any)
                    Using rfidReader.Transaction(SCardReaderDisposition.Leave)

                        Dim apdu As Byte() = {&HFF, &HCA, &H0, &H0, &H4}
                        Dim sendPci = SCardPCI.GetPci(rfidReader.Protocol)
                        Dim receivePci = New SCardPCI()

                        Dim receiveBuffer = New Byte(255) {}
                        Dim command = apdu.ToArray()
                        Dim bytesReceived = rfidReader.Transmit(sendPci, command, command.Length, receivePci, receiveBuffer, receiveBuffer.Length)
                        Dim responseApdu = New ResponseApdu(receiveBuffer, bytesReceived, IsoCase.Case2Short, rfidReader.Protocol)

                        If readingMode = 2 Then
                            Dim uid As Byte() = New Byte(3) {}
                            Dim revuid As Byte() = New Byte(3) {}
                            Array.Copy(responseApdu.GetData(), uid, 4)
                            Array.Copy(uid, revuid, 4)
                            Array.Reverse(revuid, 0, 4)

                            Dim uid2 As String = BitConverter.ToString(revuid)
                            uid2 = uid2.Replace("-", "")

                            SendKeys.SendWait(uid2 + "{ENTER}")
                        Else
                            Dim uid As String = BitConverter.ToString(responseApdu.GetData())
                            uid = uid.Replace("-", "")

                            SendKeys.SendWait(uid + "{ENTER}")
                        End If
                    End Using
                End Using
            End Using
        Catch
            'Error Handling should be developed
        End Try

        Return True
    End Function

End Class
