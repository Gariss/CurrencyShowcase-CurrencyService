using FavoritesService.Domain.Entities;

namespace FavoritesService.Tests.RepositoryTests.UserCurrencyRepositoryTests;

public partial class UserCurrencyRepositoryTests
{
    private static readonly Guid userId1 = Guid.Parse("a5c28285-15f5-4a29-9975-df3764f1ccb6");
    private static readonly Guid userId2 = Guid.Parse("47067f0f-e72d-4bb0-8306-ef0dd8a831c3");
    private static readonly Guid userId3 = Guid.Parse("f3488613-c09d-4440-bfc3-30f573d12849");
    private static readonly Guid userId4 = Guid.Parse("826d6461-1384-444f-9e7a-4a9721f36705");

    private readonly UserCurrency[] testUserCurrencies =
    [
        // User: a5c28285-15f5-4a29-9975-df3764f1ccb6 (9 currencies)
        new UserCurrency { UserId = userId1, CurrencyId = "R01235" },
        new UserCurrency { UserId = userId1, CurrencyId = "R01239" },
        new UserCurrency { UserId = userId1, CurrencyId = "R01035" },
        new UserCurrency { UserId = userId1, CurrencyId = "R01775" },
        new UserCurrency { UserId = userId1, CurrencyId = "R01350" },
        new UserCurrency { UserId = userId1, CurrencyId = "R01010" },
        new UserCurrency { UserId = userId1, CurrencyId = "R01530" },
        new UserCurrency { UserId = userId1, CurrencyId = "R01535" },
        new UserCurrency { UserId = userId1, CurrencyId = "R01770" },

        // User: 47067f0f-e72d-4bb0-8306-ef0dd8a831c3 (7 currencies)
        new UserCurrency { UserId = userId2, CurrencyId = "R01239" },
        new UserCurrency { UserId = userId2, CurrencyId = "R01235" },
        new UserCurrency { UserId = userId2, CurrencyId = "R01820" },
        new UserCurrency { UserId = userId2, CurrencyId = "R01375" },
        new UserCurrency { UserId = userId2, CurrencyId = "R01565" },
        new UserCurrency { UserId = userId2, CurrencyId = "R01700J" },
        new UserCurrency { UserId = userId2, CurrencyId = "R01100" },

        // User: f3488613-c09d-4440-bfc3-30f573d12849 (11 currencies)
        new UserCurrency { UserId = userId3, CurrencyId = "R01235" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01239" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01035" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01775" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01350" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01530" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01625" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01200" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01585F" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01760" },
        new UserCurrency { UserId = userId3, CurrencyId = "R01115" },

        // User: 826d6461-1384-444f-9e7a-4a9721f36705 (6 currencies)
        new UserCurrency { UserId = userId4, CurrencyId = "R01239" },
        new UserCurrency { UserId = userId4, CurrencyId = "R01235" },
        new UserCurrency { UserId = userId4, CurrencyId = "R01035" },
        new UserCurrency { UserId = userId4, CurrencyId = "R01820" },
        new UserCurrency { UserId = userId4, CurrencyId = "R01375" },
        new UserCurrency { UserId = userId4, CurrencyId = "R01775" }
    ];
}
