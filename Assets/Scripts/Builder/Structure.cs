using Unity.Mathematics;

namespace Builder
{
    public class Structure
    {
        public int3 Size;
        public CellData[,,] Cells;

        public Structure(int3 size)
        {
            Size = size;
            Cells = new CellData[Size.x, Size.y, Size.z];
        }
    }
}
