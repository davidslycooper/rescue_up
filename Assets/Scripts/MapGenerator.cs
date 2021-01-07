using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{

	public int width;
	public int height;

	[Range(0, 100)]
	public int randomFillPercent;

	[Range(0,20)]
	public int borderSize;

	[Range(0,200)]
	public int wallThresholdSize;

	[Range(0,200)]
	public int roomThresholdSize;

	public string[] seeds;

	public string seed;

	public bool useRandomSeed;

	int[,] map;
	List<Room> rooms;
	public Vector3 startPoint;

	List<Coord> blockedPassageTiles;

	public int[,] GenerateMap()
    {
		return GenerateMap(width, height);
    }

	public int[,] GenerateMap (int givenWidth, int givenHeight) {

		width = givenWidth;
		height = givenHeight;

		if (useRandomSeed) {
			seed = DateTime.Now.ToString();
			print(seed);
        } else
        {
			seed = seeds[UnityEngine.Random.Range(0, seeds.Length)];
        }
		blockedPassageTiles = new List<Coord>();

		map = new int[width,height];
		RandomFillMap ();

		SmoothMap ();

		width += borderSize * 2;
		height += borderSize * 2;

		int[,] borderedMap = new int[width,height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) { 
				if (x >= borderSize && x < width-borderSize && y >= borderSize && y < height-borderSize) {
					borderedMap[x,y] = map[x-borderSize, y-borderSize];
				} else {
					borderedMap[x,y] = 1;
				}
			}
		}

		map = borderedMap;

		ProcessMap();

		return map;
	}

	void RandomFillMap() {

		Generator prng = new Generator(seed.GetHashCode());

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) { 
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					map [x, y] = 1;
				} else {
					map [x, y] = (prng.Next (0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) { 
				int neighborWall = GetSurround (x, y);

				if (neighborWall > 4) {
					map [x, y] = 1;
				} else if (neighborWall < 4) {
					map [x, y] = 0;
				}
			}
		}
	}

	void ProcessMap() {
		List<List<Coord>> wallRegions = GetRegions (1);

		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion) {
					map[tile.x, tile.y] = 0;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions (0);

		List<Room> survivingRooms = new List<Room> ();

		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					map [tile.x, tile.y] = 1;
				}
			} else {
				survivingRooms.Add (new Room (roomRegion, map));
			}
		}

		survivingRooms.Sort ();
		survivingRooms [0].main = true;
		survivingRooms [0].accessible = true;

		rooms = survivingRooms;

		ConnectClosestRooms ();

		SetStartTunnel ();
	}

	void ConnectClosestRooms(bool forceAccessible = false) {

		List<Room> roomListA = new List<Room> ();
		List<Room> roomListB = new List<Room> ();

		if (forceAccessible) {
			foreach (Room room in rooms) {
				if (room.accessible) {
					roomListB.Add (room);
				} else {
					roomListA.Add (room);
				}
			}
		} else {
			roomListA = rooms;
			roomListB = rooms;
		}

		int bestDistance = 0;
		Coord bestTileA = new Coord ();
		Coord bestTileB = new Coord ();
		Room bestRoomA = new Room();
		Room bestRoomB = new Room();
		bool connect = false;

		foreach (Room roomA in roomListA) {
			if (!forceAccessible) {
				connect = false;
				if (roomA.connectedRooms.Count > 0) {
					continue;
				}
			}

			foreach (Room roomB in roomListB) {
				if (roomA == roomB || roomA.IsConnected(roomB)) {
					continue;
				}

				for (int tileIndA = 0; tileIndA < roomA.edgeTiles.Count; tileIndA++) {
					for (int tileIndB = 0; tileIndB < roomB.edgeTiles.Count; tileIndB++) {
						Coord tileA = roomA.edgeTiles[tileIndA];
						Coord tileB = roomB.edgeTiles[tileIndB];

						int distance = (int)(Mathf.Pow (tileA.x - tileB.x, 2) + Mathf.Pow (tileA.y - tileB.y, 2));

						if (distance < bestDistance || !connect) {
							bestDistance = distance;
							connect = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;
						}
					}
				}
			}
			if (connect && !forceAccessible) {
				CreatePassage (bestRoomA, bestRoomB, bestTileA, bestTileB);
			}
		}

		if (connect && forceAccessible) {
			CreatePassage (bestRoomA, bestRoomB, bestTileA, bestTileB);
			ConnectClosestRooms (true);
		}

		if (!forceAccessible) {
			ConnectClosestRooms (true);
		}
	}

	void SetStartTunnel() {

		int topY = 0;
		int biggest = 0;
		Room bestRoom = new Room();
		Coord bestTile = new Coord ();

		bool connect = false;

		foreach (Room room in rooms) {
			for (int tileInd = 0; tileInd < room.edgeTiles.Count; tileInd++) {
				Coord tile = room.edgeTiles [tileInd];

				if (tile.y > topY || (tile.y == topY && room.size > biggest) || !connect) {
					topY = tile.y;
					biggest = room.size;
					connect = true;
					bestRoom = room;
					bestTile = tile;
				}
			}
		}

		bestRoom.start = true;

		Coord topTile = new Coord (bestTile.x, height-5);

		Coord startTile = new Coord(topTile.x, topTile.y-2);

		startPoint = CoordToWorldPoint(startTile) + new Vector3(0,6,0);

		List<Coord> line = GetLine (bestTile, topTile);

		foreach (Coord c in line) {
			DrawCircle(c,3);
		}
	}

	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB) {
		Room.ConnectRooms (roomA, roomB);

		List<Coord> line = GetLine (tileA, tileB);
		foreach (Coord c in line) {
			DrawCircle(c,2);
		}

		blockedPassageTiles.Add(line[0]);
	}

	List<Coord> GetLine(Coord from, Coord to) {
		List<Coord> line = new List<Coord> ();

		int x = from.x;
		int y = from.y;

		int dx = to.x - from.x;
		int dy = to.y - from.y;

		bool inverted = false;
		int step = Math.Sign (dx);
		int gradientStep = Math.Sign (dy);

		int longest = Mathf.Abs (dx);
		int shortest = Mathf.Abs (dy);

		if (longest < shortest) {
			inverted = true;
			longest = Mathf.Abs (dy);
			shortest = Mathf.Abs (dx);

			step = Math.Sign (dy);
			gradientStep = Math.Sign (dx);
		}
		int gradientAcc = longest / 2;
		for (int i = 0; i < longest; i++) {
			line.Add(new Coord(x,y));

			if (inverted) {
				y += step;
			} else {
				x += step;
			}

			gradientAcc += shortest;
			if (gradientAcc >= longest) {
				if (inverted) {
					x += gradientStep;
				} else {
					y += gradientStep;
				}
				gradientAcc -= longest;
			}
		}
		return line;
	}

	void DrawCircle(Coord c, int r) {
		for (int x = -r; x <= r; x++) {
			for (int y = -r; y <= r; y++) {
				if (x * x + y * y <= r * r) {
					int drawX = c.x + x;
					int drawY = c.y + y;
					if (IsInMapRange (drawX, drawY)) {
						map [drawX, drawY] = 0;
					}
				}
			}
		}
	}

	Vector3 CoordToWorldPoint (Coord tile) {
		return new Vector3 (-width/2 + 0.5f + tile.x, -height/2 + 0.5f + tile.y);
	}

	List<List<Coord>> GetRegions(int tileType) {
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if(mapFlags[x,y] == 0 && map[x,y] == tileType) {
					List<Coord> region = GetRegionTiles (x, y);
					regions.Add (region);

					foreach(Coord tile in region) {
						mapFlags [tile.x, tile.y] = 1;
					}
				}
			}
		}
		return regions;
	}

	List<Coord> GetRegionTiles(int startX, int startY) {
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width, height];
		int tileType = map [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags[startX,startY] = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue ();
			tiles.Add (tile);

			for (int x = tile.x - 1; x <= tile.x + 1; x++) {
				for (int y = tile.y - 1; y <= tile.y + 1; y++) {
					if (IsInMapRange(x,y) && (x == tile.x || y == tile.y)) {
						if(mapFlags[x,y] == 0 && map[x,y] == tileType) {
							mapFlags[x,y] = 1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}
			}
		}

		return tiles;
	}


	bool IsInMapRange(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	int GetSurround(int x, int y) {
		int wallCount = 0;
		for (int neighborX = x - 1; neighborX <= x + 1; neighborX++) {
			for (int neighborY = y - 1; neighborY <= y + 1; neighborY++) {
				if (IsInMapRange(neighborX, neighborY)) {
					if (neighborX != x || neighborY != y) {
						wallCount += map [neighborX, neighborY];
					}
				} else {
					wallCount++;
				}
			}
		}
		return wallCount;
	}

	public List<Vector3> GetOpenCoords()
    {
		List<Vector3> openCoords = new List<Vector3>();

		foreach (Room room in rooms)
        {
			foreach (Coord tile in room.tiles)
            {
				Vector3 pos = CoordToWorldPoint(tile);

				if (Vector3.Distance(startPoint, pos) > 60)
				{
					openCoords.Add(pos);
				}
            }
        }

		return openCoords;
    }

	public List<Vector3> GetTightSpaces()
	{
		List<Vector3> tightSpaces = new List<Vector3>();

		foreach (Coord tile in blockedPassageTiles)
		{
			tightSpaces.Add(CoordToWorldPoint(tile));
		}
		return tightSpaces;
	}

	struct Coord {
		public int x;
		public int y;

		public Coord(int coordX, int coordY) {
			x = coordX;
			y = coordY;
		}
	}

	class Room : IComparable<Room> {
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int size;
		public bool accessible;
		public bool main;
		public bool start;

		public Room() {
		}

		public Room(List<Coord> roomTiles, int[,] map) {
			tiles = roomTiles;
			size = tiles.Count;
			connectedRooms = new List<Room>();

			edgeTiles = new List<Coord>();
			foreach (Coord tile in tiles) {
				for (int x = tile.x-1; x <= tile.x+1; x++) {
					for (int y = tile.y-1; y <= tile.y+1; y++) {
						if(x == tile.x || y == tile.y) {
							if (map[x,y] == 1) {
								edgeTiles.Add(tile);
							}
						}
					}
				}
			}
		}

		public void SetAccessible() {
			if (!accessible) {
				accessible = true;
				foreach (Room connectedRoom in connectedRooms) {
					connectedRoom.SetAccessible ();
				}
			}
		}

		public static void ConnectRooms(Room roomA, Room roomB) {
			if (roomA.accessible) {
				roomB.SetAccessible ();
			} else if (roomB.accessible) {
				roomA.SetAccessible ();
			}

			roomA.connectedRooms.Add (roomB);
			roomB.connectedRooms.Add (roomA);
		}

		public bool IsConnected (Room other) {
			return connectedRooms.Contains (other);
		}

		public int CompareTo(Room other) {
			return other.size.CompareTo (size);
		}
	}

	public class Generator: System.Random {
		
		private const int MBIG = Int32.MaxValue;
		private const int MSEED = 161803398;
		private const int MZ = 0;

		private int inext;
		private int inextp;
		private int[] SeedArray = new int[56];

		public Generator()
			: this(Environment.TickCount) {
		}

		public Generator(int seed){ 
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

		protected override double Sample() {
			return (InternalSample() * (1.0 / MBIG));
		}

		private int InternalSample() {
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

		public override int Next() {
			return InternalSample();
		}

		private double GetSampleForLargeRange() {
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


		public override int Next(int minValue, int maxValue) {
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

		public override void NextBytes(byte[] buffer) {
			if (buffer == null) throw new ArgumentNullException("buffer");
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = (byte)(InternalSample() % (Byte.MaxValue + 1));
			}
		}
	}		
}
