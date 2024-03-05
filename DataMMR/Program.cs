// See https://aka.ms/new-console-template for more information
using System.Text.Json;

int[][] dirs = new int[][]
{
    new int[] { 0, 1 },
    new int[] { 0, -1 },
    new int[] { 1, 0 },
    new int[] { -1, 0 },
    new int[] { 1, -1 },
    new int[] { -1, 1 },
};

var data = JsonSerializer.Deserialize<List<List<JsonElement>>>(File.ReadAllText(@"G:\CSharpDev\Learn\MMRProblem\DataMMR\AbilityPanelTest.json"));
Dictionary<Pos, (int, int)> map = new()
{
    { new Pos(0, 0), (0, 0) }
};

List<int> W = [0];
int idx = 1;
foreach (var item in data)
{
    map.Add(new Pos(item[7].GetInt32(), item[8].GetInt32()), (idx, item[3].GetInt32()));
    W.Add(item[3].GetInt32());
    idx++;
}

var pointsCnt = data.Count + 1;
var points = map.Keys.ToArray();
List<Edge> edges = new();

for (int i = 0; i < pointsCnt; i++)
{
    var neighbours = GetNeighbours(points[i]);
    foreach (var neighbour in neighbours)
    {
        var (idx1, w1) = map[points[i]];
        var (idx2, w2) = map[neighbour];
        edges.Add(new Edge(idx1, idx2, Math.Abs(w2)));
    }
}

Console.WriteLine(pointsCnt);
foreach (var w in W)
{
    Console.Write($"{w} ");
}
Console.WriteLine();
Console.WriteLine(edges.Count);
foreach (var edge in edges)
{
    Console.WriteLine($"{edge.S} {edge.T}");
}

// // 输出一下边
// foreach (var edge in edges)
// {
//     Console.WriteLine($"{edge.S} -> {edge.T} : {edge.W}");
// }





Pos[] GetNeighbours(Pos pos)
{
    List<Pos> res = new();
    foreach (var dir in dirs)
    {
        var newPos = new Pos(pos.X + dir[0], pos.Y + dir[1]);
        if (map.ContainsKey(newPos))
        {
            res.Add(newPos);
        }
    }
    return res.ToArray();
}

record Pos(int X, int Y);
/// <summary>
/// 边
/// </summary>
/// <param name="S"></param>
/// <param name="T"></param>
/// <param name="W"></param>
record Edge(int S, int T, int W);

