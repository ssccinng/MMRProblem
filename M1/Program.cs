// See https://aka.ms/new-console-template for more information

int N = int.Parse(Console.ReadLine());

// 0 ~ N-1节点的权重, 表示到达该点所需要耗费的资源
int[] W = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();

// 边数量
int M = int.Parse(Console.ReadLine());

List<List<int>> neighbours = new List<List<int>>();
for (int i = 0; i < N; i++)
{
    neighbours.Add(new List<int>());
}

// 读入边 一个长度为2的数组 代表起点和终点
List<int[]> edges = new List<int[]>();
for (int i = 0; i < M; i++)
{
    edges.Add(Console.ReadLine().Split(' ').Select(int.Parse).ToArray());
    neighbours[edges[i][0]].Add(edges[i][1]);
}


// 利用floyd算法求最短路径

int[,] dis = new int[N, N];
for (int i = 0; i < N; i++)
{
    for (int j = 0; j < N; j++)
    {
        dis[i, j] = int.MaxValue;
    }
}

for (int i = 0; i < N; i++)
{
    dis[i, i] = 0;
}

foreach (var edge in edges)
{
    dis[edge[0], edge[1]] = W[edge[1]];
    dis[edge[1], edge[0]] = W[edge[0]];
}

for (int k = 0; k < N; k++)
{
    for (int i = 0; i < N; i++)
    {
        for (int j = 0; j < N; j++)
        {
            if (dis[i, k] != int.MaxValue && dis[k, j] != int.MaxValue && dis[i, k] + dis[k, j] < dis[i, j])
            {
                dis[i, j] = dis[i, k] + dis[k, j];
            }
        }
    }
}

// 读入起始点和n个终点
int start = 0;
int[] ends = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();

// 从起始点开始，每次找到一个可以到达的点，这个点可以使所有终点距离当前已到达的点的距离总和最小

HashSet<int> canReach = new HashSet<int>();
HashSet<int> visited = new HashSet<int>();
canReach.Add(start);

// 记录每个终点到visited中的任意一点的最短距离
int[] endDistance = new int[ends.Length];
for (int i = 0; i < ends.Length; i++)
{
    endDistance[i] = int.MaxValue / 1000;
}

// 输出一下endDistance
foreach (var d in endDistance)
{
    Console.Write($"{d} ");
}
Console.WriteLine();

while (endDistance.Any(s => s > 0))
{
    int maxReduceDis = -9999;
    int nextNode = -1;
    int minCost = int.MaxValue;
    // 从canReach中找到一个点 使所有ends中的点到visited中的任意一点的距离和最小
    foreach (var node in canReach) // 撕烤维护（？
    {
        int reduceDis = 0;
        for (int i = 0; i < ends.Length; i++)
        {
            if (dis[node, ends[i]] < endDistance[i])
            {
                reduceDis += endDistance[i] - dis[node, ends[i]];
            }
        }
        if (reduceDis - W[node] > maxReduceDis)
        {
            maxReduceDis = reduceDis - W[node];
            nextNode = node;
            minCost = W[node]; // 这个到底值不值还得撕烤
        }
        else if (reduceDis - W[node] ==  maxReduceDis)
        {
            if (W[node]< minCost)
            {
                nextNode = node;
                minCost = W[node]; // 这个到底值不值还得撕烤
            }
        }
    }


    // 更新endDistance
    for (int i = 0; i < ends.Length; i++)
    {
        if (dis[nextNode, ends[i]] < endDistance[i])
        {
            endDistance[i] = dis[nextNode, ends[i]];
        }
    }


    // 将这个点加入visited
    visited.Add(nextNode);
    canReach.Remove(nextNode);

    System.Console.WriteLine($"nextNode: {nextNode} maxReduceDis: {maxReduceDis}");
    // 输出一下endDistance
    foreach (var d in endDistance)
    {
        Console.Write($"{d} ");
    }
    Console.WriteLine();

    // canReach中加入这个点的邻居
    foreach (var neighbour in neighbours[nextNode])
    {
        if (!visited.Contains(neighbour))
        {
            canReach.Add(neighbour);
        }
    }
}

Console.WriteLine(visited.Count);
foreach (var node in visited)
{
    Console.Write($"{node} ");
}

