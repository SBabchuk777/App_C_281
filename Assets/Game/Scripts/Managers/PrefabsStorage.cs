using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Datas;
using Views;

namespace Managers
{
    public class PrefabsStorage : Singleton<PrefabsStorage>
    {
        [Header("Configs")]
        public LevelsConfig LevelsConfig;

        [Header("Prefabs")]
        public ElementPuzzleView PuzzleView;
        public GridView GridView;
        public WrongView WrongView;
    }
}
