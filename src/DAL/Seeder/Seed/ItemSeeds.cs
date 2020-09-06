using System;
using System.Collections.Generic;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Models.Seeder
{
    public static class ItemSeeds
    {
        public static void AddShopItemSeeds(OrganizationDbContext dbContext)
        {
            var itemsSeed = new List<Item>
            {
                new Item
                {
                    Name = "A Mild Case of Dedication",
                    Description = "Item Desc",
                    Image = "https://api.tayra.io/imgs/icon56.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Skilled!",
                    Description = "Have confidence in your skills.",
                    Image = "https://api.tayra.io/imgs/icon02.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Merchant",
                    Description = "Caveat Venditor",
                    Image = "https://api.tayra.io/imgs/icon07.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Overachiever",
                    Description = "When you do more than you’re paid for, eventually you’ll be paid for more than you do.",
                    Image = "https://api.tayra.io/imgs/icon04.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "My Precious",
                    Description = "We wants it. We needs it. Must have the precious!",
                    Image = "https://api.tayra.io/imgs/icon06.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Wizard",
                    Description = "Don't mess with a wizard when he's wizarding!",
                    Image = "https://api.tayra.io/imgs/icon10.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Spooky",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon11.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Champion",
                    Description = "It's pretty cool to be called a champion.",
                    Image = "https://api.tayra.io/imgs/icon03.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Lucky",
                    Description = "'I’m a great believer in luck, and I find the harder I work the more I have of it.' -Thomas Jefferson, American Founding Father",
                    Image = "https://api.tayra.io/imgs/icon16.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "A Helping Paw",
                    Description = "Who does not need a helping paw in their life?",
                    Image = "https://api.tayra.io/imgs/icon17.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Tactical genius",
                    Description = "The best tactics are elegantly simple.",
                    Image = "https://api.tayra.io/imgs/icon18.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "No stop 'til the Top",
                    Description = "There are no shortcuts to any place worth going.",
                    Image = "https://api.tayra.io/imgs/icon01.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "HTML/CSS Hipster",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon20.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "C# Disciple",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon21.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "React.js Lover",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon24.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Angular Admirer",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon22.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Python Devotee",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon23.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Terminal Aficionado",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon25.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Linux Fanatic",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon26.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Team Booster",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon27.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Design Junkie",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon28.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Common,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "PHP Believer",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon29.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Code Detective",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon57.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Java Enthusiast",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon31.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "SQL Whiz",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon32.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "C# Prodigy",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon21.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "React.js Prodigy",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon24.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Angular Prodigy",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon22.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "JavaScript Prodigy",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon33.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Python Prodigy",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon23.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "PHP Prodigy",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon29.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Java Prodigy",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon31.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "SQL Prodigy",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon32.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "C++ Prodigy",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon34.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "codeConventionsFreak",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon35.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Legendary,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Slack Spammer",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon36.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "I speak Code",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon37.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Coffee Addict",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon38.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "No unit tests needed",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon39.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Works on my machine",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon40.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "I will refactor later",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon41.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "{Hello World!}",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon42.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Vim Lover",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon43.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Vim FTW",
                    Description = "Mouse is overrated!",
                    Image = "https://api.tayra.io/imgs/icon43.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Gaming brought me here",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon44.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "I started coding because of games",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon45.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Casual gamer",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon46.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "GG EZ",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon47.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Easy game easy life",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon48.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Bug? ...or feature?",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon50.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Bug Fixer",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon49.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Production Guardian",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon51.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Hero of the day!",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon52.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "Bugs 404",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon53.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Epic,
                    Type = ItemTypes.TayraBadge
                },
                new Item
                {
                    Name = "All tests matter!",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon54.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Build breaker",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon55.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Rare,
                    Type = ItemTypes.TayraTitle
                },
                new Item
                {
                    Name = "Hacker",
                    Description = null,
                    Image = "https://api.tayra.io/imgs/icon30.png",
                    Price = 10.2f,
                    IsActivable = true,
                    IsDisenchantable = true,
                    IsGiftable = true,
                    Rarity = ItemRarities.Legendary,
                    Type = ItemTypes.TayraTitle
                },
            };

            foreach (var item in itemsSeed)
            {
                switch (item.Rarity)
                {
                    case ItemRarities.Common:
                        item.Price = 30;
                        break;

                    case ItemRarities.Rare:
                        item.Price = 70;
                        break;

                    case ItemRarities.Epic:
                        item.Price = 120;
                        break;

                    case ItemRarities.Legendary:
                        item.Price = 250;
                        break;
                }

                item.ShopQuantityRemaining = null;
                item.QuestsQuantityRemaining = null;
                item.GiveawayQuantityRemaining = null;
            }

            dbContext.AddRange(itemsSeed);
            Console.WriteLine("Added " + nameof(itemsSeed));

            var shopItemsSeed = new List<ShopItem>();
            foreach (var item in itemsSeed)
            {
                shopItemsSeed.Add(new ShopItem
                {
                    ItemId = item.Id,
                    IsGlobal = true,
                });
            }

            dbContext.AddRange(shopItemsSeed);
            Console.WriteLine("Added " + nameof(shopItemsSeed));
        }
    }
}