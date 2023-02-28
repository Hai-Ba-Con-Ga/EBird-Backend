namespace EBird.Application.Services.Algorithm
{
    public class Blossom
    {
        int n, m;
        List<int> mate;
        List<int> p, d, bl;
        List<List<int>> b, g;

        public Blossom(int n) {
            this.n = n;
            m = n + n / 2;
            mate = Enumerable.Repeat(-1, n).ToList();

			// b = Enumerable.Repeat(new List<int>(), m).ToList();
			b = new List<List<int>>();
			for (int i = 0; i < m; i++)
				b.Add(new List<int>());

            p = Enumerable.Repeat(0, m).ToList();
            d = Enumerable.Repeat(0, m).ToList();
            bl = Enumerable.Repeat(0, m).ToList();

			// g = Enumerable.Repeat(Enumerable.Repeat(-1, m).ToList(), m).ToList();
			g = new List<List<int>>();
			for (int i = 0; i < m; i++)
				g.Add(Enumerable.Repeat(-1, m).ToList());

        }

        public List<int> GetMate() {
            return mate;
        }

        public void AddEdge(int u, int v) {
            g[u][v] = u;
            g[v][u] = v;
        }

        public void Match(int u, int v) {
            g[u][v] = g[v][u] = -1;
            mate[u] = v;
            mate[v] = u;
        }

        public List<int> Trace(int x) {
            var vx = new List<int>();
            while(true) {
                while(bl[x] != x) x = bl[x];
                if (vx.Any() && vx.Last() == x) break;
                vx.Add(x);
                x = p[x];
            }
            return vx;
        }

        public void Contract(int c, int x, int y, List<int> vx, List<int> vy) {
            this.b[c].Clear();
            int r = vx.Last();
            while(vx.Any() && vy.Any() && vx.Last() == vy.Last()) {
                r = vx.Last();
                vx.RemoveAt(vx.Count - 1);
                vy.RemoveAt(vy.Count - 1);
            }
            b[c].Add(r);

            b[c].AddRange(vx.AsEnumerable().Reverse());
            b[c].AddRange(vy);

            for (int i = 0; i <= c; i++) {
                g[c][i] = g[i][c] = -1;
            }

            foreach (var z in b[c]) {
                bl[z] = c;
                for (int i = 0; i < c; i++) {
                    if (g[z][i] != -1) {
                        g[c][i] = z;
                        g[i][c] = g[i][z];
                    }
                }
            }
        }

        public List<int> Lift(List<int> vx) {
            var A = new List<int>();
            while(vx.Count >= 2) {
                var z = vx.Last();
                vx.RemoveAt(vx.Count - 1);
                if (z < n) {
                    A.Add(z);
                    continue;
                }

                var w = vx.Last();
                var i = (A.Count % 2 == 0 ? b[z].IndexOf(g[z][w]) : 0);
                var j = (A.Count % 2 == 1 ? b[z].IndexOf(g[z][A.Last()]) : 0);
                var k = b[z].Count;
                var dif = (A.Count % 2 == 0 ? i % 2 == 1 : j % 2 == 0)
                                            ? 1 : k - 1;
                while (i != j) {
                    vx.Add(b[z][i]);
                    i = (i + dif) % k;
                }
                vx.Add(b[z][i]);
            }
            return A;
        }

        public int Solve() {
            for (int ans = 0; true; ans++) {
                for (int i = 0; i < this.d.Count; i++) 
                    d[i] = 0;
                var Q = new Queue<int>();
                for (int i = 0; i < m; i++) bl[i] = i;
                for (int i = 0; i < n; i++) {
                    if (mate[i] != -1) continue;
                    Q.Enqueue(i);
                    p[i] = i;
                    d[i] = 1;
                }
				int c = n;
				bool aug = false;
				while (Q.Count != 0 && !aug) {
                    int x = Q.Dequeue();
                    if (bl[x] != x) continue;
                    for (int y = 0; y < c; y++) {
                        if (!(bl[y] == y &&  g[x][y] != -1)) continue;
                        if (d[y] == 0) {
                            p[y] = x;
                            d[y] = 2;
                            p[mate[y]] = y;
                            d[mate[y]] = 1;
                            Q.Enqueue(mate[y]);
                        }
                        else if (d[y] == 1) {
                            var vx = Trace(x);
                            var vy = Trace(y);
                            if (vx.Last() == vy.Last()) {
                                this.Contract(c, x, y, vx, vy);
                                Q.Enqueue(c);
                                p[c] = p[b[c][0]];
                                d[c] = 1;
                                c++;
                            } else {
                                aug = true;
                                vx.Insert(0, y);
                                vy.Insert(0, x);
                                var A = Lift(vx);
                                var B = Lift(vy);
                                A.AddRange(B.AsEnumerable().Reverse());
                                for (int i = 0; i < A.Count; i += 2) {
                                    Match(A[i], A[i + 1]);
                                    if (i + 2 < A.Count)
                                        AddEdge(A[i + 1], A[i + 2]);
                                }
                            }
                            break;
                        }
                    }
                }
                if (!aug) return ans;
            }
        }

    }
}