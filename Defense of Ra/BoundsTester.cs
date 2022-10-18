using System;
using System.Collections.Generic;
using System.Text;

namespace DefenseOfRa
{
    class BoundsTester : StaticObject
    {
        // FIELDS

        private GameObject target;

        // PROPERTIES

        // CONSTRUCTORS

        public BoundsTester(GameObject target) : base(target.Position, Game1.Placeholder)
        {
            this.target = target;
            this.bounds = target.Bounds;
        }

        // METHODS

        public void Update()
        {
            this.position = target.Position;
            this.bounds = target.Bounds;
        }
    }
}
