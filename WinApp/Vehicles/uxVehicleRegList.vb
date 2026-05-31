Public Class uxVehicleRegList 

    Public Sub New(ByVal IdVehicle As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim VehRegList As VehicleRegistrationList = VehicleRegistrationList.GetVehicleRegistrationList(IdVehicle)
        VehicleRegistrationListBindingSource.DataSource = VehRegList
        VehicleRegistrationListGridControl.DataSource = VehicleRegistrationListBindingSource
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Me.Close()

    End Sub
End Class