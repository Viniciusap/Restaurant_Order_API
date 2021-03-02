using Domain.Enums;
using Domain.Intefaces.Services;
using Domain.Models;
using Domain.Service;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Restaurant_Order.API.Controllers;
using System;
using System.Linq;

namespace Restaurant_Order.Test
{
    public class Tests
    {
        private IRequestDishes _irequestDishes = new RequestDishes();
        private readonly ILogger<Dish> _logger;

        [Test]
        public void TheyShouldChoiceMorningOrNigth()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _irequestDishes.ValidateMealtime("evening", 8));
        }

        [Test]
        public void MustEnterCommaDelimitedListOfDishs()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _irequestDishes.ValidateOutput("ThereIsNotCommaHere", 20, Mealtime.morning));
        }

        [Test]
        public void AllThoseFoodsMustToBeSorted()
        {
            //Unnordered
            var choices = "morning, 1,3,3,2,,";

            var ordened = _irequestDishes.ValidateOutput(choices, 8 ,Mealtime.morning);

            Assert.AreEqual("eggs, Toast, coffee(x2)", ordened);

        }

        [Test]
        public void MorningDoesntHaveDessert()
        {
            Dish dish = _irequestDishes.Request(Mealtime.morning, DishType.dessert);
            Assert.AreEqual((DishType)0, dish.Type);

        }

        [Test]
        public void InputAreNotCaseSensitive()
        {
            //case not sensitive
            var choices = "moRRing, 3,3,2,3,1";

            var ordened = _irequestDishes.ValidateOutput(choices, 8, Mealtime.morning);

            Assert.AreEqual("eggs, Toast, coffee(x3)", ordened);

        }

        [Test]
        public void InvalidInputShouldPrintError()
        {
            //Invalid input = Wine multiple times
            var choices = "night, 3,3,2,3,1,2";

            var ordened = _irequestDishes.ValidateOutput(choices, 8, Mealtime.night);

            Assert.AreEqual("wine, error", ordened);

        }

        [Test]
        public void InTheMorningShouldBeAbleToOrderMultipleCoffees()
        {
            //Asking for four cups of coffee
            var choices = "morning, 3,3,3,3,1,2";

            var ordened = _irequestDishes.ValidateOutput(choices, 8, Mealtime.morning);

            Assert.AreEqual("eggs, Toast, coffee(x4)", ordened);

        }

        [Test]
        public void AtNigthShouldBeAbleToOrderMultiplePotatoes()
        {
            //Asking for three plates of potatoes
            var choices = "nigth, 2,2,2,3,1,2";

            var ordened = _irequestDishes.ValidateOutput(choices, 8, Mealtime.night);

            Assert.AreEqual("steak, potato(x3), wine", ordened);

        }

        [Test]
        public void ExceptPotatoesAndCoffeeThereIsNotMultiples()
        {

            //Morning
            //Asking for two toasts
            var choices = "morning, 2,3,3,1,2";

            var ordened = _irequestDishes.ValidateOutput(choices, 8, Mealtime.morning);

            Assert.AreEqual("Toast, error", ordened);

            //Asking for three plates of eggs
            var choices1 = "morning, 1,2,3,3,1";

            var ordened1 = _irequestDishes.ValidateOutput(choices1, 8, Mealtime.morning);

            Assert.AreEqual("eggs, error", ordened1);


            //Night
            //Asking for three plates of steaks
            var choices2 = "night, 1, 1";

            var ordened2 = _irequestDishes.ValidateOutput(choices2, 6, Mealtime.night);

            Assert.AreEqual("steak, error", ordened2);

            //Asking for two cups of wine
            var choices3 = "night, 3, 3";

            var ordened3 = _irequestDishes.ValidateOutput(choices3, 6, Mealtime.night);

            Assert.AreEqual("wine, error", ordened3);

            //Asking for two cakes
            var choices4 = "night, 4,4";

            var ordened4 = _irequestDishes.ValidateOutput(choices4, 6, Mealtime.night);

            Assert.AreEqual("cake, error", ordened4);

        }

        [Test]
        public void HTTPGet()
        {

            var controller = new DishController(_logger, _irequestDishes);

            var response = controller.Get("morning, 1, 3, 2, 3");

            Assert.AreEqual(1, response.Count());

        }


    }
}