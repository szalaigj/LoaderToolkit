using BatchLoader.Streams;
using LoaderLibrary.Load;
using LoaderLibrary.Services;
using LoaderLibrary.Streams;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader
{
    public class LoaderRegistry : Registry
    {
        public LoaderRegistry()
        {
            For(typeof(FileUtils<>)).Use(typeof(FileUtils<>));
            For(typeof(SqlUtils<>)).Use(typeof(SqlUtils<>));

            // The following selector will be used with prefix (which is the associated source file name):
            For(typeof(ISelector<>)).Use(typeof(DefaultSelector));
            // The following setting for JSON parsing:
            //For(typeof(ISelector<>)).Use(typeof(JsonSelector));

            For(typeof(BaseStreamReaderForLoader<>)).Use(typeof(StreamReaderForLoaderWithPrefix));
            For(typeof(BaseBulkInsertFileCreator<>)).Use(typeof(DefaultBulkInsertFileCreator));

            //TODO:
            // The following list of mappers/mergers are examples only (for tweeter) but the real mappers/mergers should be enumerated here:
            //For(typeof(Mapper<>)).Add(typeof(Mappers.Tweet));
            //For(typeof(Mapper<>)).Add(typeof(Mappers.TweetRetweet));
            //For(typeof(Mapper<>)).Add(typeof(Mappers.User));
            //For<Merger>().Add<Mergers.Tweet>();
            //For<Merger>().Add<Mergers.TweetRetweet>();
            //For<Merger>().Add<Mergers.User>();

            For(typeof(ChunkService<>)).Add(typeof(ChunkService<>)).Named("WithAutoWiring");

            // If the all mappers/mergers are not necessary the explicit mappers/mergers can be defined in the following manner:
            //For(typeof(ChunkService<>)).Add(typeof(ChunkService<>)).Named("ExplicitMappersAndMergers")
            //    .EnumerableOf<Mapper>().Contains(y =>
            //    {
            //        y.OfConcreteType<Mappers.Tweet>();
            //        y.OfConcreteType<Mappers.User>();
            //    }
            //    ).EnumerableOf<Merger>().Contains(y =>
            //    {
            //        y.OfConcreteType<Mergers.Tweet>();
            //        y.OfConcreteType<Mergers.User>();
            //    }
            //    );
        }
    }
}
