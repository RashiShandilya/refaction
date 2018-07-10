using refactor_me.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactor_me.Helpers
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> SearchByName(string name);
        Product GetProduct(Guid id);
        void Create(Product product);
        void Update(Guid id, Product product);
        void Delete(Product product);
        IEnumerable<ProductOption> GetOptions(Guid productId);
        ProductOption GetOption(Guid productId, Guid id);
        void CreateOption(Guid productId, ProductOption option);
        void UpdateOption(Guid id, ProductOption option);
        void DeleteOption(ProductOption option);
    }
}
