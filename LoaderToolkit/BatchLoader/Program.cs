using BatchLoader.Verbs;
using LoaderLibrary.CommandLineParser;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Type> verbs = new List<Type>() 
            {
                typeof(Create),
                typeof(Start),
            };

            BatchLoaderVerb v = null;

            try
            {
                PrintHeader();
                v = (BatchLoaderVerb)ArgumentParser.Parse(args, verbs);
                ObjectFactory.Initialize(x =>
                {
                    x.AddRegistry(new LoaderRegistry(v.GetMapperType(), v.GetMergerType()));
                });
            }
            catch (ArgumentParserException ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.WriteLine();

                ArgumentParser.PrintUsage(verbs, Console.Out);
            }

            if (v != null)
            {
                v.Run();
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine(
            @"Batch loader utility
            (c) 2012-2014 László Dobos dobos@complex.elte.hu, János Szalai-Gindl szalai@complex.elte.hu
            Department of Physics of Complex Systems, Eötvös Loránd University

            ");
        }
    }
}
