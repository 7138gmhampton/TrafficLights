using System;
using UnityEngine;

public partial class ZoneCreator
{
    private struct Rule
    {
        public Action<GameObject, int, int, int> Method { get; set; }
        public GameObject ZoneTile { get; set; }
        public int Axis { get; set; }
        public int Position { get; set; }
        public int Azimuth { get; set; }

        internal Rule(
            Action<GameObject, int, int, int> method,
            GameObject zoneTile,
            int axis,
            int position,
            int azimuth)
        {
            Method = method;
            ZoneTile = zoneTile;
            Axis = axis;
            Position = position;
            Azimuth = azimuth;
        }
    }
}