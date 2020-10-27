using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using ZEntityFrameworkExtensionsDemo;
using ZEntityFrameworkExtensionsDemo.Models;

namespace EntityFrameworkExtensionsTests
{
    public class Tests
    {
        private readonly string BETTY_OWNER_NAME = "Betty Johnson";
        private readonly string JOE_OWNER_STRING = "Joe Johnson";

        [Test]
        public void N1_BulkInsertChickenBreeds()
        {
            var sqlBulkUploader = new SqlBulkUploader(context: new MyDbContext());

            var chickenBreeds = new List<ChickenBreed>()
            {
                new ChickenBreed(
                    name: "Brahma",
                    primaryColor: Color.White),
                new ChickenBreed(
                    name: "Orpington",
                    primaryColor: Color.Blonde),
                new ChickenBreed(
                    name: "Rhode Island Red",
                    primaryColor: Color.Brown),
                new ChickenBreed(
                    name: "Australorp",
                    primaryColor: Color.Black),
            };

            sqlBulkUploader.BulkInsert(chickenBreeds);
        }

        [Test]
        public void N2_BulkInsertOwners_InsertKeepIdentityOn()
        {
            var sqlBulkUploader = new SqlBulkUploader(context: new MyDbContext());

            var owners = new List<Owner>()
            {
                new Owner(
                    id: 1,
                    name: JOE_OWNER_STRING),
                new Owner(
                    id: 2,
                    name: "Mary Johnson"),
                new Owner(
                    id: 3,
                    name: BETTY_OWNER_NAME)
            };

            sqlBulkUploader.BulkInsert(owners);
        }

        [Test]
        public void N3_BulkInsertChickens_IncludeGraphFalse_Issue()
        {
            var sqlBulkUploader = new SqlBulkUploader(context: new MyDbContext());

            var samMooreOwner = new Owner(7, "Sam Moore");
            var silkieBreed = new ChickenBreed(
                        name: "Silkie",
                        primaryColor: Color.Black);
            var silkieSuperstructureCoop = new ChickenCoop(
                        name: "Silkie Superstructure",
                        ownerId: 0,
                        owner: samMooreOwner,
                        housedChickens: null);

            var chickens = new List<Chicken>()
            {
                new Chicken(
                    name: "Horton",
                    isAdoptable: false,
                    chickenBreedId: 0,
                    chickenBreed: silkieBreed,
                    chickenCoopId: 0,
                    chickenCoop: silkieSuperstructureCoop,
                    ownerId: null,
                    owner: samMooreOwner),
                new Chicken(
                    name: "Furby",
                    isAdoptable: false,
                    chickenBreedId: 0,
                    chickenBreed: silkieBreed,
                    chickenCoopId: 0,
                    chickenCoop: silkieSuperstructureCoop,
                    ownerId: null,
                    owner: samMooreOwner)
            };

            sqlBulkUploader.BulkInsert(items: chickens,
                                       includeGraph: false);
        }

        [Test]
        public void N4_BulkInsertChickens_IncludeGraphTrue_MultipleReferenceIssue_FixedInV3()
        {
            var sqlBulkUploader = new SqlBulkUploader(context: new MyDbContext());

            var samMooreOwner = new Owner(7, "Sam Moore");
            var silkieBreed = new ChickenBreed(
                        name: "Silkie",
                        primaryColor: Color.Black);
            var silkieSuperstructureCoop = new ChickenCoop(
                        name: "Silkie Superstructure",
                        ownerId: 0,
                        owner: samMooreOwner,
                        housedChickens: null);

            var chickens = new List<Chicken>()
            {
                new Chicken(
                    name: "Horton",
                    isAdoptable: false,
                    chickenBreedId: 0,
                    chickenBreed: silkieBreed,
                    chickenCoopId: 0,
                    chickenCoop: silkieSuperstructureCoop,
                    ownerId: null,
                    owner: samMooreOwner),
                new Chicken(
                    name: "Furby",
                    isAdoptable: false,
                    chickenBreedId: 0,
                    chickenBreed: silkieBreed,
                    chickenCoopId: 0,
                    chickenCoop: silkieSuperstructureCoop,
                    ownerId: null,
                    owner: samMooreOwner)
            };
                
            sqlBulkUploader.BulkInsert(items: chickens,
                                       includeGraph: true);
        }

        [Test]
        public void N5_BulkInsertChickenCoops_InsertKeepIdentityOff()
        {
            var context = new MyDbContext();
            var sqlBulkUploader = new SqlBulkUploader(context: context);

            var bettyOwner = context.Owners.First(o => o.Name == BETTY_OWNER_NAME);
            var joeOwner = context.Owners.First(o => o.Name == JOE_OWNER_STRING);

            var coops = new List<ChickenCoop>() {
                new ChickenCoop(
                    name: "Bettytown",
                    ownerId: bettyOwner.Id,
                    owner: null,
                    housedChickens: context.Chickens.Where(c => c.Owner == bettyOwner).ToList()
                    ),
                new ChickenCoop(
                    chickenCoopId: 200, // UH-OH
                    name: "Joe's Place",
                    ownerId: joeOwner.Id,
                    owner: null,
                    housedChickens: context.Chickens.Where(c => c.Owner == joeOwner).ToList())
            };

            sqlBulkUploader.BulkInsert(coops);
        }

