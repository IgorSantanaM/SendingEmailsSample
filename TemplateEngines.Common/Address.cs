namespace TemplatingEngines.Common;

public class Address() {
	public Address(string addressLine1, string addressLine2, string city, string country, string postalCode) : this() {
		AddressLine1 = addressLine1;
		AddressLine2 = addressLine2;
		City = city;
		Country = country;
		PostalCode = postalCode;
	}

	public string AddressLine1 { get; set; } = String.Empty;
	public string AddressLine2 { get; set; } = String.Empty;
	public string City { get; set; } = String.Empty;
	public string Country { get; set; } = String.Empty;
	public string PostalCode { get; set; } = String.Empty;
}
