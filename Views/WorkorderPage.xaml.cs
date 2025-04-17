using MAUIAMPPUMPLITEPOC.Data;
using MAUIAMPPUMPLITEPOC.Models;
using System.Text.RegularExpressions;

namespace MAUIAMPPUMPLITEPOC.Views;

public partial class WorkorderPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private WorkOrder _selectedWorkorder;

    public WorkorderPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadWorkorders();
    }

    private async void LoadWorkorders()
    {
        var workorders = await _databaseService.GetItemsAsync<WorkOrder>();
        WorkordersListView.ItemsSource = workorders;
    }

    private async void OnSaveWorkorderClicked(object sender, EventArgs e)
    {
        var workorder = _selectedWorkorder ?? new WorkOrder
        {
            CreatedDate = DateTime.Now
        };

        workorder.WorkOrderNumber = WorkorderEntry.Text;
        workorder.ServiceCenterId = int.Parse(ServiceCenterIdEntry.Text);
        workorder.UpdatedDate = DateTime.Now;
        workorder.IsSync = false;
        if (workorder.WorkOrderId == 0)
        {
            await _databaseService.SaveItemAsync(workorder);
        }
        else
        {
            await _databaseService.UpdateItemAsync(workorder);
        }
        _selectedWorkorder = null;
        ClearFields();
        LoadWorkorders();
    }

    private async void OnDeleteWorkorderClicked(object sender, EventArgs e)
    {
        if (_selectedWorkorder == null) return;

        await _databaseService.DeleteItemAsync(_selectedWorkorder);
        _selectedWorkorder = null;
        ClearFields();
        LoadWorkorders();
    }

    private void OnWorkorderSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _selectedWorkorder = e.SelectedItem as WorkOrder;
        if (_selectedWorkorder != null)
        {
            WorkorderEntry.Text = _selectedWorkorder.WorkOrderNumber;
            ServiceCenterIdEntry.Text = _selectedWorkorder.ServiceCenterId.ToString();
        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        // Ensure the input is numeric
        string result = System.Text.RegularExpressions.Regex.Replace(e.NewTextValue, @"[^0-9\-]", "");

        // Check if the input is a valid Int32
        if (int.TryParse(result, out int intValue))
        {
            // If valid, update the Entry text
            entry.Text = result;
        }
        else
        {
            // If invalid (e.g., out of Int32 range), revert to the previous valid value
            entry.Text = e.OldTextValue;
        }
    }

    private void ClearFields()
    {
        WorkorderEntry.Text = string.Empty;
        ServiceCenterIdEntry.Text = "0";
    }
}