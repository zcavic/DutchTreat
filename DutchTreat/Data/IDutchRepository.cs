using DutchTreat.Data.Entities;
using System.Collections.Generic;

namespace DutchTreat.Data
{
	public interface IDutchRepository
	{
		IEnumerable<Product> GetAllProducts();
		IEnumerable<Product> GetProductByCategory(string category);
		
		IEnumerable<Order> GetAllOrders(bool includedItems);
		Order GetOrderById(string name, int id);
		
		public bool SaveAll();
		void AddEntity(object model);
		object GetAllOrdersByUser(string username, bool includeItems);
	}
}