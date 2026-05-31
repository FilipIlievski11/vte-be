namespace VTE.WPF.Views.Pages;

using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class DocumentDetailPage : UserControl
{
    private readonly long? _documentId;
    private Document? _document;

    public DocumentDetailPage(long? documentId)
    {
        _documentId = documentId;
        InitializeComponent();
        ApplyLanguage();
        Loaded += async (_, _) =>
        {
            await LoadDropdownsAsync();
            LoadDocument();
        };
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        LblDocumentType.Text = Strings.DocumentTypeRequired;
        LblCustomerVehicle.Text = Strings.CustomerVehicleRequired;
        LblDateCreated.Text = Strings.DateCreated;
        LblDateEnded.Text = Strings.DateEnded;
        LblNote.Text = Strings.Note;
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
    }

    private System.Threading.Tasks.Task LoadDropdownsAsync()
    {
        PickerDocumentType.SetItems(Services.LookupCache.DocumentTypes);
        return System.Threading.Tasks.Task.CompletedTask;
    }

    private void LoadDocument()
    {
        try
        {
            if (_documentId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _document = db.Documents.AsNoTracking().FirstOrDefault(d => d.Id == _documentId.Value);

                if (_document == null)
                {
                    MessageBox.Show("Document not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                PageTitle.Text = $"{Strings.EditDocumentTitle} #{_document.Id}";
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _document = new Document();
                PageTitle.Text = Strings.NewDocumentTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
                TxtDateCreated.Text = "(will be set on save)";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading document:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_document == null) return;

        PickerDocumentType.SetValue(_document.DocumentTypeId, Services.LookupCache.GetName(Services.LookupCache.DocumentTypes, _document.DocumentTypeId));
        if (_document.CustomerVehicleRelationId > 0)
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var rel = db.CustomerVehicleRelations
                .Include(r => r.Customer).Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == _document.CustomerVehicleRelationId);
            if (rel != null)
                RelationPicker.SetValue(rel.Id,
                    $"{rel.Customer.FirstName} {rel.Customer.LastName} - {rel.Vehicle.LastRegistrationNumber ?? rel.Vehicle.ShellNumber}");
        }
        TxtDateCreated.Text = _document.CreatedAt.ToString("dd.MM.yyyy HH:mm");
        DpDateEnded.SelectedDate = _document.DateEnded;
        TxtNote.Text = _document.Note ?? "";
    }

    private void ReadFields()
    {
        if (_document == null) return;

        _document.DocumentTypeId = PickerDocumentType.SelectedId ?? 0L;
        _document.CustomerVehicleRelationId = RelationPicker.SelectedRelationId ?? 0L;
        _document.DateEnded = DpDateEnded.SelectedDate;
        _document.Note = string.IsNullOrWhiteSpace(TxtNote.Text) ? null : TxtNote.Text.Trim();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ReadFields();

            if (_document!.DocumentTypeId == 0)
            {
                MessageBox.Show("Document Type is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_document.CustomerVehicleRelationId == 0)
            {
                MessageBox.Show("Customer - Vehicle is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_documentId.HasValue)
            {
                var existing = db.Documents.Find(_documentId.Value);
                if (existing == null)
                {
                    MessageBox.Show("Document no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.DocumentTypeId = _document.DocumentTypeId;
                existing.CustomerVehicleRelationId = _document.CustomerVehicleRelationId;
                existing.DateEnded = _document.DateEnded;
                existing.Note = _document.Note;
                existing.ModifiedAt = DateTime.UtcNow;
            }
            else
            {
                _document.CreatedAt = DateTime.UtcNow;
                _document.CreatedByUserId = 1;
                db.Documents.Add(_document);
            }

            db.SaveChanges();

            MessageBox.Show("Document saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving document:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (!_documentId.HasValue) return;

        var result = MessageBox.Show(
            "Are you sure you want to delete this document?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var document = db.Documents.Find(_documentId.Value);
            if (document != null)
            {
                db.Documents.Remove(document);
                db.SaveChanges();
            }

            MessageBox.Show("Document deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting document:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        NavigateBack();
    }

    private void NavigateBack()
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new DocumentListPage();
        }
    }
}
