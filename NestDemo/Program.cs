using Nest;
using System;

namespace NestDemo
{
    class Program
    {
        public static Uri node;
        public static ConnectionSettings settings;
        public static ElasticClient client;

        static void Main(string[] args)
        {
            node = new Uri("http://localhost:9200");
            settings = new ConnectionSettings(node);
            client = new ElasticClient(settings);
            settings.DefaultIndex("my_blog");

            //var indexSettings = new IndexSettings();
            //indexSettings.NumberOfReplicas = 1;
            //indexSettings.NumberOfShards = 1;

            //var indexCreated = client.CreateIndex("my_blog",
            //     s => s.Mappings(ms => ms
            //     .Map<newBlogPost>(m => m)));

            //InsertData();
            //PerformTermQuery();
            //PerformMatchPhrase();
            PerformDateRangeQuery();
        }

        public static void InsertData()
        {
            var newBlogPost = new Post
            {
                UserID = 1,
                PostDate = DateTime.Now,
                PostText = "This is another blog post from NEST!"
            };

            var newBlogPost2 = new Post
            {
                UserID = 2,
                PostDate = DateTime.Now,
                PostText = "This is a third blog post from NEST!"
            };

            var newBlogPost3 = new Post
            {
                UserID = 2,
                PostDate = DateTime.Now.AddDays(5),
                PostText = "This is a blog post from the future!"
            };


            client.IndexDocument(newBlogPost);
            client.IndexDocument(newBlogPost2);
            client.IndexDocument(newBlogPost3);
        }

        public static void PerformTermQuery()
        {
            var result =
            client.Search<Post>(s => s
            .Query(p => p.Term(q => q.PostText, "blog")));
        }
        public static void PerformMatchPhrase()
        {
            var result =
            client.Search<Post>(s => s
            .Query(q => q.MatchPhrase (m => m.Field("postText").Query("This is a third blog post from NEST"))));
        }

        public static void PerformDateRangeQuery()
        {
            var result =
            client.Search<Post>(s => s
            .Query(q => q.DateRange(c => c
                            .Field(p => p.PostDate)
                            .GreaterThan("3/15/2018")
                            .LessThan("3/20/2018")
                            .Format("MM/dd/yyyy"))));
        }

    }
}
