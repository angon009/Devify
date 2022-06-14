namespace ECommerce.Core.Entities.Orders
{
    public class OrderCount
    {
        public OrderCount()
        {

        }

        public double SumTotal { get; set; }
        public double TotalCost { get; set; }
        public double SumDiscount { get; set; }
        public int CountTotal { get; set; }
    }
}
