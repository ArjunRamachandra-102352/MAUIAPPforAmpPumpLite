using MAUIAMPPUMPLITEPOC.Data;
using MAUIAMPPUMPLITEPOC.Models;
using System.Text.RegularExpressions;

namespace MAUIAMPPUMPLITEPOC.Views;

public partial class AssetPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private Asset _selectedAsset;

    public AssetPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadAssets();
    }

    private async void LoadAssets()
    {
        var assets = await _databaseService.GetItemsAsync<Asset>();
        AssetsListView.ItemsSource = assets;
    }

    private async void OnSaveAssetClicked(object sender, EventArgs e)
    {
        var asset = _selectedAsset ?? new Asset
        {
            CreatedDate = DateTime.Now
        };

        asset.SerialNumber = SerialNumberEntry.Text;
        asset.UpdatedDate = DateTime.Now;
        asset.IsSync = false;
        if (asset.AssetId == 0)
        {
            await _databaseService.SaveItemAsync(asset);
        }
        else
        {
            await _databaseService.UpdateItemAsync(asset);
        }
        _selectedAsset = null;
        ClearFields();
        LoadAssets();
    }

    private async void OnDeleteAssetClicked(object sender, EventArgs e)
    {
        if (_selectedAsset == null) return;

        await _databaseService.DeleteItemAsync(_selectedAsset);
        _selectedAsset = null;
        ClearFields();
        LoadAssets();
    }

    private void OnAssetSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _selectedAsset = e.SelectedItem as Asset;
        if (_selectedAsset != null)
        {
            SerialNumberEntry.Text = _selectedAsset.SerialNumber;
        }
    }
       
    private void ClearFields()
    {
        SerialNumberEntry.Text = string.Empty;
    }
}