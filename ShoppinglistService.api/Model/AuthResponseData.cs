namespace ShoppinglistService.api.Model
{
    public class AuthResponseData
    {
        public string kind { get; set; }
        public string idToken { get; set; }
        public string email { get; set; }
        public string refreshToken { get; set; }
        public string expiresIn { get; set; }
        public string localId { get; set; }
        public string registered { get; set; }
        public string message { get; set; }

    }
}
