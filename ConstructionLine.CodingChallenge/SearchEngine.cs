using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly IQueryable<Shirt> _shirts;

        public SearchEngine(IQueryable<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }


        public SearchResults Search(SearchOptions options)
        {
            var shirts = _shirts.AsQueryable();

            if (options.Sizes.Count > 0)
            {
                shirts = shirts.Where(x => options.Sizes.Contains(x.Size));
            }

            if (options.Colors.Count > 0)
            {
                shirts = shirts.Where(x => options.Colors.Contains(x.Color));
            }

            var SizeCounts = _shirts.Where(x => options.Colors.Contains(x.Color)).GroupBy(s => s.Size).Select(c => new SizeCount { Size = c.Key, Count = c.Count() }).ToList();
            var ColorCounts = _shirts.Where(x => options.Sizes.Contains(x.Size)).GroupBy(s => s.Color).Select(c => new ColorCount { Color = c.Key, Count = c.Count() }).ToList();

            DefaultSize(SizeCounts);
            DefaultColor(ColorCounts);

            return new SearchResults
            {
                Shirts = shirts.ToList(),
                ColorCounts = ColorCounts,
                SizeCounts = SizeCounts
            };
        }

        private static void DefaultSize(List<SizeCount> sizeCounts)
        {
            foreach (var item in Size.All)
            {
                var sizeCount = new SizeCount { Size = item, Count = 0 };
                if (!sizeCounts.Where(x => x.Size.Id == item.Id).Any())
                {
                    sizeCounts.Add(sizeCount);
                }
            }
        }

        private static void DefaultColor(List<ColorCount> colorCounts)
        {
            foreach (var item in Color.All)
            {
                var sizeCount = new ColorCount { Color = item, Count = 0 };
                if (!colorCounts.Where(x => x.Color.Id == item.Id).Any())
                {
                    colorCounts.Add(sizeCount);
                }
            }
        }


    }
}