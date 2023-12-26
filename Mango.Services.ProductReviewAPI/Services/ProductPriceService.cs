using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mango.Services.ProductGrpc;
using Mango.Services.ProductReviewAPI;
using Mango.Services.ProductReviewAPI.ProductGrpc;
using Microsoft.AspNetCore.Identity.Data;

namespace Mango.Services.ProductGrpc.Services
{
    public class ProductRatingService : productRating.productRatingBase
    {

        public override async Task GetProductRating(ProductRatingRequest request, IServerStreamWriter<RatingResponse> responseStream, ServerCallContext context)
        {

            var i = 0;
            while (!context.CancellationToken.IsCancellationRequested && i < 20)
            {
                await Task.Delay(500);
                await responseStream.WriteAsync(new RatingResponse() { LiveRating = i });
                i++;
            }
        }
    }
}


