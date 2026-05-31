Imports System.IO
Imports System.Drawing
Imports System.Runtime.InteropServices

Public Class dijTwain

    Private WithEvents _tw As openTwain.Twain
    Private Property TW() As openTwain.Twain
        Get
            If _tw Is Nothing Then
                _tw = New openTwain.Twain(_SupressKnownDialogs)
            End If
            Return _tw
        End Get
        Set(ByVal value As openTwain.Twain)
            _tw = value
        End Set
    End Property

    Private _activeProfileName As String = String.Empty
    Private _ActiveProfile As New openTwain.Profile
    Private _activeSourceName As String = String.Empty

    Private _tvPreviousWidth As Int32

    Private profilesLoaded As Boolean = False
    Private InProgress As Boolean = False

    Private FilterString As String = "Image files |*.bmp;*.gif;*.jpg;*.jpeg*.tif;*.tiff*.png"
    Private _SupressKnownDialogs As Boolean = True

    Private NewTwStarted As Boolean = True

    Private _RootNode As DevExpress.XtraTreeList.Nodes.TreeListNode = Nothing
    Private _DeviceNode As DevExpress.XtraTreeList.Nodes.TreeListNode = Nothing
    Private _ProfileNode As DevExpress.XtraTreeList.Nodes.TreeListNode = Nothing


    'Private newProfile As ProfileEditor
    Private WithEvents _attachment As Attachment 'RequestAttachment
    Public ReadOnly Property Attachment() As Attachment 'RequestAttachment
        Get
            Return _attachment
        End Get
    End Property
    Private WithEvents _attachmentTypeList As AttachmentTypeList

    Public Sub New(ByVal attachment As Attachment) 'RequestAttachment)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        _attachmentTypeList = AttachmentTypeList.GetAttachmentTypeList
        Me.AttachmentTypeListBindingSource.DataSource = _attachmentTypeList
        _attachment = attachment
        Me.DocumentAttachmentBindingSource.DataSource = _attachment

    End Sub

    Private Sub dijTwain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            UpdateGui()
            LoadProfiles()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        SetupActiveProfile()
    End Sub


    Private Sub LoadProfiles()

        Try
            '//-----setup device childred
            TW.RefreshSources()
            For Each ds As String In TW.AvailableSources
                '  _DeviceNode = Me.DevicesTreeList.AppendNode(New Object() {"Twain Manager", ds, ds}, _RootNode, "_DeviceNode")
                Me.DevicesListBox.Items.Add(ds)
            Next

            '//-----add each profile under the appropriate device

            'the 'key' is the path to the profile
            TW.RefreshAvailableProfiles()
            Dim keyProfilePath As Dictionary(Of String, String).ValueCollection = _
                      TW.AvailableProfiles.Values

            For Each ProfilePath As String In keyProfilePath
                '_ActiveProfile = openTwain.Profile.Load(ProfilePath)
                '_ProfileNode = Me.DevicesTreeList.AppendNode(New Object() {"Twain Manager", _ActiveProfile.ProfileName}, _DeviceNode, "_ProfileNode")


                '_RootNode.Nodes.Add(_ProfileNode)


            Next

            profilesLoaded = Me.DevicesListBox.Items.Count > 0

        Catch ex As Exception
            Trace.WriteLine(ex.ToString)
        End Try

    End Sub

    Private Sub SetupActiveProfile()
        With _ActiveProfile
            .CombineImages = True
            .CreateSubDirectoryForEachAcquire = True
            .ImageAcquireFormat = openTwain.Enumerations.ImageFileFormat.BMP
            .ImageOutputFormat = openTwain.Enumerations.ImageFileFormat.BMP
            .ModalUi = False
            '.OutputDirectory = "c:\Temp"
            .ShowIndicators = True
            .ShowUi = False
        End With

    End Sub

    Private Sub DevicesListBox_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DevicesListBox.SelectedValueChanged
        If Not Me.DevicesListBox.SelectedValue Is Nothing Then
            _activeSourceName = Me.DevicesListBox.SelectedValue.ToString
        End If

    End Sub


    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
        Acquire(Me._activeSourceName)
    End Sub



