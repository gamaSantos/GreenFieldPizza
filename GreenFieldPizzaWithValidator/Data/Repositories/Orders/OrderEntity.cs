using GreenFieldPizza.Application.Customers;
using GreenFieldPizza.Application.Orders;

namespace GreenFieldPizza.Data.Repositories.Orders;

class OrderEntity
{
    public Guid Id { get; set; }
    public Nullable<Guid> CustomerId { get; set; }
    public IEnumerable<PizzaEntity> Pizzas { get; set; } = new List<PizzaEntity>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string StreetName { get; set; } = string.Empty;
    public string StreetNumber { get; set; } = string.Empty;
    public string? AdressObservation { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string PhoneRegion { get; set; } = string.Empty;

    public static OrderEntity FromDomain(Order domain)
    {
        return new OrderEntity
        {
            Id = domain.Id,
            CustomerId = domain.CustomerId?.Value,
            AdressObservation = domain.Deliverable.Adress.Observation,
            StreetName = domain.Deliverable.Adress.StreetName,
            StreetNumber = domain.Deliverable.Adress.Number,
            CreatedAt = domain.CreatedAt,
            PhoneNumber = domain.Deliverable.Phone.PhoneNumber,
            PhoneRegion = domain.Deliverable.Phone.RegionCode,
            Pizzas = domain.Pizzas.Select(p => new PizzaEntity
            {
                Flavors = p.Flavors.Select(f => new FlavorEntity
                {
                    Price = f.BasePrice,
                    Sku = f.Code
                })
            })
        };
    }

    public Order ToDomain()
    {
        var pizzasResult = this.Pizzas.Select(
            pizzaEntity => Pizza.Create(
                pizzaEntity.Flavors.Select(flavorEntity =>
                    new Flavor(flavorEntity.Sku, flavorEntity.Price))));

        var pizzas = pizzasResult.Select(cp => cp.Match(
            e => throw new InvalidDataException($"Error while loading order {Id}"),
            p => p
        ));
        var anonymousResult = AnonymousCustomer.Create(new Telephone(PhoneNumber, PhoneRegion), new Address(StreetName, StreetNumber, AdressObservation));
        var deliverable = anonymousResult.Match(
            e => throw new InvalidDataException($"Error while loading order {Id}"),
            ac => (IDeliverable)ac
        );
        var loadOrderCommandResult = Order.Load(Id, pizzas, CustomerId, (IDeliverable)deliverable, CreatedAt);
        return loadOrderCommandResult.Match(
            e => throw new InvalidDataException($"Error while loading order {Id}"),
            order => order
        );
    }
}

class PizzaEntity
{
    public IEnumerable<FlavorEntity> Flavors { get; set; } = new List<FlavorEntity>();
}

class FlavorEntity
{
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
