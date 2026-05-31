namespace VTE.WPF.Views.Pages;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class PaymentDetailPage : UserControl
{
    private readonly long? _paymentId;
    private PaymentDocument? _payment;
    private ObservableCollection<LineItemRow> _lineItems = [];

    public PaymentDetailPage(long? paymentId)
    {
        _paymentId = paymentId;
        InitializeComponent();
        ApplyLanguage();
        Loaded += async (_, _) =>
        {
            await LoadDropdownsAsync();
            LoadPayment();
        };
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        LblDocumentNumber.Text = Strings.DocumentNumberRequired;
        LblPaymentDate.Text = Strings.PaymentDate;
        LblDueDate.Text = Strings.DueDate;
        LblDiscount.Text = Strings.DiscountPercent;
        LblRelation.Text = Strings.CustomerVehicleRelationRequired;
        LblPaymentType.Text = Strings.PaymentTypeRequired;
        ChkIsPaid.Content = Strings.IsPaidLabel;
        ChkIsCancelled.Content = Strings.IsCancelledLabel;
        LblNote.Text = Strings.Note;
        LblLineItems.Text = Strings.LineItems;
        ColDescription.Header = Strings.Description;
        ColQuantity.Header = Strings.Quantity;
        ColUnitPrice.Header = Strings.UnitPrice;
        ColVAT.Header = Strings.VATPercent;
        ColTotal.Header = Strings.TotalAmount;
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
        BtnPrint.Content = Strings.PrintInvoice;
        BtnPrintReceipt.Content = Strings.PrintReceipt;
    }

    private System.Threading.Tasks.Task LoadDropdownsAsync()
    {
        PickerPaymentType.SetItems(Services.LookupCache.PaymentTypes);
        return System.Threading.Tasks.Task.CompletedTask;
    }

    private void LoadPayment()
    {
        try
        {
            if (_paymentId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _payment = db.PaymentDocuments
                    .Include(p => p.LineItems)
                    .AsNoTracking()
                    .FirstOrDefault(p => p.Id == _paymentId.Value);

                if (_payment == null)
                {
                    MessageBox.Show("Payment not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                PageTitle.Text = $"{Strings.EditPaymentTitle} - {_payment.DocumentNumber}";
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _payment = new PaymentDocument();
                PageTitle.Text = Strings.NewPaymentTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
                _lineItems = new ObservableCollection<LineItemRow>();
                LineItemsGrid.ItemsSource = _lineItems;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading payment:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_payment == null) return;

        TxtDocumentNumber.Text = _payment.DocumentNumber;
        DpPaymentDate.SelectedDate = _payment.PaymentDate;
        DpDueDate.SelectedDate = _payment.DueDate;
        TxtDiscountPercent.Text = _payment.DiscountPercent.ToString(CultureInfo.InvariantCulture);
        ChkIsPaid.IsChecked = _payment.IsPaid;
        ChkIsCancelled.IsChecked = _payment.IsCancelled;
        TxtNote.Text = _payment.Note ?? "";
        if (_payment.CustomerVehicleRelationId > 0)
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var rel = db.CustomerVehicleRelations
                .Include(r => r.Customer).Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == _payment.CustomerVehicleRelationId);
            if (rel != null)
                RelationPicker.SetValue(rel.Id,
                    $"{rel.Customer.FirstName} {rel.Customer.LastName} - {rel.Vehicle.LastRegistrationNumber ?? rel.Vehicle.ShellNumber}");
        }
        PickerPaymentType.SetValue(_payment.PaymentTypeId, Services.LookupCache.GetName(Services.LookupCache.PaymentTypes, _payment.PaymentTypeId));

        _lineItems = new ObservableCollection<LineItemRow>(
            _payment.LineItems.Select(li => new LineItemRow
            {
                Id = li.Id,
                Description = li.Description,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
                VATPercent = li.VATPercent
            }));
        LineItemsGrid.ItemsSource = _lineItems;
    }

    private void ReadFields()
    {
        if (_payment == null) return;

        _payment.DocumentNumber = TxtDocumentNumber.Text.Trim();
        _payment.PaymentDate = DpPaymentDate.SelectedDate;
        _payment.DueDate = DpDueDate.SelectedDate;

        if (decimal.TryParse(TxtDiscountPercent.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var disc))
            _payment.DiscountPercent = disc;
        else
            _payment.DiscountPercent = 0;

        _payment.IsPaid = ChkIsPaid.IsChecked == true;
        _payment.IsCancelled = ChkIsCancelled.IsChecked == true;
        _payment.Note = string.IsNullOrWhiteSpace(TxtNote.Text) ? null : TxtNote.Text.Trim();

        if (RelationPicker.SelectedRelationId is long cvrId)
            _payment.CustomerVehicleRelationId = cvrId;

        _payment.PaymentTypeId = PickerPaymentType.SelectedId ?? 0;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ReadFields();

            if (string.IsNullOrWhiteSpace(_payment!.DocumentNumber))
            {
                MessageBox.Show("Document Number is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtDocumentNumber.Focus();
                return;
            }

            if (_payment.CustomerVehicleRelationId <= 0)
            {
                MessageBox.Show("Customer-Vehicle Relation is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                RelationPicker.Focus();
                return;
            }

            if (_payment.PaymentTypeId <= 0)
            {
                MessageBox.Show("Payment Type is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                PickerPaymentType.Focus();
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_paymentId.HasValue)
            {
                var existing = db.PaymentDocuments
                    .Include(p => p.LineItems)
                    .FirstOrDefault(p => p.Id == _paymentId.Value);

                if (existing == null)
                {
                    MessageBox.Show("Payment no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.DocumentNumber = _payment.DocumentNumber;
                existing.PaymentDate = _payment.PaymentDate;
                existing.DueDate = _payment.DueDate;
                existing.DiscountPercent = _payment.DiscountPercent;
                existing.IsPaid = _payment.IsPaid;
                existing.IsCancelled = _payment.IsCancelled;
                existing.Note = _payment.Note;
                existing.CustomerVehicleRelationId = _payment.CustomerVehicleRelationId;
                existing.PaymentTypeId = _payment.PaymentTypeId;
                existing.ModifiedAt = DateTime.UtcNow;

                // Update line items: remove old, add current
                db.RemoveRange(existing.LineItems);

                foreach (var row in _lineItems.Where(r => !string.IsNullOrWhiteSpace(r.Description)))
                {
                    existing.LineItems.Add(new PaymentLineItem
                    {
                        PaymentDocumentId = existing.Id,
                        Description = row.Description,
                        Quantity = row.Quantity,
                        UnitPrice = row.UnitPrice,
                        VATPercent = row.VATPercent,
                        PaymentItemId = 1, // default catalog item
                        SortOrder = 0
                    });
                }
            }
            else
            {
                _payment.CreatedAt = DateTime.UtcNow;

                // Add line items from grid
                foreach (var row in _lineItems.Where(r => !string.IsNullOrWhiteSpace(r.Description)))
                {
                    _payment.LineItems.Add(new PaymentLineItem
                    {
                        Description = row.Description,
                        Quantity = row.Quantity,
                        UnitPrice = row.UnitPrice,
                        VATPercent = row.VATPercent,
                        PaymentItemId = 1, // default catalog item
                        SortOrder = 0
                    });
                }

                db.PaymentDocuments.Add(_payment);
            }

            db.SaveChanges();

            MessageBox.Show("Payment saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving payment:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (!_paymentId.HasValue) return;

        var result = MessageBox.Show(
            "Are you sure you want to delete this payment?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var payment = db.PaymentDocuments
                .Include(p => p.LineItems)
                .FirstOrDefault(p => p.Id == _paymentId.Value);

            if (payment != null)
            {
                db.RemoveRange(payment.LineItems);
                db.PaymentDocuments.Remove(payment);
                db.SaveChanges();
            }

            MessageBox.Show("Payment deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting payment:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Print_Click(object sender, RoutedEventArgs e)
    {
        if (_paymentId == null) { MessageBox.Show(Strings.SaveFirst); return; }

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var payment = db.PaymentDocuments
                .Include(p => p.LineItems)
                .Include(p => p.CustomerVehicleRelation).ThenInclude(r => r.Customer).ThenInclude(c => c.LivingCity)
                .Include(p => p.CustomerVehicleRelation).ThenInclude(r => r.Customer).ThenInclude(c => c.LivingAddressStreet)
                .Include(p => p.CustomerVehicleRelation).ThenInclude(r => r.Vehicle)
                .FirstOrDefault(p => p.Id == _paymentId);

            if (payment == null) return;

            var customer = payment.CustomerVehicleRelation.Customer;
            var vehicle = payment.CustomerVehicleRelation.Vehicle;
            var org = db.TechnicalExamOrganizations.FirstOrDefault();
            var orgName = org?.Name ?? "VTE";

            var pdfBytes = VTE.WPF.Services.InvoicePdfGenerator.Generate(
                payment, customer, vehicle, payment.LineItems.ToList(), orgName, org?.Address);

            var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"Invoice_{payment.DocumentNumber.Replace("/", "_")}.pdf");
            System.IO.File.WriteAllBytes(tempPath, pdfBytes);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating PDF:\n{ex.Message}", Strings.Error);
        }
    }

    private void PrintReceipt_Click(object sender, RoutedEventArgs e)
    {
        if (_paymentId == null) { MessageBox.Show(Strings.SaveFirst); return; }

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var payment = db.PaymentDocuments
                .Include(p => p.LineItems)
                .Include(p => p.CustomerVehicleRelation).ThenInclude(r => r.Customer)
                .Include(p => p.CustomerVehicleRelation).ThenInclude(r => r.Vehicle)
                .FirstOrDefault(p => p.Id == _paymentId);

            if (payment == null) return;

            var customer = payment.CustomerVehicleRelation.Customer;
            var vehicle = payment.CustomerVehicleRelation.Vehicle;
            var org = db.TechnicalExamOrganizations.Include(o => o.Company).FirstOrDefault();
            var orgName = org?.Name ?? "VTE";
            var orgAddress = org?.Address ?? org?.Company?.Address;

            // Get current user name as operator (if available)
            string? operatorName = null;
            var userService = App.Services?.GetService(typeof(VTE.Infrastructure.Services.CurrentUserService)) as VTE.Infrastructure.Services.CurrentUserService;
            if (userService?.UserId != null)
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userService.UserId);
                operatorName = user?.FullName ?? userService.Username;
            }

            var pdfBytes = Services.PaymentReceiptPdf.Generate(
                payment, customer, vehicle, payment.LineItems.ToList(),
                orgName, orgAddress, operatorName);

            var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                $"Receipt_{payment.DocumentNumber.Replace("/", "_")}.pdf");
            System.IO.File.WriteAllBytes(tempPath, pdfBytes);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating PDF:\n{ex.Message}", Strings.Error);
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
            mainWindow.ContentArea.Content = new PaymentListPage();
        }
    }
}

public class LineItemRow
{
    public long Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal VATPercent { get; set; }
}
