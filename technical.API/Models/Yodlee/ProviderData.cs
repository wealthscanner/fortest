namespace technical.API.Models.Yodlee
{
    public class ProviderData
    {
	public string PRIORITY { get; set; }

	public int id { get; set; }

	public string Name { get; set; }

	public string LoginUrl { get; set; }

	public string BaseUrl { get; set; }

	public string Favicon { get; set; }

	public string Logo { get; set; }

	public string Status { get; set; }

	public string AuthType { get; set; }

	public string LanguageISOCode { get; set; }

	public string PrimaryLanguageISOCode { get; set; }

	public string CountryISOCode { get; set; }

	public string LastModified { get; set; }

	public string ForgetPasswordUrl { get; set; }

	public string IsAutoRefreshEnabled { get; set; }

	public CapabilityData [] Capability { get; set; }

	public DatasetDataProv [] Dataset { get; set; }

    }
}