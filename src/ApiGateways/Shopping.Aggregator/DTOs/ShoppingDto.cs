using System.Collections.Generic;

namespace Shopping.Aggregator.DTOs
{
    public class ShoppingDto
    {
        public string UserName { get; set; }
        public BasketDto BasketWithProducts { get; set; }
        public IEnumerable<OrderResponseDto> Orders { get; set; }
    }
}
