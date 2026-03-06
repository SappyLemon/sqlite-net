using NUnit.Framework;

namespace SQLite.Tests
{
    [TestFixture]
    public class JsonExtensionTest
    {
        static TestDb CreateDb ()
		{
			var db = new TestDb ();
            db.CreateTable<JsonExt> ();

			return db;
		}

        [Test]
		public void GeneratedAttribute ()
		{
			var db = CreateDb ();

            db.Insert (new JsonExt {
                JsonValue = """{"Value1": "A", "Value2": 20}"""
            });

            var result =  db.Query<JsonExt>("SELECT * FROM JsonExt");

            Assert.AreEqual (1, result.Count);
            Assert.AreEqual ("A", result [0].Value1);
        }

        [Test]
        public void JSONExtractRaw()
        {
            var db = CreateDb ();

            db.Insert(new JsonExt
            {
                JsonValue = """{"Value1": "A", "Value2": 20}"""
            });
              db.Insert(new JsonExt
            {
                JsonValue = """{"Value1": "B", "Value2": 30}"""
            });

            var result = db.Query<JsonExt>("SELECT * FROM JsonExt WHERE json_extract(JsonValue, '$.Value2') = 20");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("A", result[0].Value1);
        }

        [Test]
        public void JSONExtract ()
        {
            var db = CreateDb ();

            db.Insert(new JsonExt
            {
                JsonValue = """{"Value1": "A", "Value2": 20}"""
            });
              db.Insert(new JsonExt
            {
                JsonValue = """{"Value1": "B", "Value2": 30}"""
            });

            var result = db.Table<JsonExt>()
                .Where(x => x.JsonValue.JsonExtract<int>("$.Value2") == 20)
                .ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("A", result[0].Value1);
        }
    }


}