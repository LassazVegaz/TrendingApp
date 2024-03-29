using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PurchaseService;
using PurchaseService.Models;

namespace PurchaseServiceTest;

public class PurchaseManagerTest
{
    private ServiceProvider _provider;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddDbContext<PurchaseServiceContext>(options => options.UseInMemoryDatabase(databaseName: "TestDb"));

        _provider = services.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        var context = _provider.GetRequiredService<PurchaseServiceContext>();
        context.Database.EnsureDeleted();
        _provider.Dispose();
    }


    [Test]
    public void SufficientCreditsPruchaseTest()
    {
        var context = _provider.GetRequiredService<PurchaseServiceContext>();
        context.Users.Add(new User { Id = 1, Credits = 1000, });
        context.Items.Add(new Item { Id = 1, Price = 100, });
        context.SaveChanges();

        var purchaseManager = new PurchaseManager(context);

        Assert.DoesNotThrowAsync(async () => await purchaseManager.Purchase(1, 1));

        var user = context.Users.Find(1);
        var item = context.Items.Find(1);

        Assert.Multiple(() =>
        {
            Assert.That(user?.Credits, Is.EqualTo(900));

            Assert.That(item?.Users.Count, Is.EqualTo(1));
            Assert.That(user?.Items.Count, Is.EqualTo(1));

            Assert.That(item?.Users[0].Id, Is.EqualTo(1));
            Assert.That(user?.Items[0].Id, Is.EqualTo(1));
        });
    }

    [Test]
    public void InsufficientCreditsPruchaseTest()
    {
        var context = _provider.GetRequiredService<PurchaseServiceContext>();
        context.Users.Add(new User { Id = 1, Credits = 100, });
        context.Items.Add(new Item { Id = 1, Price = 1000, });
        context.SaveChanges();

        var purchaseManager = new PurchaseManager(context);

        Assert.ThrowsAsync<ArgumentException>(async () => await purchaseManager.Purchase(1, 1));

        var user = context.Users.Find(1);
        var item = context.Items.Find(1);

        Assert.Multiple(() =>
        {
            Assert.That(user?.Credits, Is.EqualTo(100));

            Assert.That(item?.Users.Count, Is.EqualTo(0));
            Assert.That(user?.Items.Count, Is.EqualTo(0));
        });
    }


    [Test]
    public void AlreadyPurchasedTest()
    {
        var context = _provider.GetRequiredService<PurchaseServiceContext>();
        {
            var _item = new Item { Id = 1, Price = 100, };
            context.Items.AddRange(_item, new() { Id = 2, Price = 200 });
            context.Users.Add(new User { Id = 1, Credits = 1000, Items = [_item] });
            context.SaveChanges();
        }

        var purchaseManager = new PurchaseManager(context);

        Assert.ThrowsAsync<ArgumentException>(async () => await purchaseManager.Purchase(1, 1));
        Assert.DoesNotThrowAsync(async () => await purchaseManager.Purchase(1, 2));
    }


    [Test]
    public void GetPurchasedItemsTest()
    {
        var context = _provider.GetRequiredService<PurchaseServiceContext>();
        context.Users.Add(new User { Id = 1, Credits = 1000, });
        context.Items.AddRange(
            new Item { Id = 1, Price = 100, }, new Item { Id = 2, Price = 200, }, new Item { Id = 3, Price = 200, });
        context.SaveChanges();

        var purchaseManager = new PurchaseManager(context);

        IEnumerable<int> purchasedItems = [];
        Assert.DoesNotThrowAsync(async () =>
        {
            await purchaseManager.Purchase(1, 1);
            await purchaseManager.Purchase(1, 2);
            purchasedItems = await purchaseManager.GetPurchasedItems(1);
        });

        Assert.Multiple(() =>
        {
            Assert.That(purchasedItems.Count(), Is.EqualTo(2));
            Assert.That(purchasedItems.SingleOrDefault(p => p == 1), Is.Not.EqualTo(default(int)));
            Assert.That(purchasedItems.SingleOrDefault(p => p == 2), Is.Not.EqualTo(default(int)));
            Assert.That(purchasedItems.SingleOrDefault(p => p == 3), Is.EqualTo(default(int)));
        });
    }
}