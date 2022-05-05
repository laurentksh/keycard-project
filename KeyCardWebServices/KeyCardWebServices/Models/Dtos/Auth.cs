namespace KeyCardWebServices.Models.Dtos;

public class LoginDto
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class GrantPhysicalDeviceDto
{
    public string DeviceName { get; set; }
}