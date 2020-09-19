namespace justibot_server.Modules.Hacktoberfest.Model
{
    public class Joke
    {
        public int id { get; set; }
        public string type { get; set; }
        public string setup { get; set; }
        public string punchline { get; set; }
    }

    public class Another
    {
        public string value { get; set; }
        public string quote { get; set; }
    }
}
