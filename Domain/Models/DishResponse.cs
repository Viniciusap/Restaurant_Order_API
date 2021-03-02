
namespace Domain.Models
{
    public class DishResponse
    {
        public DishResponse()
        {
        }
        public DishResponse(string output, string input)
        {
            this.Output = output;
            this.Input = input;
        }
        
        public string Input { get; set; }
        public string Output { get; set; }

        
    }
}
