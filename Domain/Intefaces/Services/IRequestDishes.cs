using Domain.Enums;
using Domain.Models;
using System.Collections.Generic;

namespace Domain.Intefaces.Services
{
    public interface IRequestDishes
    {
        Dish Request(Mealtime mealtime, DishType dishType);
        List<Dish> ValidateDishes(List<Dish> dishes);
        Mealtime ValidateMealtime(string _choices, int fc);
        string ValidateOutput(string _choices, int fc, Mealtime mealtime);
    }
}
