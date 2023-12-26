using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json;
using StarWars.Types;

namespace StarWars;

public class StarWarsQuery : ObjectGraphType<object>
{
    private readonly HttpClient _httpClient;

    public StarWarsQuery(StarWarsData data, HttpClient httpClient)
    {
        _httpClient = httpClient;
        Name = "Query";

        Field<ProductType>("product")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the product" }
            ))
            .ResolveAsync(async context =>
        {
            var response = string.Empty;
            var productId = context.GetArgument<int>("id");
            var product = await httpClient.GetAsync($"https://localhost:7000/api/product/{productId}");
            if (product.IsSuccessStatusCode)
            {
                response = await product.Content.ReadAsStringAsync();
            }


            dynamic jsonObj = JsonConvert.DeserializeObject(response);

            var result = JsonConvert.DeserializeObject<Product>(jsonObj?.result.ToString());
            return result;
        }

        );
        Field<CharacterInterface>("hero").ResolveAsync(async context => await data.GetDroidByIdAsync("3"));
        Field<HumanType>("human")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the human" }
            ))
            .ResolveAsync(async context => await data.GetHumanByIdAsync(context.GetArgument<string>("id")));

        Func<IResolveFieldContext, string, Task<Droid>> func = (context, id) => data.GetDroidByIdAsync(id);

        Field<DroidType>("droid")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the droid" }
            ))
            .ResolveDelegate(func);
    }





}
