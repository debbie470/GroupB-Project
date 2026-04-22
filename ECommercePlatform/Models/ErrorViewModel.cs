namespace ECommercePlatform.Models // Defines the organizational group for application data models
{ // Start of namespace block
    public class ErrorViewModel // Data model used for passing error details to the error view
    { // Start of class block
        public string? RequestId { get; set; } // Stores the unique tracking ID for the failed request

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); // Logic to determine if the Request ID should be displayed in the UI
    } // End of class block
} // End of namespace block