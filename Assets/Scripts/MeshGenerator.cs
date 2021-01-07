using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeshGenerator : MonoBehaviour {

	SquareGrid squareGrid;

	public List<Vector3> vertices;
	public List<int> triangles;

	public string seed;

	Dictionary<int,List<Triangle>> triangleDict = new Dictionary<int,List<Triangle>>();
	public List<List<int>> outlines = new List<List<int>> ();
	HashSet<int> checkVertices = new HashSet<int> ();

	public Mesh GenerateMesh(int[,] map, float squareSize){

		triangleDict.Clear ();
		outlines.Clear ();
		checkVertices.Clear ();

		squareGrid = new SquareGrid (map, squareSize);

		vertices = new List<Vector3>();
		triangles = new List<int>();

		for (int x = 0; x < squareGrid.squares.GetLength (0); x++) {
			for (int y = 0; y < squareGrid.squares.GetLength (1); y++) {
				TriangulateSquare (squareGrid.squares[x,y]);
			}
		}

		Mesh mesh = new Mesh ();

		mesh.vertices = vertices.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.RecalculateNormals ();

		Vector2[] uvs = new Vector2[vertices.Count];
		for (int i = 0; i < vertices.Count; i++) {
			float percentX = Mathf.InverseLerp (-map.GetLength(0)/2*squareSize,
				map.GetLength(0)/2*squareSize, vertices[i].x);
			float percentY = Mathf.InverseLerp (-map.GetLength(0)/2*squareSize,
				map.GetLength(0)/2*squareSize, vertices[i].y);
			uvs [i] = new Vector2 (percentX, percentY);
		}

		mesh.uv = uvs;

		return mesh;
	}

	public List<List<float[]>> GetSpawns(Vector3 startPoint, int mineralPercent, int enemyNumber)
	{
		List<List<float[]>> spawns = new List<List<float[]>>();

		List<float[]> rocks = new List<float[]>();
		List<float[]> minerals = new List<float[]>();
		List<float[]> corals = new List<float[]>();
		List<float[]> enemies = new List<float[]>();
		List<float[]> blockedPassages = new List<float[]>();
		List<float[]> treasure = new List<float[]>();

		List<Vector3> openSpaces = GetComponent<MapGenerator>().GetOpenCoords();
		List<Vector3> tightSpaces = GetComponent<MapGenerator>().GetTightSpaces();

		Generator prng = new Generator(seed.GetHashCode());

		Vector3 treasurePos = new Vector3(0, 64, 0);


		// RESOURCE SPAWNER

		foreach (List<int> outline in outlines)
		{
			for (int i = 0; i < outline.Count; i++)
			{
				Vector3 pos = new Vector3(vertices[outline[i]].x,
					vertices[outline[i]].y);

				rocks.Add(new float[] { pos.x, pos.y });

				if (Vector3.Distance(startPoint, pos) > 20 && prng.Next(0, 100) < mineralPercent)
				{
					int mineralInd = 0;

					if (prng.Next(0, 10) < mineralPercent)
					{
						mineralInd++;

						if (prng.Next(0, 13) < mineralPercent)
						{
							if (UnityEngine.Random.value > 0.5f)
							{
								mineralInd++;
							}
							else
							{
								mineralInd += 2;
							}
						}
					}
					minerals.Add(new float[] { pos.x, pos.y, (float) mineralInd });
				}
				else if (Vector3.Distance(startPoint, pos) > 20 && prng.Next(0, 100) < 50)
				{
					corals.Add(new float[] { pos.x, pos.y });
				}

				if (pos.y < treasurePos.y)
				{
					treasurePos = pos + Vector3.up * 0.5f;
				}
			}
		}

		treasure.Add(new float[] { treasurePos.x, treasurePos.y });

		// ENEMY SPAWNER

		for (int i = 0; i < openSpaces.Count; i += openSpaces.Count/enemyNumber-1)
		{
			Vector3 pos = openSpaces[i];

			if (Vector3.Distance(startPoint, pos) > 60)
			{
				int enemyInd = UnityEngine.Random.Range(0, 2);
				enemies.Add(new float[] { pos.x, pos.y, (float)enemyInd });
			}
		}

		// BROKEN ROCK SPAWNER

		foreach (Vector3 pos in tightSpaces)
		{
			if (Vector3.Distance(startPoint, pos) > 60)
			{
				blockedPassages.Add(new float[] { pos.x, pos.y });
			}
		}

		spawns.Add(rocks);
		spawns.Add(minerals);
		spawns.Add(corals);
		spawns.Add(enemies);
		spawns.Add(blockedPassages);
		spawns.Add(treasure);

		return spawns;
	}

	public void Generate2DColliders() {

		EdgeCollider2D[] currentColliders = gameObject.GetComponents<EdgeCollider2D> ();
		for (int i = 0; i < currentColliders.Length; i++) {
			Destroy (currentColliders [i]);
		}

		CalculateOutlines ();

		foreach (List<int> outline in outlines) {
			EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D> ();
			Vector2[] edgePoints = new Vector2[outline.Count];

			for (int i = 0; i < outline.Count; i++) {
				edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].y);
			}
			edgeCollider.points = edgePoints;
		}
	}

	void TriangulateSquare(Square square) {
		switch (square.configuration) {
		case 0:
			break;

		// 1 point:
		case 1:
			MeshFromPoints (square.cl, square.cb, square.bl);
			break;
		case 2:
			MeshFromPoints (square.br, square.cb, square.cr);
			break;
		case 4:
			MeshFromPoints (square.tr, square.cr, square.ct);
			break;
		case 8:
			MeshFromPoints (square.tl, square.ct, square.cl);
			break;

		// 2 points:
		case 3:
			MeshFromPoints (square.cr, square.br, square.bl, square.cl);
			break;
		case 6:
			MeshFromPoints (square.ct, square.tr, square.br, square.cb);
			break;
		case 9:
			MeshFromPoints (square.tl, square.ct, square.cb, square.bl);
			break;
		case 12:
			MeshFromPoints (square.tl, square.tr, square.cr, square.cl);
			break;
		case 5:
			MeshFromPoints (square.ct, square.tr, square.cr, square.cb, square.bl, square.cl);
			break;
		case 10:
			MeshFromPoints (square.tl, square.ct, square.cr, square.br, square.cb, square.cl);
			break;

		//3 points:
		case 7:
			MeshFromPoints (square.ct, square.tr, square.br, square.bl, square.cl);
			break;
		case 11:
			MeshFromPoints (square.tl, square.ct, square.cr, square.br, square.bl);
			break;
		case 13:
			MeshFromPoints (square.tl, square.tr, square.cr, square.cb, square.bl);
			break;
		case 14:
			MeshFromPoints (square.tl, square.tr, square.br, square.cb, square.cl);
			break;

		//4 points:
		case 15:
			MeshFromPoints (square.tl, square.tr, square.br, square.bl);
			checkVertices.Add((square.tl.vertex));
			checkVertices.Add((square.tr.vertex));
			checkVertices.Add((square.br.vertex));
			checkVertices.Add((square.bl.vertex));
			break;
		}
		
	}

	void MeshFromPoints(params Node[] points) {
		AssignVertices (points);

		if (points.Length >= 3) {
			CreateTriangle (points[0],points[1],points[2]);
		}
		if (points.Length >= 4) {
			CreateTriangle (points[0],points[2],points[3]);
		}
		if (points.Length >= 5) {
			CreateTriangle (points[0],points[3],points[4]);
		}
		if (points.Length >= 6) {
			CreateTriangle (points[0],points[4],points[5]);
		}
	}

	void AssignVertices (Node[] points) {
		for (int i = 0; i < points.Length; i++) {
			if (points [i].vertex == -1) {
				points [i].vertex = vertices.Count;
				vertices.Add(points[i].pos);
			}
		}
	}

	void CreateTriangle(Node a, Node b, Node c) {
		triangles.Add (a.vertex);
		triangles.Add (b.vertex);
		triangles.Add (c.vertex);

		Triangle triangle = new Triangle (a.vertex, b.vertex, c.vertex);
		AddTriangle (triangle.vertexA, triangle);
		AddTriangle (triangle.vertexB, triangle);
		AddTriangle (triangle.vertexC, triangle);
	}

	void AddTriangle(int key, Triangle triangle) {
		if (triangleDict.ContainsKey (key)) {
			triangleDict [key].Add (triangle);
		} else {
			List<Triangle> triangles = new List<Triangle> ();
			triangles.Add (triangle);
			triangleDict.Add (key, triangles);
		}
	}

	void CalculateOutlines() {
		for(int vertex = 0; vertex < vertices.Count; vertex++) {
			if (!checkVertices.Contains (vertex)) {
				int outlineVertex = GetConnected (vertex);
				if (outlineVertex != -1) {
					checkVertices.Add (vertex);

					List<int> outline = new List<int> ();
					outline.Add (vertex);
					outlines.Add (outline);
					FollowOutline (outlineVertex, outlines.Count-1);
					outlines[outlines.Count - 1].Add (vertex);
				}
			}
		}
	}

	void FollowOutline(int vertex, int outline) {
		outlines [outline].Add (vertex);
		checkVertices.Add (vertex);
		int nextVertex = GetConnected (vertex);

		if (nextVertex != -1) {
			FollowOutline (nextVertex, outline);
		}
	}

	int GetConnected(int vertex) {
		List<Triangle> containsVertex = triangleDict [vertex];

		for (int i = 0; i < containsVertex.Count; i++) {
			Triangle triangle = containsVertex [i];

			for (int j = 0; j < 3; j++) {
				int vertexB = triangle [j];
				if (vertexB != vertex && !checkVertices.Contains(vertexB)) {
					if (IsOutline (vertex, vertexB)) {
						return vertexB;
					}
				}
			}
		}
		return -1;
	}

	bool IsOutline(int vertexA, int vertexB) {
		List<Triangle> containsA = triangleDict [vertexA];
		int shared = 0;

		for (int i = 0; i < containsA.Count; i++) {
			if (containsA [i].Contains (vertexB)) {
				shared++;
				if (shared > 1) {
					break;
				}
			}
		}
		return shared == 1;
	}

	struct Triangle {
		public int vertexA;
		public int vertexB;
		public int vertexC;
		int[] vertices;

		public Triangle(int a, int b, int c) {
			vertexA = a;
			vertexB = b;
			vertexC = c;

			vertices = new int[3];
			vertices[0] = a;
			vertices[1] = b;
			vertices[2] = c;
		}

		public int this[int i] {
			get {
				return vertices [i];
			}
		}

		public bool Contains(int vertex) {
			return vertex == vertexA || vertex == vertexB || vertex == vertexC;
		}

	}

	public class SquareGrid {
		public Square[,] squares;

		public SquareGrid(int[,] map, float squareSize) {
			int countX = map.GetLength(0);
			int countY = map.GetLength(1);
			float mapWidth = countX * squareSize;
			float mapHeight = countY * squareSize;

			ControlNode[,] controlNodes = new ControlNode[countX, countY];

			for (int x = 0; x < countX; x++) {
				for (int y = 0; y < countY; y++) {
					Vector3 pos = new Vector3(-mapWidth/2 + x * squareSize + squareSize/2, -mapHeight/2 + y * squareSize + squareSize/2);
					controlNodes[x, y] = new ControlNode(pos, map[x,y] == 1, squareSize);
				}
			}

			squares = new Square[countX - 1, countY - 1];

			for (int x = 0; x < countX-1; x++) {
				for (int y = 0; y < countY-1; y++) {
					squares[x,y] = new Square(controlNodes[x,y+1],
						controlNodes[x+1,y+1], 
						controlNodes[x+1,y], 
						controlNodes[x,y]);
				}
			}
		}
	}

	public class Square {
		public ControlNode tl, tr, br, bl;
		public Node ct, cr, cb, cl;
		public int configuration;

		public Square(ControlNode _tl, ControlNode _tr, ControlNode _br, ControlNode _bl) {
			tl = _tl;
			tr = _tr;
			br = _br;
			bl = _bl;

			ct = tl.right;
			cr = br.above;
			cb = bl.right;
			cl = bl.above;

			if (tl.active) {
				configuration += 8;
			}
			if (tr.active) {
				configuration += 4;
			}
			if (br.active) {
				configuration += 2;
			}
			if (bl.active) {
				configuration += 1;
			}
		}
	}

	public class Node {
		public Vector3 pos;
		public int vertex = -1;

		public Node(Vector3 position) {
			pos = position;
		}
	}

	public class ControlNode : Node {
		public bool active;
		public Node above, right;

		public ControlNode(Vector3 position, bool _active, float size) : base(position) {
			active = _active;
			above = new Node(pos + Vector3.up * size/2f);
			right = new Node(pos + Vector3.right * size/2f);
		}
	}

	public class Generator : System.Random
	{

		private const int MBIG = Int32.MaxValue;
		private const int MSEED = 161803398;
		private const int MZ = 0;

		private int inext;
		private int inextp;
		private int[] SeedArray = new int[56];

		public Generator()
			: this(Environment.TickCount)
		{
		}

		public Generator(int seed)
		{
			int ii;
			int mj, mk;

			int subtraction = (seed == Int32.MinValue) ? Int32.MaxValue : Math.Abs(seed);
			mj = MSEED - subtraction;
			SeedArray[55] = mj;
			mk = 1;
			for (int i = 1; i < 55; i++)
			{
				ii = (21 * i) % 55;
				SeedArray[ii] = mk;
				mk = mj - mk;
				if (mk < 0) mk += MBIG;
				mj = SeedArray[ii];
			}
			for (int k = 1; k < 5; k++)
			{
				for (int i = 1; i < 56; i++)
				{
					SeedArray[i] -= SeedArray[1 + (i + 30) % 55];
					if (SeedArray[i] < 0) SeedArray[i] += MBIG;
				}
			}
			inext = 0;
			inextp = 21;
		}

		protected override double Sample()
		{
			return (InternalSample() * (1.0 / MBIG));
		}

		private int InternalSample()
		{
			int retVal;
			int locINext = inext;
			int locINextp = inextp;

			if (++locINext >= 56) locINext = 1;
			if (++locINextp >= 56) locINextp = 1;

			retVal = SeedArray[locINext] - SeedArray[locINextp];

			if (retVal == MBIG) retVal--;
			if (retVal < 0) retVal += MBIG;

			SeedArray[locINext] = retVal;

			inext = locINext;
			inextp = locINextp;

			return retVal;
		}

		public override int Next()
		{
			return InternalSample();
		}

		private double GetSampleForLargeRange()
		{
			int result = InternalSample();
			bool negative = (InternalSample() % 2 == 0) ? true : false;
			if (negative)
			{
				result = -result;
			}
			double d = result;
			d += (Int32.MaxValue - 1);
			d /= 2 * (uint)Int32.MaxValue - 1;
			return d;
		}


		public override int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException("minValue");
			}

			long range = (long)maxValue - minValue;
			if (range <= (long)Int32.MaxValue)
			{
				return ((int)(Sample() * range) + minValue);
			}
			else
			{
				return (int)((long)(GetSampleForLargeRange() * range) + minValue);
			}
		}

		public override void NextBytes(byte[] buffer)
		{
			if (buffer == null) throw new ArgumentNullException("buffer");
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = (byte)(InternalSample() % (Byte.MaxValue + 1));
			}
		}
	}
}
