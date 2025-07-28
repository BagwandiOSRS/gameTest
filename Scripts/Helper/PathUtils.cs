public static class PathUtils
{
    public static int Hash(string s)
    {
        unchecked
        {
            int hash = 23;
            foreach (char c in s) hash = hash * 31 + c;
            return hash;
        }
    }
}
