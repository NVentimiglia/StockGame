namespace Stocks.Services
{
    /// <summary>
    /// Allows for the averaging of a set of values
    /// </summary>
    public class RollingWindow
    {
        public int Index;
        public int[] Values;

        public RollingWindow(int size, int initAverage = 0)
        {
            Values = new int[size];
            for (int i = 0; i < size; i++)
            {
                Values[i] = initAverage;
            }
        }

        public void Add()
        {
            Values[Index]++;
        }

        public void Roll()
        {
            Index = Index + 1 >= Values.Length ? 0 : Index + 1;
            Values[Index] = 0;
        }

        public float Average()
        {
            int total = 0;
            for (int i = 0; i < Values.Length; i++)
            {
                if (i == Index)
                    continue;
                total += Values[i];
            }

            return (float)total / (Values.Length - 1f);
        }
    }
}
