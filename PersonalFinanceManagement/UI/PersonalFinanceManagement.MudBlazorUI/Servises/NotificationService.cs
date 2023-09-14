using MudBlazor;

namespace PersonalFinanceManagement.MudBlazorUI.Servises
{
    public class NotificationService
    {
        private readonly ISnackbar _snackbar;
        public event Action<string, Severity> OnShowAlert;

        public NotificationService(ISnackbar snackbar) => _snackbar = snackbar;

        public void ShowSnackbar(string message) => _snackbar.Add(message, Severity.Success);

        public void ShowAlert(string message, Severity severity) => OnShowAlert?.Invoke(message, severity);
    }
}
