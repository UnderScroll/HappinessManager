using Builder;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace LevelLoader
{
    [CreateAssetMenu(fileName = "Level", menuName = "Structure/Level")]

    public class Level : ScriptableObject
    {
        public Structure Structure;
        public CellTypes CellTypes;
        public CellTypes PlaceableCellTypes;

        public void TempInitStruct2()
        {
            Structure = new Structure(new int3(10, 10, 10));

            for (uint x = 0; x < Structure.Cells.GetLength(0); x++)
                for (uint z = 0; z < Structure.Cells.GetLength(2); z++)
                {
                    CellData cellData = new(CellTypes.Get("InvisibleBlock")){ position = new int3((int)x, 0, (int)z) };
                    Structure.Cells[x, 0, z] = cellData;
                }
                        

            Structure.Cells[6, 8, 5] = new(CellTypes.Get("EmployeeBlock")) { position = new int3(6, 8, 5) };
            Structure.Cells[1, 8, 4] = new(CellTypes.Get("EmployeeBlock")) { position = new int3(1, 8, 4) };

            Structure.Cells[2, 0, 2] = new(CellTypes.Get("BasicBlock")) { position = new int3(2, 0, 2) };
        }

        public void TempInitStruct1()
        {
            Structure = new Structure(new int3(5, 5, 5));

            for (uint x = 0; x < Structure.Cells.GetLength(0); x++)
                for (uint z = 0; z < Structure.Cells.GetLength(2); z++)
                {
                    CellData cellData = new(CellTypes.Get("InvisibleBlock")) { position = new int3((int)x, 0, (int)z) };
                    Structure.Cells[x, 0, z] = cellData;
                }


            Structure.Cells[2, 3, 2] = new(CellTypes.Get("EmployeeBlock")) { position = new int3(2, 3, 2) };

            Structure.Cells[0, 0, 0] = new(CellTypes.Get("BasicBlock")) { position = new int3(0, 0, 0) };

            Structure.Cells[4, 0, 4] = new(CellTypes.Get("BasicBlock")) { position = new int3(4, 0, 4) };
        }
    }
}
