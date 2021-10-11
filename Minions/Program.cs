using System;
using System.Linq;
using Minions.Data;
namespace Minions
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task2();
            // Task3();
            // Task4();
            // Task5();
            // Task6();
        }
        public static void Task2()
        {
            using (var ctx = new MinionsContext())
            {
                var villains = ctx.MinionsVillains
                    .GroupBy(e => e.Villain.Name)
                    .Select(e => new
                    {
                        Name = e.Key,
                        NumberOfMinions = e.Count()
                    })
                    .Where(e => e.NumberOfMinions >= 3)
                    .OrderByDescending(g => g.NumberOfMinions);
                
                foreach (var villain in villains)
                {
                    Console.WriteLine($"{villain.Name} {villain.NumberOfMinions}");
                }
            }
        }
        public static void Task3()
        {
            using (var ctx = new MinionsContext())
            {
                try
                {
                    int villainId = Convert.ToInt32(Console.ReadLine());
                    var villain = ctx.Villains.SingleOrDefault(e => e.Id == villainId);
                    if (villain == null)
                    {
                        throw new ArgumentException($"No villain with ID 10 exists in the database.");
                    }
                    
                    var minions = ctx.MinionsVillains
                        .Where(e => e.Villain.Name == villain.Name)
                        .Select((e, index) => new
                        {
                            index,
                            e.Minion.Name,
                            e.Minion.Age
                        })
                        .ToList();
                    
                    Console.WriteLine($"Villain: {villain.Name}");
                    
                    if (minions.Count != 0)
                    {
                        foreach (var minion in minions)
                        {
                            Console.WriteLine($"{minion.index}. {minion.Name} {minion.Age}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("(no minions)");
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        
        public static void Task4()
        {
            using (var ctx = new MinionsContext())
            {
                Console.Write("Minion: ");
                var minionInfo = Console.ReadLine()?.Split(" ").ToList();
                Console.Write("Villain: ");
                var villainInfo = Console.ReadLine();
                
                var town = ctx.Towns.SingleOrDefault(e => e.Name == minionInfo[2]);
                if (town == null)
                {
                    town = new Town
                    {
                        Name = minionInfo?[2]
                    };
                    
                    ctx.Add(town);
                    Console.WriteLine($"Город {town.Name} был добавлен в базу данных.");
                }
                
                var minion = new Minion()
                {
                    Name = minionInfo?[0],
                    Age = Convert.ToInt32(minionInfo?[1]),
                    Town = town
                };
                    
                var villain = ctx.Villains.SingleOrDefault(e => e.Name == villainInfo);
                if (villain == null)
                {
                    villain = new Villain
                    {
                        Name = villainInfo,
                        EvilnessFactor = ctx.EvilnessFactors.Single(e => e.Name == "Evil")
                    };
                    
                    ctx.Add(villain);
                    Console.WriteLine($"Злодей {villain.Name} был добавлен в базу данных.");
                }
                
                var mv = new MinionsVillain()
                {
                    Minion = minion,
                    Villain = villain
                };
                
                ctx.Add(minion);
                ctx.Add(mv);
                ctx.SaveChanges();
                Console.WriteLine($"Успешно добавлен {minion.Name}, чтобы быть миньоном {villain.Name}.");
            }
        }
        
        public static void Task5()
        {
            using (var ctx = new MinionsContext())
            {
                int villainId = Convert.ToInt32(Console.ReadLine());
                
                var villain = ctx.Villains.SingleOrDefault(e => e.Id == villainId);
                if (villain != null)
                {
                    var freeMinions = ctx.MinionsVillains
                        .Where(e => e.VillainId == villainId);
                
                    ctx.Villains.Remove(villain);
                    Console.WriteLine($"{villain.Name} был удален.");
                    ctx.MinionsVillains.RemoveRange(freeMinions);
                    Console.WriteLine($"{freeMinions.Count()} миньонов было освобождено.");
                    ctx.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Такой злодей не найден");
                }
            }
        }
        
        public static void Task6()
        {
            using (var ctx = new MinionsContext())
            {
                var minionsId = Console.ReadLine()?.Split(" ").Select(int.Parse).ToList();
                var minions = ctx.Minions;
                foreach (var minion in minions)
                {
                    if (minionsId != null && minionsId.Contains(minion.Id))
                    {
                        minion.Age++;
                    }
                    Console.WriteLine($"{minion.Name} {minion.Age}");
                }
                ctx.UpdateRange(minions);
                ctx.SaveChanges();
            }
        }
    }
}