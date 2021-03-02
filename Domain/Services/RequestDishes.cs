using Domain.Enums;
using Domain.Intefaces.Services;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Domain.Service
{
    public class RequestDishes : IRequestDishes
    {
        List<Dish> dishes = new List<Dish>();
        public RequestDishes()
        {      
            
        }

        public Dish Request(Mealtime mealtime, DishType dishType)
        {
            return GetDish(mealtime, dishType);
        }

        public List<Dish> ValidateDishes(List<Dish> _dishes)
        {
            List<Dish> result = new List<Dish>();
            List<Dish> has = new List<Dish>();

            //Validation to confirm if all the dishs requested is MultipleOrders == true
            //If they dont and was requested to add, will add just one and finish with error
            //If they dont, add just one 
            
            foreach (var dish in _dishes)
            {
                var di = _dishes.Where(d => d.Id == dish.Id).ToList();
                has = result.Where(d => d.Id == di[0].Id).ToList();

                if (di.Count() > 1 && (di[0].MultipleOrders == true) && has.Count() == 0)
                {
                    //It is allowed to add multiple
                    di[0].DishName = di[0].DishName + String.Format("(x{0})", di.Count());
                    result.Add(di[0]);
                }
                else
                {
                    var d = has.Where(d => d.Id == dish.Id).ToList();
                    if (di.Count() > 1 && di[0].MultipleOrders == false)
                    {
                        //It isn't allowed to add multiple and they try
                        result.Add(dish);
                        result.Add(new Dish(99, "error"));
                        break;
                    }
                    else if (d.Count() == 0)
                    {
                        //It isn't allowed to add multiple
                        result.Add(dish);
                    }

                }
            }

            return result;
        }

        public Mealtime ValidateMealtime(string _choices, int fc)
        {
            if (!Regex.IsMatch(_choices.Substring(0, fc).Trim(), @"^[a-zA-Z]+$"))
                throw new Exception();

            return (Mealtime)Enum.Parse(typeof(Mealtime), _choices.Substring(0, fc));
        }

        public string ValidateOutput(string _choices, int fc, Mealtime mealtime)
        {
            List<Dish> list = new List<Dish>();
            _choices = _choices.Substring(fc, _choices.Length - fc);
            string[] choices = _choices.Split(',', StringSplitOptions.RemoveEmptyEntries);



            foreach (var item in choices)
            {
                int choice = int.TryParse(item, out choice) ? choice : 0;
                list.Add(Request(mealtime, (DishType)choice));
            }
            list = ValidateDishes(list);

            list = list.OrderBy(x => x.Id).ToList();

            string result = "";
            foreach (var d in list)
            {
                result += String.Format("{0}, ", d.DishName);
            }
            return result[0..^2];
        }

        private Dish GetDish(Mealtime mealtime, DishType dishType)
        {
            //morning
            dishes.Add(new Dish(1, DishType.entree, Mealtime.morning, "eggs", false));

            dishes.Add(new Dish(2, DishType.side, Mealtime.morning, "Toast", false));

            dishes.Add(new Dish(3, DishType.drink, Mealtime.morning, "coffee", true));



            //night
            dishes.Add(new Dish(4, DishType.entree, Mealtime.night, "steak", false));

            dishes.Add(new Dish(5, DishType.side, Mealtime.night, "potato", true));

            dishes.Add(new Dish(6, DishType.drink, Mealtime.night, "wine", false));

            dishes.Add(new Dish(7, DishType.dessert, Mealtime.night, "cake", false));

            dishes = dishes.Where(d => d._Mealtime == mealtime &&
                                         d.Type == dishType).ToList();

            if (dishes == null || dishes.Count == 0)
            {
                dishes.Add(new Dish(99, "error")) ;
            }

            return dishes[0];

        }
    }
}
