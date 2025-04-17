using MAUIAMPPUMPLITEPOC.Data;
using MAUIAMPPUMPLITEPOC.Models;

namespace MAUIAMPPUMPLITEPOC.Views;


public partial class PersonPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private Person _selectedPerson;

    public PersonPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadPersons();
    }

    private async void LoadPersons()
    {
        var persons = await _databaseService.GetItemsAsync<Person>();
        PersonsListView.ItemsSource = persons;
    }

    private async void OnSavePersonClicked(object sender, EventArgs e)
    {
        var person = _selectedPerson ?? new Person
        {
            CreatedDate = DateTime.Now
        };

        person.FirstName = FirstNameEntry.Text;
        person.LastName = LastNameEntry.Text;
        person.Address = AddressEntry.Text;
        person.State = StateEntry.Text;
        person.City = CityEntry.Text;
        person.Pincode = PincodeEntry.Text;
        person.UpdatedDate = DateTime.Now;
        person.IsSync = false;
        if (person.Id == 0)
        {
            await _databaseService.SaveItemAsync(person);
        }
        else
        {
            await _databaseService.UpdateItemAsync(person);
        }
        _selectedPerson = null;
        ClearFields();
        LoadPersons();
    }

    private async void OnDeletePersonClicked(object sender, EventArgs e)
    {
        if (_selectedPerson == null) return;

        await _databaseService.DeleteItemAsync(_selectedPerson);
        _selectedPerson = null;
        ClearFields();
        LoadPersons();
    }

    private void OnPersonSelected(object sender, SelectedItemChangedEventArgs e)
    {
        _selectedPerson = e.SelectedItem as Person;
        if (_selectedPerson != null)
        {
            FirstNameEntry.Text = _selectedPerson.FirstName;
            LastNameEntry.Text = _selectedPerson.LastName;
            AddressEntry.Text = _selectedPerson.Address;
            StateEntry.Text = _selectedPerson.State;
            CityEntry.Text = _selectedPerson.City;
            PincodeEntry.Text = _selectedPerson.Pincode;
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
        FirstNameEntry.Text = string.Empty;
        LastNameEntry.Text = string.Empty;
        AddressEntry.Text = string.Empty;
        StateEntry.Text = string.Empty;
        CityEntry.Text = string.Empty;
        PincodeEntry.Text = "0";
    }
}