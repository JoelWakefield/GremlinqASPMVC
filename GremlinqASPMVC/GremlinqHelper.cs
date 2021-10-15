using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Samples.Shared;
using Microsoft.Extensions.Caching.Memory;

namespace GremlinqASPMVC
{
    public class GremlinqHelper
    {
        private readonly IGremlinQuerySource source;
        private readonly IMemoryCache memoryCache;

        public GremlinqHelper(IGremlinQuerySource source, IMemoryCache memoryCache)
        {
            this.source = source;
            this.memoryCache = memoryCache;
        }

        public async Task<List<dynamic>> People(string relationshipType, string id = null)
        {
            dynamic items;
            string cacheKey = $"Gremlinq-People-{relationshipType}";

            if (!memoryCache.TryGetValue(cacheKey, out items))
            {
                //  fetch the value from the azure
                switch (relationshipType)
                {
                    case "1:n":
                        items = await source
                            .V<Person>(id)
                            .As((__, person) => __
                                .OutE<Knows>()
                                .InV<Person>()
                                .As((__, people) => __
                                    .Select(person, people)));
                        break;
                    case "n:1":
                        items = await source
                            .V<Person>(id)
                            .As((__, people) => __
                                .InE<Knows>()
                                .OutV<Person>()
                                .As((__, person) => __
                                    .Select(person, people)));
                        break;
                    case "n:n":
                        items = await source
                            .V<Person>()
                            .As((__, person) => __
                                .OutE<Knows>()
                                .InV<Person>()
                                .As((__, people) => __
                                    .Select(person, people)));
                        break;
                    default:
                        return null;
                }

                //  save the value
                memoryCache.Set(
                    cacheKey,
                    ((Person, Person)[])items,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
            }

            return Repackage(items);
        }

        public async Task<List<dynamic>> Pet(string relationshipType, string id = null)
        {
            dynamic items;
            string cacheKey = $"Gremlinq-Pet-{relationshipType}";

            if (!memoryCache.TryGetValue(cacheKey, out items))
            {
                switch (relationshipType)
                {
                    case "1:n":
                        items = await source
                            .V<Person>(id)
                            .As((__, person) => __
                                .OutE<Owns>()
                                .InV<Pet>()
                                .As((__, pet) => __
                                    .Select(person, pet)));
                        break;
                    case "n:1":
                        items = await source
                            .V<Pet>(id)
                            .As((__, pet) => __
                                .InE<Owns>()
                                .OutV<Person>()
                                .As((__, person) => __
                                    .Select(person, pet)));
                        break;
                    case "n:n":
                        items = await source
                            .V<Person>()
                            .As((__, person) => __
                                .OutE<Owns>()
                                .InV<Pet>()
                                .As((__, pet) => __
                                    .Select(person, pet)));
                        break;
                    default:
                        return null;
                }

                //  save the value
                memoryCache.Set(
                    cacheKey,
                    ((Person, Pet)[])items,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
            }

            return Repackage(items);
        }

        public async Task<List<dynamic>> Software(string relationshipType, string id = null)
        {
            dynamic items;

            string cacheKey = $"Gremlinq-Software-{relationshipType}";

            if (!memoryCache.TryGetValue(cacheKey, out items))
            {
                switch (relationshipType)
                {
                    case "1:n":
                        items = await source
                            .V<Person>(id)
                            .As((__, person) => __
                                .OutE<Created>()
                                .InV<Software>()
                                .As((__, software) => __
                                    .Select(person, software)));
                        break;
                    case "n:1":
                        items = await source
                            .V<Software>(id)
                            .As((__, software) => __
                                .InE<Created>()
                                .OutV<Person>()
                                .As((__, person) => __
                                    .Select(person, software)));
                        break;
                    case "n:n":
                        items = await source
                            .V<Person>()
                            .As((__, person) => __
                                .OutE<Created>()
                                .InV<Software>()
                                .As((__, software) => __
                                    .Select(person, software)));
                        break;
                    default:
                        return null;
                }

                //  save the value
                memoryCache.Set(
                    cacheKey,
                    ((Person, Software)[])items,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
            }

            return Repackage(items);
        }


        private List<dynamic> Repackage((Person, Person)[] items)
        {
            List<dynamic> list = new List<dynamic>();

            foreach (var item in items)
            {
                var (h, t) = item;
                list.Add(new dynamic[] { h, t });
            }

            return list;
        }

        private List<dynamic> Repackage((Person, Pet)[] items)
        {
            List<dynamic> list = new List<dynamic>();

            foreach (var item in items)
            {
                var (h, t) = item;
                list.Add(new dynamic[] { h, t });
            }

            return list;
        }

        private List<dynamic> Repackage((Person, Software)[] items)
        {
            List<dynamic> list = new List<dynamic>();

            foreach (var item in items)
            {
                var (h, t) = item;
                list.Add(new dynamic[] { h, t });
            }

            return list;
        }
    }
}
