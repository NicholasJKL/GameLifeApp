namespace GameLifeApp.Entities
{
	public abstract class Tile
	{
		public virtual char CharDisplay { get; set; }

		public virtual bool IsAlive { get; set; }

	}
}
