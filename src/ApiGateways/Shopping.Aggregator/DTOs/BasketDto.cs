using System.Collections.Generic;

namespace Shopping.Aggregator.DTOs
{
    public class BasketDto
    {
        public string UserName { get; set; }
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public decimal TotalPrice { get; set; }
    }
}
