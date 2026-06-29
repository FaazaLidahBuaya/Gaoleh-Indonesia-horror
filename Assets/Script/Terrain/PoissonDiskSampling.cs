using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Poisson Disk Sampling untuk distribusi rumput yang lebih alami.
/// Memastikan jarak minimum antar titik terpenuhi.
/// </summary>
public static class PoissonDiskSampling
{
    /// <summary>
    /// Generate posisi menggunakan algoritma Bridson's Poisson Disk Sampling.
    /// </summary>
    /// <param name="bounds">Area spawn</param>
    /// <param name="minDist">Jarak minimum antar titik</param>
    /// <param name="maxPoints">Batas maksimum titik</param>
    /// <param name="maxAttempts">Percobaan per titik aktif</param>
    public static List<Vector3> Generate(
        Bounds bounds,
        float minDist,
        int maxPoints,
        int maxAttempts = 30)
    {
        float cellSize = minDist / Mathf.Sqrt(2f);
        float width = bounds.size.x;
        float depth = bounds.size.z;
        float y = bounds.max.y;

        int cols = Mathf.CeilToInt(width / cellSize);
        int rows = Mathf.CeilToInt(depth / cellSize);

        // Grid untuk spatial lookup
        int[,] grid = new int[cols, rows];
        for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
                grid[c, r] = -1;

        List<Vector3> points = new List<Vector3>();
        List<int> activeList = new List<int>();

        // Titik awal di tengah
        Vector3 startPos = new Vector3(
            bounds.center.x,
            y,
            bounds.center.z
        );

        AddPoint(startPos, points, activeList, grid, bounds, cellSize, cols, rows);

        while (activeList.Count > 0 && points.Count < maxPoints)
        {
            int idx = Random.Range(0, activeList.Count);
            Vector3 origin = points[activeList[idx]];
            bool found = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // Random arah & jarak antara minDist dan 2*minDist
                float angle = Random.Range(0f, Mathf.PI * 2f);
                float dist = Random.Range(minDist, minDist * 2f);

                Vector3 candidate = new Vector3(
                    origin.x + Mathf.Cos(angle) * dist,
                    y,
                    origin.z + Mathf.Sin(angle) * dist
                );

                if (!bounds.Contains(candidate)) continue;
                if (!IsValidPoint(candidate, points, grid, bounds, cellSize, cols, rows, minDist)) continue;

                AddPoint(candidate, points, activeList, grid, bounds, cellSize, cols, rows);
                found = true;

                if (points.Count >= maxPoints) break;
            }

            if (!found)
                activeList.RemoveAt(idx);
        }

        return points;
    }

    static void AddPoint(
        Vector3 pt,
        List<Vector3> points,
        List<int> active,
        int[,] grid,
        Bounds bounds,
        float cellSize,
        int cols, int rows)
    {
        int cx = Mathf.Clamp(Mathf.FloorToInt((pt.x - bounds.min.x) / cellSize), 0, cols - 1);
        int cz = Mathf.Clamp(Mathf.FloorToInt((pt.z - bounds.min.z) / cellSize), 0, rows - 1);

        int idx = points.Count;
        points.Add(pt);
        active.Add(idx);
        grid[cx, cz] = idx;
    }

    static bool IsValidPoint(
        Vector3 pt,
        List<Vector3> points,
        int[,] grid,
        Bounds bounds,
        float cellSize,
        int cols, int rows,
        float minDist)
    {
        int cx = Mathf.FloorToInt((pt.x - bounds.min.x) / cellSize);
        int cz = Mathf.FloorToInt((pt.z - bounds.min.z) / cellSize);

        int searchRadius = 2;
        for (int dx = -searchRadius; dx <= searchRadius; dx++)
        {
            for (int dz = -searchRadius; dz <= searchRadius; dz++)
            {
                int nx = cx + dx;
                int nz = cz + dz;

                if (nx < 0 || nx >= cols || nz < 0 || nz >= rows) continue;

                int neighborIdx = grid[nx, nz];
                if (neighborIdx < 0) continue;

                float sqrDist = Vector2.SqrMagnitude(
                    new Vector2(pt.x - points[neighborIdx].x,
                                pt.z - points[neighborIdx].z)
                );

                if (sqrDist < minDist * minDist) return false;
            }
        }
        return true;
    }
}
