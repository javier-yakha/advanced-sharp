using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ProductBuilder
    {
        private Models.Product product = new();
        public Models.Product CreateProduct()
        {
            CreateTitle();
            CreatePrice();
            CreateDescription();
            CreateDiscountPrice();
            CreateEnabled();

            return product;
        }

        private void CreateTitle()
        {
            string title;
            while (true)
            {
                Console.Write("Title: ");
                title = Console.ReadLine();
                if (title is not null && title.Length < 50)
                {
                    break;
                }
            }

            product.Title = title;
        }

        private void CreatePrice()
        {
            bool priceStatus = false;
            decimal parsedPrice = default;
            while (!priceStatus)
            {
                Console.Write("Price: ");
                string price = Console.ReadLine();
                priceStatus = decimal.TryParse(price, out parsedPrice) && parsedPrice >= 0;
            }

            product.Price = parsedPrice;
        }

        private void CreateDescription()
        {
            string? description = null;

            while (description is null)
            {
                Console.Write("Description: ");
                description = Console.ReadLine();
                if (description is not null && description.Length < 90)
                {
                    break;
                }
                Console.WriteLine("Enter a valid description.");
            }

            product.Description = description;
        }

        private void CreateDiscountPrice()
        {
            Console.WriteLine("Type nothing if no discount is applicable.");
            Console.Write("Discount Price: ");

            string? discountPrice = Console.ReadLine();
            if (discountPrice is not null && discountPrice.Length > 0)
            {
                bool status = decimal.TryParse(discountPrice, out decimal parsedDiscountPrice) 
                     && parsedDiscountPrice >= 0;
                if (status)
                {
                    product.DiscountPrice = parsedDiscountPrice;
                }
            }
        }

        private void CreateEnabled()
        {
            Random r = new();
            product.Enabled = r.NextDouble() < 0.5;
        }

    }
}
