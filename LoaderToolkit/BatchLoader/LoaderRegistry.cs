﻿using BatchLoader.Streams;
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
        public LoaderRegistry(Type mapperType, Type mergerType)
        {
            For(typeof(FileUtils<string>)).Use(typeof(FileUtils<string>));
            For(typeof(SqlUtils<string>)).Use(typeof(SqlUtils<string>));

            // The following selector will be used with prefix (which is the associated source file name):
            For(typeof(ISelector<string>)).Use(typeof(DefaultSelector));
            // The following setting for JSON parsing:
            //For(typeof(ISelector<>)).Use(typeof(JsonSelector));

            For<StreamReaderForLoaderWithPrefix>().Use<StreamReaderForLoaderWithPrefix>();
            For(typeof(BaseBulkInsertFileCreator<string>)).Use(typeof(DefaultBulkInsertFileCreator));

            For(typeof(Mapper<string>)).Add(mapperType);
            For(typeof(Merger)).Add(mergerType);

            For(typeof(ChunkService<string>)).Add(typeof(ChunkService<string>)).Named("WithAutoWiring");

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
