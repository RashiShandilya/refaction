using refactor_me.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.Helpers
{
    public class ProductService : IProductService
    {
        private ProductContext db = new ProductContext();

        public ProductService() { }
        public IEnumerable<Product> GetAll()
        {
            return db.Products;
        }

        public IQueryable<Product> SearchByName(string name)
        {
            return db.Products.Where(b => b.Name.Contains(name.ToLower()));

        }

         public Product GetProduct(Guid id)
         {
            return db.Products.FirstOrDefault(p => p.Id == id);
         }

        public void Create(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            
        }
        public void Update(Guid id, Product product)
        {
            var entity = db.Products.FirstOrDefault(e => e.Id == id);
            entity.Name = product.Name;
            entity.Description = product.Description;
            entity.DeliveryPrice = product.DeliveryPrice;
            entity.Price = product.Price;

            db.SaveChanges();

        }

        public void Delete(Product product)
        {
            db.Products.Remove(product);
            db.SaveChanges();
        }

        public IQueryable<ProductOption> GetOptions(Guid productId)
        {
           return db.ProductOptions.Where(p => p.ProductId == productId);
        }

        public ProductOption GetOption(Guid productId, Guid id)
        {
           return db.ProductOptions.FirstOrDefault(p => p.Id == id && p.ProductId == productId);
        }

        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            db.ProductOptions.Add(option);
            db.SaveChanges();
        }

        public void UpdateOption(Guid id, ProductOption option)
        {
            var entity = db.ProductOptions.FirstOrDefault(e => e.Id == id);

            entity.ProductId = option.ProductId;
            entity.Name = option.Name;
            entity.Description = option.Description;
            db.SaveChanges();
        }
        public void DeleteOption(ProductOption option)
        {
            db.ProductOptions.Remove(option);
            db.SaveChanges();
        }
    }
}