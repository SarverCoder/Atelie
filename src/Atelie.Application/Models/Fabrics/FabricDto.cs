namespace Atelie.Application.Models.Fabrics;

public class FabricDto
{
    public long Id { get; set; }    
    public string Name { get; set; }        
    public string? Description { get; set; } 
    public decimal BasePrice { get; set; }   
    public string? ColorOptions { get; set; }
}