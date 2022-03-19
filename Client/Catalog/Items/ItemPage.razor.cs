using Microsoft.AspNetCore.Components;
using Catalog.Client;
using MudBlazor;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Catalog.Items
{
    public partial class ItemPage
    {
        ItemDto? item;

        bool isLoading = false;
        bool loadingFailed = false;

        [Parameter]
        public string Id { get; set; } = null !;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        async Task LoadAsync()
        {
            isLoading = true;
            loadingFailed = false;

            StateHasChanged();

        #if DEBUG
            //await Task.Delay(2000);
        #endif

            try
            {
                //throw new Exception();

                item = await ItemsClient.GetItemAsync(Id);
            }
            catch (ApiException<ProblemDetails> exc)
            {
                loadingFailed = true;

                Snackbar.Add(exc.Result.Detail, Severity.Error);
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
            catch (Exception exc)
            {                
                loadingFailed = true;   

                Snackbar.Add(exc.Message, Severity.Error);
            }
            finally 
            {
                isLoading = false;
            }

            StateHasChanged();
        }

        async void OnLocationChanged(object sender, LocationChangedEventArgs ev)
        {
            await LoadAsync();
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}