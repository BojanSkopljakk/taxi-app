namespace DriverVerificationService.Models
{
    public class DriverVerificationRequestDTO
    {
        public string DriverId { get; set; }
        public List<IFormFile> Documents { get; set; }
    }

}
