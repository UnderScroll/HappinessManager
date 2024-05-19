using Unity.Mathematics;

namespace Builder
{
    public class Structure
    {
        private int3 _size;
        public CellData[,,] Cells;

        public Structure(int3 size)
        {
            _size = size;
            Cells = new CellData[_size.x, _size.y, _size.z];
        }
    }
}
