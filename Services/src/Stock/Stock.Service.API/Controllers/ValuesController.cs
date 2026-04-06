using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Kernel.Base.Contracts;
using RabbitMQ.Kernel.Builders;
using Stock.Service.Domain.Dtos;
using Stock.Service.Domain.Entities;

namespace Stock.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IQueueProducerBuilder<OrderDto> Queuebuilder;

        public ValuesController(IQueueProducerBuilder<OrderDto> _Queuebuilder)
        {
            Queuebuilder= _Queuebuilder;   
        }

        [HttpPost]
        public IActionResult Get(OrderDto orderDto) {


            Order order = new()
            {
                ProductName = orderDto.ProductName,
                Price = orderDto.Price,
                Quantity = orderDto.Quantity
            };
            var producer =Queuebuilder
                .UseStrategy(Strategy.Direct)
                .WithQueue("new Direct Queque x")
                .Build();


            producer.PublishMessage(orderDto);
            return Ok();
        }
    }
}
