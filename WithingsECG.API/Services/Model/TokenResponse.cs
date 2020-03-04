namespace WithingsECG.API.Services.Model
{
    public struct TokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
      //Cannot deserialize, in test mode it's an int. in normal mode a string  public string userid { get; set; }
    }
}
