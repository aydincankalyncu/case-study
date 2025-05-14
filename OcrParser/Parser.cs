using Newtonsoft.Json;

namespace OcrParser;

public class Parser : IDisposable
{
    private const string JsonPath = "response.json";
    private const double LineTolerance = 10.0;

    public void Run()
    {
        var jsonContent = File.ReadAllText(JsonPath);
        var items = JsonConvert.DeserializeObject<List<OcrItem>>(jsonContent);
        if (items is null)
        {
            return;
        }

        var words = items.Skip(1)
            .Select(i => new Word
            {
                Text = i.Description,
                CenterY = i.BoundingPoly.Vertices.Average(v => v.Y),
                CenterX = i.BoundingPoly.Vertices.Average(v => v.X)
            }).ToList();
        
        var lines = new List<List<Word>>();
        foreach (var word in words.OrderBy(w => w.CenterY))
        {
            var matchedLine = lines.FirstOrDefault(line => Math.Abs(line[0].CenterY - word.CenterY) <= LineTolerance);
            if (matchedLine is not null)
            {
                matchedLine.Add(word);
            }
            else
            {
                lines.Add([word]);           
            }
        }

        var result = lines.OrderBy(l => l[0].CenterY)
            .Select((lineWords, index) => new
            {
                line = index + 1,
                text = string.Join(" ", lineWords.OrderBy(w => w.CenterX).Select(w => w.Text))
            }).ToList();

        foreach (var line in result)
        {
            Console.WriteLine($"{line.line}: {line.text}");
        }

    }

    public class Vertex
    {
        [JsonProperty("x")] public int X { get; set; }
        [JsonProperty("y")] public int Y { get; set; }
    }

    public class BoundingPoly
    {
        [JsonProperty("vertices")] public List<Vertex> Vertices { get; set; }
    }

    public class OcrItem
    {
        [JsonProperty("boundingPoly")] public BoundingPoly BoundingPoly { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
    }

    public class Word
    {
        public string Text { get; set; }
        public double CenterY { get; set; }
        public double CenterX { get; set; }
    }

    public void Dispose()
    {
    }
}