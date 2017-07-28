Imports System.IO
Imports System.Net.Sockets
Imports System.Threading

Public Class Window
    Dim Listener As New TcpListener(65535)
    Dim Client As New TcpClient
    Dim Message As String = ""

    Dim ListenerThread As New Thread(New ThreadStart(AddressOf Listening))

    Private Sub Listening()
        Listener.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Listener.Pending = True Then
            Message = ""
            Client = Listener.AcceptTcpClient()

            Dim Reader As New StreamReader(Client.GetStream())
            While Reader.Peek > -1
                Message = Message + Convert.ToChar(Reader.Read()).ToString
            End While
            With RichTextBox1
                .ForeColor = Color.Black
                .Text += Message + vbCrLf
            End With
        End If
    End Sub
    Private Sub btnSend_Click(ByVal sender As System.Object,
            ByVal e As System.EventArgs) Handles btnSend.Click
        If txtName.Text = "" Or cmbAddress.Text = "" Then
            'Check to make sure that the user has entered 
            'a display name, and a client IP Address
            'If Not, Show a Message Box
            MessageBox.Show("All Fields must be Filled",
                            "Error Sending Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Try
                Client = New TcpClient(cmbAddress.Text, 65535)

                'Declare the Client as an IP Address. 
                'Must be in the Correct form. eg. 000.0.0.0
                Dim Writer As New StreamWriter(Client.GetStream())
                Writer.Write(txtName.Text & " Says:  " & txtmessage.Text)
                Writer.Flush()

                'Write the Message in the stream

                RichTextBox1.Text += (txtName.Text & " Says:  " & txtmessage.Text) + vbCrLf
                txtmessage.Text = ""
            Catch ex As Exception
                Console.WriteLine(ex)
                Dim Errorresult As String = ex.Message
                MessageBox.Show(Errorresult & vbCrLf & vbCrLf &
                                "Please Review Client Address",
                                "Error Sending Message",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If
    End Sub

    Private Sub Window_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Listening()
        Timer1.Enabled = True
    End Sub
End Class