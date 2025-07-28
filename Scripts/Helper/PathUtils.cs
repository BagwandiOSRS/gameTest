using System.Collections.Generic;

public static class PathUtils
{
    // Cache for frequently hashed strings
    private static readonly Dictionary<string, int> hashCache = new Dictionary<string, int>();
    
    public static int Hash(string s)
    {
        // Check cache first for performance
        if (hashCache.TryGetValue(s, out int cachedHash))
        {
            return cachedHash;
        }
        
        // Optimized hash function using FNV-1a algorithm
        unchecked
        {
            const uint FNV_OFFSET_BASIS = 2166136261;
            const uint FNV_PRIME = 16777619;
            
            uint hash = FNV_OFFSET_BASIS;
            
            for (int i = 0; i < s.Length; i++)
            {
                hash ^= s[i];
                hash *= FNV_PRIME;
            }
            
            int result = (int)hash;
            
            // Cache the result for future use
            if (hashCache.Count < 1000) // Prevent unbounded growth
            {
                hashCache[s] = result;
            }
            
            return result;
        }
    }
    
    // Clear cache if needed (for memory management)
    public static void ClearHashCache()
    {
        hashCache.Clear();
    }
}
