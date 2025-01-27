namespace GameLifeApp.Entities
{
	public class Border : Tile
	{
		public Tile LinkedTile { get; set; }

		public override bool IsAlive
		{
			get
			{
				return LinkedTile.IsAlive;
			}
			set 
			{
				LinkedTile.IsAlive = value;
			}
		}

		public Border(char charDisplay, Tile linkedTile) 
		{
			LinkedTile = linkedTile;
			CharDisplay = charDisplay;
		}
	}
}
