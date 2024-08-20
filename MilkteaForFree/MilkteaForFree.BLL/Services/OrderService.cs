using MilkteaForFree.DAL.Entities;

public class OrderService
{
    private readonly MilkTeaContext _context;

    public OrderService()
    {
        _context = new MilkTeaContext();
    }

    public void AddOrder(Order order, List<OrderDetail> orderDetails)
    {
        using (var context = new YourDbContext())
        {
            // Add the order
            context.Orders.Add(order);

            // Add the order details
            foreach (var detail in orderDetails)
            {
                context.OrderDetails.Add(detail);
            }

            // Save changes
            context.SaveChanges();
        }
    }

    public IEnumerable<Order> GetOrders(DateTime fromDate, DateTime toDate)
    {
        return _context.Orders
            .Where(o => o.OrderDate >= fromDate && o.OrderDate <= toDate)
            .ToList();
    }
}
