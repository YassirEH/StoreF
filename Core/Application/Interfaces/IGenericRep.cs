using Core.Domain.Models;

namespace Core.Application.Interfaces
{
    public interface IGenericRep
    {
        public bool Save();

        //Buyer

        public bool BuyerExists(int id);

        public Buyer GetBuyer(int id);

        public ICollection<Buyer> GetBuyers();

        public bool CreateBuyer(Buyer buyer);

        public bool UpdateBuyer(Buyer buyer);

        public bool DeleteBuyer(Buyer buyer);

        //Category

        public bool CategoryExists(int id);

        public Category GetCategory(int id);

        public ICollection<Category> GetCategories();

        public bool CreateCategory(Category category);

        public bool UpdateCategory(Category category);

        public bool DeleteCategory(Category category);

        //Product

        Product GetProduct(int id);
        Product GetProduct(string name);
        ICollection<Product> GetProducts();
        bool ProductExists(int productId);
        bool CreateProduct(Product product, int categoryId);
        bool UpdateProduct(int productId, Product updatedProduct);
        bool DeleteProduct(Product product);

    }
}
