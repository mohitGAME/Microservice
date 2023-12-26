using Grpc.Core;
using Grpc.Net.Client;
using Mango.Services.ProductAPI.ProductGrpc;

namespace Mango.Services.ProductGrpc.Services
{
    public class ProductRatingService //: productRating.productRatingClient
    {
        private readonly ILogger<ProductRatingService> _logger;
        public ProductRatingService(ILogger<ProductRatingService> logger)
        {
            _logger = logger;
        }

        public async Task Ment()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7228");
            var client = new productRating.productRatingClient(channel);
            var request = new ProductRatingRequest() { Id = 2323 };
            try
            {
                var reply = client.GetProductRating(request);
                while (await reply.ResponseStream.MoveNext())
                {
                    var current = reply.ResponseStream.Current;
                    Console.WriteLine($"{current.LiveRating}");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        //public override AsyncServerStreamingCall<RatingResponse> GetProductRating(ProductRatingRequest request, CallOptions options)
        //{
        //    return base.GetProductRating(request, options);
        //}
    }
}


