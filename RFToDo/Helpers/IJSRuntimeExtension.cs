using Microsoft.JSInterop;

namespace RFToDo.Helpers
{
    public static class IJSRuntimeExtension
    {
        #region Toastr
        public static async ValueTask ToastrSuccess(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr", "success", message);
        }
        public static async ValueTask ToastrError(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }
        public static async ValueTask ToastrInfo(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr", "info", message);
        }
        public static async ValueTask ToastrWarning(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr", "warning", message);
        }
        #endregion

        #region Swal
        public static async ValueTask SwalSuccess(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowSwal", "success", message);
        }
        public static async ValueTask SwalError(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowSwal", "error", message);
        }
        public static async Task<bool> SwalDelete(this IJSRuntime jsRuntime, string message)
        {
            var result = await jsRuntime.InvokeAsync<bool>("deleteConfirmation",message);

            return result;
        }
        public static async ValueTask SwalRegistrationComplete(this IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeVoidAsync("ShowSwal", "RegistrationComplete", string.Empty);
        }

        #endregion
    }
}