#Region " openTwain event handlers and helpers  "

    ''' <summary>
    ''' User clicked OK on a vendor dialog
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub tw_CloseDsOkEvent() Handles _tw.CloseDsOkEvent
        Me.StatusBarStaticItem.Caption = "Скенерот е спремен"
        Enabled = True
        _ActiveProfile.CustomDsData = _tw.LastCustomDsData
        Me.Enabled = True
    End Sub

    ''' <summary>
    ''' User selected Cancel from a vendor dialog
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub tw_CloseRequestEvent() Handles _tw.CloseRequestEvent
        Me.StatusBarStaticItem.Caption = "Скенирањето е завршено"
        InProgress = False
        Me.Enabled = Not InProgress
    End Sub

    ''' <summary>
    ''' Last triplet failed
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub tw_Failure() Handles _tw.FailureEvent
        'TODO: lblCurrentOperation.Text = _tw.FailedReason
        Me.StatusBarStaticItem.Caption = "Скенирањето не е успешно, поради: " & _tw.FailedReason
        InProgress = False
        Me.Enabled = Not InProgress
    End Sub

    ''' <summary>
    ''' An image has been transfered
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub tw_TransferReady() Handles _tw.TransferReadyEvent

        If Not InProgress Then
            Me.StatusBarStaticItem.Caption = "Скенирам..."
            Return
        End If

        Try
            Me.StatusBarStaticItem.Caption = "Зимам податоци од скенерот"
            InProgress = False
            Me.Enabled = Not InProgress
            If TW.Images Is Nothing Then Return

            '//----- Post Acquire manipulation here
            Dim PostAcquire As New BitmapManip.BitmapManip(TW.Images, _
                                                           _ActiveProfile.ImageOutputFormat, _
                                                           _ActiveProfile.CombineImages)

            '//----- 
            Dim picList As New List(Of String)
            picList = PostAcquire.retList
            PostAcquire.Dispose()
            PostAcquire = Nothing
            '//----- 

            If Not picList.Count > 0 Then
                Trace.WriteLine("No results")
                Return
            End If

            'Me.PictureEdit1.Image = Image.FromFile(picList.Item(0))
            _attachment.AttachmentPath = picList.Item(0)
            'Dim newpic As New ImageEditor(picList) 'PostAcquire.retList) 'fileArraylist)
            'If IsMdiContainer Then
            '  newpic.MdiParent = Me
            'End If

            'newpic.Show()
            Me.StatusBarStaticItem.Caption = "Скенирањето е завршено"
        Catch ex As Exception
            'LogLog.logException(ex)
            Trace.WriteLine(ex.ToString)
        End Try

    End Sub
    Private Sub tw_EndingScan() Handles _tw.EndScanEvent
        Me.StatusBarStaticItem.Caption = "Скенирањето e е успешно"
    End Sub
    Private Sub tw_UnknownDialogDetected(ByVal foundVia As String, _
                                         ByVal dialog As Monitor.KnownDialog) _
                                         Handles _tw.UnknownDialogDetected
        'TODO: let the user know 
        'TODO:  UpdateUnknownDialogCountLabel(_tw.DialogMonitor.UnKnownDialogList.Items.Count & _
        '                                     " Unknown dialogs")
        Me.StatusBarStaticItem.Caption = "Детектиран е дијалог"
    End Sub

    Private Sub UpdateGui()
        SetTransferMode(_ActiveProfile.TransferMech)
        SetShowUI(_ActiveProfile.ShowUi)
        'lblCurrentIdentity.Text = TW.CurrentIdentityName
    End Sub

    Private Sub SetTransferMode(ByVal mode As String)
        _ActiveProfile.TransferMech = mode
    End Sub

    Private Function GetTransferMode() As String
        Return openTwain.Enumerations.XferMech.Native.ToString
    End Function

    Private Sub SetShowUI(ByVal show As Boolean)
        _ActiveProfile.ShowUi = show
    End Sub
    Private Sub Acquire(ByVal profile As openTwain.Profile)

        'CreateTw()

        TW.Acquire(profile, False)
        postAcquire()

    End Sub
    Private Sub Acquire(ByVal sourceName As String)

        'CreateTw()

        TW.Acquire(sourceName, False)
        postAcquire()

    End Sub
    Private Sub Acquire()
        TW.Acquire(False)
        postAcquire()
    End Sub

    Private Sub postAcquire()
        'CreateTw()
        If TW.Failed = False Then
            InProgress = True
            Me.Enabled = Not InProgress
        End If
    End Sub

#End Region



    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try

            If Not (TW.Images Is Nothing) Then
                Dim fl As New FileInfo(TW.Images(0).ToString)
                fl.Delete() '.CopyTo(objOpcii.PictureServerPath & "\", True)
            End If
            If Not (Me.PictureEdit1.Image Is Nothing) Then

                'ova go dodadov
                Dim _attachments As Attachments = Attachments.GetAttachments
                'do tuka

                Dim picGUID As Guid = Guid.NewGuid
                Dim picPath As String = objCurentTehExamOrganization.PictureServerPath & "\" & picGUID.ToString & ".jpg"
                Me.PictureEdit1.Image.Save(picPath, System.Drawing.Imaging.ImageFormat.Jpeg)
                _attachment.AttachmentPath = picPath
                '_attachments.Save()


                Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End Try

        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            If Not (TW.Images Is Nothing) Then
                Dim fl As New FileInfo(TW.Images(0).ToString)
                fl.Delete() '.CopyTo(objOpcii.PictureServerPath & "\", True)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Try
            Me.OpenFileDialog1.Filter = FilterString
            If (Me.OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK) Then
                _attachment.AttachmentPath = Me.OpenFileDialog1.FileName
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


End Class