using FluentAssertions;
using GreenFieldPizza.Application.Customers;
using GreenFieldPizza.Application.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests;

public class OrderTests
{
    private IDeliverable _deliverable;
    private List<Pizza> _pizzas;

    public OrderTests()
    {
        _pizzas = new List<Pizza>();
        var flavors = new List<Flavor>();
        var anonymousCreateResult = AnonymousCustomer.Create(new Telephone("123456789", "11"), new Adress("streetname", "01"));
        anonymousCreateResult.TryGetContent(out var customer);
        flavors.Add(new Flavor("code", 1));
        var createResult = Pizza.Create(flavors);
        createResult.TryGetContent(out var pizza);
        _pizzas.Add(pizza!);
        _deliverable = customer.AsDeliverable();
    }

    [Fact]
    public void Order_Create_ShouldNotCreate_WhenPizzaListIsEmpty()
    {
        // Given
        var pizzas = new List<Pizza>();
        // When
        var sut = Order.Create(pizzas, _deliverable);
        // Then
        sut.Success.Should().BeFalse();
    }

    [Fact]
    public void Order_Create_ShouldNotCreate_WhenPizzaListHasMoreThanTenItens()
    {
        // Given
        var flavors = new List<Flavor>();
        flavors.Add(new Flavor("code", 1));

        var pizzas = FillPizzaList(12, flavors);
        // When
        var sut = Order.Create(pizzas, _deliverable);
        // Then
        sut.Success.Should().BeFalse();
    }

    [Fact]
    public void Order_Create_ShouldCreate_WhenPizzaListHasTenItens()
    {
        // Given
        var flavors = new List<Flavor>();

        flavors.Add(new Flavor("code", 1));
        var pizzas = FillPizzaList(10, flavors);
        // When
        var commandResult = Order.Create(pizzas, _deliverable);
        commandResult.TryGetContent(out var sut);
        // Then
        sut.Should().NotBeNull();
        sut!.Id.IsValid().Should().BeTrue();
        sut.Pizzas.Count().Should().Be(pizzas.Count);
    }


    [Fact]
    public void Order_Create_ShouldCreate_WhenPizzaListIsNotEmpty()
    {
        // When
        var commandResult = Order.Create(_pizzas, _deliverable);
        commandResult.TryGetContent(out var sut);
        // Then
        sut.Should().NotBeNull();
        sut.Id.IsValid().Should().BeTrue();
        sut.Pizzas.Count().Should().Be(_pizzas.Count);
    }

    [Fact]
    public void Load_ShouldNotLoad_WithEmptyId()
    {
        // When
        var commandResult = Order.Load(Guid.Empty, _pizzas, null, _deliverable, DateTime.UtcNow);
        // Then
        commandResult.Success.Should().BeFalse();
    }

    [Fact]
    public void Load_ShouldLoadOrder()
    {
        // Given
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        // When
        var commandResult = Order.Load(id, _pizzas, null, _deliverable, createdAt);
        commandResult.TryGetContent(out var sut);
        // Then
        commandResult.Success.Should().BeTrue();
        sut.Should().NotBeNull();
        sut.Id.Value.Should().Be(id);
        sut.CreatedAt.Should().Be(createdAt);
        sut.Deliverable.Should().Be(_deliverable);
        sut.Pizzas.Should().BeEquivalentTo(_pizzas);
    }

    private List<Pizza> FillPizzaList(int listSize, List<Flavor> flavors)
    {
        var pizzas = new List<Pizza>();
        for (var i = 0; i < listSize; i++)
        {
            var result = Pizza.Create(flavors);
            if (result.TryGetContent(out var pizza) == false) throw new NullReferenceException("test couldn't create  a pizza");
            pizzas.Add(pizza);
        }
        return pizzas;
    }

}