        [Test]
        public void N6_LargeBenchmark_BulkChickenBreedOperations()
        {
            var dbContext = new MyDbContext();
            var sqlBulkUploader = new SqlBulkUploader(context: new MyDbContext());
            var timer = new Stopwatch();

            var chickenBreeds_Large = new List<ChickenBreed>();
            for (int i = 0; i < 50000; i++)
            {
                chickenBreeds_Large.Add(new ChickenBreed(name: $"{i} LargeBenchmarkBreed", primaryColor: Color.Blonde));
            }

            (string ActionDescription, Action Run)[] timedActions =
            {
                (
                    "dbContextAddRange",
                    () => {
                        dbContext.ChickenBreeds.AddRange(chickenBreeds_Large);
                        dbContext.SaveChanges(); }
                ),
                (
                    "fieldUpdates (not part of benchmark)",
                    () => dbContext.ChickenBreeds.ToList().ForEach(b => b.Name += " Edit")
                ),
                (
                    "dbContextUpdate",
                    () =>
                    {
                        dbContext.SaveChanges();
                    }
                ),
                (
                    "dbContextRemoveRange",
                    () => {
                        dbContext.ChickenBreeds.RemoveRange(chickenBreeds_Large);
                        dbContext.SaveChanges(); }
                ),
                (
                    "efExtensionsBulkInsert",
                    () => sqlBulkUploader.BulkInsert(chickenBreeds_Large)
                ),
                (
                    "fieldUpdates (not part of benchmark)",
                    () => dbContext.ChickenBreeds.ToList().ForEach(b => b.Name += " Edit")
                ),
                (
                    "efExtensionsBulkUpdate",
                    () =>
                    {
                        dbContext.BulkUpdate(dbContext.ChickenBreeds);
                    }
                ),
                (
                    "efExtensionsBulkDelete",
                    () => sqlBulkUploader.BulkDelete(chickenBreeds_Large)
                )
            };

            foreach (var (ActionDescription, Run) in timedActions)
            {
                timer.Restart();
                Run();
                timer.Stop();
                Console.WriteLine($"Large {ActionDescription} time taken: {timer.Elapsed:m\\:ss\\.fff}");
            }
        }

        [Test]
        public void N7_SmallBenchmark_BulkChickenBreedOperations()
        {
            var dbContext = new MyDbContext();
            var sqlBulkUploader = new SqlBulkUploader(context: new MyDbContext());
            var timer = new Stopwatch();

            var chickenBreeds_Small = new List<ChickenBreed>();
            for (int i = 0; i < 3; i++)
            {
                chickenBreeds_Small.Add(new ChickenBreed(name: $"{i} SmallBenchmarkBreed", primaryColor: Color.Blonde));
            }


            (string ActionDescription, Action Run)[] timedActions =
            {
                (
                    "dbContextAddRange",
                    () => {
                        dbContext.ChickenBreeds.AddRange(chickenBreeds_Small);
                        dbContext.SaveChanges(); }
                ),
                (
                    "fieldUpdates (not part of benchmark)",
                    () => dbContext.ChickenBreeds.ToList().ForEach(b => b.Name += " Edit")
                ),
                (
                    "dbContextUpdate",
                    () =>
                    {
                        dbContext.SaveChanges();
                    }
                ),
                (
                    "dbContextRemoveRange",
                    () => {
                        dbContext.ChickenBreeds.RemoveRange(chickenBreeds_Small);
                        dbContext.SaveChanges(); }
                ),
                (
                    "efExtensionsBulkInsert",
                    () => sqlBulkUploader.BulkInsert(chickenBreeds_Small)
                ),
                (
                    "fieldUpdates (not part of benchmark)",
                    () => dbContext.ChickenBreeds.ToList().ForEach(b => b.Name += " Edit")
                ),
                (
                    "efExtensionsBulkUpdate",
                    () =>
                    {
                        dbContext.BulkUpdate(dbContext.ChickenBreeds);
                    }
                ),
                (
                    "efExtensionsBulkDelete",
                    () => sqlBulkUploader.BulkDelete(chickenBreeds_Small)
                )
            };

            foreach (var (ActionDescription, Run) in timedActions)
            {
                timer.Restart();
                Run();
                timer.Stop();
                Console.WriteLine($"Small {ActionDescription} time taken: {timer.Elapsed:m\\:ss\\.fff}");
            }
        }
    }
}