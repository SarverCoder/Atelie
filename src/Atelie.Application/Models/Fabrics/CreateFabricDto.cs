namespace Atelie.Application.Models.Fabrics;

public class CreateFabricDto
{
    public string Name { get; set; }        
    public string? Description { get; set; }  
    public decimal BasePrice { get; set; }    
    public string? ColorOptions { get; set; }
}