using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Datas
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "ScriptableObject.LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        public List<Level> Levels = new List<Level>();

        [Serializable]
        public class Level
        {
            public Sprite LevelSprite;
            public int CountElements;
            public List<Element> ElementsPuzzles = new List<Element>();

            [Serializable]
            public class Element
            {
                public Sprite ElementSprite;
                public int ElementIndex;
            }
        }
    }
}
