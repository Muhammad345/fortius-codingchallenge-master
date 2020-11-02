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

            if (options.Sizes.Count > 0 )
            {
                shirts  = shirts.Where(x => options.Sizes.Contains(x.Size));
            }

            if (options.Colors.Count > 0)
            {
                shirts = shirts.Where(x =>  options.Colors.Contains(x.Color));
            }

            var SizeCounts = Size.All.AsQueryable().GroupBy(s=>s).Select(c => new SizeCount { Size = c.Key, Count = _shirts.Where(x => options.Sizes.Contains(x.Size)).Count(x=>x.Size.Id == c.Key.Id) });
            var ColorCounts = Color.All.AsQueryable().GroupBy(s => s).Select(c => new ColorCount { Color = c.Key, Count = _shirts.Where(x => options.Colors.Contains(x.Color)).Count(x => x.Color.Id == c.Key.Id) });
            
           
            return new SearchResults
            {
                Shirts = shirts.ToList(),
                ColorCounts = ColorCounts.ToList(),
                SizeCounts = SizeCounts.ToList()
            };
        }
    }
}