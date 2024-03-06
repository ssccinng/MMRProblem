// See https://aka.ms/new-console-template for more information

using P = (int, int);
using PP = ((int, int), int); 

// 点权斯坦纳树

const int root = 0; // 根节点
const int INF = 0x3f3f3f3f;

int n = int.Parse(Console.ReadLine()); // 总共有n个点

int[] w = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse); // 点权

List<List<int>> neighbours = new List<List<int>>();

int m = int.Parse(Console.ReadLine()); // 总共有m条边

for (int i = 0; i < n; i++)
{
    neighbours.Add(new List<int>());
}

for (int i = 0; i < m; i++)
{
    int[] edge = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
    neighbours[edge[0]].Add(edge[1]);
    neighbours[edge[1]].Add(edge[0]);
}

int k = int.Parse(Console.ReadLine()); // 需要包含的点的数量

int[] include = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse); // 需要包含的点的编号

int[,] dp = new int[200, 1 << k]; // dp[i][j]表示以i为根的子树，包含j中的点的最小权值
P[,] pre = new P[200, 1 << k];

for (int i = 0; i < 200; i++)
{
    for (int j = 0; j < 1 << k; j++)
    {
        dp[i, j] = INF;
    }
}

for (int i = 0; i < k; i++)
{
    // dp[include[i], 1 << i] = w[include[i]];
    dp[include[i], 1 << i] = 0;
}


 Queue<int> q = new Queue<int>();

for (int s = 1; s < 1 << k; s++)
{
    for (int i = 0; i < n; i++)
    {
        for (int j = (s - 1) & s; j > 0; j = (j - 1) & s)
        {
            if (dp[i, j] + dp[i, s ^ j] - w[i] < dp[i, s])
            {
                dp[i, s] = dp[i, j] + dp[i, s ^ j] - w[i];
                pre[i, s] = (i, j);
            }
        }
        if (dp[i, s] < INF)
        {
            q.Enqueue(i);

        }
    }
    spfa(s);

}



System.Console.WriteLine(dp[include.Last(), (1 << k) - 1]);

int[] ans = new int[200];
dfs(0, (1 << k) - 1);
for (int i = 0; i < n; i++)
{
    if (ans[i] == 1)
    {
        Console.Write(i + " ");
    }
}

// 输出一下非inf的点
// for (int i = 0; i < n; i++)
// {
//     for (int j = 0; j < 1 << k; j++)
//     {
//         if (dp[i, j] != INF)
//         {
//             System.Console.WriteLine($"{i} {j} {dp[i, j]}");
//         }
//     }
// }

// for (int i = 0; i < n; i++)
// {
//     if (dp[i, (1 << k) - 1] == dp[include.Last(), (1 << k) - 1])
//     {
//         Console.Write(i + " ");
//         dfs(pre[i, (1 << k) - 1].Item1, pre[i, (1 << k) - 1].Item2);
//         break;
//     }
// }


void spfa(int s)
{
    bool[] inq = new bool[200];
    // Queue<int> q = new Queue<int>();
    q.Enqueue(s);
    while (q.Count > 0)
    {
        int u = q.Dequeue();
        inq[u] = false;
        foreach (var v in neighbours[u])
        {
            if (dp[v, s] > dp[u, s] + w[v])
            {
                dp[v, s] = dp[u, s] + w[v];
                if (!inq[v])
                {
                    q.Enqueue(v);
                    inq[v] = true;
                }
             pre[v, s] = (u, s);
            }
        }
    }
    // for (int i = 0; i < 1 << k; i++)
    // {
    //     if (dp[s, i] != INF)
    //     {
    //         dp[s, i] += dis[s];
    //     }
    // }
}

void dfs(int u, int s)
{
    Console.WriteLine($"u:{u} s:{s}");
    if (pre[u, s].Item2 == 0)
    {
        return;
    }
    ans[u] = 1;
    if (pre[u, s].Item1 == u)
    {
        dfs(u, s ^ pre[u, s].Item2);
    }
    dfs(pre[u, s].Item1, pre[u, s].Item2);

}


public record Edge(int u, int v);