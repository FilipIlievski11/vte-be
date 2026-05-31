namespace VTE.WPF.Views.Pages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Core.Lookups;
using VTE.Infrastructure.Data;

public partial class LookupsPage : UserControl
{
    private readonly List<LookupCategory> _categories;
    private LookupCategory? _selectedConfig;
    private List<LookupEntity> _currentItems = new();

    public LookupsPage()
    {
        InitializeComponent();

        _categories = new List<LookupCategory>
        {
            Create<Country>("Countries"),
            Create<Community>("Communities"),
            Create<City>("Cities"),
            Create<Street>("Streets"),
            Create<BusinessType>("Business Types"),
            Create<VehicleBodyType>("Vehicle Body Types"),
            Create<VehicleCategory>("Vehicle Categories"),
            Create<VehiclePaymentCategory>("Vehicle Payment Categories"),
            Create<VehicleUseType>("Vehicle Use Types"),
            Create<EngineType>("Engine Types"),
            Create<EnginePowerSourceType>("Engine Power Source Types"),
            Create<GearBoxType>("Gear Box Types"),
            Create<BrakeType>("Brake Types"),
            Create<SupportingType>("Supporting Types"),
            Create<EcoProgram>("Eco Programs"),
            Create<VehicleMaker>("Vehicle Makers"),
            Create<TireType>("Tire Types"),
            Create<Color>("Colors"),
            Create<RegistrationIssuer>("Registration Issuers"),
            Create<DocumentType>("Document Types"),
            Create<PaymentType>("Payment Types"),
            Create<PaymentCategory>("Payment Categories"),
            Create<TechnicalExamType>("Technical Exam Types"),
            Create<ExamDetailStatus>("Exam Detail Statuses"),
            Create<RelationType>("Relation Types"),
            Create<DrivingLicenseCategory>("Driving License Categories"),
            Create<RequestType>("Request Types"),
            Create<AttachmentType>("Attachment Types"),
            Create<OwnershipProofType>("Ownership Proof Types"),
            Create<PaymentProofType>("Payment Proof Types"),
        };

        CategoryList.ItemsSource = _categories.Select(c => c.DisplayName).ToList();
    }

    private static LookupCategory Create<T>(string displayName) where T : LookupEntity, new()
    {
        return new LookupCategory(
            displayName,
            db => db.Set<T>().OrderBy(x => x.Name).Cast<LookupEntity>().ToList(),
            db =>
            {
                var item = new T { Name = "New Item", IsActive = true };
                db.Set<T>().Add(item);
                return item;
            },
            (db, entity) =>
            {
                var existing = db.Set<T>().Find(entity.Id);
                if (existing != null)
                {
                    existing.Name = entity.Name;
                    existing.IsActive = entity.IsActive;
                }
            },
            (db, id) =>
            {
                var existing = db.Set<T>().Find(id);
                if (existing != null)
                {
                    db.Set<T>().Remove(existing);
                }
            }
        );
    }

    private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        if (CategoryList.SelectedItem is not string selected)
            return;

        _selectedConfig = _categories.FirstOrDefault(c => c.DisplayName == selected);
        HeaderText.Text = selected;
        LoadCurrentCategory();
    }

    private void LoadCurrentCategory()
    {
        if (_selectedConfig == null) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            _currentItems = _selectedConfig.Load(db);
            LookupGrid.ItemsSource = _currentItems;
            StatusText.Text = $"Loaded {_currentItems.Count} items";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Error loading data: {ex.Message}";
        }
    }

    private void OnAddNew(object sender, RoutedEventArgs e)
    {
        if (_selectedConfig == null)
        {
            StatusText.Text = "Please select a category first.";
            return;
        }

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var newItem = _selectedConfig.Add(db);
            db.SaveChanges();

            LoadCurrentCategory();
            StatusText.Text = $"Added new item (ID: {newItem.Id})";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Error adding item: {ex.Message}";
        }
    }

    private void OnSaveChanges(object sender, RoutedEventArgs e)
    {
        if (_selectedConfig == null || _currentItems.Count == 0)
        {
            StatusText.Text = "Nothing to save.";
            return;
        }

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            foreach (var item in _currentItems)
            {
                _selectedConfig.Update(db, item);
            }

            var count = db.SaveChanges();
            StatusText.Text = $"Saved {count} change(s)";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Error saving: {ex.Message}";
        }
    }

    private void OnDeleteSelected(object sender, RoutedEventArgs e)
    {
        if (_selectedConfig == null)
        {
            StatusText.Text = "Please select a category first.";
            return;
        }

        if (LookupGrid.SelectedItem is not LookupEntity selected)
        {
            StatusText.Text = "Please select an item to delete.";
            return;
        }

        var result = MessageBox.Show(
            $"Delete \"{selected.Name}\" (ID: {selected.Id})?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            _selectedConfig.Delete(db, selected.Id);
            db.SaveChanges();

            LoadCurrentCategory();
            StatusText.Text = $"Deleted \"{selected.Name}\"";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Error deleting: {ex.Message}";
        }
    }

    private sealed record LookupCategory(
        string DisplayName,
        Func<VteDbContext, List<LookupEntity>> Load,
        Func<VteDbContext, LookupEntity> Add,
        Action<VteDbContext, LookupEntity> Update,
        Action<VteDbContext, long> Delete
    );
}
