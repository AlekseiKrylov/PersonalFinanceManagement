using Microsoft.AspNetCore.Components;
using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.Domain.Interfaces.WebApiClients;

namespace PersonalFinanceManagement.MudBlazorUI.ViewModels
{
    public class CategoriesViewModel : ComponentBase
    {
        protected IEnumerable<CategoryDTO> _categories;
        [Inject] ICategoriesWebApiClient CategoriesWebApiClient { get; init; }
        
        protected override async Task OnInitializedAsync()
        {
            _categories = (await CategoriesWebApiClient.GetAllAsync()).ToList();
        }
    }
}
