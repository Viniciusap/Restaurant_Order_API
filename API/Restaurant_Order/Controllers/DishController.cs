using System;
using System.Collections.Generic;
using Domain.Enums;
using Domain.Intefaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Restaurant_Order.API.Controllers
{
    [EnableCors("Policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly ILogger<Dish> _logger;
        private readonly IRequestDishes _requestDishes;

        public DishController(ILogger<Dish> logger, IRequestDishes requestDishes)
        {
            _logger = logger;
            _requestDishes = requestDishes;
        }

        [HttpGet]
        public List<DishResponse> Get(string _choices)
        {
            List<DishResponse> response = new List<DishResponse>();
            

            if (_choices != null)
            {
                Mealtime mealtime;
                DishResponse dishResponse = new DishResponse();
                dishResponse.Input = _choices;                

                //first comma
                int fc = _choices.IndexOf(",");

                try{

                    //Find Mealtime inside input
                    mealtime = _requestDishes.ValidateMealtime(_choices, fc);
                    //Find Meals, ordenate list and create a string for output
                    dishResponse.Output = _requestDishes.ValidateOutput(_choices, fc, mealtime);
                }
                catch
                {
                    response.Add(new DishResponse("error", dishResponse.Input));
                    return response;
                }    

                response.Add(dishResponse);
            }

            

            return response;
        }

        

        
    }
}
