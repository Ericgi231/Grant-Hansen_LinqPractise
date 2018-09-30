using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Demo_LINQ_ClassOfProducts
{

    //
    // demo adapted from MSDN demo
    // https://code.msdn.microsoft.com/SQL-Ordering-Operators-050af19e/sourcecode?fileId=23914&pathId=1978010539
    //
    class Program
    {
        static void Main(string[] args)
        {
            //
            // write all data files to Data folder
            //
            GenerateDataFiles.InitializeXmlFile();

            List<Product> productList = ReadAllProductsFromXml();


            OrderByCatagory(productList);

            OrderByCatagoryAnoymous(productList);

            //
            // Write the following methods
            //

            // Connor
            // OrderByUnits(): List the names and units of all products with less than 10 units in stock. Order by units.
            OrderByUnits(productList);

            // Eric
            // OrderByPrice(): List all products with a unit price less than $10. Order by price.
            OrderByPrice(productList);

            // Connor
            // FindExpensive(): List the most expensive Seafood. Consider there may be more than one.
            FindExpensive(productList);

            // Eric
            // OrderByTotalValue(): List all condiments with total value in stock (UnitPrice * UnitsInStock). Sort by total value.
            OrderByTotalValue(productList);

            // Eric
            // OrderByName(): List all products with names that start with "S" and calculate the average of the units in stock.
            OrderByName(productList);

            // Query: Student Choice - Minimum of one per team member
            //Eric
            SumCategory(productList);
            //Connor - Query and display the most expensive product from each category of product list
            expensiveEachCategory(productList);

        }


        /// <summary>
        /// Connor
        /// Find most expensive product in each category of list
        /// </summary>
        private static void expensiveEachCategory(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(TAB + "Most expensive product per category.\n");
            Console.WriteLine(TAB + "Product Category".PadRight(17) + "Product Name".PadLeft(15) + "Product Price".PadLeft(31)); // TODO: Display price as well???
            Console.WriteLine(TAB + "--------".PadRight(17) + "-----".PadLeft(8) + "-----".PadLeft(30));

            // Select all products
            var allProducts = products.Select(i => new { category = i.Category, name = i.ProductName, price = i.UnitPrice }).OrderByDescending(i => i.price);

            // Instantiate array to hold categories
            List<string> productCategories = new List<string>();

            // Iterate through all products and store new categories to array
            foreach (var product in allProducts)
            {
                productCategories.Add(product.category);
            }

            // Remove duplicate entries 
            productCategories = productCategories.Distinct().ToList();

            // For each category in list, set a default maxPrice value then iterate through all products and compare to maxPrice
            foreach (var category in productCategories)
            {
                // Set default for maxPrice
                decimal maxPrice = 0;
                foreach (var product in allProducts)
                {
                    if ( (product.price >= maxPrice) && (product.category == category) )
                    {
                        maxPrice = product.price;
                        Console.WriteLine(TAB + category.PadRight(20) + product.name.PadRight(30) + product.price.ToString());
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }



        /// <summary>
        /// Eric
        /// Total stock in each category
        /// </summary>
        private static void SumCategory(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(TAB + "Total stock per category.\n");
            Console.WriteLine(TAB + "Category".PadRight(15) + "Stock".PadLeft(10));
            Console.WriteLine(TAB + "--------".PadRight(15) + "-----".PadLeft(10));

            var sortedProducts = products.GroupBy(i => i.Category).Select(n => new { category = n.Key, stock = n.Sum(s => s.UnitsInStock)});
            var total = sortedProducts.Sum(i => i.stock);
            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.category.PadRight(15) + product.stock.ToString().PadLeft(10));
            }
            Console.WriteLine();
            Console.WriteLine(TAB + "Total".PadRight(15) + total.ToString().PadLeft(10));


            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Eric
        /// List All producuts that start with S and calculate the average
        /// </summary>
        private static void OrderByName(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(TAB + "Products that start with S.\n");
            Console.WriteLine(TAB + "Product Name".PadRight(30) + "Units In Stock".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(30) + "-------------".PadLeft(15));

            var sortedProducts = products.Where(i => i.ProductName.Substring(0,1).ToLower() == "s").Select(i => new { name = i.ProductName, stock = i.UnitsInStock }).OrderBy(i => i.name);
            var average = sortedProducts.Average(i => i.stock);
            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.name.PadRight(30) + product.stock.ToString().PadLeft(15));
            }
            Console.WriteLine();
            Console.WriteLine(TAB + "Average".PadRight(30) + average.ToString("0.00").PadLeft(15));


            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Eric
        /// Lists the total value of each condiments stock
        /// </summary>
        private static void OrderByTotalValue(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(TAB + "Total worth of condiment stock.\n");
            Console.WriteLine(TAB + "Product Name".PadRight(35) + "Stock".PadRight(8) + "Price".PadRight(8) + "Stock Worth");
            Console.WriteLine(TAB + "------------".PadRight(35) + "-----".PadRight(8) + "-----".PadRight(8) + "-----------");

            var sortedProducts = products.Where(i => i.Category == "Condiments").Select(i => new { name = i.ProductName, price = i.UnitPrice, stock = i.UnitsInStock }).OrderBy(i => i.price * i.stock);
            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.name.PadRight(35) + product.stock.ToString("0").PadRight(8) + "$" + product.price.ToString("0.00").PadRight(7) + "$" + (product.price * product.stock).ToString("0.00"));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Eric
        /// Filter out data over $10 and order data by price
        /// </summary>
        private static void OrderByPrice(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(TAB + "Drinks under $10. Sorted by price.\n");
            Console.WriteLine(TAB + "Product Name".PadRight(35) + "Unit Price");
            Console.WriteLine(TAB + "------------".PadRight(35) + "----------");

            var sortedProducts = products.Where(i => i.UnitPrice < 10).OrderBy(i => i.UnitPrice).Select( i => new { name = i.ProductName, price = i.UnitPrice});
            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.name.PadRight(35) + "$" + product.price.ToString("0.00"));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Connor
        /// Display the most expensive seafood item in the list
        /// </summary>
        private static void FindExpensive(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(TAB + "Most expensive seafood\n");
            Console.WriteLine(TAB + "Product Name".PadRight(35) + "Unit Price");
            Console.WriteLine(TAB + "------------".PadRight(35) + "----------");

            // Pull all items in seafood category and sort by descending price
            var allSeafood = products.Where(i => i.Category == "Seafood").OrderByDescending(i => i.UnitPrice).Select(i => new { name = i.ProductName, price = i.UnitPrice });

            // Run check to display all if price is the same
            decimal highestPrice = 0;
            foreach (var product in allSeafood)
            {
                // Store the highest price
                if (product.price >= highestPrice)
                {
                    highestPrice = product.price;
                    Console.WriteLine(TAB + product.name.PadRight(35) + "$" + product.price.ToString("0.00"));
                } else { break; }  // The results are already sorted in ascending order so if the next item is not >= highestPrice, we can break
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }




        /// <summary>
        /// List the names and units of all products with less than 10 units in stock. Order by units.
        /// </summary>
        private static void OrderByUnits(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List the names and units of all products with less than 10 units in stock. Order by units.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.UnitsInStock < 10
                orderby product.UnitsInStock descending
                select new
                {
                    unitsInStock = product.UnitsInStock,
                    Name = product.ProductName
                };

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages" && p.UnitPrice > 15).OrderByDescending(p => p.UnitPrice).Select(p => new
            //{
            //    Name = p.ProductName,
            //    Price = p.UnitPrice
            //});


            decimal average = products.Average(p => p.UnitPrice);

            Console.WriteLine(TAB + "Product Name".PadRight(20) + "Units In Stock".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(20) + "-------------".PadLeft(15));

            foreach (var product in sortedProducts)

            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.unitsInStock.ToString().PadLeft(15));
            }

            //Console.WriteLine();
            //Console.WriteLine(TAB + "Average Price:".PadRight(20) + average.ToString("C2").PadLeft(15));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }


        /// <summary>
        /// read all products from an XML file and return as a list of Product
        /// in descending order by price
        /// </summary>
        /// <returns>List of Product</returns>
        private static List<Product> ReadAllProductsFromXml()
        {
            string dataPath = @"Data\Products.xml";
            List<Product> products;

            try
            {
                StreamReader streamReader = new StreamReader(dataPath);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Product>), new XmlRootAttribute("Products"));

                using (streamReader)
                {
                    products = (List<Product>)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return products;
        }


        private static void OrderByCatagory(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages and sort by the unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Category == "Beverages"
                orderby product.UnitPrice descending
                select product;

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages").OrderByDescending(p => p.UnitPrice);

            Console.WriteLine(TAB + "Category".PadRight(15) + "Product Name".PadRight(25) + "Unit Price".PadLeft(10));
            Console.WriteLine(TAB + "--------".PadRight(15) + "------------".PadRight(25) + "----------".PadLeft(10));

            foreach (Product product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Category.PadRight(15) + product.ProductName.PadRight(25) + product.UnitPrice.ToString("C2").PadLeft(10));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }

        private static void OrderByCatagoryAnoymous(List<Product> products)
        {
            string TAB = "   ";

            Console.Clear();
            Console.WriteLine(TAB + "List all beverages that cost more the $15 and sort by the unit price.");
            Console.WriteLine();

            //
            // query syntax
            //
            var sortedProducts =
                from product in products
                where product.Category == "Beverages" &&
                    product.UnitPrice > 15
                orderby product.UnitPrice descending
                select new
                {
                    Name = product.ProductName,
                    Price = product.UnitPrice
                };

            //
            // lambda syntax
            //
            //var sortedProducts = products.Where(p => p.Category == "Beverages" && p.UnitPrice > 15).OrderByDescending(p => p.UnitPrice).Select(p => new
            //{
            //    Name = p.ProductName,
            //    Price = p.UnitPrice
            //});


            decimal average = products.Average(p => p.UnitPrice);

            Console.WriteLine(TAB + "Product Name".PadRight(20) + "Product Price".PadLeft(15));
            Console.WriteLine(TAB + "------------".PadRight(20) + "-------------".PadLeft(15));

            foreach (var product in sortedProducts)
            {
                Console.WriteLine(TAB + product.Name.PadRight(20) + product.Price.ToString("C2").PadLeft(15));
            }

            Console.WriteLine();
            Console.WriteLine(TAB + "Average Price:".PadRight(20) + average.ToString("C2").PadLeft(15));

            Console.WriteLine();
            Console.WriteLine(TAB + "Press any key to continue.");
            Console.ReadKey();
        }
    }
}
