using System;
namespace Lab1_new.ViewModels.Authentication
{
    public class ConfirmUserRequest
    {
        public string Email { get; set; }
        public string ConfirmationToken { get; set; }
    }
}
