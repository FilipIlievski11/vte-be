' Developer Express Code Central Example:
' How to change default filter operator for autofilter mechanism of the LookUpEdit
' 
' This example demonstrates how to create the RepositoryItemLookUpEdit descendant
' and override its CreateDataAdapter method, to provide your own filter mechanism.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E336

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Drawing
Imports System.ComponentModel
Imports DevExpress.XtraEditors.ListControls
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers

Namespace FancyLookupEdit



  <UserRepositoryItem("RegisterCustomEdit")> _
  Public Class CustomRepositoryItemLookupEdit
    Inherits RepositoryItemLookUpEdit
    Public Const CustomEditName As String = "MyLookUpdit"

    Shared Sub New()
      RegisterCustomEdit()
    End Sub

    Public Overloads Overrides ReadOnly Property EditorTypeName() As String
      Get
        Return CustomEditName
      End Get
    End Property

    Public Shared Sub RegisterCustomEdit()
      EditorRegistrationInfo.Default.Editors.Add( _
        New EditorClassInfo(CustomEditName, _
        GetType(CustomLookUpEdit), _
        GetType(CustomRepositoryItemLookupEdit), _
        GetType(LookUpEditViewInfo), _
        New ButtonEditPainter(), True, _
        EditImageIndexes.LookUpEdit))
    End Sub

    Public Overloads Overrides Sub Assign(ByVal item As RepositoryItem)
      BeginUpdate()
      Try
        MyBase.Assign(item)
        Dim source As CustomRepositoryItemLookupEdit = DirectCast(item, CustomRepositoryItemLookupEdit)
      Finally
        EndUpdate()
      End Try
    End Sub

    Protected Overloads Overrides Function CreateDataAdapter() As LookUpListDataAdapter
      Return New MyLookUpListDataAdapter(Me)
    End Function
  End Class

  Public Class CustomLookUpEdit
    Inherits LookUpEdit
    Shared Sub New()
      CustomRepositoryItemLookupEdit.RegisterCustomEdit()
    End Sub

    Public Overloads Overrides ReadOnly Property EditorTypeName() As String
      Get
        Return CustomRepositoryItemLookupEdit.CustomEditName
      End Get
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Shadows ReadOnly Property Properties() As CustomRepositoryItemLookupEdit
      Get
        Return DirectCast(MyBase.Properties, CustomRepositoryItemLookupEdit)
      End Get
    End Property
  End Class

  Public Class MyLookUpListDataAdapter
    Inherits LookUpListDataAdapter
    Public Sub New(ByVal item As CustomRepositoryItemLookupEdit)
      MyBase.New(item)
    End Sub

    Protected Overloads Overrides Function CreateFilterExpression() As String
      If String.IsNullOrEmpty(FilterPrefix) Then
        Return String.Empty
      End If
      Dim likeClause As String = DevExpress.Data.Filtering.Helpers.LikeData.CreateStartsWithPattern(FilterPrefix)
      Return New BinaryOperator(FilterField, "%" + likeClause + "%", BinaryOperatorType.[Like]).ToString()
    End Function
  End Class

End Namespace