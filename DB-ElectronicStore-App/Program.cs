using DB_ElectronicStore_App.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

///stuff goes here
///method calling, DB creation- add/remove etc

namespace DB_ElectronicStore_App
{
    class Progran
    {
        static void Main(string[] args)
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                //Insert/Delete methods will go here

                //CleanCategories();
                //InsertCategories();

                //CleanProducers();
                //InsertProducers();

                //CleanProducts();
                //InsertProducts();

                //CleanLabelProduct();
                //InsertLabelProduct();

            }
        }
        #region categorii
        public static void InsertCategories()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var categories = new[]
                {
                new Category {Name = "SmartPhones", Icon = "https://www.example.com/smartphones.png" },
                new Category { Name = "Laptops", Icon = "https://www.example.com/laptops.png" },
                new Category { Name = "PC", Icon = "https://www.example.com/personalcomputer.png" },
                new Category { Name = "Smartwatch", Icon = "https://www.example.com/smartwatch.png" },
                };

                edb.Category.AddRange(categories);
                edb.SaveChanges();
            }
        }

        public static void CleanCategories()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                TruncateTable<Category>(edb);
                edb.SaveChanges();
            }
        }

        public static void AddCategories(string categoryName, string categortIcon)
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var category = new Category { Name = categoryName, Icon = categortIcon };
                edb.SaveChanges();
            }
        }

        #endregion final categorii

        #region producatori
        public static void InsertProducers()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var producers = new[]
                {
                    new Producer {Name = "Apple", Address ="Cupertino, CA, USA", CUI = "232421421"},
                    new Producer {Name = "ASUS", Address ="Fremont, CA, USA", CUI = "242155561"},
                    new Producer {Name = "Dell", Address ="Round Rock, Texas, USA", CUI = "9867856"},
                    new Producer {Name = "Samsung", Address ="Suwon-si, South Korea", CUI = "1132445"},
            };

                edb.Producer.AddRange(producers);
                edb.SaveChanges();
            }
        }

        public static void CleanProducers()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                TruncateTable<Producer>(edb);
                edb.SaveChanges();
            }
        }

        public static void AddProducer(string producerName, string producerCountry, string producerCUI)
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var producer = new Producer { Name = producerName, Address = producerCountry, CUI = producerCUI };
                edb.SaveChanges();
            }
        }
        #endregion final producatori

        #region produse
        public static void InsertProducts()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var products = new[]
                {
                    new Product { Name = "Iphone 13", Stock = 100, ProducerId = 1, CategoryId = 1 },
                    new Product { Name = "Rogue Z34", Stock = 195, ProducerId = 2, CategoryId = 2 },
                    new Product { Name = "Optiplex 5050", Stock = 55, ProducerId = 3, CategoryId = 3 },
                    new Product { Name = "GalaxyS15Watch", Stock = 15, ProducerId = 4, CategoryId = 4 },
                };

                edb.Product.AddRange(products);
                edb.SaveChanges();
            }
        }

        public static void CleanProducts()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                TruncateTable<Product>(edb);
                edb.SaveChanges();
            }
        }
        #endregion final produs

        #region etichete
        public static void InsertLabelProduct()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var labelProduct = new[]
                {
                    new LabelProduct {Barcode = Guid.NewGuid().ToString(), Price = 3999.99M, ProductId = 8 },
                    new LabelProduct {Barcode = Guid.NewGuid().ToString(), Price = 5555.89M, ProductId = 9 },
                    new LabelProduct {Barcode = Guid.NewGuid().ToString(), Price = 2455.99M, ProductId = 10 },
                    new LabelProduct {Barcode = Guid.NewGuid().ToString(), Price = 2099.98M, ProductId = 11 },
                };
                edb.LabelProduct.AddRange(labelProduct);
                edb.SaveChanges();
            }
        }

        public static void CleanLabelProduct()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                TruncateTable<LabelProduct>(edb);
                edb.SaveChanges();
            }
        }

        public static void UpdateProductPrice(int productId, decimal newPrice)
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var product = edb.Product.Find(productId);
                if (product == null)
                {
                    Console.WriteLine($"Product with ID {productId} doest not exist");
                    return;
                }

                var labelProduct = edb.LabelProduct.FirstOrDefault(x => x.ProductId == productId);
                if (labelProduct == null) 
                {
                    Console.WriteLine($"LabelProduct for product with ID {productId} doest not exist");
                }

                labelProduct.Price = newPrice;
                edb.SaveChanges();
                Console.WriteLine($"Price of product with ID {productId} has been updated to {newPrice}.");

            }
        }
        #endregion final etichete

        #region Delete
        public static void TruncateTable<T>(DbContext edb) where T : class
        {
            IEntityType entityType = edb.Model.FindEntityType(typeof(T));
            string tableName = entityType.GetTableName();
            edb.Database.ExecuteSqlRaw($"Delete from {tableName}");
        }
        #endregion final Delete

        public static decimal GetStockValueForProducer()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var totalValue = edb.LabelProduct.Where(x=>x.ProductId == x.ProductId).Sum(x=>x.Price * x.Product.Stock);
                return totalValue;
            }
        }

        public static Dictionary<string, decimal> GetTotalStockValuePerCategory()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var result = edb.LabelProduct.Include(x=>x.Product)
                    .GroupBy(p=>p.Product.Category.Name)
                    .Select(g=>new {CategoryName=g.Key, TotalValue = g.Sum(p=>p.Product.Stock* p.Price)})
                    .ToDictionary(x=>x.CategoryName,y=>y.TotalValue);

                return result;
            }
        }
        public static Dictionary<(string, int), decimal> GetStockValueByCategoryAndManufacturer()
        {
            using (var edb = new ElectronicStoreMgmtDB())
            {
                var result = edb.LabelProduct.Include(lp => lp.Product)
                    .GroupBy(lp => new { lp.Product.Category.Name, lp.Product.Producer.Id })
                    .Select(g => new
                    {
                        CategoryName = g.Key.Name,
                        ProducerName = g.Key.Id,
                        TotalValue = g.Sum(lp => lp.Product.Stock * lp.Price)
                    })
                    .ToDictionary(x => (x.CategoryName, x.ProducerName), x => x.TotalValue);

                return result;
            }
        }

    }
}






