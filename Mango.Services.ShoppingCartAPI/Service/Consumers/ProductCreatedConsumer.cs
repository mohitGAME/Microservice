using Mango.Contracts.Product;
using MassTransit;

namespace Mango.Services.ShoppingCartAPI.Service.Consumers
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var message = context.Message;
            Console.WriteLine(message.Name);
        }
    }
}
