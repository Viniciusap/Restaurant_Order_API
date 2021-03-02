using Domain.Enums;

namespace Domain.Models
{
    public class Dish: Base
    {
        public Dish()
        {
        }

        public Dish(int id, string dishName)
        {
            Id = id;
            DishName = dishName;
        }

        public Dish(int id, DishType type, Mealtime mealtime, string dishName, bool multipleOrders)
        {
            Id = id;
            Type = type;
            _Mealtime = mealtime;
            DishName = dishName;
            MultipleOrders = multipleOrders;
        }

        public DishType Type { get; set; }
        public Mealtime _Mealtime { get; set; }
        public string DishName { get; set; }
        public bool MultipleOrders { get; set; }
    }
}
